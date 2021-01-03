﻿using Raid_AFK_Manager.Tools;
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
        private const string _imgNamePit = "PitRedDot.bmp";
        private const string _imgNameMine = "MineDiamond.bmp";
        private const string _imgNameBattle = "BtnBattle.bmp";
        private const string _imgNameMarket = "MarketRedDot.bmp";
        private const string _imgNameVictory = "victory.bmp";
        private const string _imgNameReplay = "BtnReplay.bmp";
        private const string _imgNameEdit = "BtnEdit.bmp";
        private const string _imgNameEnergy = "BtnBuyEnergy.bmp";

        private const string KEY_ESCAPE = "{ESC}";
        private const string KEY_N = "N";
        private const string KEY_R = "R";

        private static Rectangle _recBattle = new Rectangle(1756, 550, 95, 40); //dimension et coordonnées du bouton "battle" sur bastion
        private static Rectangle _recMine = new Rectangle(810, 397, 30, 18); //dimension et coordonnées du temoin mine sur bastion
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
            Console.WriteLine("-> Starting to check the mine...");
            Thread.Sleep(1000);
            WindowHandler.RepositionRaidWindow(raidProcessId);
            MouseHandler.SetCursorPosition(0, 0); 
            Thread.Sleep(1000);
            Bitmap bitmapTemoin = new Bitmap($"{_imgDirPath}{_imgNameMine}");
            Bitmap bitmapTest = ImgHandler.GetBitmap(_recMine);
            Thread.Sleep(1000);
            if (!ImgHandler.AreBitmapsDifferent(bitmapTemoin, bitmapTest))
            {
                Console.WriteLine("New gems found. Let's get them.");
                MouseHandler.MouseClick(831, 501);
            }
            else
            {
                Console.WriteLine("No gems found.");
                Thread.Sleep(1000);
            }
        }
    }
}
