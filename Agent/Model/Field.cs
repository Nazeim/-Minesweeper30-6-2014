using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent.Model
{
    public class Field
    {
        public enum GameStateEnum
        {
            PLAYING = 0,
            LOST = 1,
            WON = 2
        }

        private int remainingMine;
        private GameStateEnum gameState;
        private int width;
        private int height;
        private Sector[,] sectors;
        private int elapsedTime;
        private int unrevealedSectorsCount;

        public int RemainingMine
        {
            get { return remainingMine; }
            set { remainingMine = value; }
        }
        public GameStateEnum GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public Sector[,] Sectors
        {
            get { return sectors; }
            set { sectors = value; }
        }
        public int ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }
        public int UnrevealedSectorsCount
        {
            get { return unrevealedSectorsCount; }
            set { unrevealedSectorsCount = value; }
        }
    }
}
