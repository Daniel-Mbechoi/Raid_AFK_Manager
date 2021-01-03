using Raid_AFK_Manager.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raid_AFK_Manager
{
    internal class RaidManager
    {


        private const string _raidWindowTitle = "Raid: Shadow Legends";
        private int _raidProcessId = 0;
        private readonly int _mainProcessId = Process.GetCurrentProcess().Id;

        internal void DoManagement(string exePath, string exeArgs)
        {
            RepositionMainWindow();
            if (CheckRaidApp(exePath, exeArgs))
            {
                RepositionRaidWindow();
                ConsoleWriter.WriteLineInformation("No error detected\nSTEP 3 is a success. Congrats and see you soon for step 4.");
            }
        }

        private void RepositionRaidWindow()
        {
            WindowHandler.PositionWindow(_raidProcessId, 700, 0, 1235, 615);
        }

        private bool CheckRaidApp(string exePath, string exeArgs)
        {
            bool stepSuccess = false;
            if (IsRaidRunning())
            {
                Console.WriteLine("Raid app has been detected.");
                Thread.Sleep(2000);
                stepSuccess = true;

            }
            else if (!string.IsNullOrEmpty(exePath))
            {
                Console.WriteLine("Raid processus not found.\nTrying to launch raid....");
                if (LaunchRaid(exePath, exeArgs)) stepSuccess = true;
            }

            return stepSuccess;
        }

        private void RepositionMainWindow()
        {
            WindowHandler.PositionWindow(_mainProcessId, 0, 0, 720, 800);
        }

        private bool LaunchRaid(string exePath, string exeArgs)
        {
            bool success = false;
            try
            {
                Process.Start(exePath, exeArgs);
                ConsoleWriter.CountDown("Waiting for raid to finish its shenanigans...{0}", 50);
                if (IsRaidRunning())
                {
                    Console.WriteLine("\nRaid launched successfully !");
                    success = true;
                }
                else
                {
                    ConsoleWriter.WriteLineError($"Error while trying to launch ({exePath} {exeArgs}) : Cannot found procees Id");
                }
            }
            catch (Exception e)
            {
                ConsoleWriter.WriteLineError(e);
            }

            return success;
        }

        private bool IsRaidRunning()
        {
            Process[] pList = Process.GetProcessesByName("Raid");
            bool isRunning = false;

            foreach (Process p in pList)
            {
                if (p.MainWindowTitle == _raidWindowTitle)
                {
                    _raidProcessId = p.Id;
                    isRunning = true;
                    break;
                }
            }

            return isRunning;
        }


    }
}
