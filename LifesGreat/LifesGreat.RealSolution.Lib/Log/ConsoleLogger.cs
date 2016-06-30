using System;

namespace LifesGreat.RealSolution.Lib.Log
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void AddError(string error, Exception exception)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var txt = $"{dt} [ERROR]\t";
            Console.Write(txt);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{error} {exception}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void AddInfo(string info)
        {
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.Write($"{dt} [INFO] \t");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{info}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void AddEmptyLine()
        {
            Console.WriteLine();
        }
    }
}