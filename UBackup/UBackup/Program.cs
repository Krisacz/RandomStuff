using System;
using Topshelf;

namespace UBackup
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower().Equals("console"))
            {
                var consoleLogger = new ConsoleLogger();
                var settings = SettingsReader.Read(consoleLogger);
                var backuper = new Backuper(consoleLogger, settings);
                var backupTimer = new BackupTimer(backuper, consoleLogger, settings);
                backupTimer.Start();
                Console.ReadKey();
            }
            else
            {
                HostFactory.Run(x =>
                {
                    x.Service<WinServiceAppController>(s =>
                    {
                        s.ConstructUsing(name => new WinServiceAppController());
                        s.WhenStarted(tc => tc.Start());
                        s.WhenStopped(tc => tc.Stop());
                    });
                    x.RunAsLocalService();
                    x.SetDescription("UBackup service.");
                    x.SetDisplayName("UBackup");
                    x.SetServiceName("UBackup");
                });
            }
        }
    }
}
