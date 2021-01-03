using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raid_AFK_Manager.Tools
{
    internal static class ImgHandler
    {
        internal static Bitmap GetBitmap(Rectangle rect)
        {
            Bitmap b = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(b);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, b.Size, CopyPixelOperation.SourceCopy);
            Thread.Sleep(1000);
            return b;
        }

        internal static bool AreBitmapsDifferent(Bitmap bmpOriginal, Bitmap bitmapToTest)
        {
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
                            return true;
                        }
                    }
                }
            }

            return false;

        }
    }
}
