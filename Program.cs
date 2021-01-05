using Raid_AFK_Manager.Tools;
using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace Raid_AFK_Manager
{
    class Program
    {
        private static readonly string _version = "0.0.7";
        private static string _plariumExePath = ConfigurationManager.AppSettings["PlariumExePath"];
        private static string _plariumExeArgs = ConfigurationManager.AppSettings["PlariumExeArguments"];
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Hello and welcome to ");
            ConsoleWriter.WriteLineInformation($"RAID AFK MANAGER v{_version} !");
            Thread.Sleep(800);
            Console.WriteLine("This application use terreract-OCR by Charles Weld\n(more info @ https://github.com/charlesw/tesseract)");
            Console.WriteLine("_______________________________");
            Thread.Sleep(800);
            try
            {
                CheckConfigFile();
                RaidManager raid = new RaidManager();
                raid.DoManagement(_plariumExePath, _plariumExeArgs);
            }
            catch (Exception e)
            {
                ConsoleWriter.WriteLineError("Unexpected error detected. This is bad...");
                ConsoleWriter.WriteLineError(e);
                Console.WriteLine("Push any key to close the app");
                Console.ReadKey();
            }
        }

        private static void CheckConfigFile()
        {
            if (string.IsNullOrEmpty(_plariumExePath))
                ConsoleWriter.WriteLineWarning("Path to the plarium app is empty.\nPlease check the config file if it is unintentional.");
            else if (!File.Exists(_plariumExePath))
            {
                ConsoleWriter.WriteLineWarning($"The file \"{_plariumExePath}\" don't exist.");
                _plariumExePath = string.Empty;
            }

        }
    }
}
