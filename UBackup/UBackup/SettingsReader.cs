using System;
using System.Collections.Generic;
using System.Configuration;

namespace UBackup
{
    public static class SettingsReader
    {
        public static Settings Read(ILogger logger)
        {
            try
            {
                logger.AddInfo("SettingsReader >>> Read: Reading config file...");

                var backupPath = ConfigurationManager.AppSettings["BackupPath"];
                logger.AddInfo($"SettingsReader >>> Read: SqlConnectionString: {backupPath}");

                var backupFrequencyInSecondsStr = ConfigurationManager.AppSettings["BackupFrequencyInSeconds"];
                var backupFrequencyInSeconds = int.Parse(backupFrequencyInSecondsStr);
                logger.AddInfo($"SettingsReader >>> Read: BackupFrequencyInSeconds: {backupFrequencyInSeconds}");

                var maxBackupFilesStr = ConfigurationManager.AppSettings["MaxBackupFiles"];
                var maxBackupFiles = int.Parse(maxBackupFilesStr);
                logger.AddInfo($"SettingsReader >>> Read: MaxBackupFiles: {maxBackupFiles}");

                var monitorPaths = GetMonitorPaths(logger);
                logger.AddInfo($"SettingsReader >>> Read: MonitorPaths found: {monitorPaths.Count}");

                return new Settings(backupPath, backupFrequencyInSeconds, maxBackupFiles, monitorPaths);
            }
            catch (Exception ex)
            {
                logger.AddError("SettingsReader >>> Read:", ex);
            }

            return null;
        }


        private static List<string> GetMonitorPaths(ILogger logger)
        {
            var monitorPaths = new List<string>();

            try
            {
                var exit = false;
                var counter = 1;
                while (!exit)
                {
                    var settingName = $"MonitorPath{counter}";
                    var monitorPath = ConfigurationManager.AppSettings[settingName];
                    if (monitorPath != null)
                    {
                        monitorPaths.Add(monitorPath);
                        counter++;
                    }
                    else
                    {
                        exit = true;
                    }
                }
                return monitorPaths;
            }
            catch (Exception ex)
            {
                logger.AddError("SettingsReader >>> GetMonitorPaths:", ex);
            }

            return monitorPaths;
        }
    }
}
