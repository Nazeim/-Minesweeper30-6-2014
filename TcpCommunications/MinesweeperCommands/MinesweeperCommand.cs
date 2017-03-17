using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpCommunications.MinesweeperCommands
{
    public class MinesweeperCommand:Command
    {
        public enum CommandType
        {
            ACK = 0,
            REVEAL_AND_MARK = 1,
            NEW_GAME = 2,
            CURRENT_STATE = 3
        }

        private CommandType type;

        public CommandType Type
        {
            get { return type; }
            set { type = value; }
        }

        public MinesweeperCommand(byte commandIndex)
            :this((CommandType)commandIndex)
        {
        }

        public MinesweeperCommand(CommandType type)
        {
            Type = type;
        }
    }
}
