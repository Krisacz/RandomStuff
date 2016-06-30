using System;
using System.IO;

namespace LifesGreat.RealSolution.Lib.Log
{
    public class FileLogger : ILogger
    {
        private const string LogFile = @"log.txt";

        public FileLogger()
        {
            if (!File.Exists(LogFile)) File.Create(LogFile);
        }

        public void AddError(string error, Exception exception)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var txt = $"{dt} [ERROR]\t{error} ";
            if (exception == null)
            {
                txt += Environment.NewLine;
            }
            else
            {
                txt += $"Error Message: {exception.Message}";
                if (exception.InnerException != null && !string.IsNullOrWhiteSpace(exception.InnerException.ToString()))
                {
                    txt += $" InnerException: {exception.InnerException}{Environment.NewLine}";
                }
                else
                {
                    txt += Environment.NewLine;
                }
            }
            File.AppendAllText(LogFile, txt);
        }

        public void AddInfo(string info)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var txt = $"{dt} [INFO] \t{info}{Environment.NewLine}";
            File.AppendAllText(LogFile, txt);
        }
    }
}