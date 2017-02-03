using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PacMan
{
    public class Sprite
    {
        private Bitmap _spriteImage;
        private int _frameWidth;
        private int _frameHeight;
        private int _currentFrame;
        private int _totalFrames;
        private Rectangle _collisionRectangle;
        private Point _location;

        public List<Bitmap> Frames = new List<Bitmap>();


        #region Properties
        public int CurrentFrame
        {
            get
            {
                return _currentFrame;
            }
            set
            {
                _currentFrame = value;
            }
        }

        public int TotalFrames
        {
            get
            {
                return _totalFrames;
            }
        }

        public Size Size
        {
            get
            {
                return new Size(_frameWidth, _frameHeight);
            }
        }

        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                _collisionRectangle = new Rectangle(_location, Size);
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return _collisionRectangle;
            }
        }
        #endregion


        public Sprite(Bitmap image, Point location)
        {
            _spriteImage = image;
            _location = location;
            _frameHeight = image.Height; ;
            _frameWidth = image.Width;
            _currentFrame = 0;
            _totalFrames = 0;
            _collisionRectangle = new Rectangle(_location.X, _location.Y, _frameWidth, _frameHeight);

            CreateImageFrames(); //create frame
        }

        public Sprite(string imageFile, int frameWidth, int frameHeight, Point location)
        {
            _spriteImage = BitmapTools.LoadTransparent(imageFile);
            _frameHeight = frameHeight;
            _frameWidth = frameWidth;
            _currentFrame = 0;
            _totalFrames = 0;
            _location = location;
            _collisionRectangle = new Rectangle(_location.X, _location.Y, _frameWidth, _frameHeight);

            CreateImageFrames(); //create frames
        }

        public void CreateImageFrames()
        {
            //calculate rows & columns
            int imageWidth = _spriteImage.Width;
            int imageHeight = _spriteImage.Height;

            int rows = imageHeight / _frameHeight;
            int columns = imageWidth / _frameWidth;
            _totalFrames = rows * columns;

            //create frames
            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    //calculate crop rectangle
                    int x = c * _frameWidth;
                    int y = r * _frameHeight;
                    Rectangle cropRect = new Rectangle(new Point(x, y), new Size(_frameWidth, _frameHeight));
                    Rectangle destRect = new Rectangle(0, 0, _frameWidth, _frameHeight);

                    //create empty bitmap
                    Bitmap b = new Bitmap(_frameWidth, _frameHeight);

                    //crop image
                    using (Graphics g = Graphics.FromImage(b))
                    {
                        g.DrawImage(_spriteImage, destRect, cropRect, GraphicsUnit.Pixel);
                    }

                    //add to array
                    Frames.Add(b);
                }
            }//for
                
        }

        public virtual void Draw(Graphics g)
        {
            Graphics graphBack = g;
            graphBack.DrawImageUnscaled(Frames[_currentFrame], Location.X, Location.Y);
        }

        public virtual void Draw(Graphics g, int frame)
        {
            Graphics graphBack = g;
            graphBack.DrawImageUnscaled(Frames[frame], Location.X, Location.Y);
        }

        public virtual void UnDraw(Graphics g)
        {
            Graphics graphBack = g;
            graphBack.FillEllipse(new SolidBrush(GameEngine.BackgroundColor), Location.X, Location.Y, Size.Width, Size.Height);
        }

        public Bitmap GetFrame(int frameNumber)
        {
            if (frameNumber >= TotalFrames)
                return null;

            return Frames[frameNumber];
        }

        public Bitmap NextFrame()
        {
            _currentFrame++;
            if (_currentFrame >= _totalFrames)
                _currentFrame = 0;
            return Frames[_currentFrame];
        }

        public Bitmap PreviousFrame()
        {
            _currentFrame--;
            if (_currentFrame < 0)
                _currentFrame = _totalFrames - 1;

            return Frames[_currentFrame];
        }

        public bool IsCollidingWithRectangleBased(Sprite anotherSprite)
        {
            bool result = false;

            Rectangle rectangle1 = _collisionRectangle;
            Rectangle rectangle2 = anotherSprite._collisionRectangle;

            //collision detection
            if ((rectangle1.X <= rectangle2.X + rectangle2.Width &&
                rectangle1.Y <= rectangle2.Y + rectangle2.Height) &&
                (rectangle2.X <= rectangle1.X + rectangle1.Width &&
                rectangle2.Y <= rectangle1.Y + rectangle1.Height))
            {
                result = true;
                
            }
            
            return result;
        }

        public bool IsCollidingWithRectangleBased(Rectangle rectangle)
        {
            bool result = false;

            Rectangle rectangle1 = _collisionRectangle;
            Rectangle rectangle2 = rectangle;

            //collision detection
            if ((rectangle1.X <= rectangle2.X + rectangle2.Width &&
                rectangle1.Y <= rectangle2.Y + rectangle2.Height) &&
                (rectangle2.X <= rectangle1.X + rectangle1.Width &&
                rectangle2.Y <= rectangle1.Y + rectangle1.Height))
            {
                result = true;

            }

            return result;
        }

        public bool IsCollidingWithPixelBased(Sprite anotherSprite)
        {
            //get sprite collsion rectangles and current bitmaps
            Rectangle rectangleA = this.CollisionRectangle;
            Rectangle rectangleB = anotherSprite.CollisionRectangle;

            Bitmap BitmapA = this.Frames[this.CurrentFrame];
            Bitmap BitmapB = anotherSprite.Frames[anotherSprite.CurrentFrame];

            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = BitmapA.GetPixel(x - rectangleA.Left, y - rectangleA.Top);
                    Color colorB = BitmapB.GetPixel(x - rectangleB.Left, y - rectangleB.Top);
                   
                    // If both pixels are not completely transparent
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        public bool IsCollidingWith(Sprite anotherSprite)
        {
            bool result = false;

            result = (this.IsCollidingWithRectangleBased(anotherSprite) 
                && this.IsCollidingWithPixelBased(anotherSprite));
            return result;
        }


    }
}
