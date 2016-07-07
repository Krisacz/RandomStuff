using System;
using System.Runtime.InteropServices;

namespace RL
{
    class Program
    {
        const int SwpNosize = 0x0001;
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static readonly IntPtr MyConsole = GetConsoleWindow();
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        
        static void Main(string[] args)
        {
            SetConsoleWindosProperties();

            var levelGenerator = new LevelGenerator();
            levelGenerator.Generate();
            do
            {
                while (!Console.KeyAvailable)
                {
                    //Console.Write("!");
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        private static void SetConsoleWindosProperties()
        {
            var xpos = 0;
            var ypos = 0;
            SetWindowPos(MyConsole, 0, xpos, ypos, 0, 0, SwpNosize);
            Console.WindowHeight = 70;
            Console.WindowWidth = 201;
        }
    }

    public class LevelGenerator
    {
        public void Generate()
        {
            const int maxRoomRows = 4;
            const int maxRoomColumns = 8;
            const int maxRows = 15;
            const int maxColumns = 25;

            for (var roomRow = 1; roomRow <= maxRoomRows; roomRow++)
            {
                for (var roomColumn = 1; roomColumn <= maxRoomColumns; roomColumn++)
                {
                    var yOffset = (roomRow - 1) * maxRows;
                    var xOffset = (roomColumn - 1) * maxColumns;
                    DrawRoom(maxRows, maxColumns, xOffset, yOffset);
                }
            }
            Console.SetCursorPosition(0, 0);
        }

        private static void DrawRoom(int maxY, int maxX, int xOffset, int yOffset)
        {
            for (var y = 1; y <= maxY; y++)
            {
                for (var x = 1; x <= maxX; x++)
                {
                    Console.SetCursorPosition(x + xOffset - 1, y + yOffset -1);
                    if (x == 1 || x == maxX || y == 1 || y == maxY)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
            }
        }
    }
}
