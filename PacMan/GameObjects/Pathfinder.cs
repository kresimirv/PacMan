using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PacMan
{
    //A STAR PATHFIND ALGORITHM
    public class Pathfinder
    {
        public class PathFindTile
        {
            private Point _location;
            private int _g = 0;
            private int _h = 0;
            private PathFindTile _parent;

            public Point Location
            {
                get
                {
                    return _location;
                }
                set
                {
                    _location = value;
                }
            }

            public int F
            {
                get
                {
                    return _g + _h;
                }
            }

            public int G
            {
                get
                {
                    return _g;
                }
                set
                {
                    _g = value;
                }
            }

            public int H
            {
                get
                {
                    return _h;
                }
                set
                {
                    _h = value;
                }
            }

            public PathFindTile Parent
            {
                get
                {
                    return _parent;
                }
                set
                {
                    _parent = value;
                }
            }

            public PathFindTile()
            {
            }

            public PathFindTile(Point location, int g, int h, PathFindTile parent)
            {
                _location = location;
                _g = g;
                _h = h;
                _parent = parent;
            }
        }

        private int[,] _gameField; //array (0 - path, 1 - wall)
        public enum PathFindObject
        {
            Path = 0,
            Wall = 1
        }

        public Pathfinder(int[,] gameField)
        {
            _gameField = gameField;
        }

        public Stack<Point> PathFind(Point startTile, Point endTile)
        {
            Stack<Point> result = new Stack<Point>();

            Stack<Point> resultPath = new Stack<Point>();
            List<PathFindTile> openList = new List<PathFindTile>();
            List<PathFindTile> closedList = new List<PathFindTile>();

            //push start node on open list
            openList.Add(new PathFindTile(startTile, 0, 0, null));

            //while open list is not empty
            while (openList.Count > 0)
            {
                //currentNode = find lowest f in openList
                int lowInd = 0;
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].F < openList[lowInd].F)
                    {
                        lowInd = i;
                    }
                }
                PathFindTile currentNode = openList[lowInd];

                //if currentNode is final, return the successful path
                if (currentNode.Location == endTile)
                {
                    PathFindTile curr = currentNode;

                    while (curr.Parent != null)
                    {
                        resultPath.Push(curr.Location);

                        curr = curr.Parent;
                    }

                    return resultPath;
                }//if


                //push currentNode onto closedList and remove from openList
                closedList.Add(currentNode);
                openList.Remove(currentNode);


                //foreach neighbor of currentNode
                List<PathFindTile> neighbours = new List<PathFindTile>();
                neighbours = GetNeighbours(currentNode.Location);

                for (int i = 0; i < neighbours.Count; i++)
                {
                    //if neighbor is not in openList 
                    //    save g, h, and f then save the current parent
                    //    add neighbor to openList
                    PathFindTile neighbour = neighbours[i];


                    bool isInOpenList = false;
                    for (int j = 0; j < openList.Count; j++)
                    {
                        if (openList[j].Location == neighbour.Location)
                        {
                            isInOpenList = true;
                            break;
                        }
                    }

                    int gScore = currentNode.G + 1;
                    bool gScoreIsBest = false;

                    if (isInOpenList == false)
                    {
                        PathFindTile n = new PathFindTile(neighbours[i].Location, gScore, CalculateDistance(currentNode.Location, endTile), currentNode);
                        openList.Add(n);
                    }
                    else if (gScore < neighbour.G)
                    {
                        gScoreIsBest = true;
                    }

                    if (gScoreIsBest)
                    {
                        neighbour.Parent = currentNode;
                        neighbour.G = gScore;
                        neighbour.H = CalculateDistance(neighbour.Location, endTile);

                    }

                } //for

            }//while

            return result;
        }

        public int CalculateDistance(Point location1, Point location2)
        {
            //Manhattan distance
            int dx = Math.Abs(location1.X - location2.X);
            int dy = Math.Abs(location1.Y - location2.Y);
            return dx + dy;
        }

        public List<PathFindTile> GetNeighbours(Point location)
        {

            List<PathFindTile> neighboursList = new List<PathFindTile>();
            int c = location.X;
            int r = location.Y;


            //up
            int upR = r - 1;
            int upC = c;

            if (upR > 0 && upC > 0 &&
                 _gameField[upC, upR] == (int)PathFindObject.Path)
            {
                PathFindTile pft = new PathFindTile(new Point(upC, upR), 0, 0, null);
                neighboursList.Add(pft);
            }

            //down
            int downR = r + 1;
            int downC = c;

            if (downR > 0 && downC > 0 &&
                _gameField[downC, downR] == (int)PathFindObject.Path)
            {
                PathFindTile pft = new PathFindTile(new Point(downC, downR), 0, 0, null);
                neighboursList.Add(pft);
            }

            //left
            int leftR = r;
            int leftC = c - 1;

            if (leftR > 0 && leftC > 0 &&
                 _gameField[leftC, leftR] == (int)PathFindObject.Path)
            {
                PathFindTile pft = new PathFindTile(new Point(leftC, leftR), 0, 0, null);
                neighboursList.Add(pft);
            }


            //right
            int rightR = r;
            int rightC = c + 1;

            if (rightR > 0 && rightC > 0 &&
                 _gameField[rightC, rightR] == (int)PathFindObject.Path)
            {
                PathFindTile pft = new PathFindTile(new Point(rightC, rightR), 0, 0, null);
                neighboursList.Add(pft);
            }


            return neighboursList;

        }
    }
}
