namespace UBackup
{
    public class WinServiceAppController
    {
        private BackupTimer _backupTimer;

        public WinServiceAppController()
        {

        }

        public void Start()
        {
            Init();
            _backupTimer.Start();
        }

        public void Stop()
        {
            _backupTimer.Stop();
        }

        private void Init()
        {
            var logger = new FileLogger() {EnableDetailedLog = false};
            var settings = SettingsReader.Read(logger);
            var backuper = new Backuper(logger, settings);
            _backupTimer = new BackupTimer(backuper, logger, settings);
        }
    }
}