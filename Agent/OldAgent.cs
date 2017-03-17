using System;
using System.Collections.Generic;
using System.Text;
using TcpCommunications;
using System.Threading;
using Agent.Model;

namespace Agent
{
    class OldAgent:AbstractAgent
    {
        public OldAgent(string initialGameIpAddress, int initialGamePort)
            :base(initialGameIpAddress, initialGamePort)
        {}
        #region Helper

        protected override bool SendResponse()
        {
            bayesianUnit.DoWork();
            List<Position> toMark = bayesianUnit.GetMineSector();
            List<Position> toReveal = new List<Position>();
            List<ProbabilityPosition> leastP = bayesianUnit.GetLeastProbabilitySectors();

            if (leastP.Count > 0)
            {
                if (H.Equals(leastP[0].Probability, 0.0))
                {
                    foreach (ProbabilityPosition sector in leastP)
                    {
                        toReveal.Add(sector);
                    }
                }
                else
                {
                    int index = randomGenerator.Next(0, leastP.Count);
                    toReveal.Add(leastP[index]);
                }
            }
            else
            {
                ForceStop();
            }

            return networkUnit.SendRevealAndMarkCommand(toReveal, toMark);
        }
        #endregion
    }
}
