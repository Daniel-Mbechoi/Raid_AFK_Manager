using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raid_AFK_Manager
{
    internal static class RaidOptions
    {
        internal static bool CheckMineAllowed { get; private set; }
        internal static bool CheckPitAllowed { get; private set; }
        internal static bool CheckRewards { get; private set; }
        internal static bool ArcaneDungeonAllowed { get; set; }
        internal static bool DurhamForestAllowed { get; set; }
        internal static int LoopNumber { get => _loopNumber; set => _loopNumber = value; }
        internal static int ArcaneLoopNumber { get => _arcaneLoopNumber; set => _arcaneLoopNumber = value; }
        internal static int DurhamLoopNumber { get => _durhamLoopNumber; set => _durhamLoopNumber = value; }
        private static int _loopNumber = 0;
        private static int _arcaneLoopNumber = 0;
        private static int _durhamLoopNumber = 0;

        internal static void ReadArguments(string[] args)
        {
            if (args.Length > 0)
            {
                List<string> listeArgs = args.ToList(); //Parce que les listes ont plus de classe que les tableaux !
                CheckMineAllowed = listeArgs.Exists(x => x.ToUpper().Trim() == "CHECKMINE" || x.ToUpper().Trim() == "M");
                CheckPitAllowed = listeArgs.Exists(x => x.ToUpper().Trim() == "CHECKPIT" || x.ToUpper().Trim() == "P");
                CheckRewards = listeArgs.Exists(x => x.ToUpper().Trim() == "CHECKREWARDS" || x.ToUpper().Trim() == "R");
                var argLoop = listeArgs.Find(x => x.ToUpper().Trim().StartsWith("LOOP") || x.ToUpper().Trim().StartsWith("L:"));
                if (argLoop != null)
                {
                    var argNb = argLoop.Split(':');
                    if (argNb.Count() == 2) //c'est le seul cas autorisé
                    {
                        int.TryParse(argNb[1].Trim(), out _loopNumber);
                    }
                }

                var argLoopArcaneDungeon = listeArgs.Find(x => x.ToUpper().Trim().StartsWith("ARCANEDUNGEON:") || x.ToUpper().Trim().StartsWith("A:"));
                if (argLoopArcaneDungeon != null)
                {
                    ArcaneDungeonAllowed = true;
                    var argNbArcane = argLoopArcaneDungeon.Split(':');
                    if (argNbArcane.Count() == 2) //c'est le seul cas autorisé
                    {
                        int.TryParse(argNbArcane[1].Trim(), out _arcaneLoopNumber);
                    }
                }

                var argLoopDurhamForest = listeArgs.Find(x => x.ToUpper().Trim().StartsWith("DURHAMFOREST:") || x.ToUpper().Trim().StartsWith("F:"));
                if (argLoopDurhamForest != null)
                {
                    DurhamForestAllowed = true;
                    var argNbDurham = argLoopDurhamForest.Split(':');
                    if (argNbDurham.Count() == 2) //c'est le seul cas autorisé
                    {
                        int.TryParse(argNbDurham[1].Trim(), out _durhamLoopNumber);
                    }
                }
            }
        }

        internal static void AfficherRoutine()
        {
            Console.WriteLine("RAM's routine plan :");
            if (CheckMineAllowed) Console.WriteLine("=> The mine will be checked automatically");
            if (CheckPitAllowed) Console.WriteLine("=> The pit will be checked automatically");
            if (CheckRewards) Console.WriteLine("=> The daily rewards will be checked automatically");
            if (ArcaneDungeonAllowed) Console.WriteLine("=> The arcane dungeon battle will be launched automatically");
            if ((ArcaneDungeonAllowed) && (ArcaneLoopNumber > 0)) Console.WriteLine($"=> The number of battle at the arcane dungeon battle will be limited to {ArcaneLoopNumber}");
            if ((ArcaneDungeonAllowed) && (ArcaneLoopNumber == 0)) Console.WriteLine($"=> The number of battle at the arcane dungeon battle will be unlimited");
            if (DurhamForestAllowed) Console.WriteLine("=> The campaign Durham Forest (Stage 1) will be launched automatically");
            if ((DurhamForestAllowed) && (DurhamLoopNumber > 0)) Console.WriteLine($"=> The number of battle at the Durham forest will be limited to {DurhamLoopNumber}");
            if ((DurhamForestAllowed) && (DurhamLoopNumber == 0)) Console.WriteLine($"=> The number of battle at the Durham forest will be unlimited");
            if (LoopNumber == 0) Console.WriteLine("=> This routine will loop forever");
            if (LoopNumber > 0) Console.WriteLine($"=> This routine will loop {LoopNumber} time(s) and then stop");
        }
    }
}
