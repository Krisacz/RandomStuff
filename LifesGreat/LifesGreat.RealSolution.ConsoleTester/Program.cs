using System;
using LifesGreat.RealSolution.Lib.Flow;
using LifesGreat.RealSolution.Lib.Log;

namespace LifesGreat.RealSolution.ConsoleTester
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var flowManager = new FlowManager(logger);

            var simpleStart = DateTime.Now;

            const string t1 = "";
            logger.AddInfo($"Input string:\t{t1}");
            logger.AddInfo($"Result string:\t{flowManager.Process(t1)}");
            logger.AddEmptyLine();

            const string t2 = "A";
            logger.AddInfo($"Input string:\t{t2}");
            logger.AddInfo($"Result string:\t{flowManager.Process(t2)}");
            logger.AddEmptyLine();

            const string t3 = "ABC";
            logger.AddInfo($"Input string:\t{t3}");
            logger.AddInfo($"Result string:\t{flowManager.Process(t3)}");
            logger.AddEmptyLine();

            const string t4 = "AB>CC";
            logger.AddInfo($"Input string:\t{t4}");
            logger.AddInfo($"Result string:\t{flowManager.Process(t4)}");
            logger.AddEmptyLine();

            const string t5 = "AB>CC>FD>AE>BF";
            logger.AddInfo($"Input string:\t{t5}");
            logger.AddInfo($"Result string:\t{flowManager.Process(t5)}");
            logger.AddEmptyLine();

            const string t6 = "ABC>C";
            logger.AddInfo($"Input string:\t{t6}");
            logger.AddInfo($"Result string:\t{flowManager.Process(t6)}");
            logger.AddEmptyLine();

            const string t7 = "AB>CC>FD>AEF>B";
            logger.AddInfo($"Input string:\t{t7}");
            logger.AddInfo($"Result string:\t{flowManager.Process(t7)}");
            logger.AddEmptyLine();

            var simpleEnd = DateTime.Now - simpleStart;
            Console.WriteLine($"Total Duration: {simpleEnd}");
            Console.ReadKey();
        }
    }
}
