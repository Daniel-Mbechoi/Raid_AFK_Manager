using Raid_AFK_Manager.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raid_AFK_Manager
{
    internal static class RaidOptions
    {
        internal static bool GlobalBattleAllowed { get; private set; }
        internal static bool GlobalCheckAllowed { get; private set; }
        internal static bool UseGemsAllowed { get; private set; }
        internal static bool SavePicturesAllowed { get; private set; }

        private static bool _checkMineAllowed;
        private static bool _checkPitAllowed;
        private static bool _checkRewardsAllowed;
        private static bool _checkMarketAllowed;
        private static bool _arcaneKeepAllowed;
        private static bool _durhamForestAllowed;
        private static bool _forceKeepAllowed;
        private static bool _magicKeepAllowed;
        private static bool _voidKeepAllowed;
        private static bool _spiritKeepAllowed;
        private static bool _iceGolemAllowed;
        private static bool _minotaurAllowed;
        private static bool _godfreyAllowed;

        internal static bool CheckMineAllowed
        {
            get { return _checkMineAllowed; }
            private set { _checkMineAllowed = value; CheckGlobal(); }
        }

        internal static bool CheckPitAllowed
        {
            get { return _checkPitAllowed; }
            private set { _checkPitAllowed = value; CheckGlobal(); }
        }

        internal static bool CheckRewardsAllowed
        {
            get { return _checkRewardsAllowed; }
            private set { _checkRewardsAllowed = value; CheckGlobal(); }
        }

        internal static bool CheckMarketAllowed
        {
            get { return _checkMarketAllowed; }
            private set { _checkMarketAllowed = value; CheckGlobal(); }
        }

        internal static bool ArcaneKeepAllowed
        {
            get { return _arcaneKeepAllowed; }
            set { _arcaneKeepAllowed = value; CheckGlobal(); }
        }

        internal static bool ForceKeepAllowed
        {
            get { return _forceKeepAllowed; }
            set { _forceKeepAllowed = value; CheckGlobal(); }
        }

        internal static bool MagicKeepAllowed
        {
            get { return _magicKeepAllowed; }
            set { _magicKeepAllowed = value; CheckGlobal(); }
        }

        internal static bool VoidKeepAllowed
        {
            get { return _voidKeepAllowed; }
            set { _voidKeepAllowed = value; CheckGlobal(); }
        }

        internal static bool SpiritKeepAllowed
        {
            get { return _spiritKeepAllowed; }
            set { _spiritKeepAllowed = value; CheckGlobal(); }
        }

        internal static bool DurhamForestAllowed
        {
            get { return _durhamForestAllowed; }
            set { _durhamForestAllowed = value; CheckGlobal(); }
        }

        internal static bool IceGolemAllowed
        {
            get { return _iceGolemAllowed; }
            set { _iceGolemAllowed = value; CheckGlobal(); }
        }

        internal static bool MinotaurAllowed
        {
            get { return _minotaurAllowed; }
            set { _minotaurAllowed = value; CheckGlobal(); }
        }

        internal static bool GodfreyAllowed
        {
            get { return _godfreyAllowed; }
            set { _godfreyAllowed = value; CheckGlobal(); }
        }

        private static int _loopNumber = 0;
        private static int _arcaneLoopNumber = 0;
        private static int _forceLoopNumber = 0;
        private static int _magicLoopNumber = 0;
        private static int _voidLoopNumber = 0;
        private static int _spiritLoopNumber = 0;
        private static int _durhamLoopNumber = 0;
        private static int _iceGolemLoopNumber = 0;
        private static int _minotaurLoopNumber = 0;
        private static int _godfreyLoopNumber = 0;

        internal static int LoopNumber { get => _loopNumber; set => _loopNumber = value; }
        internal static int ArcaneLoopNumber { get => _arcaneLoopNumber; set => _arcaneLoopNumber = value; }
        internal static int DurhamLoopNumber { get => _durhamLoopNumber; set => _durhamLoopNumber = value; }
        internal static int ForceLoopNumber { get => _forceLoopNumber; set => _forceLoopNumber = value; }
        internal static int MagicLoopNumber { get => _magicLoopNumber; set => _magicLoopNumber = value; }
        internal static int VoidLoopNumber { get => _voidLoopNumber; set => _voidLoopNumber = value; }
        internal static int SpiritLoopNumber { get => _spiritLoopNumber; set => _spiritLoopNumber = value; }
        internal static int IceGolemPeakLoopNumber { get => _iceGolemLoopNumber; set => _iceGolemLoopNumber = value; }
        internal static int MinotaurLoopNumber { get => _minotaurLoopNumber; set => _minotaurLoopNumber = value; }
        internal static int GodfreyLoopNumber { get => _godfreyLoopNumber; set => _godfreyLoopNumber = value; }


        internal static void ReadArguments(string[] args)
        {
            if (args.Length > 0)
            {
                List<string> listeArgs = args.ToList(); //Parce que les listes ont plus de classe que les tableaux !

                if (listeArgs.Exists(x => x.ToUpper().Trim() == "HELP" || x.ToUpper().Trim() == "H"))
                {
                    DiplayHelp();
                    return;
                }

                CheckMineAllowed = listeArgs.Exists(x => x.ToUpper().Trim() == "CHECKMINE" || x.ToUpper().Trim() == "M");
                CheckPitAllowed = listeArgs.Exists(x => x.ToUpper().Trim() == "CHECKPIT" || x.ToUpper().Trim() == "P");
                CheckRewardsAllowed = listeArgs.Exists(x => x.ToUpper().Trim() == "CHECKREWARDS" || x.ToUpper().Trim() == "R");
                CheckMarketAllowed = listeArgs.Exists(x => x.ToUpper().Trim() == "CHECKMARKET" || x.ToUpper().Trim() == "K");
                SavePicturesAllowed = listeArgs.Exists(x => x.ToUpper().Trim() == "SAVE");

                var argLoop = listeArgs.Find(x => x.ToUpper().Trim().StartsWith("LOOP:") || x.ToUpper().Trim().StartsWith("L:"));
                if (argLoop != null)
                {
                    var argNb = argLoop.Split(':');
                    if (argNb.Count() == 2) //c'est le seul cas autorisé
                    {
                        int.TryParse(argNb[1].Trim(), out _loopNumber);
                    }
                }

                LireArgumentsBattle(listeArgs, "ARCANEKEEP", "AK", ref _arcaneKeepAllowed, ref _arcaneLoopNumber);
                LireArgumentsBattle(listeArgs, "VOIDKEEP", "VK", ref _voidKeepAllowed, ref _voidLoopNumber);
                LireArgumentsBattle(listeArgs, "MAGICKEEP", "MK", ref _magicKeepAllowed, ref _magicLoopNumber);
                LireArgumentsBattle(listeArgs, "SPIRITKEEP", "SK", ref _spiritKeepAllowed, ref _spiritLoopNumber);
                LireArgumentsBattle(listeArgs, "FORCEKEEP", "FK", ref _forceKeepAllowed, ref _forceLoopNumber);
                LireArgumentsBattle(listeArgs, "DURHAMFOREST", "DF", ref _durhamForestAllowed, ref _durhamLoopNumber);
                LireArgumentsBattle(listeArgs, "ICECOLEMPEAK", "IGP", ref _iceGolemAllowed, ref _iceGolemLoopNumber);
                LireArgumentsBattle(listeArgs, "MINOTAURLABYRINTH", "ML", ref _minotaurAllowed, ref _minotaurLoopNumber);
                LireArgumentsBattle(listeArgs, "GODFREYSCROSSING", "GC", ref _godfreyAllowed, ref _godfreyLoopNumber);
                CheckGlobal();
                
                if(GlobalBattleAllowed)
                {
                    UseGemsAllowed = listeArgs.Exists(x => x.ToUpper().Trim() == "USEGEMS" || x.ToUpper().Trim() == "U");
                }
            }
        }

        private static void DiplayHelp()
        {
            Console.WriteLine("\t ------- * -------");
            Console.WriteLine("\t RAM HELP DISPLAY");
            Console.WriteLine("\t ------- * -------");
            Thread.Sleep(500);
            Console.WriteLine("\n                --- CHECK OPTIONS ---\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[CHECKMINE]|[M]      Will allow RAM to check the mine every loops.");
            Console.WriteLine("                     Disabled by default.");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[CHECKPIT]|[P]       Will allow RAM to check the sparring pit");
            Console.WriteLine("                     every loops. Disabled by default.");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[CHECKREWARDS]|[R]   Will allow RAM to check the daily playtime");
            Console.WriteLine("                     rewards AND the daily login rewards every loops.");
            Console.WriteLine("                     Disabled by default.");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[CHECKMARKET]|[K]    Will allow RAM to check the market every loops.");
            Console.WriteLine("                     Disabled by default.");
            Console.WriteLine("\n                --- BATTLE OPTIONS ---\n");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[ARCANEDUNGEON]|[AK]<:nb of battle>");
            Console.WriteLine("   Will allow RAM to run a battle at stage 810 of the arcane dungeon for a ");
            Console.WriteLine("   number of battle.If the number is not given(or zero is given), it will");
            Console.WriteLine("   run forever.");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[VOIDKEEP]|[VK]<:nb of battle>");
            Console.WriteLine("   Will allow RAM to run a battle at stage 10 of the void keep for a ");
            Console.WriteLine("   number of battle.If the number is not given(or zero is given), it will");
            Console.WriteLine("   run forever.");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[MAGICKEEP]|[MK]<:nb of battle>");
            Console.WriteLine("   Will allow RAM to run a battle at stage 10 of the magic keep for a ");
            Console.WriteLine("   number of battle.If the number is not given(or zero is given), it will");
            Console.WriteLine("   run forever.");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[SPIRITKEEP]|[SK]<:nb_of_battle>");
            Console.WriteLine("   Will allow RAM to run a battle at stage 10 of the spirit keep for a ");
            Console.WriteLine("   number of battle.If the number is not given(or zero is given), it will");
            Console.WriteLine("   run forever.");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[FORCEKEEP]|[FK]<:nb_of_battle>");
            Console.WriteLine("   Will allow RAM to run a battle at stage 10 of the force keep for a ");
            Console.WriteLine("   number of battle.If the number is not given(or zero is given), it will");
            Console.WriteLine("   run forever.");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[DURHAMFOREST]|[DF]<:nb_of_battle>");
            Console.WriteLine("   Will allow RAM to run a battle at stage 1 of the Durham Forest campaign");
            Console.WriteLine("   for a number of battle.If the number is not given(or zero is given),");
            Console.WriteLine("   it will run forever.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[ICECOLEMPEAK]|[IGP]<:nb_of_battle>");
            Console.WriteLine("   Will allow RAM to run a battle at stage 10 of the Ice golem peak");
            Console.WriteLine("   for a number of battle.If the number is not given(or zero is given),");
            Console.WriteLine("   it will run forever.");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[MINOTAURSLABYRINTH]|[ML]<:nb_of_battle>");
            Console.WriteLine("   Will allow RAM to run a battle at stage 10 of the minotaur's labyrinth");
            Console.WriteLine("   for a number of battle.If the number is not given(or zero is given),");
            Console.WriteLine("   it will run forever.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[GODFREYSCROSSING]|[GC]<:nb_of_battle>");
            Console.WriteLine("   Will allow RAM to run a battle at stage 1 of the godfrey's crossing");
            Console.WriteLine("   for a number of battle.If the number is not given(or zero is given),");
            Console.WriteLine("   it will run forever.");

            Console.WriteLine("\nNOTE : If a battle loop last more that 20 minutes,");
            Console.WriteLine("       it will be paused, then resume later.");
            Console.WriteLine("\n                --- MISC. ---\n");
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[USEGEMS]|[U]        Will allow RAM to use gems if low energy during");
            Console.WriteLine("                     battles.");
            Console.WriteLine("                     Disabled by default.");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[LOOP]|[L]:nb_of_total_loops");
            Console.WriteLine("  Will set the max number of total loop.");
            Console.WriteLine("  NOTE : Will continue if nb battle still available and not infinite.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("[SAVE]");
            Console.WriteLine("  Will save the pictures scanned in '.\\saves' folder.");
            Thread.Sleep(2000);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void LireArgumentsBattle(List<string> listeArgs, string argLong, string argCourt, ref bool isAllowed, ref int loopNb)
        {
            var argLoop = listeArgs.Find(x => x.ToUpper().Trim().StartsWith(argLong) || x.ToUpper().Trim().StartsWith(argCourt));
            if (argLoop != null)
            {
                isAllowed = true;
                var argNb = argLoop.Split(':');
                if (argNb.Count() == 2) //c'est le seul cas autorisé
                {
                    int.TryParse(argNb[1].Trim(), out loopNb);
                }
            }
        }

        internal static void AfficherRoutine()
        {
            Console.WriteLine("RAM's routine plan :");
            if (CheckRewardsAllowed) Console.WriteLine("=> The daily rewards will be checked automatically");
            if (CheckMineAllowed) Console.WriteLine("=> The mine will be checked automatically");
            if (CheckPitAllowed) Console.WriteLine("=> The pit will be checked automatically");
            if (CheckMarketAllowed) Console.WriteLine("=> The market will be checked automatically");
            if (SavePicturesAllowed) Console.WriteLine("=> Pictures will be saved automatically");

            AfficherRouteBattle(ArcaneKeepAllowed, ArcaneLoopNumber, "arcane keep");
            AfficherRouteBattle(DurhamForestAllowed, DurhamLoopNumber, "durham forest campaign (Stage 1)");
            AfficherRouteBattle(ForceKeepAllowed, ForceLoopNumber, "force keep");
            AfficherRouteBattle(MagicKeepAllowed, MagicLoopNumber, "magic keep");
            AfficherRouteBattle(SpiritKeepAllowed, SpiritLoopNumber, "spirit keep");
            AfficherRouteBattle(VoidKeepAllowed, VoidLoopNumber, "void keep");
            AfficherRouteBattle(IceGolemAllowed, IceGolemPeakLoopNumber, "ice golem peak");
            AfficherRouteBattle(MinotaurAllowed, MinotaurLoopNumber, "minotaur's labyrinth");
            AfficherRouteBattle(GodfreyAllowed, GodfreyLoopNumber, "godfrey's crossing");

            if (GlobalBattleAllowed && UseGemsAllowed)
            {
                Console.WriteLine("=> CAUTION : GEMS WILL BE USED IF LOW ENERGY !");
            }

            if (GlobalCheckAllowed 
                || ((DurhamForestAllowed) && (DurhamLoopNumber == 0))
                || ((ArcaneKeepAllowed) && (ArcaneLoopNumber == 0))
                || ((MagicKeepAllowed) && (MagicLoopNumber == 0))
                || ((VoidKeepAllowed) && (VoidLoopNumber == 0))
                || ((IceGolemAllowed) && (IceGolemPeakLoopNumber == 0))
                || ((MinotaurAllowed) && (MinotaurLoopNumber == 0))
                || ((GodfreyAllowed) && (GodfreyLoopNumber == 0))
                || ((ForceKeepAllowed) && (ForceLoopNumber == 0)))
            {
                if (LoopNumber == 0) Console.WriteLine("=> This routine will loop forever");
                if (LoopNumber > 0) Console.WriteLine($"=> This routine will loop {LoopNumber} time(s) and then stop"); 
            }
            if (!GlobalBattleAllowed && !GlobalCheckAllowed)
            {
                Console.WriteLine("No instructions found. Closing...");
            }
            else
            {
                Console.WriteLine("\nUse (Ctrl+C) to cancel the launch.");
#if !DEBUG
                ConsoleWriter.CountDown("Lanching in ... {0}", 30);
#endif
            }

        }

        private static void AfficherRouteBattle(bool isAllowed, int loopNb, string title)
        {
            if (isAllowed) Console.WriteLine($"=> The {title} battle will be launched automatically");
            if (isAllowed && (loopNb > 0)) Console.WriteLine($"=> The number of battle at the {title} will be limited to {loopNb}");
            if (isAllowed && (loopNb == 0)) Console.WriteLine($"=> The number of battle at the {title} will be unlimited");
        }

        private static void CheckGlobal()
        {
            if (
                DurhamForestAllowed 
                || ForceKeepAllowed 
                || ArcaneKeepAllowed 
                || MagicKeepAllowed
                || VoidKeepAllowed
                || SpiritKeepAllowed
                || IceGolemAllowed
                || MinotaurAllowed
                || GodfreyAllowed
                )
            {
                GlobalBattleAllowed = true;
            }
            else
            {
                GlobalBattleAllowed = false;
            }

            if (CheckMineAllowed || CheckPitAllowed || CheckRewardsAllowed || CheckMarketAllowed)
            {
                GlobalCheckAllowed = true;
            }
            else
            {
                GlobalCheckAllowed = false;
            }
        }
    }
}
