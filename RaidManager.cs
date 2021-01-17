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


        private int _raidProcessId = 0;
        private readonly int _mainProcessId = Process.GetCurrentProcess().Id;

        private const string _raidWindowTitle = "Raid: Shadow Legends";

        internal void DoManagement(string exePath, string exeArgs)
        {
            int compteur = 0;
            WindowHandler.RepositionMainWindow(_mainProcessId);
            if (CheckRaidApp(exePath, exeArgs))
            {
                WindowHandler.RepositionRaidWindow(_raidProcessId);
                RaidChecker raid = new RaidChecker(_raidProcessId);
                int nbLoopArcane = RaidOptions.ArcaneLoopNumber;
                int nbLoopDurham = RaidOptions.DurhamLoopNumber;
                bool infiniteArcaneBattle = (nbLoopArcane == 0);
                bool infiniteDurhamBattle = (nbLoopDurham == 0);

                while (raid.ShowBastion())
                {
                    compteur++;
                    if (RaidOptions.CheckMineAllowed) raid.CheckMine();
                    raid.ShowBastion();
                    if (RaidOptions.CheckRewards) raid.CheckPlaytimeRewards();
                    raid.ShowBastion();
                    if (RaidOptions.CheckPitAllowed) raid.CheckThePit();
                    raid.ShowBastion();
                    if ((RaidOptions.ArcaneDungeonAllowed) && ((nbLoopArcane > 0 || infiniteArcaneBattle))) nbLoopArcane = raid.RunArcaneDungeon(nbLoopArcane);
                    raid.ShowBastion();
                    if ((RaidOptions.DurhamForestAllowed) && ((nbLoopDurham > 0 || infiniteDurhamBattle))) nbLoopDurham = raid.RunDurhamForest(nbLoopDurham);
                    raid.ShowBastion();
                    ConsoleWriter.CountDown($"** End of loop n°{compteur}. Next loop start in {{0}} **", 320);
                    Console.WriteLine("_______________________________");
                }
            }
        }

        private bool CheckRaidApp(string exePath, string exeArgs)
        {
            bool stepSuccess = false;
            if (IsRaidRunning())
            {
                Console.WriteLine("\nRaid has been detected.");
                Thread.Sleep(2000);
                stepSuccess = true;

            }
            else if (!string.IsNullOrEmpty(exePath))
            {
                Console.WriteLine("\nRaid processus not found.\nTrying to launch raid....");
                if (LaunchRaid(exePath, exeArgs)) stepSuccess = true;
            }

            return stepSuccess;
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
