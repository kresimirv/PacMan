using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PacMan
{
    public class Ghost : Sprite
    {
        private static string spriteFile = Application.StartupPath + "\\" + "Images" + "\\" + "Sprites.gif";

        private int ghostSpeed = 2;
        private GhostDirection ghostDirection = GhostDirection.Right;
        private GhostDirection ghostNextDirection = GhostDirection.Right;
        private Point ghostTileLocation;
        private bool isChasing = true;
        private bool hasLOS = false;


        public AIGhost ghostAi;


        public enum GhostDirection
        {
            Up,
            Down,
            Left, 
            Right,
            None
        }

        private Bitmap GhostBodyU;
        private Bitmap GhostBodyD;
        private Bitmap GhostBodyL;
        private Bitmap GhostBodyR;
        private Bitmap GhostBodyScared;

        //frames number
        private int FrameU = 3;
        private int FrameD = 1;
        private int FrameL = 2;
        private int FrameR = 0;
        private int FrameScared = 48;

        //proximity detection rectangles
        Rectangle DetectU;
        Rectangle DetectD;
        Rectangle DetectL;
        Rectangle DetectR;

        private int proximityWidth;

        #region Properties
        public int Speed
        {
            get
            {
                return ghostSpeed;
            }
            set
            {
                ghostSpeed = value;
            }
        }

        public GhostDirection Direction
        {
            get
            {
                return ghostDirection;
            }
            set
            {
                ghostDirection = value;
            }
        }

        public GhostDirection NextDirection
        {
            get
            {
                return ghostNextDirection;
            }
            set
            {
                ghostNextDirection = value;
            }
        }

        public Point TileLocation
        {
            get
            {
                return ghostTileLocation;
            }
            set
            {
                ghostTileLocation = value;
            }
        }

        public bool HasLineOfSight
        {
            get
            {
                return hasLOS;
            }
            set
            {
                hasLOS = value;
            }
        }

        public bool IsChasing
        {
            get
            {
                return isChasing;
            }
            set
            {
                isChasing = value;
            }
        }

        #endregion

        public Ghost(int x, int y) : base(spriteFile, 32, 32, new Point(x,y))
        {
            //sprite
            Random rand = new Random();
            int random = rand.Next(0, 9);

            //calculate frame numbers for images
            FrameU += random * 4;
            FrameD += random * 4;
            FrameL += random * 4;
            FrameR += random * 4;

            //load sprites
            GhostBodyU = base.GetFrame(FrameU);
            GhostBodyD = base.GetFrame(FrameD);
            GhostBodyL = base.GetFrame(FrameL);
            GhostBodyR = base.GetFrame(FrameR);
            GhostBodyScared = base.GetFrame(FrameScared);

            base.CurrentFrame = FrameR;


            //set proximity detection rectangles
            proximityWidth = ghostSpeed - 1;

            DetectU = new Rectangle(this.Location.X + 1, this.Location.Y - proximityWidth, 30, proximityWidth);
            DetectD = new Rectangle(this.Location.X + 1, this.Location.Y + this.Size.Height, 30, proximityWidth);
            DetectL = new Rectangle(this.Location.X - proximityWidth, this.Location.Y + 1, proximityWidth, 30);
            DetectR = new Rectangle(this.Location.X + this.Size.Width, this.Location.Y + 1, proximityWidth, 30);

            //ghost AI
            ghostAi = new AIGhost(this);
        }

        public override void Draw(Graphics g)
        {
            //for testing
            //if (HasLineOfSight)
            //{
            //    g.DrawRectangle(new Pen(Brushes.Red), DetectU);
            //    g.DrawRectangle(new Pen(Brushes.Red), DetectD);
            //    g.DrawRectangle(new Pen(Brushes.Red), DetectL);
            //    g.DrawRectangle(new Pen(Brushes.Red), DetectR);
            //}
            base.Draw(g);
        }

        public override void UnDraw(Graphics g)
        {
            base.UnDraw(g);
        }

        public bool Move()
        {
            bool result = false;

            //if can move in next direction
            if (CanMoveInDirection(ghostNextDirection) == true)
                ghostDirection = ghostNextDirection; //set current direction

            //can move in direction
            if (CanMoveInDirection(ghostDirection) == false)
                return false;

            int x = base.Location.X;
            int y = base.Location.Y;

            

            switch (ghostDirection)
            {
                case GhostDirection.Up:
                    y -= ghostSpeed;

                    if (isChasing == true)
                        base.CurrentFrame = FrameU;
                    else
                        base.CurrentFrame = FrameScared;

                    break;

                case GhostDirection.Down:
                    y += ghostSpeed;

                    if (isChasing == true)
                        base.CurrentFrame = FrameD;
                    else
                        base.CurrentFrame = FrameScared;
                    break;

                case GhostDirection.Left:
                    x -= ghostSpeed;
                    if (isChasing == true)
                        base.CurrentFrame = FrameL;
                    else
                        base.CurrentFrame = FrameScared;
                    break;

                case GhostDirection.Right:
                    x += ghostSpeed;

                    if (isChasing == true)
                        base.CurrentFrame = FrameR;
                    else
                        base.CurrentFrame = FrameScared;
                    break;

                case GhostDirection.None:
                    break;
                
            }


            base.Location = new Point(x, y);
            SetProximityDetectionRectangles(); //set detection rectangles location

            return result;
        }

        private void SetProximityDetectionRectangles()
        {
            //proximity detection rectangles
            DetectU.Location = new Point(this.Location.X + 1, this.Location.Y - proximityWidth);
            DetectD.Location = new Point(this.Location.X + 1, this.Location.Y + this.Size.Height);
            DetectL.Location = new Point(this.Location.X - proximityWidth, this.Location.Y + 1);
            DetectR.Location = new Point(this.Location.X + this.Size.Width, this.Location.Y + 1);
        }

        public bool CanMoveInDirection(GhostDirection direction)
        {
            bool result = true;

            GhostDirection currentDirection = direction;

            for (int r = 0; r < GameEngine.backLayer.Rows; r++)
            {
                for (int c = 0; c < GameEngine.backLayer.Columns; c++)
                {
                    if (GameEngine.backLayer.TileObject[c, r] == null)
                        continue;

                    if (GameEngine.backLayer.TileObject[c, r].IsWall == true)
                    {

                        Sprite wall = GameEngine.backLayer.TileObject[c, r].Sprite;

                        switch (currentDirection)
                        {
                            case GhostDirection.Up:
                                result = !wall.IsCollidingWithRectangleBased(DetectU);
                                break;

                            case GhostDirection.Down:
                                result = !wall.IsCollidingWithRectangleBased(DetectD);
                                break;

                            case GhostDirection.Left:
                                result = !wall.IsCollidingWithRectangleBased(DetectL);
                                break;

                            case GhostDirection.Right:
                                result = !wall.IsCollidingWithRectangleBased(DetectR);
                                break;
                        }

                        if (result == false)
                            break;

                    }//if

                }//for
                if (result == false)
                    break;
            }//for

            return result;
        }


    }
}
