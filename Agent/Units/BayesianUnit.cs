using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agent.Model;
using System.Xml;
using System.IO;
using Smile;

namespace Agent.Units
{
    public class BayesianUnit : BoundaryBasedUnit
    {
        private double[] xdefinition;
        private double[] markDefinition = { 1.0, 0.0 };
        private double[] yDefinition;
        private List<Position> mineSectors;
        private List<ProbabilityPosition> leastProbabilitySectors;
        
        public BayesianUnit()
        {
            mineSectors = new List<Position>();
            leastProbabilitySectors = new List<ProbabilityPosition>();
        }

        //Public Interface
        public List<Position> GetMineSector()
        {
            return mineSectors;
        }
        public List<ProbabilityPosition> GetLeastProbabilitySectors()
        {
            return leastProbabilitySectors;
        }
        public override void DoWork()
        {
            base.DoWork();
            BuildBayesianNetwork();
        }

        //Helper
        private string GetSectorName(Sector sector)
        {
            if (sector.State != Sector.SectorState.UN_REVEALED && sector.State != Sector.SectorState.MARKED)//UnRevealed Sector
                return "Y" + sector.X + "_" + sector.Y;
            else
                return "X" + sector.X + "_" + sector.Y;
        }

        //Network Building
        private void BuildBayesianNetwork()
        {
            leastProbabilitySectors.Clear();
            mineSectors.Clear();

            Network net = new Network();
            AddUnrevealedOrMarkedSectorsToNetwork(net);
            AddRevealedSectorsToNetwork(net);
            SetEvidences(net);
            net.UpdateBeliefs();

            GetResultsOfApplyingInference(net);
        }

        private void AddUnrevealedOrMarkedSectorsToNetwork(Network net)
        {
            string name;
            xdefinition = new double[] { (double)field.RemainingMine / field.UnrevealedSectorsCount, 1 - ((double)field.RemainingMine / field.UnrevealedSectorsCount) };

            for (int i = 0; i < sectorsOnBoundaries.Count; i++)
            {
                if (sectorsOnBoundaries[i].State == Sector.SectorState.UN_REVEALED || sectorsOnBoundaries[i].State == Sector.SectorState.MARKED)//UnRevealed Sector Or Marked
                {
                    name = GetSectorName(sectorsOnBoundaries[i]);
                    net.AddNode(Network.NodeType.Cpt, name);
                    net.SetOutcomeId(name, 0, "mine");
                    net.SetOutcomeId(name, 1, "nomine");

                    if (sectorsOnBoundaries[i].State == Sector.SectorState.UN_REVEALED)
                        net.SetNodeDefinition(name, xdefinition);
                    else
                        net.SetNodeDefinition(name, markDefinition);
                }
            }
        }
        private void AddRevealedSectorsToNetwork(Network net)
        {
            string yName, xName;
            int parentsCount = 0;
            List<Sector> causes;

            for (int i = 0; i < sectorsOnBoundaries.Count; i++)
            {
                if (sectorsOnBoundaries[i].State != Sector.SectorState.UN_REVEALED && sectorsOnBoundaries[i].State != Sector.SectorState.MARKED)//Revealed Sector
                {
                    parentsCount = 0;
                    yName = GetSectorName(sectorsOnBoundaries[i]);
                    net.AddNode(Network.NodeType.TruthTable, yName);
                    net.SetOutcomeId(yName, 0, "S0");
                    net.SetOutcomeId(yName, 1, "S1");
                    net.AddOutcome(yName, "S2");
                    net.AddOutcome(yName, "S3");
                    net.AddOutcome(yName, "S4");
                    net.AddOutcome(yName, "S5");
                    net.AddOutcome(yName, "S6");
                    net.AddOutcome(yName, "S7");
                    net.AddOutcome(yName, "S8");
                    causes = GetAdjacentBoundarySectors(sectorsOnBoundaries[i]);

                    foreach (Sector cause in causes)
                    {
                        if (cause.State == Sector.SectorState.UN_REVEALED || cause.State == Sector.SectorState.MARKED)//UnRevealed Sector Or Marked
                        {
                            xName = GetSectorName(cause);
                            net.AddArc(xName, yName);
                            parentsCount++;
                        }
                    }

                    yDefinition = GetYDefinitionTabel(parentsCount);
                    net.SetNodeDefinition(yName, yDefinition);
                }
            }
        }
        private void SetEvidences(Network net)
        {
            bool isAllCausesMarked;
            string yName;
            List<Sector> causes;

            for (int i = 0; i < sectorsOnBoundaries.Count; i++)
            {
                if (sectorsOnBoundaries[i].State != Sector.SectorState.UN_REVEALED && sectorsOnBoundaries[i].State != Sector.SectorState.MARKED)//Revealed Sector
                {
                    yName = GetSectorName(sectorsOnBoundaries[i]);
                    causes = GetAdjacentBoundarySectors(sectorsOnBoundaries[i]);
                    isAllCausesMarked = true;

                    foreach (Sector s in causes)
                    {
                        if (s.State == Sector.SectorState.UN_REVEALED || s.State == Sector.SectorState.MARKED)//UnRevealed Sector Or Marked
                        {
                            if (s.State != Sector.SectorState.MARKED)
                                isAllCausesMarked = false;
                        }
                    }

                    try
                    {
                        if (!isAllCausesMarked)
                        {
                            net.SetEvidence(yName, "S" + (int)sectorsOnBoundaries[i].State);
                        }

                    }
                    catch (Exception e)
                    { }
                }
            }
        }

        private double[] GetYDefinitionTabel(int Max)
        {
            int count;
            double[] truthTabel = new double[Convert.ToInt32(Math.Pow(2, Max) * 9)];
            int k = 0;

            for (int i = (int)Math.Pow(2, Max) - 1; i >= 0; i--)
            {
                count = GetOnesCountInBinaryRepresentation(i, Max);
                truthTabel[(k * 9) + count] = 1.0;
                k++;
            }

            return truthTabel;
        }
       
        private int GetOnesCountInBinaryRepresentation(int num, int bits)
        {
            int onesCount = 0;

            while (num != 0)
            {
                if (num % 2 == 1)
                    onesCount++;

                num = num / 2;
            }

            return onesCount;
        }
           
        //Inference
        private void GetResultsOfApplyingInference(Network net)
        {
            String name;
            double minProbability = 2;
            double[] values;

            for (int i = 0; i < sectorsOnBoundaries.Count; i++)
            {
                if (sectorsOnBoundaries[i].State == Sector.SectorState.UN_REVEALED)//UnRevealed Sector
                {
                    name = GetSectorName(sectorsOnBoundaries[i]);
                    values = net.GetNodeValue(name);

                    if (H.Equals(values[0], 1.0))//sector has mine
                    {
                        mineSectors.Add(new Position { X = sectorsOnBoundaries[i].X, Y = sectorsOnBoundaries[i].Y });
                    }
                    else
                    {
                        if (H.Smaller(values[0], minProbability))
                        {
                            minProbability = values[0];
                        }
                    }
                }
            }

            for (int i = 0; i < sectorsOnBoundaries.Count; i++)
            {
                if (sectorsOnBoundaries[i].State == Sector.SectorState.UN_REVEALED)//UnRevealed Sector
                {
                    name = GetSectorName(sectorsOnBoundaries[i]);
                    values = net.GetNodeValue(name);

                    if (H.Equals(values[0], minProbability))
                    {
                        leastProbabilitySectors.Add(new ProbabilityPosition { X = sectorsOnBoundaries[i].X, Y = sectorsOnBoundaries[i].Y, Probability = minProbability });
                    }
                }
            }

        }
    }
}
