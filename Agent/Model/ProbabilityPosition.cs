using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent.Model
{
    public class ProbabilityPosition:Position
    {
        private double probability;

        public double Probability
        {
            get { return probability; }
            set { probability = value; }
        }
    }
}
