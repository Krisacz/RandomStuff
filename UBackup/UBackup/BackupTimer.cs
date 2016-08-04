using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace UBackup
{
    public class BackupTimer
    {
        private readonly Backuper _backuper;
        private readonly ILogger _logger;
        private readonly Timer _timer;

        public BackupTimer(Backuper backuper, ILogger logger, Settings settings)
        {
            _backuper = backuper;
            _logger = logger;

            _timer = new Timer();
            _timer.Elapsed += Execute;
            _timer.Interval = settings.BackupFrequencyInSeconds * 1000;
        }

        #region START
        public void Start()
        {
            try
            {
                _logger.AddInfo("BackupTimer >>> Started.");
                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                _logger.AddError($"BackupTimer >>> Start:", ex);
            }
        }
        #endregion

        #region STOP
        public void Stop()
        {
            try
            {
                _logger.AddInfo("BackupTimer >>> Stopped.");
                _timer.Enabled = false;
            }
            catch (Exception ex)
            {
                _logger.AddError($"BackupTimer >>> Stop:", ex);
            }
        }
        #endregion

        #region EXECUTE
        private void Execute(object sender, ElapsedEventArgs e)
        {
            Execute();
        }

        public void Execute()
        {
            try
            {
                _logger.AddInfo("BackupTimer >>> Execute: Starting...");

                _timer.Enabled = false;
                _backuper.BackupMonitorPaths();

                _logger.AddInfo("BackupTimer >>> Execute: Finished!");
                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                _logger.AddError($"TimerExecuter >>> Execute:", ex);
            }
            finally
            {
                _timer.Enabled = true;
            }
        }
        #endregion
    }
}