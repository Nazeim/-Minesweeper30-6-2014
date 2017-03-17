using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using TcpCommunications;
using TcpCommunications.MinesweeperCommands;
using System.Threading;
using Minesweeper.Properties;

namespace Minesweeper.Model
{
    class Minesweeper
    {
        public enum GameState
        {
            PLAYING = 0,
            LOST = 1,
            WON = 2
        }

        private int seed;
        private int minesCount;
        private Sector[,] sectors;
        private Random random;
        private GameState state = GameState.PLAYING;
        private int remainingMineFreeSectorsCount;
        private int markedSetorsCount = 0;
        private NetworkUnit networkUnit;
        private bool blockMode;
        private bool IsFirstGame = true;
        private bool blockGUI;

        public bool BlockGUI
        {
            get { return blockGUI; }
            set { blockGUI = value; }
        }

        public GameState State
        {
            get { return state; }
            set { state = value; }
        }

        public int MarkedSetorsCount
        {
            get { return markedSetorsCount; }
        }

        public int MinesCount
        {
            get { return minesCount; }
        }

        public int Seed
        {
            get { return seed; }
        }

        public Minesweeper()
        {
            networkUnit = new NetworkUnit(this);
            blockMode = false;
        }

        #region Events
        public class SectorPosition
        {
            public int XIndex
            {
                set;
                get;
            }

            public int YIndex
            {
                set;
                get;
            }

            public SectorPosition(int xIndex, int yIndex)
            {
                XIndex = xIndex;
                YIndex = yIndex;
            }
        }

        public class RevealSectorsEventArgs : EventArgs
        {
            private List<Sector> toRevealSectors;

            public List<Sector> ToRevealSectors
            {
                get { return toRevealSectors; }
                set { toRevealSectors = value; }
            }

            public RevealSectorsEventArgs(List<Sector> sector)
            {
                toRevealSectors = sector;
            }
        }

        public class MarkSectorEventArgs : EventArgs
        {
            private SectorPosition position;

            public SectorPosition Position
            {
                get { return position; }
                set { position = value; }
            }

            public MarkSectorEventArgs(SectorPosition position)
            {
                Position = position;
            }
        }

        public class NewGameEventArgs : EventArgs
        {
            public readonly int HEIGHT;
            public readonly int WIDTH;
            public readonly int MINES_COUNT;
            public readonly int SEED;
            public readonly bool HAS_SEED;

            public NewGameEventArgs(int height, int width, int minesCount, int seed)
            {
                HEIGHT = height;
                WIDTH = width;
                MINES_COUNT = minesCount;
                SEED = seed;
                HAS_SEED = true;
            }

            public NewGameEventArgs(int height, int width, int minesCount)
            {
                HEIGHT = height;
                WIDTH = width;
                MINES_COUNT = minesCount;
                SEED = 0;
                HAS_SEED = false;
            }
        }

        public event EventHandler<RevealSectorsEventArgs> RevealSectorRequested;
        /// <summary>
        /// added
        /// </summary>
        /// <param name="e"></param>
        public void OnRevealSectors(RevealSectorsEventArgs e)
        {
            if (RevealSectorRequested != null && !blockMode)
                RevealSectorRequested(this, e);
        }

        public event EventHandler<MarkSectorEventArgs> MarkSectorRequested;
        public void OnMarkSector(MarkSectorEventArgs e)
        {
            if (MarkSectorRequested != null && !blockMode)
                MarkSectorRequested(this, e);
        }

        public event EventHandler<NewGameEventArgs> NewGameRequested;
        public void OnNewGame(NewGameEventArgs e)
        {
            if (NewGameRequested != null && !blockMode)
                NewGameRequested(this, e);
        }

        public event EventHandler<EventArgs> AgentConnected;
        public void OnAgentConnected()
        {
            if (AgentConnected != null && !blockMode)
            {
                AgentConnected(this, new EventArgs());
            }
        }

        public event EventHandler<EventArgs> AgentDisconnected;
        public void OnAgentDisconnected()
        {
            if (AgentDisconnected != null && !blockMode)
            {
                AgentDisconnected(this, new EventArgs());
            }
        }
        #endregion

        #region New Game
        public void NewGame(int height, int width, int minesCount, int seed)
        {
            sectors = new Sector[height, width];

            for (int i = 0; i < sectors.GetLength(0); i++)
                for (int j = 0; j < sectors.GetLength(1); j++)
                    sectors[i, j] = new Sector(i, j);

            this.seed = seed;
            this.minesCount = minesCount;
            random = new Random(seed);
            remainingMineFreeSectorsCount = (width * height) - minesCount;
            markedSetorsCount = 0;
            State = GameState.PLAYING;
            blockMode = blockGUI;

            if (!IsFirstGame)
            {
                Settings.Default.totalGamesCount += 1;//add
                Settings.Default.Save();
            }
            else
                IsFirstGame = false;
            Initialize();
        }

        public void NewGame(int height, int width, int minesCount)
        {
            int seed = (int)DateTime.Now.Ticks;
            NewGame(height, width, minesCount, seed);
        }

        private void Initialize()
        {
            Sector current;

            for (int i = 1; i <= minesCount; i++)
            {
                current = ChooseRandomSector();
                AddMineInSector(current);
            }
        }

        private Sector ChooseRandomSector()
        {
            Sector chosen = null;
            int xIndex = -1;
            int yIndex = -1;

            do
            {
                xIndex = random.Next(0, sectors.GetLength(0));
                yIndex = random.Next(0, sectors.GetLength(1));
                chosen = sectors[xIndex, yIndex];

                if (!chosen.HasMine)
                {
                    return chosen;
                }
            } while (true);
        }

        private void AddMineInSector(Sector sector)
        {
            sector.HasMine = true;
            Sector[,] adjacent = GetAdjacentSectors(sector);

            for (int i = 0; i < adjacent.GetLength(0); i++)
                for (int j = 0; j < adjacent.GetLength(1); j++)
                    if (!adjacent[i, j].HasMine)
                        adjacent[i, j].Value++;

        }

        private Sector[,] GetAdjacentSectors(Sector sector)
        {
            int xFrom = Math.Max(sector.X - 1, 0);
            int xTo = Math.Min(sector.X + 1, sectors.GetLength(0) - 1);
            int yFrom = Math.Max(sector.Y - 1, 0);
            int yTo = Math.Min(sector.Y + 1, sectors.GetLength(1) - 1);

            Sector[,] result = new Sector[(xTo - xFrom) + 1, (yTo - yFrom) + 1];

            for (int i = xFrom, x = 0; i <= xTo; i++, x++)
                for (int j = yFrom, y = 0; j <= yTo; j++, y++)
                    result[x, y] = sectors[i, j];

            return result;
        }
        #endregion

        #region Sector Revealing
        public List<Sector> RevealSector(int xIndex, int yIndex)
        {
            Sector sector = sectors[xIndex, yIndex];
            List<Sector> result = new List<Sector>();

            if (sector.IsRevealed || sector.IsMarked)
                return result;

            sector.IsRevealed = true;

            if (sector.HasMine)
            {
                State = GameState.LOST;

                foreach (Sector s in sectors)
                    result.Add(s);
            }
            else
            {
                result.AddRange(GetAllRevealableSectors(sector));

                if (remainingMineFreeSectorsCount == 0)
                    State = GameState.WON;
            }

            return result;
        }

        private List<Sector> GetAllRevealableSectors(Sector sector)
        {
            List<Sector> sectors = new List<Sector>();
            Queue<Sector> queue = new Queue<Sector>();
            sector.IsRevealed = true;
            queue.Enqueue(sector);
            Sector current;

            while (queue.Count > 0)
            {
                remainingMineFreeSectorsCount--;
                current = queue.Dequeue();
                sectors.Add(current);

                if (current.Value == 0)
                {
                    EnqueueNewNeighbors(sectors, queue, current);
                }
            }

            return sectors;
        }

        private void EnqueueNewNeighbors(List<Sector> alreadyAdded, Queue<Sector> inProcess, Sector sector)
        {
            Sector[,] neighbors = GetAdjacentSectors(sector);

            foreach (Sector s in neighbors)
                if (!s.IsRevealed && !s.IsMarked)
                {
                    inProcess.Enqueue(s);
                    s.IsRevealed = true;
                }
        }
        #endregion

        #region Sector Marking
        public void ToggleMark(int xIndex, int yIndex)
        {
            Sector current = sectors[xIndex, yIndex];

            if (current.IsRevealed)
                return;

            if (current.IsMarked)
            {
                current.IsMarked = false;
                markedSetorsCount--;
            }
            else
            {
                current.IsMarked = true;
                markedSetorsCount++;
            }
        }
        #endregion

        #region Network
        public string GenerateXmlRepresentation()
        {
            StringWriter stringwriter = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(stringwriter);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartElement("MineSweeper");
            writer.WriteElementString("RemainingMines", (minesCount - markedSetorsCount).ToString());
            writer.WriteElementString("GameState", ((int)State).ToString());
            writer.WriteStartElement("Dimensions");
            writer.WriteElementString("Width", sectors.GetLength(1).ToString());
            writer.WriteElementString("Height", sectors.GetLength(0).ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("Sectors");
            string currentState;

            for (int i = 0; i < sectors.GetLength(0); i++)
            {
                for (int j = 0; j < sectors.GetLength(1); j++)
                {
                    writer.WriteStartElement("Sector");
                    writer.WriteElementString("X", sectors[i, j].X.ToString());
                    writer.WriteElementString("Y", sectors[i, j].Y.ToString());

                    if (sectors[i, j].IsMarked)
                        currentState = "-3";
                    else if (!sectors[i, j].IsRevealed)
                        currentState = "-2";
                    else if (sectors[i, j].HasMine)
                        currentState = "-1";
                    else
                        currentState = sectors[i, j].Value.ToString();

                    writer.WriteElementString("State", currentState);
                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement();
            writer.WriteElementString("ElapsedTime", "0");
            writer.WriteEndElement();
            string result = stringwriter.ToString();

            return result;
        }

        public void StartNetwork(int port)
        {
            networkUnit.Start(port);
        }

        public void StopNetwork()
        {
            networkUnit.Stop();
        }

        public class NetworkUnit
        {
            private readonly MinesweeperCommand RESPONSE_COMMAND;
            private Minesweeper model;
            private ServerSideConnection serverConnection;
            private TcpCommandSender sender;
            private TcpCommandReceiver receiver;
            private TcpCommandMessage currentReceivedMessage = new TcpCommandMessage();
            private const int RECEIVE_BUFFER_SIZE = 10 * 1024 * 1024;
            private Thread thread;
            private const int DELAY = 1000;

            public NetworkUnit(Minesweeper model)
            {
                this.model = model;
                RESPONSE_COMMAND = new MinesweeperCommand(MinesweeperCommand.CommandType.CURRENT_STATE);
            }

            public void Start(int port)
            {
                thread = new Thread(new ParameterizedThreadStart(DoWork));
                thread.Start(port);
            }

            public void Stop()
            {
                if (serverConnection != null)
                {
                    serverConnection.StopServer();
                }
            }

            private void DoWork(object port)
            {
                serverConnection = new ServerSideConnection((int)port, RECEIVE_BUFFER_SIZE, new MinesweeperCommandConverter());

                while (serverConnection.WaitForIncomingConnection())
                {
                    model.OnAgentConnected();
                    sender = serverConnection.GetTcpCommandSender();
                    receiver = serverConnection.GetTcpCommandReceiver();

                    if (ReceiveInitializationMessage())
                    {
                        RepetetiveWork();
                    }
                }
            }

            private bool ReceiveInitializationMessage()
            {
                if (receiver.ReceiveWithoutAck(-1, currentReceivedMessage) == TcpCommandIOBase.TransmissionResult.Successful)
                {
                    ProcessMessage(currentReceivedMessage);
                    return true;
                }

                return false;
            }

            private void RepetetiveWork()
            {
                while (true)
                {
                    if(!model.blockGUI)
                        Thread.Sleep(DELAY);

                    if (SendCurrentGameState() == TcpCommandIOBase.TransmissionResult.Successful)
                    {
                        if (receiver.ReceiveWithoutAck(-1, currentReceivedMessage) == TcpCommandIOBase.TransmissionResult.Successful)
                            ProcessMessage(currentReceivedMessage);
                        else
                            break;
                    }
                    else
                    {
                        break;
                    }
                }

                model.OnAgentDisconnected();
            }

            private void ProcessMessage(TcpCommandMessage message)
            {
                MinesweeperCommand command = (MinesweeperCommand)message.Command;

                switch (command.Type)
                {
                    case MinesweeperCommand.CommandType.NEW_GAME:
                        ExecuteNewGameCommand(message.Parameters);
                        break;
                    case MinesweeperCommand.CommandType.REVEAL_AND_MARK:
                        ExecuteRevealAndMarkCommand(message.Parameters);
                        break;
                }
            }

            private void ExecuteNewGameCommand(byte[] commandParameters)
            {
                int height = commandParameters[0];
                int width = commandParameters[1];
                int minesCount = commandParameters[2];
                int x = commandParameters[3];
                int y = commandParameters[4];
                NewGameEventArgs e;

                if (commandParameters.Length == 9)//Seed is sent
                {
                    byte[] seedBytes = new byte[4];
                    commandParameters.CopyTo(seedBytes, 3);
                    int seed = ByteArrayConverter.ToInt32(seedBytes);
                    model.NewGame(height, width, minesCount, seed);
                    e = new NewGameEventArgs(height, width, minesCount, seed);
                }
                else//Seed is not sent
                {
                    model.NewGame(height, width, minesCount);
                    e = new NewGameEventArgs(height, width, minesCount);
                }

                model.OnNewGame(e);
                List<Sector> initialList = model.RevealSector(x, y);
                RevealSectorsEventArgs ee = new RevealSectorsEventArgs(initialList);
                model.OnRevealSectors(ee);
            }

            private void ExecuteRevealAndMarkCommand(byte[] commandParameters)
            {
                int toRevealSectorsCount;
                int toMarkSectorsCount;
                int xIndex;
                int yIndex;
                toRevealSectorsCount = commandParameters[0];
                List<Sector> currentRevealList;

                for (int i = 1; i <= toRevealSectorsCount * 2; i += 2)
                {
                    xIndex = commandParameters[i];
                    yIndex = commandParameters[i + 1];
                    currentRevealList = model.RevealSector(xIndex, yIndex);
                    model.OnRevealSectors(new RevealSectorsEventArgs(currentRevealList));
                }

                toMarkSectorsCount = commandParameters[toRevealSectorsCount * 2 + 1];

                int start = toRevealSectorsCount * 2 + 2;
                int end = start + toMarkSectorsCount * 2;

                for (int i = start; i < end; i += 2)
                {
                    xIndex = commandParameters[i];
                    yIndex = commandParameters[i + 1];
                    model.ToggleMark(xIndex, yIndex);
                    model.OnMarkSector(new MarkSectorEventArgs(new SectorPosition(xIndex, yIndex)));
                }
            }

            private TcpCommandIOBase.TransmissionResult SendCurrentGameState()
            {
                string stateAsString = model.GenerateXmlRepresentation();
                byte[] stateAsBytes = ASCIIEncoding.UTF8.GetBytes(stateAsString);
                TcpCommandMessage message = new TcpCommandMessage(RESPONSE_COMMAND, stateAsBytes);
                if (model.state == GameState.WON)//added
                {
                    Settings.Default.totalGamesWon += 1;
                    Settings.Default.Save();
                }
                return sender.SendWithoutAck(message);
            }
        }
        #endregion
        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < sectors.GetLength(0); i++)
            {
                for (int j = 0; j < sectors.GetLength(1); j++)
                    result += "  " + (sectors[i, j].HasMine ? "*" : sectors[i, j].Value.ToString());

                result += "\n";
            }

            return result;
        }
    }
}