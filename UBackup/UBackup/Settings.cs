using System.Collections.Generic;

namespace UBackup
{
    public class Settings
    {
        public string BackupPath { get; private set; }
        public int BackupFrequencyInSeconds { get; private set; }
        public int MaxBackupFiles { get; set; }
        public List<string> MonitorPaths { get; private set; }

        public Settings(string backupPath, int backupFrequencyInSeconds, int maxBackupFiles, List<string> monitorPaths)
        {
            BackupPath = backupPath;
            BackupFrequencyInSeconds = backupFrequencyInSeconds;
            MaxBackupFiles = maxBackupFiles;
            MonitorPaths = monitorPaths;
        }
    }
}
