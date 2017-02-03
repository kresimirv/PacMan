using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PacMan
{
    public class TiledLayer
    {
        private int _rows;
        private int _columns;
        private Size _tileSize;
        public Tile[,] TileObject;

        public int Rows
        {
            get
            {
                return _rows;
            }
        }

        public int Columns
        {
            get
            {
                return _columns;
            }
        }


        public TiledLayer(int columns, int rows, Size tileSize)
        {
            _rows = rows;
            _columns = columns;
            _tileSize = tileSize;
            TileObject = new Tile[_columns, _rows];
        }

        public void Draw(Graphics g)
        {
            for (int c = 0; c < _columns; c++)
            {
                for(int r = 0; r < _rows; r++)
                {

                    if (TileObject[c, r] != null)
                    {
                        TileObject[c, r].Sprite.Draw(g);

                        //for testing
                        //g.DrawRectangle(Pens.White, new Rectangle(TileObject[c, r].Sprite.Location, TileObject[c, r].Sprite.Size));
                        //g.DrawString(c + ", " + r, new Font("Arial", 8f, FontStyle.Bold), Brushes.Red, TileObject[c, r].Sprite.Location);
                    }
                }
            }
        }

        public void Add(Sprite sprite, bool isWall,  int column, int row)
        {
            sprite.Location = new Point(column * _tileSize.Width, row * _tileSize.Height);
            Tile tile = new Tile(sprite, isWall);
            TileObject[column, row] = tile;
        }

        public void Add(Tile tile, int column, int row)
        {
            tile.Sprite.Location = new Point(column * _tileSize.Width, row * _tileSize.Height);
            TileObject[column, row] = tile;
        }

        public void Remove(int column, int row)
        {
            TileObject[column, row] = null;
        }

        public Point GetTileLocationForLocation(Point location)
        {
            int xLocation = 0;
            for (int c = 0; c < _columns; c++)
            {
                int cStart = c * _tileSize.Width;
                int cEnd = cStart + _tileSize.Width;
                if (cStart <= location.X && location.X <= cEnd)
                {
                    xLocation = c;
                    break;
                }
            }

            int yLocation = 0;
            for (int r = 0; r < _rows; r++)
            {
                int rStart = r * _tileSize.Height;
                int rEnd = rStart + _tileSize.Height;
                if (rStart <= location.Y && location.Y <= rEnd)
                {
                    yLocation = r;
                    break;
                }
            }

            return new Point(xLocation, yLocation);
        }


    }
}
