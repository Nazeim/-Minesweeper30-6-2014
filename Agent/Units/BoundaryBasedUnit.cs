using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agent.Model;

namespace Agent.Units
{
    public class BoundaryBasedUnit
    {
        protected List<Sector> sectorsOnBoundaries;
        protected List<Sector> nonBoundariesUnRevealedSectors;
        protected Field field;

        //Public Interface
        public void SetPercept(Field field)
        {
            this.field = field;
        }
        public virtual void DoWork()
        {
            ProcessBoundaries();
        }

        //Find Sectors On Boundaries
        
        protected void ProcessBoundaries()
        {
            Sector sector;
            sectorsOnBoundaries = new List<Sector>();
            nonBoundariesUnRevealedSectors = new List<Sector>();

            for (int i = 0; i < field.Sectors.GetLength(0); i++)
            {
                for (int j = 0; j < field.Sectors.GetLength(1); j++)
                {
                    sector = field.Sectors[i, j];
                    if (sector.State != Sector.SectorState.UN_REVEALED && sector.State != Sector.SectorState.ZERO)
                    {
                        sectorsOnBoundaries.Add(sector);//add to the sectors list
                    }
                    else if (sector.State == Sector.SectorState.UN_REVEALED)//UnRevealed Sector
                    {
                        if (AdjacentSectorsRevealed(sector))
                        {
                            sectorsOnBoundaries.Add(sector);
                        }
                        else
                        {
                            nonBoundariesUnRevealedSectors.Add(sector);
                        }
                    }
                }
            }
        }
        protected bool AdjacentSectorsRevealed(Sector sector)
        {
            Sector[,] AdjSectors = GetAdjacentSectors(sector);

            for (int i = 0; i < AdjSectors.GetLength(0); i++)
            {
                for (int j = 0; j < AdjSectors.GetLength(1); j++)
                {
                    if (AdjSectors[i, j].State != Sector.SectorState.UN_REVEALED && AdjSectors[i, j].State != Sector.SectorState.MARKED)
                        return true;

                }
            }

            return false;
        }
        protected Sector[,] GetAdjacentSectors(Sector sector)
        {
            int xFrom = Math.Max(sector.X - 1, 0);
            int xTo = Math.Min(sector.X + 1, field.Sectors.GetLength(0) - 1);
            int yFrom = Math.Max(sector.Y - 1, 0);
            int yTo = Math.Min(sector.Y + 1, field.Sectors.GetLength(1) - 1);

            Sector[,] result = new Sector[(xTo - xFrom) + 1, (yTo - yFrom) + 1];

            for (int i = xFrom, x = 0; i <= xTo; i++, x++)
                for (int j = yFrom, y = 0; j <= yTo; j++, y++)
                    result[x, y] = field.Sectors[i, j];

            return result;
        }
        protected List<Sector> GetAdjacentBoundarySectors(Sector sector)
        {
            int xFrom = Math.Max(sector.X - 1, 0);
            int xTo = Math.Min(sector.X + 1, field.Sectors.GetLength(0) - 1);
            int yFrom = Math.Max(sector.Y - 1, 0);
            int yTo = Math.Min(sector.Y + 1, field.Sectors.GetLength(1) - 1);

            List<Sector> result = new List<Sector>();
            for (int i = 0; i < sectorsOnBoundaries.Count; i++)
            {
                if (sectorsOnBoundaries[i] != sector && sectorsOnBoundaries[i].X <= xTo && sectorsOnBoundaries[i].X >= xFrom && sectorsOnBoundaries[i].Y <= yTo && sectorsOnBoundaries[i].Y >= yFrom)
                    result.Add(sectorsOnBoundaries[i]);
            }
            return result;
        }

    }
}
