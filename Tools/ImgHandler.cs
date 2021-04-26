using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract;

namespace Raid_AFK_Manager.Tools
{
    internal static class ImgHandler
    {
        internal static void RecordBitmap(int x, int y, int w, int h, string nameFile)
        {
            Rectangle r = new Rectangle(x, y, w, h);
            RecordBitmap(r, nameFile);
        }

        internal static void RecordBitmap(Rectangle r, string nameFile)
        {
            Bitmap b = GetBitmap(r);
            b.Save(nameFile);
        }

        internal static Bitmap GetBitmap(Rectangle rect)
        {
            Bitmap b = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(b);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, b.Size, CopyPixelOperation.SourceCopy);
            Thread.Sleep(1000);
            return b;
        }

        internal static bool AreBitmapsDifferent(Bitmap bmpOriginal, Bitmap bitmapToTest, bool sauvegarde = false)
        {
            if (sauvegarde)
            {
                Directory.CreateDirectory("Saves");
                bitmapToTest.Save("Saves\\_img_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            }
            int maxTolerance = (int)Math.Round(bmpOriginal.Height * bmpOriginal.Width * 0.15, MidpointRounding.ToEven); //tolérance : On ne doit pas dépacer 15% de différence            
            int nbDifference = 0;

            for (int i = 0; i < bmpOriginal.Width; i++)
            {
                for (int j = 0; j < bmpOriginal.Height; j++)
                {
                    if (bmpOriginal.GetPixel(i, j) != bitmapToTest.GetPixel(i, j))
                    {
                        nbDifference++;
                        if (nbDifference > maxTolerance)
                        {
                            //bitmapToTest.Save("_Test_failed_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".bmp");
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        internal static string ReadBitmap(Rectangle rec)
        {
            return ReadBitmap(GetBitmap(rec));
        }

        internal static string ReadBitmap(Bitmap bmp)
        {
            var bwBmp = ToBlackAndWhite(bmp);
            var path = "tmp.bmp";
            var word = string.Empty;
            bwBmp.Save(path);

            using (var engine = new TesseractEngine(@"./TessData", "eng", EngineMode.Default))
            using (var img = Pix.LoadFromFile(path))
            using (var page = engine.Process(img))
            {
                if (page.GetMeanConfidence() >= (0.7f))
                {
                    word = page.GetText().Trim();
                }
            }

            File.Delete(path);
            return word;
        }

        //Thanks to Jón Trausti Arason and Zachary Canann
        //https://stackoverflow.com/questions/4669317/how-to-convert-a-bitmap-image-to-black-and-white-in-c
        private static Bitmap ToBlackAndWhite(Bitmap bmp)
        {
            int rgb;
            Color c;

            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    c = bmp.GetPixel(x, y);
                    rgb = (int)(Math.Round(((double)(c.R + c.G + c.B) / 3.0) / 255) * 255);
                    bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }

            return bmp;
        }
    }
}
