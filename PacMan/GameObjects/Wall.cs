using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class Wall : Sprite
    {
        private static Color wallColor = Color.Blue;
        private static int wallHeight = 5;

        public enum WallType
        {
            Horizontal,
            Vertical,
            Left,
            Right,
            TopRight,
            TopLeft,
            BottomRight,
            BottomLeft,
            TopJunction,
            BottomJunction
            
        }

        private static Bitmap CreateImage(WallType type)
        {
            Bitmap b = new Bitmap(GameEngine.tileSize.Width, GameEngine.tileSize.Height);

            using (Graphics g = Graphics.FromImage(b))
            {
                Brush wallBrush = new SolidBrush(wallColor);

                switch (type)
                {
                    case WallType.Horizontal:
                        g.FillRectangle(wallBrush, new Rectangle(0, b.Height /2, b.Width, wallHeight));
                        break;

                    case WallType.Vertical:
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, 0, wallHeight, b.Height));
                        break;
                  
                    case WallType.Left:
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, 0 , wallHeight, b.Height));
                        break;

                    case WallType.Right:
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, 0, wallHeight, b.Height));
                        break;

                    case WallType.TopRight:
                        g.FillRectangle(wallBrush, new Rectangle(0, b.Height / 2, b.Width / 2, wallHeight));
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, b.Height / 2, wallHeight, b.Height / 2));
                        break;

                    case WallType.TopLeft:
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, b.Height /2, b.Width / 2, wallHeight));
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, b.Height / 2, wallHeight, b.Height / 2));
                        break;

                    case WallType.TopJunction:
                        g.FillRectangle(wallBrush, new Rectangle(0, b.Height / 2, b.Width, wallHeight));
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, b.Height / 2, wallHeight, b.Height));
                        break;

                    case WallType.BottomLeft:
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, b.Height / 2, b.Width / 2, wallHeight));
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, 0, wallHeight, b.Height / 2));
                        break;
                        
                    case WallType.BottomRight:
                        g.FillRectangle(wallBrush, new Rectangle(0, b.Height / 2, b.Width / 2 + wallHeight, wallHeight));
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, 0, wallHeight, b.Height / 2));
                        break;

                    case WallType.BottomJunction:
                        g.FillRectangle(wallBrush, new Rectangle(0, b.Height / 2, b.Width, wallHeight));
                        g.FillRectangle(wallBrush, new Rectangle(b.Width / 2, 0, wallHeight, b.Height/2));
                        break;

                    
                }
            }

            return b;
        }


        public Wall(WallType type) : this(0, 0, type)
        { 
        }

        public Wall(int x, int y, WallType type) : base(CreateImage(type), new Point(x, y))  
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
