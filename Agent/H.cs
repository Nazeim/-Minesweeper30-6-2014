using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
    class H
    {
        private const double DOUBLE_EPSILON = 0.00001;

        private static int Compare(double first, double second)
        {
            double difference = first - second;

            if (difference > DOUBLE_EPSILON)
                return 1;
            if (difference < -DOUBLE_EPSILON)
                return -1;
            return 0;
        }

        public static bool Equals(double first, double second)
        {
            return Compare(first, second) == 0;
        }

        public static bool Larger(double first, double second)
        {
            return Compare(first, second) > 0;
        }

        public static bool Smaller(double first, double second)
        {
            return Compare(first, second) < 0;
        }
    }
}
