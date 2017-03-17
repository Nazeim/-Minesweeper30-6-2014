using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpCommunications.MinesweeperCommands
{
    public class MinesweeperCommandConverter:CommandConverter
    {
        public override int LengthInBytes
        {
            get { return 1; }
        }

        public override Command GenerateCommand(byte[] commandBytes)
        {
            return new MinesweeperCommand(commandBytes[0]);
        }

        public override byte[] GenerateBytes(Command command)
        {
            return new byte[] { (byte)((MinesweeperCommand)command).Type };
        }

        public override Command GenerateAckCommand()
        {
            return new MinesweeperCommand(MinesweeperCommand.CommandType.ACK);
        }

        public override bool IsAckCommand(Command command)
        {
            return (((MinesweeperCommand)command).Type == MinesweeperCommand.CommandType.ACK) ;
        }
    }
}
