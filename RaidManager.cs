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
            if (!RaidOptions.GlobalBattleAllowed && !RaidOptions.GlobalCheckAllowed) return;

            int compteur = 0;
            WindowHandler.RepositionMainWindow(_mainProcessId);
            if (CheckRaidApp(exePath, exeArgs))
            {
                WindowHandler.RepositionRaidWindow(_raidProcessId);
                RaidChecker raid = new RaidChecker(_raidProcessId);
                int nbLoopArcane = RaidOptions.ArcaneLoopNumber;
                int nbLoopDurham = RaidOptions.DurhamLoopNumber;
                int nbLoopForce = RaidOptions.ForceLoopNumber;
                int nbLoopMagic = RaidOptions.MagicLoopNumber;
                int nbLoopVoid = RaidOptions.VoidLoopNumber;
                int nbLoopSpirit = RaidOptions.SpiritLoopNumber;
                int nbLoopIceGolemPeak = RaidOptions.IceGolemPeakLoopNumber;
                int nbLoopMinotaur = RaidOptions.MinotaurLoopNumber;
                int nbLoopGodfrey = RaidOptions.GodfreyLoopNumber;
                bool infiniteArcaneBattle = (nbLoopArcane == 0);
                bool infiniteForceBattle = (nbLoopForce == 0);
                bool infiniteDurhamBattle = (nbLoopDurham == 0);
                bool infiniteMagicBattle = (nbLoopMagic == 0);
                bool infiniteVoidBattle = (nbLoopVoid == 0);
                bool infiniteSpiritBattle = (nbLoopSpirit == 0);
                bool infiniteIceGolemPeakBattle = (nbLoopIceGolemPeak == 0);
                bool infiniteMinotaurBattle = (nbLoopMinotaur == 0);
                bool infiniteGodfreyBattle = (nbLoopGodfrey == 0);
                bool infiniteLoop = (RaidOptions.LoopNumber == 0);

                while (raid.ShowBastion())
                {
                    compteur++;
                    if (RaidOptions.CheckRewardsAllowed)
                    {
                        raid.CheckPlaytimeRewards();
                        raid.ShowBastion();
                    }

                    if (RaidOptions.CheckMineAllowed)
                    {
                        raid.CheckMine();
                        raid.ShowBastion();
                    }

                    if (RaidOptions.CheckPitAllowed)
                    {
                        raid.CheckThePit();
                        raid.ShowBastion();
                    }

                    if (RaidOptions.CheckMarketAllowed)
                    {
                        raid.CheckMarket();
                        raid.ShowBastion();
                    }

                    if ((RaidOptions.ArcaneKeepAllowed) && ((nbLoopArcane > 0 || infiniteArcaneBattle)))
                    {
                        nbLoopArcane = raid.RunArcaneKeep(nbLoopArcane);
                        if (!infiniteArcaneBattle && nbLoopArcane == 0) RaidOptions.ArcaneKeepAllowed = false;
                        raid.ShowBastion();
                    }

                    if ((RaidOptions.ForceKeepAllowed) && ((nbLoopForce > 0 || infiniteForceBattle)))
                    {
                        nbLoopForce = raid.RunForceKeep(nbLoopForce);
                        if (!infiniteForceBattle && nbLoopForce == 0) RaidOptions.ForceKeepAllowed = false;
                        raid.ShowBastion();
                    }

                    if ((RaidOptions.MagicKeepAllowed) && ((nbLoopMagic > 0 || infiniteMagicBattle)))
                    {
                        nbLoopMagic = raid.RunMagicKeep(nbLoopMagic);
                        if (!infiniteMagicBattle && nbLoopMagic == 0) RaidOptions.MagicKeepAllowed = false;
                        raid.ShowBastion();
                    }

                    if ((RaidOptions.SpiritKeepAllowed) && ((nbLoopSpirit > 0 || infiniteSpiritBattle)))
                    {
                        nbLoopSpirit = raid.RunSpiritKeep(nbLoopSpirit);
                        if (!infiniteSpiritBattle && nbLoopSpirit == 0) RaidOptions.SpiritKeepAllowed = false;
                        raid.ShowBastion();
                    }

                    if ((RaidOptions.VoidKeepAllowed) && ((nbLoopVoid > 0 || infiniteVoidBattle)))
                    {
                        nbLoopVoid = raid.RunVoidKeep(nbLoopVoid);
                        if (!infiniteVoidBattle && nbLoopVoid == 0) RaidOptions.VoidKeepAllowed = false;
                        raid.ShowBastion();
                    }

                    if ((RaidOptions.DurhamForestAllowed) && ((nbLoopDurham > 0 || infiniteDurhamBattle)))
                    {
                        nbLoopDurham = raid.RunDurhamForest(nbLoopDurham);
                        if (!infiniteDurhamBattle && nbLoopDurham == 0) RaidOptions.DurhamForestAllowed = false;
                        raid.ShowBastion();
                    }

                    if ((RaidOptions.IceGolemAllowed) && ((nbLoopIceGolemPeak > 0 || infiniteIceGolemPeakBattle)))
                    {
                        nbLoopIceGolemPeak = raid.RunIceGolemPeak(nbLoopIceGolemPeak);
                        if (!infiniteIceGolemPeakBattle && nbLoopIceGolemPeak == 0) RaidOptions.IceGolemAllowed = false;
                        raid.ShowBastion();
                    }

                    if ((RaidOptions.MinotaurAllowed) && ((nbLoopMinotaur > 0 || infiniteMinotaurBattle)))
                    {
                        nbLoopMinotaur = raid.RunMinotaurLabyrinth(nbLoopMinotaur);
                        if (!infiniteMinotaurBattle && nbLoopMinotaur == 0) RaidOptions.MinotaurAllowed = false;
                        raid.ShowBastion();
                    }

                    if ((RaidOptions.GodfreyAllowed) && ((nbLoopGodfrey > 0 || infiniteGodfreyBattle)))
                    {
                        nbLoopGodfrey = raid.RunGodfreyCrossing(nbLoopGodfrey);
                        if (!infiniteGodfreyBattle && nbLoopGodfrey == 0) RaidOptions.GodfreyAllowed = false;
                        raid.ShowBastion();
                    }

                    Console.Write($"\nEnd of loop n°{compteur}");
                    if (!infiniteLoop) Console.Write($"/{RaidOptions.LoopNumber}");
                    Console.WriteLine();
                    if (!RaidOptions.GlobalCheckAllowed && !RaidOptions.GlobalBattleAllowed || (!infiniteLoop && (compteur == RaidOptions.LoopNumber)))
                    {
                        Console.WriteLine("All actions finished...");
                        Thread.Sleep(1000);
                        Console.WriteLine("Exiting...");
                        Thread.Sleep(1000);
                        break;
                    }
                    ConsoleWriter.CountDown($"Next loop start in {{0}} ", 180);
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
