using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent.Model
{
    public class Sector
    {
        public enum SectorState
        {
            MARKED = -3,
            UN_REVEALED = -2,
            HAS_MINE = -1,
            ZERO = 0,
            ONE = 1,
            TWO = 2,
            THREE = 3,
            FOUR = 4,
            FIVE = 5,
            SIX = 6,
            SEVEN = 7,
            EIGHT = 8
        }

        private SectorState state;
        public readonly int X;
        public readonly int Y;
        //private bool isChosen;

        public SectorState State
        {
            get { return state; }
            set { state = value; }
        }
        public Sector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            Sector other = (Sector)obj;

            return other.X == X && other.Y == Y;
        }
        //public bool IsChosen
        //{
        //    get { return isChosen; }
        //    set { isChosen = value; }
        //}
    }
}
