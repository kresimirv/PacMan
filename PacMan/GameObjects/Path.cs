using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PacMan
{
    public class Path : Sprite
    {
        private static Color pathColor = Color.Black;

        private static Bitmap CreateImage()
        {
            Bitmap b = new Bitmap(GameEngine.tileSize.Width, GameEngine.tileSize.Height);

            using (Graphics g = Graphics.FromImage(b))
            {
                Brush pathBrush = new SolidBrush(pathColor);
                g.FillRectangle(pathBrush, new Rectangle(0,0, b.Width, b.Height));
            }

            return b;
        }


        public Path() : this(0, 0)
        { 
        }

        public Path(int x, int y)
            : base(CreateImage(), new Point(x, y))
        {
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        public override void UnDraw(Graphics g)
        {
            base.UnDraw(g);
        }

    }
}
