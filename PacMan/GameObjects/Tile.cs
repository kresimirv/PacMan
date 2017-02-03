using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacMan
{
    public class Tile
    {
        private bool _isWall;
        private Sprite _sprite;

        public Sprite Sprite
        {
            get
            {
                return _sprite;
            }
            set
            {
                _sprite = value;
            }
        }

        public bool IsWall
        {
            get
            {
                return _isWall;
            }
        }


        public Tile(Sprite sprite, bool isWall)
        {
            _sprite = sprite;
            _isWall = isWall;
        }

    }
}
