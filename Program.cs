using Raid_AFK_Manager.Tools;
using System;
using System.Configuration;
using System.IO;

namespace Raid_AFK_Manager
{
    class Program
    {
        private static readonly string _version = "0.0.2";
        private static string _plariumExePath = ConfigurationManager.AppSettings["PlariumExePath"];
        private static string _plariumExeArgs = ConfigurationManager.AppSettings["PlariumExeArguments"];
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Hello and welcome to RAID AFK MANAGER v{_version}");
            try
            {
                CheckConfigFile();
                RaidManager raid = new RaidManager();
                raid.DoManagement(_plariumExePath, _plariumExeArgs);
                ConsoleTool.CountDown("Closing app in {0}", 30);
            }
            catch (Exception e)
            {
                ConsoleTool.WriteLineError("Unexpected error detected. This is bad...");
                ConsoleTool.WriteLineError(e);
            }
        }

        private static void CheckConfigFile()
        {
            if (string.IsNullOrEmpty(_plariumExePath))
                ConsoleTool.WriteLineWarning("Path to the plarium app is empty.\nPlease check the config file if it is unintentional.");
            else if (!File.Exists(_plariumExePath))
            {
                ConsoleTool.WriteLineWarning($"The file \"{_plariumExePath}\" don't exist.");
                _plariumExePath = string.Empty;
            }

        }
    }
}
