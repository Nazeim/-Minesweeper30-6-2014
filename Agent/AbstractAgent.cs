using System;
using System.Collections.Generic;
using System.Text;
using TcpCommunications;
using System.Threading;
using Agent.Model;

namespace Agent
{
    abstract class AbstractAgent
    {
        public enum NetStatus //added
        {
            NotConnected = 0,
            Connected = 1
        }
        protected Thread thread;
        protected Thread batchManager;
        protected Random randomGenerator = new Random();

        protected NetworkUnit networkUnit;
        protected string currentGameIpAddress;
        protected int currentGamePort;
        /// <summary>
        /// /////////added
        /// </summary>
        protected NetStatus networkstatus;
        protected bool PlayToEnd;
        protected int currentHeight;
        protected int currentWidth;
        protected int currentTotalMinesCount;

        protected Field field;
        protected Units.BayesianUnit bayesianUnit = new Units.BayesianUnit();
        protected Units.PerceptionUnit perceptionUnit = new Units.PerceptionUnit();
        protected Units.NonBoundaryUnit nonBoundariesUnit = new Units.NonBoundaryUnit();

        public AbstractAgent(string initialGameIpAddress, int initialGamePort)
        {
            networkUnit = new NetworkUnit(this);
            CurrentGameIpAddress = initialGameIpAddress;
            CurrentGamePort = initialGamePort;
        }

        #region Public Interface
        public int CurrentGamePort
        {
            get { return currentGamePort; }
            set { currentGamePort = value; }
        }

        public string CurrentGameIpAddress
        {
            get { return currentGameIpAddress; }
            set { currentGameIpAddress = value; }
        }

        public void ForceStop()
        {
            if (networkUnit != null)
            {
                networkUnit.Disconnect();
                Networkstatus = NetStatus.NotConnected;
            }
        }

        public void StartPlaying(int height, int width, int minesCount, bool playToEnd)//added
        {
            currentHeight = height;
            currentWidth = width;
            currentTotalMinesCount = minesCount;
            PlayToEnd = playToEnd;
            thread = new Thread(new ThreadStart(PlayAGame));
            thread.Start();
        }

        public void PlayBatch(int height, int width, int minesCount, int gamesCount)
        {
            currentHeight = height;
            currentWidth = width;
            currentTotalMinesCount = minesCount;
            batchManager = new Thread(new ParameterizedThreadStart(PlayBatchThreadWork));
            batchManager.Start(gamesCount);
            
        }
        protected void PlayBatchThreadWork(object gamesCountObj)
        {
            int gamesCount = (int)gamesCountObj;

            for (int i = 1; i <= gamesCount; i++)
            {
                StartPlaying(currentHeight, currentWidth, currentTotalMinesCount, true);
                thread.Join();
            }
        }
        /// <summary>
        /// /////////added
        /// </summary>
        public NetStatus Networkstatus
        {
            get { return networkstatus; }
            set { networkstatus = value; }
        }

        #endregion

        #region Helper
        protected void PlayAGame()
        {
            bool disconnect = false;
            try
            {
                if (Networkstatus == NetStatus.NotConnected)
                {
                    //Connect
                    if (networkUnit.Connect(currentGameIpAddress, currentGamePort))
                    {
                        //Send New Game Command
                        if (networkUnit.SendNewGameCommand(currentHeight, currentWidth, currentTotalMinesCount))
                        {
                            Networkstatus = NetStatus.Connected;

                            if (PlayToEnd == true)
                                RepetetiveWork();
                            else
                                disconnect = !DoWorkStep();
                        }
                    }
                }
                else
                {
                    if (PlayToEnd == true)
                        RepetetiveWork();
                    else
                        disconnect = !DoWorkStep();
                }
            }
            catch (Exception e)
            {
                int a = 3;
                int b = a;
            }
            finally
            {
                if (PlayToEnd == true || disconnect)
                    OnDisconnectedFromServer();
            }
        }

        protected bool DoWorkStep()
        {
            Networkstatus = NetStatus.NotConnected;
            if (!Preceive())
                return false;

            if (Stop())
            {
                networkUnit.Disconnect();
                return false;
            }

            if (!SendResponse())
                return false;

            Networkstatus = NetStatus.Connected;
            return true;
        }
        protected void RepetetiveWork()
        {
            while (true)
            {
                if (!DoWorkStep())
                    break;
            }
        }

        protected bool Preceive()
        {
            string temp = networkUnit.Perceive();

            if (temp == "")
                return false;

            field = perceptionUnit.GenerateModel(temp);
            bayesianUnit.SetPercept(field);

            return true;
        }

        protected bool Stop()
        {
            switch (field.GameState)
            {
                case Field.GameStateEnum.PLAYING:
                    return false;
                case Field.GameStateEnum.LOST:
                    OnAgentEvent("Game Lost!");
                    return true;
                case Field.GameStateEnum.WON:
                    OnAgentEvent("Game Won!");
                    return true;
            }

            return true;
        }

        protected abstract bool SendResponse();
        #endregion

        #region Events
        public class AgentStatusEventArgs : EventArgs
        {
            protected string description;

            public string Description
            {
                get { return description; }
                set { description = value; }
            }

            public AgentStatusEventArgs(string description)
            {
                this.description = description;
            }
        }

        public event EventHandler<AgentStatusEventArgs> AgentStatus;
        public void OnAgentEvent(string message)
        {
            if (AgentStatus != null)
            {
                AgentStatusEventArgs e = new AgentStatusEventArgs(message);
                AgentStatus(this, e);
            }
        }

        public event EventHandler<EventArgs> DisconnectedFromServer;
        protected void OnDisconnectedFromServer()
        {
            if (DisconnectedFromServer != null)
                DisconnectedFromServer(this, new EventArgs());
        }

        #endregion

        #region Network
        public class NetworkUnit
        {
            protected readonly TcpCommunications.MinesweeperCommands.MinesweeperCommand REVEAL_AND_MARK_COMMAND;
            protected readonly TcpCommunications.MinesweeperCommands.MinesweeperCommand NEW_GAME_COMMAND;
            protected AbstractAgent agent;
            protected ClientSideConnection clientConnection;
            protected TcpCommandSender sender;
            protected TcpCommandReceiver receiver;
            protected TcpCommandMessage currentReceivedMessage = new TcpCommandMessage();
            protected const int BUFFER_SIZE = 10 * 1024 * 1024;
            protected int currentServerPort;
            protected string currentServerAddress;

            /*Public Interface*/
            public NetworkUnit(AbstractAgent agent)
            {
                this.agent = agent;
                REVEAL_AND_MARK_COMMAND = new TcpCommunications.MinesweeperCommands.MinesweeperCommand(TcpCommunications.MinesweeperCommands.MinesweeperCommand.CommandType.REVEAL_AND_MARK);
                NEW_GAME_COMMAND = new TcpCommunications.MinesweeperCommands.MinesweeperCommand(TcpCommunications.MinesweeperCommands.MinesweeperCommand.CommandType.NEW_GAME);
            }

            public bool Connect(string serverIpAddress, int port)
            {
                currentServerAddress = serverIpAddress;
                currentServerPort = port;
                clientConnection = new ClientSideConnection(BUFFER_SIZE, BUFFER_SIZE, new TcpCommunications.MinesweeperCommands.MinesweeperCommandConverter());

                if (clientConnection.Connect(currentServerAddress, currentServerPort))
                {
                    sender = clientConnection.GetTcpCommandSender();
                    receiver = clientConnection.GetTcpCommandReceiver();

                    return true;
                }

                return false;
            }

            public void Disconnect()
            {
                if (clientConnection != null)
                {
                    clientConnection.Disconnect();

                }
            }

            public string Perceive()
            {
                if (receiver != null)
                {
                    if (receiver.ReceiveWithoutAck(-1, currentReceivedMessage) == TcpCommandIOBase.TransmissionResult.Successful)
                    {
                        string result = ASCIIEncoding.UTF8.GetString(currentReceivedMessage.Parameters);

                        return result;
                    }
                }

                return "";
            }

            public bool SendRevealAndMarkCommand(List<Position> toRevealSectors, List<Position> toMarkSectors)
            {
                byte[] parameters = GenerateRevealAndMarkParameters(toRevealSectors, toMarkSectors);
                agent.OnAgentEvent(GenerateRevealAndMarkStatusMessage(toRevealSectors, toMarkSectors));

                return SendMessage(parameters, REVEAL_AND_MARK_COMMAND);
            }

            public bool SendNewGameCommand(int height, int width, int minesCount, int seed)
            {
                byte[] parameters = GenerateNewGameParameters(height, width, minesCount, seed);
                agent.OnAgentEvent(GenerateNewGameStatusMessage(height, width, minesCount, seed));

                return SendMessage(parameters, NEW_GAME_COMMAND);
            }

            public bool SendNewGameCommand(int height, int width, int minesCount)
            {
                byte[] parameters = GenerateNewGameParameters(height, width, minesCount);
                agent.OnAgentEvent(GenerateNewGameStatusMessage(height, width, minesCount, 0));

                return SendMessage(parameters, NEW_GAME_COMMAND);
            }

            /*Helper Methods*/
            protected bool SendMessage(byte[] parameters, Command command)
            {
                TcpCommandMessage message = new TcpCommandMessage(command, parameters);

                if (sender != null)
                {
                    return sender.SendWithoutAck(message) == TcpCommandIOBase.TransmissionResult.Successful;
                }

                return false;
            }

            protected byte[] GenerateNewGameParameters(int height, int width, int minesCount, int seed)
            {
                List<byte> result = new List<byte>(GenerateNewGameParameters(height, width, minesCount));
                result.AddRange(ByteArrayConverter.ToArrayOfBytes(seed, 4));

                return result.ToArray();
            }

            protected byte[] GenerateNewGameParameters(int height, int width, int minesCount)
            {
                List<byte> result = new List<byte>();
                result.Add((byte)height);
                result.Add((byte)width);
                result.Add((byte)minesCount);
                Position random = Position.GenerateRandomPosition(height, width);
                result.Add((byte)random.X);
                result.Add((byte)random.Y);

                return result.ToArray();
            }

            protected byte[] GenerateRevealAndMarkParameters(List<Position> toRevealSectors, List<Position> toMarkSectors)
            {
                List<byte> result = new List<byte>();

                result.Add((byte)toRevealSectors.Count);

                for (int i = 0; i < toRevealSectors.Count; i++)
                {
                    result.Add((byte)toRevealSectors[i].X);
                    result.Add((byte)toRevealSectors[i].Y);
                }

                result.Add((byte)toMarkSectors.Count);

                for (int i = 0; i < toMarkSectors.Count; i++)
                {
                    result.Add((byte)toMarkSectors[i].X);
                    result.Add((byte)toMarkSectors[i].Y);
                }

                return result.ToArray();
            }

            protected string GenerateNewGameStatusMessage(int height, int width, int minesCount, int seed)
            {
                return string.Format("Sending New_Game message: Height={0}, Width={1}, Mines Count={2}, Seed={3}", height, width, minesCount, seed == 0 ? "not set" : seed.ToString());
            }

            protected string GenerateRevealAndMarkStatusMessage(List<Position> toReveal, List<Position> toMark)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Sending Reveal_And_Mark message: ");
                builder.Append("\nReveal Sectors:\n");

                foreach (Position p in toReveal)
                {
                    builder.AppendFormat("X={0}, Y={1}\n", p.X, p.Y);
                }

                builder.Append("Mark Sectors:\n");

                foreach (Position p in toMark)
                {
                    builder.AppendFormat("X={0}, Y={1}\n", p.X, p.Y);
                }

                return builder.ToString();
            }
        }
        #endregion
    }
}
