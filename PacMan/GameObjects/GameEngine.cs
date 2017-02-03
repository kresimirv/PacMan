using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace PacMan
{
    public class GameEngine
    {
        //game engine properties
        public static Color BackgroundColor;
        public static Size tileSize = new Size(32, 32);
        public static int gameFieldWidth = 20; //width in tiles
        public static int gameFieldHeight = 13; //height in tiles

        public bool isRunning = false;
        public bool isPaused = false;
        public bool isGameOver = false;
        public bool isGameWon = false;

        //game objects
        public static List<Sprite> GameObjects;
        public PacMan pacMan;
        private Ghost ghost1;
        private Ghost ghost2;
        private Ghost ghost3;
        private Ghost ghost4;
        private Ghost ghost5;

        private int pacManSpeed = 4;
        private int ghostSpeed = 2;
       
        //layers
        public static TiledLayer backLayer;
        public static TiledLayer foodLayer;

        public static List<Point> safePlace; //for ghost chasing
        private static Point spawningPlace;


        //score tracking
        private int maxGhosts = 0;
        private int totalGhosts = 0;
        private int totalBalls = 0;
        private int totalScore = 0;
        private int scoreIncrementRegular = 10; //score increment
        private int scoreIncrementGhost = 100; //score increment



        public static Size GameFieldSize()
        {
            return new Size(gameFieldWidth * tileSize.Width, gameFieldHeight * tileSize.Height);
        }


        public void CreateGameField()
        {
            //game engine variables
            isRunning = true;
            isGameOver = false;
            isPaused = false;
            isGameWon = false;

            //score tracking
            totalBalls = 0;
            totalScore = 0;

            backLayer = new TiledLayer(gameFieldWidth, gameFieldHeight, tileSize);

            //create background
            backLayer.Add(new Wall(Wall.WallType.TopLeft), true, 0, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 1, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 2, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 3, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 4, 1);
            backLayer.Add(new Wall(Wall.WallType.TopJunction), true, 5, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 6, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 7, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 8, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 9, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 10, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 11, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 12, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 13, 1);
            backLayer.Add(new Wall(Wall.WallType.TopJunction), true, 14, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 15, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 16, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 17, 1);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 18, 1);
            backLayer.Add(new Wall(Wall.WallType.TopRight), true, 19, 1);

            backLayer.Add(new Wall(Wall.WallType.Left), true, 0, 2);
            backLayer.Add(new Path(), false, 1, 2);
            backLayer.Add(new Path(), false, 2, 2);
            backLayer.Add(new Path(), false, 3, 2);
            backLayer.Add(new Path(), false, 4, 2);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 5, 2);
            backLayer.Add(new Path(), false, 6, 2);
            backLayer.Add(new Path(), false, 7, 2);
            backLayer.Add(new Path(), false, 8, 2);
            backLayer.Add(new Path(), false, 9, 2);
            backLayer.Add(new Path(), false, 10, 2);
            backLayer.Add(new Path(), false, 11, 2);
            backLayer.Add(new Path(), false, 12, 2);
            backLayer.Add(new Path(), false, 13, 2);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 14, 2);
            backLayer.Add(new Path(), false, 15, 2);
            backLayer.Add(new Path(), false, 16, 2);
            backLayer.Add(new Path(), false, 17, 2);
            backLayer.Add(new Path(), false, 18, 2);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 19, 2);

            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 0, 3);
            backLayer.Add(new Path(), false, 1, 3);
            backLayer.Add(new Wall(Wall.WallType.TopLeft), true, 2, 3);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 3, 3);
            backLayer.Add(new Path(), false, 4, 3);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 5, 3);
            backLayer.Add(new Path(), false, 6, 3);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 7, 3);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 8, 3);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 9, 3);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 10, 3);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 11, 3);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 12, 3);
            backLayer.Add(new Path(), false, 13, 3);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 14, 3);
            backLayer.Add(new Path(), false, 15, 3);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 16, 3);
            backLayer.Add(new Wall(Wall.WallType.TopRight), true, 17, 3);
            backLayer.Add(new Path(), false, 18, 3);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 19, 3);

            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 0, 4);
            backLayer.Add(new Path(), false, 1, 4);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 2, 4);
            backLayer.Add(new Path(), false, 3, 4);
            backLayer.Add(new Path(), false, 4, 4);
            backLayer.Add(new Path(), false, 5, 4);
            backLayer.Add(new Path(), false, 6, 4);
            backLayer.Add(new Path(), false, 7, 4);
            backLayer.Add(new Path(), false, 8, 4);
            backLayer.Add(new Path(), false, 9, 4);
            backLayer.Add(new Path(), false, 10, 4);
            backLayer.Add(new Path(), false, 11, 4);
            backLayer.Add(new Path(), false, 12, 4);
            backLayer.Add(new Path(), false, 13, 4);
            backLayer.Add(new Path(), false, 14, 4);
            backLayer.Add(new Path(), false, 15, 4);
            backLayer.Add(new Path(), false, 16, 4);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 17, 4);
            backLayer.Add(new Path(), false, 18, 4);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 19, 4);

            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 0, 5);
            backLayer.Add(new Path(), false, 1, 5);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 2, 5);
            backLayer.Add(new Path(), false, 3, 5);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 4, 5);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 5, 5);
            backLayer.Add(new Path(), false, 6, 5);
            backLayer.Add(new Wall(Wall.WallType.TopLeft), true, 7, 5);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 8, 5);
            backLayer.Add(new Path(), false, 9, 5);
            backLayer.Add(new Path(), false, 10, 5);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 11, 5);
            backLayer.Add(new Wall(Wall.WallType.TopRight), true, 12, 5);
            backLayer.Add(new Path(), false, 13, 5);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 14, 5);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 15, 5);
            backLayer.Add(new Path(), false, 16, 5);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 17, 5);
            backLayer.Add(new Path(), false, 18, 5);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 19, 5);

            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 0, 6);
            backLayer.Add(new Path(), false, 1, 6);
            backLayer.Add(new Path(), false, 2, 6);
            backLayer.Add(new Path(), false, 3, 6);
            backLayer.Add(new Path(), false, 4, 6);
            backLayer.Add(new Path(), false, 5, 6);
            backLayer.Add(new Path(), false, 6, 6);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 7, 6);
            backLayer.Add(new Path(), false, 8, 6);
            backLayer.Add(new Path(), false, 9, 6);
            backLayer.Add(new Path(), false, 10, 6);
            backLayer.Add(new Path(), false, 11, 6);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 12, 6);
            backLayer.Add(new Path(), false, 13, 6);
            backLayer.Add(new Path(), false, 14, 6);
            backLayer.Add(new Path(), false, 15, 6);
            backLayer.Add(new Path(), false, 16, 6);
            backLayer.Add(new Path(), false, 17, 6);
            backLayer.Add(new Path(), false, 18, 6);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 19, 6);

            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 0, 7);
            backLayer.Add(new Path(), false, 1, 7);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 2, 7);
            backLayer.Add(new Path(), false, 3, 7);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 4, 7);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 5, 7);
            backLayer.Add(new Path(), false, 6, 7);
            backLayer.Add(new Wall(Wall.WallType.BottomLeft), true, 7, 7);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 8, 7);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 9, 7);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 10, 7);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 11, 7);
            backLayer.Add(new Wall(Wall.WallType.BottomRight), true, 12, 7);
            backLayer.Add(new Path(), false, 13, 7);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 14, 7);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 15, 7);
            backLayer.Add(new Path(), false, 16, 7);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 17, 7);
            backLayer.Add(new Path(), false, 18, 7);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 19, 7);


            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 0, 8);
            backLayer.Add(new Path(), false, 1, 8);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 2, 8);
            backLayer.Add(new Path(), false, 3, 8);
            backLayer.Add(new Path(), false, 4, 8);
            backLayer.Add(new Path(), false, 5, 8);
            backLayer.Add(new Path(), false, 6, 8);
            backLayer.Add(new Path(), false, 7, 8);
            backLayer.Add(new Path(), false, 8, 8);
            backLayer.Add(new Path(), false, 9, 8);
            backLayer.Add(new Path(), false, 10, 8);
            backLayer.Add(new Path(), false, 11, 8);
            backLayer.Add(new Path(), false, 12, 8);
            backLayer.Add(new Path(), false, 13, 8);
            backLayer.Add(new Path(), false, 14, 8);
            backLayer.Add(new Path(), false, 15, 8);
            backLayer.Add(new Path(), false, 16, 8);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 17, 8);
            backLayer.Add(new Path(), false, 18, 8);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 19, 8);

            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 0, 9);
            backLayer.Add(new Path(), false, 1, 9);
            backLayer.Add(new Wall(Wall.WallType.BottomLeft), true, 2, 9);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 3, 9);
            backLayer.Add(new Path(), false, 4, 9);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 5, 9);
            backLayer.Add(new Path(), false, 6, 9);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 7, 9);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 8, 9);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 9, 9);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 10, 9);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 11, 9);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 12, 9);
            backLayer.Add(new Path(), false, 13, 9);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 14, 9);
            backLayer.Add(new Path(), false, 15, 9);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 16, 9);
            backLayer.Add(new Wall(Wall.WallType.BottomRight), true, 17, 9);
            backLayer.Add(new Path(), false, 18, 9);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 19, 9);

            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 0, 10);
            backLayer.Add(new Path(), false, 1, 10);
            backLayer.Add(new Path(), false, 2, 10);
            backLayer.Add(new Path(), false, 3, 10);
            backLayer.Add(new Path(), false, 4, 10);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 5, 10);
            backLayer.Add(new Path(), false, 6, 10);
            backLayer.Add(new Path(), false, 7, 10);
            backLayer.Add(new Path(), false, 8, 10);
            backLayer.Add(new Path(), false, 9, 10);
            backLayer.Add(new Path(), false, 10, 10);
            backLayer.Add(new Path(), false, 11, 10);
            backLayer.Add(new Path(), false, 12, 10);
            backLayer.Add(new Path(), false, 13, 10);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 14, 10);
            backLayer.Add(new Path(), false, 15, 10);
            backLayer.Add(new Path(), false, 16, 10);
            backLayer.Add(new Path(), false, 17, 10);
            backLayer.Add(new Path(), false, 18, 10);
            backLayer.Add(new Wall(Wall.WallType.Vertical), true, 19, 10);

            backLayer.Add(new Wall(Wall.WallType.BottomLeft), true, 0, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 1, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 2, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 3, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 4, 11);
            backLayer.Add(new Wall(Wall.WallType.BottomJunction), true, 5, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 6, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 7, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 8, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 9, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 10, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 11, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 12, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 13, 11);
            backLayer.Add(new Wall(Wall.WallType.BottomJunction), true, 14, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 15, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 16, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 17, 11);
            backLayer.Add(new Wall(Wall.WallType.Horizontal), true, 18, 11);
            backLayer.Add(new Wall(Wall.WallType.BottomRight), true, 19, 11);


            //food layer
            foodLayer = new TiledLayer(gameFieldWidth, gameFieldHeight, tileSize);
            foodLayer.Add(new Ball(), false, 1, 2);
            foodLayer.Add(new Ball(), false, 2, 2);
            foodLayer.Add(new Ball(), false, 3, 2);
            foodLayer.Add(new Ball(), false, 4, 2);
            foodLayer.Add(new Ball(), false, 6, 2);
            foodLayer.Add(new Ball(), false, 7, 2);
            foodLayer.Add(new Ball(), false, 8, 2);
            foodLayer.Add(new Ball(), false, 9, 2);
            foodLayer.Add(new Ball(), false, 10, 2);
            foodLayer.Add(new Ball(), false, 11, 2);
            foodLayer.Add(new Ball(), false, 12, 2);
            foodLayer.Add(new Ball(), false, 13, 2);
            foodLayer.Add(new Ball(), false, 15, 2);
            foodLayer.Add(new Ball(), false, 16, 2);
            foodLayer.Add(new Ball(), false, 17, 2);
            foodLayer.Add(new Ball(Ball.BallType.Power), false, 18, 2);

            foodLayer.Add(new Ball(), false, 1, 3);
            foodLayer.Add(new Ball(), false, 4, 3);
            foodLayer.Add(new Ball(), false, 6, 3);
            foodLayer.Add(new Ball(), false, 13, 3);
            foodLayer.Add(new Ball(), false, 15, 3);
            foodLayer.Add(new Ball(), false, 18, 3);
            
            foodLayer.Add(new Ball(), false, 1, 4);
            foodLayer.Add(new Ball(), false, 3, 4);
            foodLayer.Add(new Ball(), false, 4, 4);
            foodLayer.Add(new Ball(), false, 5, 4);
            foodLayer.Add(new Ball(), false, 6, 4);
            foodLayer.Add(new Ball(), false, 7, 4);
            foodLayer.Add(new Ball(), false, 8, 4);
            foodLayer.Add(new Ball(), false, 9, 4);
            foodLayer.Add(new Ball(), false, 10, 4);
            foodLayer.Add(new Ball(), false, 11, 4);
            foodLayer.Add(new Ball(), false, 12, 4);
            foodLayer.Add(new Ball(), false, 13, 4);
            foodLayer.Add(new Ball(), false, 14, 4);
            foodLayer.Add(new Ball(), false, 15, 4);
            foodLayer.Add(new Ball(), false, 16, 4);
            foodLayer.Add(new Ball(), false, 18, 4);

            foodLayer.Add(new Ball(), false, 1, 5);
            foodLayer.Add(new Ball(), false, 3, 5);
            foodLayer.Add(new Ball(), false, 6, 5);
            foodLayer.Add(new Ball(), false, 13, 5);
            foodLayer.Add(new Ball(), false, 16, 5);
            foodLayer.Add(new Ball(), false, 18, 5);

            foodLayer.Add(new Ball(), false, 1, 6);
            foodLayer.Add(new Ball(), false, 2, 6);
            foodLayer.Add(new Ball(), false, 3, 6);
            foodLayer.Add(new Ball(), false, 4, 6);
            foodLayer.Add(new Ball(), false, 5, 6);
            foodLayer.Add(new Ball(), false, 6, 6);
            foodLayer.Add(new Ball(), false, 13, 6);
            foodLayer.Add(new Ball(), false, 14, 6);
            foodLayer.Add(new Ball(), false, 15, 6);
            foodLayer.Add(new Ball(), false, 16, 6);
            foodLayer.Add(new Ball(), false, 17, 6);
            foodLayer.Add(new Ball(), false, 18, 6);

            foodLayer.Add(new Ball(), false, 1, 7);
            foodLayer.Add(new Ball(), false, 3, 7);
            foodLayer.Add(new Ball(), false, 6, 7);
            foodLayer.Add(new Ball(), false, 13, 7);
            foodLayer.Add(new Ball(), false, 16, 7);
            foodLayer.Add(new Ball(), false, 18, 7);

            foodLayer.Add(new Ball(), false, 1, 8);
            foodLayer.Add(new Ball(), false, 3, 8);
            foodLayer.Add(new Ball(), false, 4, 8);
            foodLayer.Add(new Ball(), false, 5, 8);
            foodLayer.Add(new Ball(), false, 6, 8);
            foodLayer.Add(new Ball(), false, 7, 8);
            foodLayer.Add(new Ball(), false, 8, 8);
            foodLayer.Add(new Ball(), false, 9, 8);
            foodLayer.Add(new Ball(), false, 10, 8);
            foodLayer.Add(new Ball(), false, 11, 8);
            foodLayer.Add(new Ball(), false, 12, 8);
            foodLayer.Add(new Ball(), false, 13, 8);
            foodLayer.Add(new Ball(), false, 14, 8);
            foodLayer.Add(new Ball(), false, 15, 8);
            foodLayer.Add(new Ball(), false, 16, 8);
            foodLayer.Add(new Ball(), false, 18, 8);

            foodLayer.Add(new Ball(), false, 1, 9);
            foodLayer.Add(new Ball(), false, 4, 9);
            foodLayer.Add(new Ball(), false, 6, 9);
            foodLayer.Add(new Ball(), false, 13, 9);
            foodLayer.Add(new Ball(), false, 15, 9);
            foodLayer.Add(new Ball(), false, 18, 9);

            foodLayer.Add(new Ball(Ball.BallType.Power), false, 1, 10);
            foodLayer.Add(new Ball(), false, 2, 10);
            foodLayer.Add(new Ball(), false, 3, 10);
            foodLayer.Add(new Ball(), false, 4, 10);
            foodLayer.Add(new Ball(), false, 6, 10);
            foodLayer.Add(new Ball(), false, 7, 10);
            foodLayer.Add(new Ball(), false, 8, 10);
            foodLayer.Add(new Ball(), false, 9, 10);
            foodLayer.Add(new Ball(), false, 10, 10);
            foodLayer.Add(new Ball(), false, 11, 10);
            foodLayer.Add(new Ball(), false, 12, 10);
            foodLayer.Add(new Ball(), false, 13, 10);
            foodLayer.Add(new Ball(), false, 15, 10);
            foodLayer.Add(new Ball(), false, 16, 10);
            foodLayer.Add(new Ball(), false, 17, 10);
            foodLayer.Add(new Ball(Ball.BallType.Power), false, 18, 10);


            //safeplaces
            safePlace = new List<Point>();
            safePlace.Add(new Point(1, 2));
            safePlace.Add(new Point(18, 2));
            safePlace.Add(new Point(1, 10));
            safePlace.Add(new Point(18, 10));
            
            //spawning place
            spawningPlace = new Point(8, 6);


            //GAME OBJECTS
            GameObjects = new List<Sprite>();

            //add food object to game objects
            for (int c = 0; c < foodLayer.Columns; c++)
            {
                for (int r = 0; r < foodLayer.Rows; r++)
                {
                    if (foodLayer.TileObject[c, r] != null)
                    {

                        //with ball
                        if (foodLayer.TileObject[c, r].Sprite.GetType() == typeof(Ball))
                        {
                            Ball ball = (Ball)foodLayer.TileObject[c, r].Sprite;
                            ball.TileLocation = new Point(c, r);
                            GameObjects.Add(ball);
                            totalBalls++;

                        }
                        

                    }

                }
            }

            
            //create pacman
            pacMan = new PacMan(backLayer.TileObject[1,2].Sprite.Location.X,
                backLayer.TileObject[1, 2].Sprite.Location.Y);
            pacMan.TileLocation = new Point(1, 2);
            pacMan.Speed = pacManSpeed;


            //create ghosts
            ghost1 = new Ghost(backLayer.TileObject[8, 6].Sprite.Location.X, backLayer.TileObject[8, 6].Sprite.Location.Y);
            ghost1.TileLocation = new Point(8, 6);
            ghost1.Speed = ghostSpeed;

            ghost2 = new Ghost(backLayer.TileObject[18, 2].Sprite.Location.X, backLayer.TileObject[18, 2].Sprite.Location.Y);
            ghost2.TileLocation = new Point(18, 2);
            ghost2.Speed = ghostSpeed;

            ghost3 = new Ghost(backLayer.TileObject[18, 10].Sprite.Location.X, backLayer.TileObject[18, 10].Sprite.Location.Y);
            ghost3.TileLocation = new Point(18, 10);
            ghost3.Speed = ghostSpeed;

            ghost4 = new Ghost(backLayer.TileObject[13, 10].Sprite.Location.X, backLayer.TileObject[13, 10].Sprite.Location.Y);
            ghost4.TileLocation = new Point(13, 10);
            ghost4.Speed = ghostSpeed;

            ghost5 = new Ghost(backLayer.TileObject[13, 2].Sprite.Location.X, backLayer.TileObject[13, 2].Sprite.Location.Y);
            ghost5.TileLocation = new Point(13, 2);
            ghost5.Speed = ghostSpeed;

            //add ghosts to game objects
            GameObjects.Add(ghost1);
            GameObjects.Add(ghost2);
            GameObjects.Add(ghost3);
            GameObjects.Add(ghost4);
            GameObjects.Add(ghost5);

            maxGhosts = totalGhosts = 5;
        }

        private void Draw(Graphics g)
        {
            //draw layers
            backLayer.Draw(g);
            foodLayer.Draw(g);
            
            //draw pacman
            pacMan.Draw(g);

            //draw ghosts
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Draw(g);
            }

            //draw score
            g.DrawString("Score: " + totalScore, new Font("Arial", 12f, FontStyle.Bold), new SolidBrush(Color.Yellow), new Point(0,0));

            //draw
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            //is game over
            if (this.isGameOver && this.isGameWon == false)
            {
                g.DrawString("GAME OVER", new Font("Arial", 32f, FontStyle.Bold), new SolidBrush(Color.Yellow), GameEngine.GameFieldSize().Width / 2, GameEngine.GameFieldSize().Height / 3, stringFormat);
                g.DrawString("Press ENTER for new game", new Font("Arial", 18f, FontStyle.Bold), new SolidBrush(Color.Yellow), GameEngine.GameFieldSize().Width / 2, GameEngine.GameFieldSize().Height/2, stringFormat);
            }
            //is game won
            if (this.isGameOver && this.isGameWon)
            {
                g.DrawString("WINNER!", new Font("Arial", 32f, FontStyle.Bold), new SolidBrush(Color.Yellow), GameEngine.GameFieldSize().Width / 2, GameEngine.GameFieldSize().Height / 3, stringFormat);
                g.DrawString("Press ENTER for new game", new Font("Arial", 18f, FontStyle.Bold), new SolidBrush(Color.Yellow), GameEngine.GameFieldSize().Width / 2, GameEngine.GameFieldSize().Height / 2, stringFormat);
            }
            //is power up active
            if (this.isGameOver == false && pacMan.HasPower)
            {
                g.DrawString("POWER UP ACTIVATED!", new Font("Arial", 24f, FontStyle.Bold), new SolidBrush(Color.Yellow), GameEngine.GameFieldSize().Width / 2, GameEngine.GameFieldSize().Height / 4, stringFormat);
            }
            //if game is paused
            if (this.isPaused)
            {
                g.DrawString("GAME PAUSED", new Font("Arial", 24f, FontStyle.Bold), new SolidBrush(Color.Yellow), GameEngine.GameFieldSize().Width / 2, GameEngine.GameFieldSize().Height / 2, stringFormat);
            }

        }

        public void Render(Graphics g)
        {

            if (this.isRunning && this.isPaused == false)
            {
                //move
                GameLogic(g);
                MovePacman();
                MoveGhosts();
            }

            //draw
            Draw(g);
            
        }

        private void MovePacman()
        {
            pacMan.Move();
            if (FindCharacterLocation(pacMan) != new Point(-1, -1))
            {
                pacMan.TileLocation = FindCharacterLocation(pacMan);
            }
        }

        private void MoveGhosts()
        {
           
            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].GetType() == typeof(Ghost))
                {
                    
                    
                    Ghost ghost = (Ghost)GameObjects[i];
                    ghost.NextDirection = ghost.ghostAi.ChooseDirection(pacMan);
                    ghost.Move();

                    if (FindCharacterLocation(ghost) != new Point(-1, -1))
                    {
                        ghost.TileLocation = FindCharacterLocation(ghost); //location
                        
                    }
                }
            }
        }

        private void GameLogic(Graphics g)
        {

            if (totalGhosts < maxGhosts)
            {
                //respawn
                Ghost ghost = new Ghost(backLayer.TileObject[spawningPlace.X, spawningPlace.Y].Sprite.Location.X, backLayer.TileObject[spawningPlace.X, spawningPlace.Y].Sprite.Location.Y);
                ghost.TileLocation = new Point(spawningPlace.X, spawningPlace.Y);
                ghost.Speed = ghostSpeed;

                //add ghosts to game objects
                GameObjects.Add(ghost);
                totalGhosts++;
                
            }

            for (int i = 0; i < GameObjects.Count; i++)
            {
                //collision
                if (pacMan.IsCollidingWith(GameObjects[i]))
                {

                    //with ball
                    if (GameObjects[i].GetType() == typeof(Ball))
                    {
                        //remove ball
                        Ball ball = (Ball)GameObjects[i];
                        ball.UnDraw(g);
                        GameObjects.Remove(ball);
                        foodLayer.Remove(ball.TileLocation.X, ball.TileLocation.Y);

                        if (ball.Type == Ball.BallType.Power)
                        {
                            pacMan.HasPower = true;
                        }
                        totalBalls--;

                        i--;

                        //increment score
                        totalScore += scoreIncrementRegular;

                        continue;
                    }//if

                    //with ghost
                    if (GameObjects[i].GetType() == typeof(Ghost))
                    {
                        if (pacMan.HasPower == true)
                        {
                            //remove ghost
                            Ghost ghost = (Ghost)GameObjects[i];
                            ghost.UnDraw(g);
                            GameObjects.Remove(ghost);
                            totalGhosts--;
                            i--;

                            //increment score
                            totalScore += scoreIncrementGhost;

                            continue;

                        }
                        else
                        {
                            //game over
                            this.isRunning = false;
                            this.isGameOver = true;
                            this.isGameWon = false;
                            
                        }//if
                    }//if

                }//if
            }//for

            if (totalBalls == 0)
            {
                this.isRunning = false;
                this.isGameOver = true;
                this.isGameWon = true;
            }
        }


        private Point FindCharacterLocation(Sprite sprite)
        {
            for (int c = 0; c < GameEngine.backLayer.Columns; c++)
            {
                for (int r = 0; r < GameEngine.backLayer.Rows; r++)
                {

                    if (GameEngine.backLayer.TileObject[c, r] == null)
                        continue;

                    if (sprite.Location.X == GameEngine.backLayer.TileObject[c, r].Sprite.Location.X
                     && sprite.Location.Y == GameEngine.backLayer.TileObject[c, r].Sprite.Location.Y)
                    {
                        return new Point(c, r);
                    }
                }
            }
            return new Point(-1, -1);

        }



    }
}
