using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public static class BitmapTools
    {

        public static Bitmap LoadTransparent(string strImageName)
        {
            Bitmap Load_result;
            Color BackColor;

            try
            {
                Load_result = (Bitmap)Bitmap.FromFile(strImageName);
                // The transparent color (keycolor) was not informed, then it will be the color of the first pixel
                BackColor = Load_result.GetPixel(0, 0);
                Load_result.MakeTransparent(BackColor);
            }
            catch
            {
                MessageBox.Show("An image file was not found." + Keys.Enter + "Please make sure that the file " + strImageName + " exists.", ".Netterpillars", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Load_result = null;
            }
            return Load_result;
        }

        public static Bitmap LoadTransparent(string strImageName, Color keycolor)
        {
            Bitmap Load_result;
            try
            {
                Load_result = (Bitmap)Bitmap.FromFile(strImageName);
                Load_result.MakeTransparent(keycolor);
            }
            catch
            {
                MessageBox.Show("An image file was not found." + Keys.Enter + "Please make sure that the file " + strImageName + " exists.", ".Netterpillars", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Load_result = null;
            }
            return Load_result;
        }
    }
}
