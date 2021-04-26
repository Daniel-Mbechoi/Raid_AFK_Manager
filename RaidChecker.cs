using Raid_AFK_Manager.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raid_AFK_Manager
{
    internal class RaidChecker
    {
        private const string _imgDirPath = ".\\img\\";
        private const string _imgNameMine = "MineDiamond.bmp";
        private const string _imgNameBattle = "BtnBattle.bmp";
        private const string _imgNameReward = "RewardRedDot.bmp";
        private const string _imgNamePit = "PitRedDot.bmp";
        private const string _imgNameReplay = "BtnReplay.bmp";
        private const string _imgNameChampionTitle = "pitChooseChampionTitle.bmp";
        private const string _imgNameLevelUp = "LevelUp.bmp";
        private const string _imgVoidKeep = "voidKeep.bmp";
        private const string _imgSpiritKeep = "spiritKeep.bmp";
        private const string _imgForceKeep = "forceKeep.bmp";
        private const string _imgMagicKeep = "magicKeep.bmp";
        private const string _imgDailyReward = "dailyRedDot.bmp";
        private const string _imgMarket = "marketRedDot.bmp";

        private const string KEY_ESCAPE = "{ESC}";
        private const string KEY_ENTER = "{ENTER}";
        private const string KEY_R = "R";
        private const string KEY_T = "T";

        private Rectangle _recBattle = new Rectangle(1756, 550, 95, 40); //dimension et coordonnées du bouton "battle" sur bastion
        private Rectangle _recMine = new Rectangle(810, 397, 30, 18); //dimension et coordonnées du temoin mine sur bastion
        private Rectangle _recPit = new Rectangle(1636, 426, 10, 10); //dimension et coordonnées du temoin pit
        private Rectangle _recReplay = new Rectangle(1435, 536, 30, 25); //dimension et coordonnées du temoin replay
        private Rectangle _recPitSlot1 = new Rectangle(782, 97, 117, 22);
        private Rectangle _recPitSlot2 = new Rectangle(1022, 97, 117, 22);
        private Rectangle _recPitSlot3 = new Rectangle(1261, 97, 117, 22);
        private Rectangle _recPitSlot4 = new Rectangle(1500, 97, 117, 22);
        private Rectangle _recPitSlot5 = new Rectangle(1740, 97, 117, 22);
        private Rectangle _recPitChooseChampionTitle = new Rectangle(1235, 42, 175, 30);
        private Rectangle _recReward = new Rectangle(1870, 459, 15, 15);
        private Rectangle _recDailyReward = new Rectangle(735, 263, 5, 5);
        private Rectangle _recDungeonTitle = new Rectangle(1011, 122, 108, 31);
        private Rectangle _recVoidKeep = new Rectangle(1500, 163, 54, 37);
        private Rectangle _recSpiritKeep = new Rectangle(1262, 368, 52, 39);
        private Rectangle _recForceKeep = new Rectangle(1629, 264, 56, 39);
        private Rectangle _recMagicKeep = new Rectangle(1520, 368, 58, 53);
        private Rectangle _recLevelUp = new Rectangle(1218, 300, 209, 52);
        private Rectangle _recMarket = new Rectangle(1217, 559, 20, 20);

        private int raidProcessId;

        private int _battleCountDown = 30;
        private int _battleIdle = 4 * 1000;

        public RaidChecker(int raidProcessId)
        {
            this.raidProcessId = raidProcessId;
        }


        internal bool ShowBastion()
        {
            string battlePath = $"{_imgDirPath}{_imgNameBattle}";
            if (!File.Exists(battlePath))
            {
                ConsoleWriter.WriteLineError($"Cannot find file {battlePath}");
                return false;
            }
            MouseHandler.SetCursorPosition(0, 0);
            Bitmap bitmapTemoin = new Bitmap(battlePath);
            int c = 0;
        debut:            
            WindowHandler.RepositionRaidWindow(raidProcessId);
            if (c > 20)
            {
                throw new Exception("Cannot find the bastion. Number of attempts failed.");
            }
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recBattle);

            if (ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest,RaidOptions.SavePicturesAllowed))
            {
                KeyBoardHandler.SendKey(KEY_ESCAPE);
                Thread.Sleep(2000);
                c++;
                goto debut;
            }

            return true;
        }

        internal void CheckMine()
        {
            string imgPath = $"{_imgDirPath}{_imgNameMine}";
            Console.WriteLine("-> Starting to check the mine...");
            if (!File.Exists(imgPath))
            {
                ConsoleWriter.WriteLineWarning($"Cannot find file {imgPath}. Phase skipped.");
                Thread.Sleep(500);
                return;
            }
            WindowHandler.RepositionRaidWindow(raidProcessId);
            MouseHandler.SetCursorPosition(0, 0);
            Bitmap bitmapTemoin = new Bitmap(imgPath);
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recMine);
            if (!ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest, RaidOptions.SavePicturesAllowed))
            {
                ConsoleWriter.WriteLineInformation("New gems found. Let's get them.");
                MouseHandler.MouseClick(823, 404);
            }
            else
            {
                Console.WriteLine("No gems found.");
                Thread.Sleep(100);
            }
        }

        internal void CheckPlaytimeRewards()
        {
            //daily playtime reward
            string imgPath = $"{_imgDirPath}{_imgNameReward}";
            Console.WriteLine("-> Starting to check the playtime rewards...");
            if (!File.Exists(imgPath))
            {
                ConsoleWriter.WriteLineWarning($"Cannot find file {imgPath}. Phase skipped.");
                return;
            }
            WindowHandler.RepositionRaidWindow(raidProcessId);
            MouseHandler.SetCursorPosition(0, 0);
            Bitmap bitmapTemoin = new Bitmap(imgPath);
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recReward);
            //bitmapTest.Save("RewardRedDot.bmp");
            Thread.Sleep(100);
            if (!ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest, RaidOptions.SavePicturesAllowed))
            {
                ConsoleWriter.WriteLineInformation("Playtime rewards found. Let's get them.");
                MouseHandler.MouseClick(1853, 486);
                Thread.Sleep(100);
                MouseHandler.MouseClick(1038, 404);
                Thread.Sleep(100);
                MouseHandler.MouseClick(1148, 399);
                Thread.Sleep(100);
                MouseHandler.MouseClick(1259, 403);
                Thread.Sleep(100);
                MouseHandler.MouseClick(1370, 406);
                Thread.Sleep(100);
                MouseHandler.MouseClick(1474, 399);
                Thread.Sleep(100);
                MouseHandler.MouseClick(1591, 402);
                Thread.Sleep(100);
            }
            else
            {
                Console.WriteLine("No rewards found.");
                Thread.Sleep(100);
            }

            ShowBastion();

            //daily login rewards
            imgPath = $"{_imgDirPath}{_imgDailyReward}";
            Console.WriteLine("-> Starting to check the daily login rewards...");
            if (!File.Exists(imgPath))
            {
                ConsoleWriter.WriteLineWarning($"Cannot find file {imgPath}. Phase skipped.");
                Thread.Sleep(500);
                return;
            }
            WindowHandler.RepositionRaidWindow(raidProcessId);
            MouseHandler.SetCursorPosition(0, 0);
            bitmapTemoin = new Bitmap(imgPath);
            bitmapTest = ImgHandler.GetBitmap(_recDailyReward);
            if (!ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest, RaidOptions.SavePicturesAllowed))
            {
                Console.WriteLine("Daily rewards found. Let's go get them.");
                MouseHandler.MouseClick(717, 314);
                MouseHandler.MouseClick(744, 110);
                KeyBoardHandler.SendKey(KEY_ESCAPE);
            }
            else
            {
                Console.WriteLine("No rewards found.");
                Thread.Sleep(500);
            }
        }

        internal void CheckMarket()
        {
            string imgPath = $"{_imgDirPath}{_imgMarket}";
            string word = string.Empty;
            Console.WriteLine("-> Starting to check the market...");
            if (!File.Exists(imgPath))
            {
                ConsoleWriter.WriteLineWarning($"Cannot find file {imgPath}. Phase skipped.");
                Thread.Sleep(500);
                return;
            }
            WindowHandler.RepositionRaidWindow(raidProcessId);
            MouseHandler.SetCursorPosition(0, 0);
            Bitmap bitmapTemoin = new Bitmap(imgPath);
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recMarket);
            if (!ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest, RaidOptions.SavePicturesAllowed))
            {
                ConsoleWriter.WriteLineInformation("Update detected. Let's go and search for shards !");
                Console.WriteLine("Maybe later...");
                //MouseHandler.MouseClick(1161, 515);
                Thread.Sleep(800);
            }
            else
            {
                Console.WriteLine("No update detected.");
                Thread.Sleep(100);
            }
        }

        internal void CheckThePit()
        {
            string imgPath = $"{_imgDirPath}{_imgNamePit}";
            string word = string.Empty;
            Console.WriteLine("-> Starting to check the pit...");
            if (!File.Exists(imgPath))
            {
                ConsoleWriter.WriteLineWarning($"Cannot find file {imgPath}. Phase skipped.");
                Thread.Sleep(500);
                return;
            }
            WindowHandler.RepositionRaidWindow(raidProcessId);
            MouseHandler.SetCursorPosition(0, 0);
            Bitmap bitmapTemoin = new Bitmap(imgPath);
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recPit);
            if (!ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest, RaidOptions.SavePicturesAllowed))
            {
                ConsoleWriter.WriteLineInformation("The pit need intervention. Going in...");
                MouseHandler.MouseClick(1571, 379);

                if (CheckSlotPitStatus(_recPitSlot1, 1)) MouseHandler.MouseClick(764, 472);
                if (CheckSlotPitStatus(_recPitSlot2, 2)) MouseHandler.MouseClick(1004, 472);
                if (CheckSlotPitStatus(_recPitSlot3, 3)) MouseHandler.MouseClick(1245, 472);
                if (CheckSlotPitStatus(_recPitSlot4, 4)) MouseHandler.MouseClick(1497, 472);
                if (CheckSlotPitStatus(_recPitSlot5, 5)) MouseHandler.MouseClick(1868, 472);

            }
            else
            {
                Console.WriteLine("No intervention needed.");
                Thread.Sleep(100);
            }
        }

        private bool CheckSlotPitStatus(Rectangle rec, int slotNumber)
        {
            bool canHarvest = false;
            string word = ImgHandler.ReadBitmap(rec);
            if (string.IsNullOrEmpty(word)) word = "UNKNOWN";
            word = word.Trim().ToUpper();
            Console.Write($"Status of slot {slotNumber} --> ");
            if (word == "LEVEL READY") { ConsoleWriter.WriteInformation(word); canHarvest = true; }
            else if (word == "UNKNOWN") ConsoleWriter.WriteError("Cannot read status...");
            else if (word == "MAX LEVEL") { ConsoleWriter.WriteWarning(word); HandleMaxLevelSlotPit(slotNumber); }
            else if (word == @"+0 XP/HOUR") { ConsoleWriter.WriteWarning(word); HandleMaxLevelSlotPit(slotNumber); }
            else Console.Write(word);
            Console.WriteLine();

            return canHarvest;
        }

        private void HandleMaxLevelSlotPit(int slotNumber)
        {
            Thread.Sleep(500);
            ConsoleWriter.WriteWarning("\nTrying to swap characters...");

            switch (slotNumber)
            {
                case 1:
                    MouseHandler.MouseClick(875, 316);
                    break;
                case 2:
                    MouseHandler.MouseClick(1114, 289);
                    break;
                case 3:
                    MouseHandler.MouseClick(1340, 294);
                    break;
                case 4:
                    MouseHandler.MouseClick(1548, 319);
                    break;
                case 5:
                    MouseHandler.MouseClick(1766, 296);
                    break;
                default:
                    break;
            }
            Thread.Sleep(1500);
            if (ImgHandler.ReadBitmap(_recPitChooseChampionTitle).Trim().ToUpper() == "SELECT CHAMPION" || ImgHandler.AreBitmapsDifferent(ImgHandler.GetBitmap(_recPitChooseChampionTitle), new Bitmap($"{_imgDirPath}{_imgNameChampionTitle}")))
            {
                MouseHandler.MouseClick(1008, 559);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1003, 123);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1541, 562);
                //KeyBoardHandler.SendKey(KEY_ESCAPE);
            }
            else
            {
                ConsoleWriter.WriteLineError($"\nCouldn't remove the champion from slot {slotNumber}.\nYou have to do it manually");
            }
        }

        internal int RunGodfreyCrossing(int nbLoop)
        {
            int compteur = 0;
            DateTime dateStart = DateTime.Now;
            TimeSpan t;
            Bitmap bmpReplay = new Bitmap($"{_imgDirPath}{_imgNameReplay}");
            Bitmap bmpLvlUp = new Bitmap($"{_imgDirPath}{_imgNameLevelUp}");
            Console.Write("-> Starting the godfrey's crossing run ");
            if (nbLoop != 0) Console.Write($"({nbLoop} run left)");
            Console.WriteLine();
            Thread.Sleep(1000);
            if (GoToCampaignMap())
            {
                Console.WriteLine("---> Entering godfrey's crossing...");
                Thread.Sleep(500);
                MouseHandler.MouseDrag(1767, 327, 905, 467);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1239, 271);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1815, 130);
                ConsoleWriter.CountDown("Starting battle in {0}  ", _battleCountDown);
                MouseHandler.MouseClick(1790, 550);
                compteur++;
                do
                {
                    Thread.Sleep(1000);
                    WindowHandler.RepositionRaidWindow(raidProcessId);
                    Thread.Sleep(500);
                    var bmpTestReplay = ImgHandler.GetBitmap(_recReplay);
                    var bmpTestLvlUp = ImgHandler.GetBitmap(_recLevelUp);
                    if (!ImgHandler.AreBitmapsDifferent(bmpReplay, bmpTestReplay, RaidOptions.SavePicturesAllowed))
                    {
                        t = DateTime.Now - dateStart;
                        ConsoleWriter.KeepAwake();
                        if ((t.TotalMinutes > 20) && (RaidOptions.CheckMineAllowed || RaidOptions.CheckPitAllowed || RaidOptions.CheckRewardsAllowed))
                        {
                            Console.Write("\nPausing the battle to check the bastion...");
                            Thread.Sleep(800);
                            break;
                        }
                        if ((nbLoop != 0) && (compteur >= nbLoop))
                        {
                            Console.Write("\nMax number of allowed battle reached...");
                            RaidOptions.DurhamForestAllowed = false;
                            break;
                        }
                        KeyBoardHandler.SendKey(KEY_R);
                        compteur++;
                        Thread.Sleep(1000);
                    }
                    else if (!ImgHandler.AreBitmapsDifferent(bmpLvlUp, bmpTestLvlUp, RaidOptions.SavePicturesAllowed))
                    {
                        KeyBoardHandler.SendKey(KEY_ESCAPE);
                    }
                    if (nbLoop != 0) { Console.Write($"\r=> Starting run {compteur}/{nbLoop}   "); }
                    else { Console.Write($"\r=> Starting run {compteur}          "); }
                    Thread.Sleep(_battleIdle);
                } while (true);
                Console.WriteLine();
            }

            if (nbLoop != 0) return nbLoop - compteur;
            else return nbLoop;
        }
        internal int RunDurhamForest(int nbLoop)
        {
            int compteur = 0;
            DateTime dateStart = DateTime.Now;
            TimeSpan t;
            Bitmap bmpReplay = new Bitmap($"{_imgDirPath}{_imgNameReplay}");
            Bitmap bmpLvlUp = new Bitmap($"{_imgDirPath}{_imgNameLevelUp}");
            Console.Write("-> Starting the Durham Forest run ");
            if (nbLoop != 0) Console.Write($"({nbLoop} run left)");
            Console.WriteLine();
            Thread.Sleep(1000);
            if (GoToCampaignMap())
            {
                Console.WriteLine("---> Entering durham forest...");
                Thread.Sleep(1000);
                MouseHandler.MouseDrag(1767, 327, 905, 467);
                Thread.Sleep(2000);
                MouseHandler.MouseClick(1239, 271);
                Thread.Sleep(2000);
                MouseHandler.MouseClick(1815, 130);
                ConsoleWriter.CountDown("Starting battle in {0}  ", _battleCountDown);
                MouseHandler.MouseClick(1790, 550);
                compteur++;
                do
                {
                    Thread.Sleep(1000);
                    WindowHandler.RepositionRaidWindow(raidProcessId);
                    Thread.Sleep(500);
                    var bmpTestReplay = ImgHandler.GetBitmap(_recReplay);
                    var bmpTestLvlUp = ImgHandler.GetBitmap(_recLevelUp);
                    if (!ImgHandler.AreBitmapsDifferent(bmpReplay, bmpTestReplay, RaidOptions.SavePicturesAllowed))
                    {
                        t = DateTime.Now - dateStart;
                        ConsoleWriter.KeepAwake();
                        if ((t.TotalMinutes > 20) && (RaidOptions.CheckMineAllowed || RaidOptions.CheckPitAllowed || RaidOptions.CheckRewardsAllowed))
                        {
                            Console.Write("\nPausing the battle to check the bastion...");
                            Thread.Sleep(800);
                            break;
                        }
                        if ((nbLoop != 0) && (compteur >= nbLoop))
                        {
                            Console.Write("\nMax number of allowed battle reached...");
                            RaidOptions.DurhamForestAllowed = false;
                            break;
                        }
                        KeyBoardHandler.SendKey(KEY_R);
                        compteur++;
                        Thread.Sleep(1000);
                    }
                    else if (!ImgHandler.AreBitmapsDifferent(bmpLvlUp, bmpTestLvlUp, RaidOptions.SavePicturesAllowed))
                    {
                        KeyBoardHandler.SendKey(KEY_ESCAPE);
                    }
                    if (nbLoop != 0) { Console.Write($"\r=> Starting run {compteur}/{nbLoop}   "); }
                    else { Console.Write($"\r=> Starting run {compteur}          "); }
                    Thread.Sleep(_battleIdle);
                } while (true);
                Console.WriteLine();
            }

            if (nbLoop != 0) return nbLoop - compteur;
            else return nbLoop;
        }

        internal int RunArcaneKeep(int nbLoop)
        {
            return RunDungeon(nbLoop, "arcane keep", 1375, 213);
        }

        internal int RunIceGolemPeak(int nbLoop)
        {
            return RunDungeon(nbLoop, "ice golem peak", 1854, 161);
        }

        internal int RunMinotaurLabyrinth(int nbLoop)
        {
            return RunDungeon(nbLoop, "minotaur's labyrinth", 1863, 318);
        }

        private int RunDungeon(int nbLoop, string dungeonName, int mouseX, int mouseY)
        {
            int compteur = 0;
            Console.Write($"-> Starting the {dungeonName} run ");
            if (nbLoop != 0) Console.Write($"({nbLoop} run left)");
            Console.WriteLine();
            Thread.Sleep(500);
            if (GoToDungeonMap())
            {
                Console.WriteLine($"---> Entering the {dungeonName}...");
                Thread.Sleep(500);
                MouseHandler.MouseClick(mouseX, mouseY);
                Thread.Sleep(500);
                compteur = AutoBattleForKeeps(nbLoop, compteur, dungeonName);
            }

            if (nbLoop != 0) return nbLoop - compteur;
            else return nbLoop;
        }

        internal int RunVoidKeep(int nbLoop)
        {
            return RunWeeklyDungeon(nbLoop, "void keep", _recVoidKeep, $"{_imgDirPath}{_imgVoidKeep}", 1525, 186);
        }

        internal int RunMagicKeep(int nbLoop)
        {
            return RunWeeklyDungeon(nbLoop, "magic keep", _recMagicKeep, $"{_imgDirPath}{_imgMagicKeep}", 1553, 400);
        }

        internal int RunSpiritKeep(int nbLoop)
        {
            return RunWeeklyDungeon(nbLoop, "spirit keep", _recSpiritKeep, $"{_imgDirPath}{_imgSpiritKeep}", 1288, 378);
        }

        internal int RunForceKeep(int nbLoop)
        {
            return RunWeeklyDungeon(nbLoop, "force keep", _recForceKeep, $"{_imgDirPath}{_imgForceKeep}", 1652, 279);
        }

        internal int RunWeeklyDungeon(int nbLoop, string dungeonName, Rectangle recDungeon, string imgPathDungeon, int mouseX, int mouseY)
        {
            int compteur = 0;
            Console.Write($"-> Starting the {dungeonName} run ");
            if (nbLoop != 0) Console.Write($"({nbLoop} run left)");
            Console.WriteLine();
            Thread.Sleep(1000);
            if (GoToDungeonMap())
            {
                Console.WriteLine($"---> Entering the {dungeonName}...");
                Thread.Sleep(1000);
                if (!File.Exists(imgPathDungeon))
                {
                    ConsoleWriter.WriteLineWarning($"Cannot find file {imgPathDungeon}. Run skipped.");
                    return 0;
                }
                Bitmap btmDungeon = new Bitmap(imgPathDungeon);
                Bitmap btmTest = ImgHandler.GetBitmap(recDungeon);
                if (!ImgHandler.AreBitmapsDifferent(btmDungeon, btmTest, RaidOptions.SavePicturesAllowed))
                {
                    MouseHandler.MouseClick(mouseX, mouseY);
                    Thread.Sleep(1000);
                    compteur = AutoBattleForKeeps(nbLoop, compteur, dungeonName);
                }
                else
                {
                    ConsoleWriter.WriteLineWarning($"The {dungeonName} is not available. Run skipped.");
                    return 0;
                }
            }
            if (nbLoop != 0) return nbLoop - compteur;
            else return nbLoop;
        }

        private bool GoToDungeonMap()
        {
            if (!ShowBastion()) return false;
            KeyBoardHandler.SendKey(KEY_ENTER);
            if (!(ImgHandler.ReadBitmap(_recDungeonTitle).ToUpper() == "DUNGEONS"))
            {
                ConsoleWriter.WriteLineError("Cannot find the dungeon. Abort.");
                return false;
            }
            else
            {
                Console.WriteLine("--> Entering the dungeon map...");
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1057, 331);
                return true;
            }
        }

        private bool GoToCampaignMap()
        {
            if (!ShowBastion()) return false;
            KeyBoardHandler.SendKey(KEY_ENTER);
            if (!(ImgHandler.ReadBitmap(_recDungeonTitle).ToUpper() == "DUNGEONS"))
            {
                ConsoleWriter.WriteLineError("Cannot find the campaign. Abort.");
                return false;
            }
            else
            {
                Console.WriteLine("--> Entering the campaign map...");
                Thread.Sleep(1000);
                MouseHandler.MouseClick(812, 321);
                return true;
            }
        }

        private int AutoBattleForKeeps(int nbLoop, int compteur, string keepName)
        {
            MouseHandler.MouseWheelDown(25);
            Thread.Sleep(500);
            MouseHandler.MouseClick(1803, 380);
            ConsoleWriter.CountDown("Starting battle in {0}  ", _battleCountDown);
            MouseHandler.MouseClick(1790, 550);

            DateTime dateStart = DateTime.Now;
            TimeSpan t;
            Bitmap bmpReplay = new Bitmap($"{_imgDirPath}{_imgNameReplay}");
            Bitmap bmpLvlUp = new Bitmap($"{_imgDirPath}{_imgNameLevelUp}");

            compteur++;
            do
            {
                WindowHandler.RepositionRaidWindow(raidProcessId);
                Thread.Sleep(500);
                var bmpTestReplay = ImgHandler.GetBitmap(_recReplay);
                var bmpTestLvlUp = ImgHandler.GetBitmap(_recLevelUp);
                if (!ImgHandler.AreBitmapsDifferent(bmpReplay, bmpTestReplay, RaidOptions.SavePicturesAllowed))
                {
                    t = DateTime.Now - dateStart;
                    ConsoleWriter.KeepAwake();
                    if ((t.TotalMinutes > 20) && (RaidOptions.CheckMineAllowed || RaidOptions.CheckPitAllowed || RaidOptions.CheckRewardsAllowed))
                    {
                        Console.Write("\nPausing the battle to check the bastion...");
                        Thread.Sleep(800);
                        break;
                    }
                    if ((nbLoop != 0) && (compteur >= nbLoop))
                    {
                        Console.Write("\nMax number of allowed battle reached...");
                        //switch (keepName)
                        //{
                        //    case "magic keep":
                        //        RaidOptions.MagicKeepAllowed = false;
                        //        break;
                        //    case "void keep":
                        //        RaidOptions.VoidKeepAllowed = false;
                        //        break;
                        //    case "spirit keep":
                        //        RaidOptions.SpiritKeepAllowed = false;
                        //        break;
                        //    case "force keep":
                        //        RaidOptions.ForceKeepAllowed = false;
                        //        break;
                        //    case "arcane keep":
                        //        RaidOptions.ArcaneKeepAllowed = false;
                        //        break;
                        //    default:
                        //        throw new ArgumentNullException($"Cannot find the keep '{keepName}' !");
                        //}
                        break;
                    }
                    KeyBoardHandler.SendKey(KEY_R);
                    compteur++;
                    Thread.Sleep(1000);
                }
                else if (!ImgHandler.AreBitmapsDifferent(bmpLvlUp, bmpTestLvlUp, RaidOptions.SavePicturesAllowed))
                {
                    KeyBoardHandler.SendKey(KEY_ESCAPE);
                }
                if (nbLoop != 0) { Console.Write($"\r=> Starting run {compteur}/{nbLoop}   "); }
                else { Console.Write($"\r=> Starting run {compteur}          "); }
                Thread.Sleep(_battleIdle);
            } while (true);
            Console.WriteLine();
            return compteur;
        }
    }
}

