using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PacMan
{
    public class PacMan : Sprite
    {
        private static string spriteFile = Application.StartupPath + "\\" + "Images" + "\\" + "Sprites.gif";

        private int pacManSpeed = 4;
        private PacManDirection pacManDirection = PacManDirection.Right;
        private PacManDirection pacManNextDirection = PacManDirection.Right;
        private Point pacManTileLocation;
 
        public enum PacManDirection
        {
            Up,
            Down,
            Left, 
            Right,
            None
        }

        //images
        private Bitmap PacManBodyU;
        private Bitmap PacManBodyD;
        private Bitmap PacManBodyL;
        private Bitmap PacManBodyR;

        private Bitmap PacManBodyEatU;
        private Bitmap PacManBodyEatD;
        private Bitmap PacManBodyEatL;
        private Bitmap PacManBodyEatR;

        //frames number
        private int FrameU = 43;
        private int FrameD = 41;
        private int FrameL = 42;
        private int FrameR = 40;

        private int FrameEatU = 47;
        private int FrameEatD = 45;
        private int FrameEatL = 46;
        private int FrameEatR = 44;

        //proximity detection rectangles
        Rectangle DetectU;
        Rectangle DetectD;
        Rectangle DetectL;
        Rectangle DetectR;

        private int proximityWidth;

        //pacman powers
        private bool hasPower = false;
        private int powerLastInterval = 3000; //3 sec
        System.Windows.Forms.Timer hasPowerTimer = new System.Windows.Forms.Timer();


        #region Properties
        public int Speed
        {
            get
            {
                return pacManSpeed;
            }
            set
            {
                pacManSpeed = value;
            }
        }

        public PacManDirection Direction
        {
            get
            {
                return pacManDirection;
            }
            set
            {
                pacManDirection = value;
            }
        }

        public PacManDirection NextDirection
        {
            get
            {
                return pacManNextDirection;
            }
            set
            {
                pacManNextDirection = value;
            }
        }

        public Point TileLocation
        {
            get
            {
                return pacManTileLocation;
            }
            set
            {
                pacManTileLocation = value;
            }
        }

        public bool HasPower
        {
            get
            {
                return hasPower;
            }
            set
            {
                hasPowerTimer.Start();
                hasPower = value;
            }
        }

        #endregion

        public PacMan(int x, int y) : base(spriteFile, 32, 32, new Point(x,y))
        {
            //load sprites
            PacManBodyU = base.GetFrame(FrameU);
            PacManBodyD = base.GetFrame(FrameD);
            PacManBodyL = base.GetFrame(FrameL);
            PacManBodyR = base.GetFrame(FrameR);

            PacManBodyEatU = base.GetFrame(FrameEatU);
            PacManBodyEatD = base.GetFrame(FrameEatD);
            PacManBodyEatL = base.GetFrame(FrameEatL);
            PacManBodyEatR = base.GetFrame(FrameEatR);

            base.CurrentFrame = FrameR; //set frame

            //set proximity detection rectangles
            proximityWidth = pacManSpeed - 1;

            DetectU = new Rectangle(this.Location.X + 1, this.Location.Y - proximityWidth, 30, proximityWidth);
            DetectD = new Rectangle(this.Location.X + 1, this.Location.Y + this.Size.Height, 30, proximityWidth);
            DetectL = new Rectangle(this.Location.X - proximityWidth, this.Location.Y + 1, proximityWidth, 30);
            DetectR = new Rectangle(this.Location.X + this.Size.Width, this.Location.Y + 1, proximityWidth, 30);

            hasPowerTimer.Tick += new EventHandler(hasPowerTimer_Tick);
            hasPowerTimer.Interval = powerLastInterval;
        }

        void hasPowerTimer_Tick(object sender, EventArgs e)
        {
            hasPower = false;
            hasPowerTimer.Stop();
        }

        public override void Draw(Graphics g)
        {
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
            if (CanMoveInDirection(pacManNextDirection) == true)
                pacManDirection = pacManNextDirection; //set current direction

            if (CanMoveInDirection(pacManDirection) == false)
                return false;

            bool isEating = this.IsEating();
 

            //set location and bitmap
            int x = base.Location.X;
            int y = base.Location.Y;

            switch (pacManDirection)
            {
                case PacManDirection.Up:
                    y -= pacManSpeed;
                    if (isEating)
                    {
                        base.CurrentFrame = FrameEatU;
                    }
                    else
                    {
                        base.CurrentFrame = FrameU;
                    }
                    break;

                case PacManDirection.Down:
                    y += pacManSpeed;
                    if (isEating)
                    {
                        base.CurrentFrame = FrameEatD;
                    }
                    else
                    {
                        base.CurrentFrame = FrameD;
                    }
                    break;

                case PacManDirection.Left:
                    x -= pacManSpeed;
                    if (isEating)
                    {
                        base.CurrentFrame = FrameEatL;
                    }
                    else
                    {
                        base.CurrentFrame = FrameL;
                    }
                    break;

                case PacManDirection.Right:
                    x += pacManSpeed;
                    if (isEating)
                    {
                        base.CurrentFrame = FrameEatR;
                    }
                    else
                    {
                        base.CurrentFrame = FrameR;
                    }

                    break;

                case PacManDirection.None:
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

        public bool CanMoveInDirection(PacManDirection direction)
        {
            bool result = true;

            PacManDirection currentDirection = direction;

            for(int r = 0; r < GameEngine.backLayer.Rows; r++)
            {
                for (int c = 0; c < GameEngine.backLayer.Columns; c++)
                {
                    if (GameEngine.backLayer.TileObject[c, r] == null)
                        continue;

                    if(GameEngine.backLayer.TileObject[c,r].IsWall == true)
                    {

                        Sprite wall = GameEngine.backLayer.TileObject[c,r].Sprite;

                        switch (currentDirection)
                        {
                            case PacManDirection.Up:
                                result = !wall.IsCollidingWithRectangleBased(DetectU);
                                break;

                            case PacManDirection.Down:
                                result = !wall.IsCollidingWithRectangleBased(DetectD);
                                break;

                            case PacManDirection.Left:
                                result = !wall.IsCollidingWithRectangleBased(DetectL);
                                break;

                            case PacManDirection.Right:
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


        private bool IsEating()
        {
            bool result = false;

            
            for (int i = 0; i < GameEngine.GameObjects.Count; i++)
            {
                if (GameEngine.GameObjects[i].GetType() == typeof(Ball))
                {
                    Ball ball = (Ball)GameEngine.GameObjects[i];

                    //collision with ball
                    if(this.IsCollidingWith(ball))
                    {
                        result = true;
                        break;
                    }

                    //will colide with ball
                    switch (Direction)
                    {
                        case PacManDirection.Up:
                            result = ball.IsCollidingWithRectangleBased(DetectU);
                            break;

                        case PacManDirection.Down:
                            result = ball.IsCollidingWithRectangleBased(DetectD);
                            break;

                        case PacManDirection.Left:
                            result = ball.IsCollidingWithRectangleBased(DetectL);
                            break;

                        case PacManDirection.Right:
                            result = ball.IsCollidingWithRectangleBased(DetectR);
                            break;
                    }

                    if (result == true)
                        break;

                }
            }

            return result;
        }
        

    }
}
