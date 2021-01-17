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

        private const string KEY_ESCAPE = "{ESC}";
        private const string KEY_ENTER = "{ENTER}";
        private const string KEY_R = "R";

        private Rectangle _recBattle = new Rectangle(1756, 550, 95, 40); //dimension et coordonnées du bouton "battle" sur bastion
        private Rectangle _recMine = new Rectangle(810, 397, 30, 18); //dimension et coordonnées du temoin mine sur bastion
        private Rectangle _recPit = new Rectangle(1634, 426, 18, 18); //dimension et coordonnées du temoin pit
        private Rectangle _recReplay = new Rectangle(1435, 536, 30, 25); //dimension et coordonnées du temoin replay
        private Rectangle _recPitSlot1 = new Rectangle(782, 97, 117, 22);
        private Rectangle _recPitSlot2 = new Rectangle(1022, 97, 117, 22);
        private Rectangle _recPitSlot3 = new Rectangle(1261, 97, 117, 22);
        private Rectangle _recPitSlot4 = new Rectangle(1500, 97, 117, 22);
        private Rectangle _recPitSlot5 = new Rectangle(1740, 97, 117, 22);
        private Rectangle _recPitChooseChampionTitle = new Rectangle(1235, 42, 175, 30);
        private Rectangle _recReward = new Rectangle(1870, 459, 15, 15);

        private Rectangle _recDungeonTitle = new Rectangle(1011, 122, 108, 31);
        private Rectangle _recLevelUp = new Rectangle(1218, 300, 209, 52);

        private int raidProcessId;
        private int _battleCountDown = 30;

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
                ConsoleWriter.WriteLineError("Cannot find the bastion.");
                return false;
            }
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recBattle);

            if (ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest))
            {
                KeyBoardHandler.SendKey(KEY_ESCAPE);
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
                return;
            }
            Thread.Sleep(1000);
            WindowHandler.RepositionRaidWindow(raidProcessId);
            MouseHandler.SetCursorPosition(0, 0);
            Thread.Sleep(1000);
            Bitmap bitmapTemoin = new Bitmap(imgPath);
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recMine);
            Thread.Sleep(1000);
            if (!ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest))
            {
                Console.WriteLine("New gems found. Let's get them.");
                Thread.Sleep(200);
                MouseHandler.MouseClick(823, 404);
            }
            else
            {
                Console.WriteLine("No gems found.");
                Thread.Sleep(1000);
            }
        }

        internal void CheckPlaytimeRewards()
        {
            string imgPath = $"{_imgDirPath}{_imgNameReward}";
            Console.WriteLine("-> Starting to check the playtime rewards...");
            if (!File.Exists(imgPath))
            {
                ConsoleWriter.WriteLineWarning($"Cannot find file {imgPath}. Phase skipped.");
                return;
            }
            Thread.Sleep(1000);
            WindowHandler.RepositionRaidWindow(raidProcessId);
            MouseHandler.SetCursorPosition(0, 0);
            Thread.Sleep(1000);
            Bitmap bitmapTemoin = new Bitmap(imgPath);
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recReward);
            Thread.Sleep(1000);
            if (!ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest))
            {
                Console.WriteLine("Playtime rewards found. Let's get them.");
                MouseHandler.MouseClick(1853, 486);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1038, 404);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1148, 399);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1259, 403);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1370, 406);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1474, 399);
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1591, 402);
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("No rewards found.");
                Thread.Sleep(1000);
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
                return;
            }
            Thread.Sleep(1000);
            WindowHandler.RepositionRaidWindow(raidProcessId);
            MouseHandler.SetCursorPosition(0, 0);
            Thread.Sleep(1000);
            Bitmap bitmapTemoin = new Bitmap(imgPath);
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recPit);
            Thread.Sleep(1000);
            if (!ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest))
            {
                Console.WriteLine("The pit need intervention. Going in...");
                MouseHandler.MouseClick(1571, 379);
                Thread.Sleep(1500);

                if (CheckSlotPitStatus(_recPitSlot1, 1)) MouseHandler.MouseClick(764, 472);
                if (CheckSlotPitStatus(_recPitSlot2, 2)) MouseHandler.MouseClick(1004, 472);
                if (CheckSlotPitStatus(_recPitSlot3, 3)) MouseHandler.MouseClick(1245, 472);
                if (CheckSlotPitStatus(_recPitSlot4, 4)) MouseHandler.MouseClick(1497, 472);
                if (CheckSlotPitStatus(_recPitSlot5, 5)) MouseHandler.MouseClick(1868, 472);

                Thread.Sleep(800);
            }
            else
            {
                Console.WriteLine("No intervention needed.");
                Thread.Sleep(1000);
            }
        }

        private bool CheckSlotPitStatus(Rectangle rec, int slotNumber)
        {
            bool canHarvest = false;
            Thread.Sleep(800);
            string word = ImgHandler.ReadBitmap(rec);
            Thread.Sleep(800);
            if (string.IsNullOrEmpty(word)) word = "UNKNOWN";
            word = word.Trim().ToUpper();
            Console.Write($"Status of slot {slotNumber} --> ");
            if (word == "LEVEL READY") { ConsoleWriter.WriteInformation(word); canHarvest = true; }
            else if (word == "UNKNOWN") ConsoleWriter.WriteError("Cannot read status...");
            else if (word == "MAX LEVEL") { ConsoleWriter.WriteWarning(word); HandleMaxLevelSlotPit(slotNumber); }
            else Console.Write(word);
            Console.WriteLine();

            return canHarvest;
        }

        private void HandleMaxLevelSlotPit(int slotNumber)
        {
            Thread.Sleep(500);
            ConsoleWriter.WriteWarning("\nThis slot has reached max level. Removing it...");

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
                    MouseHandler.MouseClick(1580, 264);
                    break;
                case 5:
                    MouseHandler.MouseClick(1766, 296);
                    break;
                default:
                    break;
            }
            Thread.Sleep(3000);
            if (ImgHandler.ReadBitmap(_recPitChooseChampionTitle).Trim().ToUpper() == "SELECT CHAMPION" || ImgHandler.AreBitmapsDifferent(ImgHandler.GetBitmap(_recPitChooseChampionTitle),new Bitmap($"{_imgDirPath}{_imgNameChampionTitle}")))
            {
                KeyBoardHandler.SendKey(KEY_ESCAPE);
            }
            else
            {
                ConsoleWriter.WriteLineError($"\nCouldn't remove the champion from slot {slotNumber}.\nYou have to do it manually");
            }
        }

        internal int RunArcaneDungeon(int nbLoop)
        {
            int compteur = 0;
            DateTime dateStart = DateTime.Now;
            TimeSpan t;
            Bitmap bmpReplay = new Bitmap($"{_imgDirPath}{_imgNameReplay}");
            Bitmap bmpLvlUp = new Bitmap($"{_imgDirPath}{_imgNameLevelUp}");
            Console.Write("-> Starting the arcane dungeon run ");
            if (nbLoop != 0) Console.Write($"({nbLoop} run left)");
            Console.WriteLine();
            Thread.Sleep(1000);
            if (ShowArcaneDungeon())
            {
                Console.WriteLine("---> Entering the arcane dungeon...");
                Thread.Sleep(1000);
                MouseHandler.MouseClick(1375, 213);
                MouseHandler.MouseDrag(1385, 528, 1414, 123);
                MouseHandler.MouseClick(1803, 380);
                ConsoleWriter.CountDown("Starting battle in {0}  ", _battleCountDown);
                MouseHandler.MouseClick(1790, 550);
                compteur++;
                do
                {
                    Thread.Sleep(1000);
                    var bmpTestReplay = ImgHandler.GetBitmap(_recReplay);
                    var bmpTestLvlUp = ImgHandler.GetBitmap(_recLevelUp);
                    if (!ImgHandler.AreBitmapsDifferent(bmpReplay, bmpTestReplay))
                    {
                        t = DateTime.Now - dateStart;
                        ConsoleWriter.KeepAwake();
                        if (t.TotalMinutes > 20)
                        {
                            Console.Write("\nPausing the battle to check the bastion...");
                            Thread.Sleep(800);
                            break; 
                        }
                        if ((nbLoop != 0) && (compteur >= nbLoop))
                        {
                            Console.Write("\nMax number of allowed battle reached...");
                            break;
                        }
                        KeyBoardHandler.SendKey(KEY_R);
                        compteur++;
                        Thread.Sleep(1000);
                    }
                    else if(!ImgHandler.AreBitmapsDifferent(bmpLvlUp, bmpTestLvlUp))
                    {
                        KeyBoardHandler.SendKey(KEY_ESCAPE);
                    }
                    if (nbLoop != 0) { Console.Write($"\r=> Starting run {compteur}/{nbLoop}   "); }
                    else { Console.Write($"\r=> Starting run {compteur}          "); }
                    Thread.Sleep(10000);
                } while (true);
                Console.WriteLine();
            }

            if (nbLoop != 0) return nbLoop - compteur;
            else return nbLoop;
        }

        private bool ShowArcaneDungeon()
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
            if(ShowDurhamForest())
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
                    var bmpTestReplay = ImgHandler.GetBitmap(_recReplay);
                    var bmpTestLvlUp = ImgHandler.GetBitmap(_recLevelUp);
                    if (!ImgHandler.AreBitmapsDifferent(bmpReplay, bmpTestReplay))
                    {
                        t = DateTime.Now - dateStart;
                        ConsoleWriter.KeepAwake();
                        if (t.TotalMinutes > 20)
                        {
                            Console.Write("\nPausing the battle to check the bastion...");
                            Thread.Sleep(800);
                            break;
                        }
                        if ((nbLoop != 0) && (compteur >= nbLoop))
                        {
                            Console.Write("\nMax number of allowed battle reached...");
                            break;
                        }
                        KeyBoardHandler.SendKey(KEY_R);
                        compteur++;
                        Thread.Sleep(1000);
                    }
                    else if (!ImgHandler.AreBitmapsDifferent(bmpLvlUp, bmpTestLvlUp))
                    {
                        KeyBoardHandler.SendKey(KEY_ESCAPE);
                    }
                    if (nbLoop != 0) { Console.Write($"\r=> Starting run {compteur}/{nbLoop}   "); }
                    else { Console.Write($"\r=> Starting run {compteur}          "); }
                    Thread.Sleep(10000);
                } while (true);
                Console.WriteLine();
            }

            if (nbLoop != 0) return nbLoop - compteur;
            else return nbLoop;
        }

        private bool ShowDurhamForest()
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
    }
}

