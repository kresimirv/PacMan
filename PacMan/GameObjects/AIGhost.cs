using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PacMan
{
    public class AIGhost
    {
        private Stack<Point> resultPath = new Stack<Point>();
        private Pathfinder pathfinder;
        private int[,] gameField;
        private Ghost _ghost;

        //line of sight timer
        private int losInterval = 5000; //3 sec
        System.Windows.Forms.Timer losTimer = new System.Windows.Forms.Timer();

        private Point randomPoint;



        public AIGhost(Ghost ghost)
        {
            //assign ghost to ai
            _ghost = ghost;

            //timer
            losTimer.Interval = losInterval;
            losTimer.Tick += new EventHandler(losTimer_Tick);

            randomPoint = new Point(-1, -1);

            //CREATE PATHFIND INSTANCE
            //create game field array
            gameField = new int[GameEngine.backLayer.Columns, GameEngine.backLayer.Rows];
            for (int c = 0; c < GameEngine.backLayer.Columns; c++)
            {
                for (int r = 0; r < GameEngine.backLayer.Rows; r++)
                {
                    if(GameEngine.backLayer.TileObject[c,r] != null)
                    {
                        if (GameEngine.backLayer.TileObject[c, r].IsWall)
                        {
                            gameField[c, r] = (int)Pathfinder.PathFindObject.Wall;
                        }
                        else
                        {
                            gameField[c, r] = (int)Pathfinder.PathFindObject.Path;
                        }
                    }
                }//for
            }//for

            pathfinder = new Pathfinder(gameField);
            
        }

        void losTimer_Tick(object sender, EventArgs e)
        {
            _ghost.HasLineOfSight = false;
        }

        //CHOOSE DIRECTION
        public Ghost.GhostDirection ChooseDirection(PacMan pacMan)
        {
            Ghost.GhostDirection currentDirection = _ghost.Direction;

            //line of sight
            if (HasLineOfSight(pacMan))
            {
                _ghost.HasLineOfSight = true;
                losTimer.Stop();
            }
            else
            {
                losTimer.Start();
            }


            if (pacMan.HasPower == false && _ghost.HasLineOfSight == true) //chase
            {
                _ghost.IsChasing = true;
                
                //find path to pacman
                resultPath.Clear();
                resultPath = pathfinder.PathFind(_ghost.TileLocation, pacMan.TileLocation);


            }
            else if (pacMan.HasPower == true) //run
            {
                _ghost.IsChasing = false;

                //find path to safelocation
                int bestSafePlace = 0;
                int bestSafePlaceDistance = pathfinder.CalculateDistance(GameEngine.safePlace[0], pacMan.TileLocation);
                for (int i = 0; i < GameEngine.safePlace.Count; i++)
                {
                    //for each calculate distance from pacman to safe location
                    Point safePlace = GameEngine.safePlace[i];
                    int safePlaceDistance = pathfinder.CalculateDistance(GameEngine.safePlace[i], pacMan.TileLocation);
                    if (bestSafePlaceDistance < safePlaceDistance)
                    {
                        bestSafePlace = i;
                    }
                }

                resultPath.Clear();
                resultPath = pathfinder.PathFind(_ghost.TileLocation, GameEngine.safePlace[bestSafePlace]);
            }
            else
            {
                _ghost.IsChasing = true;

                if (_ghost.TileLocation == randomPoint || (randomPoint.X == -1 && randomPoint.Y == -1))
                {
                    List<Point> availableDestinations = new List<Point>();
                    //choose random destination
                    for (int c = 0; c < GameEngine.backLayer.Columns; c++)
                    {
                        for (int r = 0; r < GameEngine.backLayer.Rows; r++)
                        {
                            if (GameEngine.backLayer.TileObject[c, r] == null)
                                continue;

                            if (GameEngine.backLayer.TileObject[c, r].Sprite.GetType() == typeof(Path))
                            {
                                availableDestinations.Add(new Point(c, r));
                            }
                        }
                    }

                    Random rand = new Random();
                    int random = rand.Next(0, availableDestinations.Count - 1);
                    randomPoint = availableDestinations[random];
                }

                resultPath.Clear();
                resultPath = pathfinder.PathFind(_ghost.TileLocation, randomPoint);
            }
            

            //determine next direction
            if (resultPath != null && resultPath.Count > 0)
            {
                Point nextPoint = resultPath.Pop();
                Point ghostPoint = _ghost.TileLocation;

                if (nextPoint.X > ghostPoint.X)
                {
                    currentDirection = Ghost.GhostDirection.Right;
                }
                else if (nextPoint.X < ghostPoint.X)
                {
                    currentDirection = Ghost.GhostDirection.Left;
                }
                else if (nextPoint.Y < ghostPoint.Y)
                {
                    currentDirection = Ghost.GhostDirection.Up;
                }
                else if (nextPoint.Y > ghostPoint.Y)
                {
                    currentDirection = Ghost.GhostDirection.Down;
                }
                else
                {
                    currentDirection = Ghost.GhostDirection.None;
                }
            }
            else
            {
                currentDirection = Ghost.GhostDirection.None;
            }

            return currentDirection;


        }

        //LINE OF SIGHT
        public bool HasLineOfSight(PacMan pacMan)
        {
            bool result = true;

            Point ghostPoint = _ghost.TileLocation;
            Point pacManPoint = pacMan.TileLocation;

            if (ghostPoint.X == pacManPoint.X)
            {
                int x = ghostPoint.X;
                int minY = Math.Min(ghostPoint.Y, pacManPoint.Y);
                int maxY = Math.Max(ghostPoint.Y, pacManPoint.Y);

                for (int i = minY; i < maxY; i++)
                {
                    if (gameField[x, i] == (int)Pathfinder.PathFindObject.Wall)
                    {
                        result = false;
                        break;
                    }

                }
            }
            else if (ghostPoint.Y == pacManPoint.Y)
            {
                int y = ghostPoint.Y;
                int minX = Math.Min(ghostPoint.X, pacManPoint.X);
                int maxX = Math.Max(ghostPoint.X, pacManPoint.X);

                for (int i = minX; i < maxX; i++)
                {
                    if (gameField[i, y] == (int)Pathfinder.PathFindObject.Wall)
                    {
                        result = false;
                        break;
                    }

                }
            }
            else
            {
                result = false;
            }


            return result;
        }


    }
}
