using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper.Model
{
    class Sector
    {
        private bool hasMine;
        private bool isRevealed;
        private bool isMarked;
        private int value;
        public readonly int X;
        public readonly int Y;

        public bool IsMarked
        {
            get { return isMarked; }
            set { isMarked = value; }
        }

        public Sector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public bool IsRevealed
        {
            get { return isRevealed; }
            set { isRevealed = value; }
        }

        public bool HasMine
        {
            get { return hasMine; }
            set { hasMine = value; }
        }
    }
}
