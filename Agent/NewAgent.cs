using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agent.Model;

namespace Agent
{
    class NewAgent:AbstractAgent
    {
        public NewAgent(string initialGameIpAddress, int initialGamePort)
            :base(initialGameIpAddress, initialGamePort)
        {}
        protected override bool SendResponse()
        {
            bayesianUnit.DoWork();
            List<Position> toMark = bayesianUnit.GetMineSector();
            List<Position> toReveal = GetToRevealSecotorPositions();

            return networkUnit.SendRevealAndMarkCommand(toReveal, toMark);
        }

        protected List<Position> GetToRevealSecotorPositions()
        {
            List<Position> toReveal = new List<Position>();
            List<ProbabilityPosition> bayesianLeastP = bayesianUnit.GetLeastProbabilitySectors();

            if (bayesianLeastP.Count > 0)
            {
                if (H.Equals(bayesianLeastP[0].Probability, 0.0))
                {
                    foreach (ProbabilityPosition sector in bayesianLeastP)
                    {
                        toReveal.Add(sector);
                    }
                }
                else
                {
                    GetToRevealSectorsUnderUncertainty(bayesianLeastP, toReveal);
                }
            }
                //////
            else
            {
                bayesianLeastP.Add(new ProbabilityPosition() { Probability = double.PositiveInfinity });
                GetToRevealSectorsUnderUncertainty(bayesianLeastP, toReveal);
            }

            return toReveal;
        }

        protected void GetToRevealSectorsUnderUncertainty(List<ProbabilityPosition> bayesianLeastP, List<Position> result)
        {
            int randomIndex;
            nonBoundariesUnit.SetPercept(field);
            nonBoundariesUnit.DoWork();
            List<ProbabilityPosition> nonBoundariesP = nonBoundariesUnit.GetResult();

            if (nonBoundariesP.Count > 0)
            {
                if (H.Equals(nonBoundariesP[0].Probability, 0.0))
                {
                    foreach (ProbabilityPosition sector in nonBoundariesP)
                    {
                        result.Add(sector);
                    }

                    return;
                }
                else
                {
                    if (H.Smaller(nonBoundariesP[0].Probability, bayesianLeastP[0].Probability))
                    {
                        randomIndex = randomGenerator.Next(0, nonBoundariesP.Count);
                        result.Add(nonBoundariesP[randomIndex]);

                        return;
                    }
                }
            }
            ////////
            randomIndex = randomGenerator.Next(0, bayesianLeastP.Count);
            result.Add(bayesianLeastP[randomIndex]);
        }
    }
}