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

        private const string KEY_ESCAPE = "{ESC}";
        private const string KEY_N = "N";
        private const string KEY_R = "R";

        private static Rectangle _recBattle = new Rectangle(1756, 550, 95, 40); //dimension et coordonnées du bouton "battle" sur bastion
        private static Rectangle _recMine = new Rectangle(810, 397, 30, 18); //dimension et coordonnées du temoin mine sur bastion
        private static Rectangle _recPit = new Rectangle(1634, 426, 18, 18); //dimension et coordonnées du temoin pit
        private static Rectangle _recPitSlot1 = new Rectangle(782, 97, 117, 22);
        private static Rectangle _recPitSlot2 = new Rectangle(1022, 97, 117, 22);
        private static Rectangle _recPitSlot3 = new Rectangle(1261, 97, 117, 22);
        private static Rectangle _recPitSlot4 = new Rectangle(1498, 97, 117, 22);
        private static Rectangle _recPitSlot5 = new Rectangle(1740, 97, 117, 22);
        private static Rectangle _recReward = new Rectangle(1870, 459, 15, 15);

        private int raidProcessId;

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
                MouseHandler.MouseClick(831, 501);
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
                KeyBoardHandler.SendKey(KEY_N);
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

                if(CheckSlotPitStatus(_recPitSlot1, 1)) MouseHandler.MouseClick(764, 472);
                if(CheckSlotPitStatus(_recPitSlot2, 2)) MouseHandler.MouseClick(1004, 472);
                if(CheckSlotPitStatus(_recPitSlot3, 3)) MouseHandler.MouseClick(1245, 472);
                if(CheckSlotPitStatus(_recPitSlot4, 4)) MouseHandler.MouseClick(1487, 472);
                if(CheckSlotPitStatus(_recPitSlot5, 5)) MouseHandler.MouseClick(1868, 472);

                Thread.Sleep(800);
            }
            else
            {
                Console.WriteLine("No gintervention needed.");
                Thread.Sleep(1000);
            }
        }

        private static bool CheckSlotPitStatus(Rectangle rec, int slotNumber)
        {
            bool canHarvest = false;
            Thread.Sleep(800);
            string word = ImgHandler.ReadBitmap(rec);
            Thread.Sleep(800);
            if (string.IsNullOrEmpty(word)) word = "Unknown";
            word = word.Trim().ToUpper();
            Console.Write($"Status of slot {slotNumber} -- > ");
            if (word == "LEVEL READY") { ConsoleWriter.WriteInformation(word); canHarvest = true; }
            else if (word == "UNKNOWN") ConsoleWriter.WriteError(word);
            else if (word == "MAX LEVEL") ConsoleWriter.WriteWarning(word);
            else Console.Write(word);
            Console.WriteLine();

            return canHarvest;
        }
    }
}
