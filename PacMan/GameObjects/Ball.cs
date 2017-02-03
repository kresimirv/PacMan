using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class Ball : Sprite
    {
        private static string spriteFile = Application.StartupPath + "\\" + "Images" + "\\" + "LevelDesign.gif";

        private Point ballTileLocation;
        private BallType ballType = BallType.Standard;

        //frame number
        private int FrameS = 23;
        private int FrameP = 26;

        public enum BallType
        {
            Standard, 
            Power
        }


        public Point TileLocation
        {
            get
            {
                return ballTileLocation;
            }
            set
            {
                ballTileLocation = value;
            }
        }

        public BallType Type
        {
            get
            {
                return ballType;
            }
            set
            {
                ballType = value;
            }
        }



        public Ball() : this(0, 0, BallType.Standard)
        { 
        }

        public Ball(BallType type)
            : this(0, 0, type)
        {
        }

        public Ball(int x, int y, BallType type)
            : base(spriteFile, GameEngine.tileSize.Width, GameEngine.tileSize.Height, new Point(x,y))
        {
            switch(type)
            {
                case BallType.Standard:
                    base.CurrentFrame = FrameS;
                    break;

                case BallType.Power:
                    base.CurrentFrame = FrameP;
                    break;
            }

            ballType = type;
            
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
