using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace UBackup
{
    public class Backuper
    {
        private readonly ILogger _logger;
        private readonly Settings _settings;
        private readonly Dictionary<string, PathInfoCache> _caches;

        public Backuper(ILogger logger, Settings settings)
        {
            _logger = logger;
            _settings = settings;
            _caches = new Dictionary<string, PathInfoCache>();
        }

        #region BACKUP
        public void BackupMonitorPaths()
        {
            try
            {
                foreach (var monitorPath in _settings.MonitorPaths)
                {
                    _logger.AddInfo($"Checking path: {monitorPath}...");
                    var doBackup = CheckMonitorPath(monitorPath);
                    if (doBackup)
                    {
                        _logger.AddInfo($"Changes detected - starting backup process...");
                        BackupMonitorPath(monitorPath);
                        _logger.AddInfo($"Backup completed successfully.");
                    }
                    else
                    {
                        _logger.AddInfo($"No changes detected.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"Backuper >>> BackupMonitorPaths:", ex);
            }
        }

        private void BackupMonitorPath(string monitorPath)
        {
            try
            {
                //Prep
                var tempRoot = Path.Combine(Path.GetTempPath(), "UBackup");
                var monitorRootDir = new DirectoryInfo(monitorPath).Name;
                var dt = DateTime.Now;
                var namePostFix = $"_{dt.ToString("dd-MM-yyyy")}_{dt.ToString("HHmmss")}";
                var destinationPath = Path.Combine(tempRoot, $"{monitorRootDir}{namePostFix}");
                var zipRootPath = Path.Combine(_settings.BackupPath, monitorRootDir);
                var zipFilePath = Path.Combine(zipRootPath, $"{monitorRootDir}{namePostFix}.zip");

                //Copy to temp
                _logger.AddInfo($"Copying to temp folder: {destinationPath}...");
                var diSource = new DirectoryInfo(monitorPath);
                var diTarget = new DirectoryInfo(destinationPath);
                CopyAll(diSource, diTarget);

                //Zip (TODO - Could be replace in future with external zip lip for better performance/space saving)
                _logger.AddInfo($"Creating ZIP file {zipFilePath}...");
                if (!Directory.Exists(zipRootPath)) Directory.CreateDirectory(zipRootPath);
                ZipFile.CreateFromDirectory(destinationPath, zipFilePath);

                //Clear temp
                _logger.AddInfo($"Deleting temp folder {destinationPath}...");
                Directory.Delete(destinationPath, true);

                //Check number of backup files
                if (_settings.MaxBackupFiles > 0)
                {
                    _logger.AddInfo($"Checking number of backup files (max is set to {_settings.MaxBackupFiles})...");
                    var deletedFiles = CheckBackupFilesMax(zipRootPath, _settings.MaxBackupFiles);
                    _logger.AddInfo($"Deleted {deletedFiles} backup files.");
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"Backuper >>> BackupMonitorPath:", ex);
            }
        }

        private int CheckBackupFilesMax(string backupRootPath, int maxBackupFiles)
        {
            try
            {
                var fileNames = Directory.GetFiles(backupRootPath, "*.zip", SearchOption.TopDirectoryOnly);
                if (fileNames.Length <= maxBackupFiles) return 0;

                var filesInfo = new PathInfoCache(_logger);
                foreach (var fileName in fileNames)
                {
                    var creationTime = File.GetCreationTime(fileName);
                    filesInfo.List.Add(new PathInfo()
                    {
                        Path = fileName,
                        LastModified = creationTime
                    });
                }

                var orderedList = filesInfo.List.OrderByDescending(x=>x.LastModified);
                var savedBackupFiles = maxBackupFiles;
                var deletedBackupFiles = 0;
                foreach (var pathInfo in orderedList)
                {
                    if (savedBackupFiles > 0)
                    {
                        savedBackupFiles--;
                    }
                    else
                    {
                        deletedBackupFiles++;
                        File.Delete(pathInfo.Path);
                    }
                }

                return deletedBackupFiles;
            }
            catch (Exception ex)
            {
                _logger.AddError($"Backuper >>> CheckBackupFilesMax:", ex);
            }

            return 0;
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            try
            {
                Directory.CreateDirectory(target.FullName);

                // Copy each file into the new directory.
                foreach (var fi in source.GetFiles())
                {
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                }

                // Copy each subdirectory using recursion.
                foreach (var diSourceSubDir in source.GetDirectories())
                {
                    var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"Backuper >>> CopyAll:", ex);
            }
        }
        #endregion

        #region CHECK
        private bool CheckMonitorPath(string monitorPath)
        {
            try
            {
                var currentCache = GetFilesInfo(monitorPath);

                PathInfoCache storedCache;
                if (_caches.TryGetValue(monitorPath, out storedCache))
                {
                    if (AreCachesDifferent(currentCache, storedCache))
                    {
                        _caches[monitorPath] = currentCache;
                        return true;
                    }
                }
                else
                {
                    _caches.Add(monitorPath, currentCache);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"Backuper >>> CheckMonitorPath:", ex);
            }

            return false;
        }

        private bool AreCachesDifferent(PathInfoCache currentCache, PathInfoCache storedCache)
        {
            try
            {
                //Check stored cache against files/folder in monitored path
                foreach (var pathInfo in currentCache.List)
                {
                    if (!storedCache.FindAndComapre(pathInfo))
                    {
                        return true;
                    }
                }

                //And other way around - check if all files/folder from storeed cache exist in monitored path
                foreach (var pathInfo in storedCache.List)
                {
                    var exist = File.Exists(pathInfo.Path) || Directory.Exists(pathInfo.Path);
                    if (!exist) return true;
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"Backuper >>> AreCachesDifferent:", ex);
            }

            return false;
        }
        #endregion

        #region GET INFO
        private PathInfoCache GetFilesInfo(string monitorPath)
        {
            var filesInfo = new PathInfoCache(_logger);

            try
            {
                var fileNames = Directory.GetFiles(monitorPath, "*.*", SearchOption.AllDirectories);
                foreach (var fileName in fileNames)
                {
                    var lastModified = File.GetLastWriteTime(fileName);
                    filesInfo.List.Add(new PathInfo()
                    {
                        Path = fileName,
                        LastModified = lastModified
                    });
                }

                var folderNames = Directory.GetDirectories(monitorPath, "*", SearchOption.AllDirectories);
                foreach (var folderName in folderNames)
                {
                    var lastModified = File.GetLastWriteTime(folderName);
                    filesInfo.List.Add(new PathInfo()
                    {
                        Path = folderName,
                        LastModified = lastModified
                    });
                }

                return filesInfo;
            }
            catch (Exception ex)
            {
                _logger.AddError($"Backuper >>> GetFilesInfo:", ex);
            }

            return null;
        }
        #endregion
    }
}