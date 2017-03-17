using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent.Model
{
    public class Position
    {
        private int x;
        private int y;
        private static Random randomGenerator = new Random();

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Position()
        { }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Position GenerateRandomPosition(int height, int width)
        {
            int x = randomGenerator.Next(0, height);
            int y = randomGenerator.Next(0, width);
            Position p = new Position(x, y);

            return p;
        }

        public override bool Equals(object obj)
        {
            Position other =(Position)obj;

            return (other.X == X && other.Y == Y);          
        }

    }
}
