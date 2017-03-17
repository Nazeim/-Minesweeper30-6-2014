using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agent.Model;

namespace Agent.Units
{
    public class NonBoundaryUnit : BoundaryBasedUnit
    {
        Random random = new Random();
        List<Sector> allRevealedOnBoundaries;
        List<ProbabilityPosition> result;

        protected List<Sector> GetRevealedSectorsOnBoundaries()
        {
            List<Sector> revealedSectorsOnBoundaries = new List<Sector>();

            foreach (Sector sector in sectorsOnBoundaries)
            {
                if ((int)sector.State > 0)
                {
                    revealedSectorsOnBoundaries.Add(sector);
                }
            }

            return revealedSectorsOnBoundaries;
        }

        private List<ProbabilityPosition> GetNonBoundarySectors(double prob)
        {
            List<ProbabilityPosition> result = new List<ProbabilityPosition>();

            foreach (Sector sector in nonBoundariesUnRevealedSectors)
            {
                result.Add(new ProbabilityPosition() {X = sector.X, Y = sector.Y, Probability = prob });
            }

            return result;
        }

        private int GetMinesCountOnBoundaries()
        {
            int totalMinesCountOnBoundaries = 0;
            int minesCountSurroundingSector;
            int randIndex;
            Sector currentSector;
            List<Sector> adjSectors;

            allRevealedOnBoundaries = GetRevealedSectorsOnBoundaries();

            while (allRevealedOnBoundaries.Count != 0)
            {
                randIndex = random.Next(0, allRevealedOnBoundaries.Count);
                currentSector = allRevealedOnBoundaries[randIndex];
                allRevealedOnBoundaries.Remove(currentSector);
                minesCountSurroundingSector = (int)currentSector.State;
                adjSectors = GetAdjacentBoundarySectors(currentSector);

                foreach (Sector sector in adjSectors)
                {
                    if (sector.State == Sector.SectorState.MARKED)
                    {
                        minesCountSurroundingSector--;
                    }
                    else if(sector.State == Sector.SectorState.UN_REVEALED)
                    {
                        DeleteAdjacentRevealedSectors(sector);
                    }
                }

                totalMinesCountOnBoundaries += minesCountSurroundingSector;
            }

            return totalMinesCountOnBoundaries;
        }

        private void DeleteAdjacentRevealedSectors(Sector sector)
        {
            Sector[,] adjSectors = GetAdjacentSectors(sector);

            for (int i = 0; i < adjSectors.GetLength(0); i++)
            {
                for (int j = 0; j < adjSectors.GetLength(1); j++)
                {
                    if ((int)adjSectors[i, j].State > 0)
                    {
                        allRevealedOnBoundaries.Remove(adjSectors[i, j]);
                    }
                }
            }
        }

        private double GetNonBoudariesSectorsProbability()
        {
            int minesCountOnBoundaries = GetMinesCountOnBoundaries();
            int minesCountNotOnBoundaries = field.RemainingMine - minesCountOnBoundaries;
            double nonBoundariesProb = (double)minesCountNotOnBoundaries / nonBoundariesUnRevealedSectors.Count;

            return nonBoundariesProb;
        }
        public override void DoWork()
        {
            base.DoWork();
            double prob = GetNonBoudariesSectorsProbability();
            result = GetNonBoundarySectors(prob);
        }

        public List<ProbabilityPosition> GetResult()
        {
            return result;
        }





    }
}
