using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agent.Model;
using System.IO;
using System.Xml;

namespace Agent.Units
{
    class PerceptionUnit
    {
        private int x;//current sector x-coo
        private int y;//current sector y-coo
        private int state;//state

        private void ReadElement(XmlTextReader reader, Field field)
        {
            switch (reader.Name)
            {
                case "RemainingMines":
                    reader.Read();
                    field.RemainingMine = Convert.ToInt32(reader.Value);
                    break;
                case "GameState":
                    reader.Read();
                    field.GameState = (Field.GameStateEnum)Convert.ToInt32(reader.Value);
                    break;
                case "Width":
                    reader.Read();
                    field.Width = Convert.ToInt32(reader.Value);
                    break;
                case "Height":
                    reader.Read();
                    field.Height = Convert.ToInt32(reader.Value);
                    break;
                case "X":
                    reader.Read();
                    x = Convert.ToInt32(reader.Value);
                    break;
                case "Y":
                    reader.Read();
                    y = Convert.ToInt32(reader.Value);
                    break;
                case "State":
                    reader.Read();
                    state = Convert.ToInt32(reader.Value);
                    break;
                case "ElapsedTime":
                    reader.Read();
                    field.ElapsedTime = Convert.ToInt32(reader.Value);
                    break;
            }
        }

        //Returns true when it reads the final End Element of the file
        private bool ReadEndElement(XmlTextReader reader, Field field)
        {
            if (reader.Name == "Sector")
            {
                field.Sectors[x, y] = new Sector(x, y);
                field.Sectors[x, y].State = (Sector.SectorState)state;

                if (state == (int)Sector.SectorState.UN_REVEALED)//UnRevealed Sector
                    field.UnrevealedSectorsCount++;
            }
            else if (reader.Name == "Dimensions")
            {
                field.Sectors = new Sector[field.Height, field.Width];
            }
            else if (reader.Name == "MineSweeper")
            {
                reader.Close();
                return true;
            }

            return false;
        }

        public Field GenerateModel(string xmlRepresentation)
        {
            StringReader stringReader = new StringReader(xmlRepresentation);
            XmlTextReader reader = new XmlTextReader(stringReader);
            Field field = new Field();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    ReadElement(reader, field);
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (ReadEndElement(reader, field))
                        return field;
                }
            }

            return null;
        }
    }
}
