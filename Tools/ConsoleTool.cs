using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raid_AFK_Manager.Tools
{
    internal static class ConsoleTool
    {
        internal static void WriteLineError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        internal static void WriteLineError(Exception exception)
        {
            WriteLineError($"ERROR : {exception.Message}\nDETAIL : {exception}");
        }

        internal static void WriteLineWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"WARNING: {message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        internal static void WriteLineInformation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        internal static void CountDown(string message, int compteur)
        {
            for (int i = compteur; i >= 0; i--)
            {
                Console.Write($"\r{message}   ", i);
                Thread.Sleep(1000);
            }
        }
    }
}
