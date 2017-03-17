using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpCommunications
{
    public abstract class CommandConverter
    {
        public abstract int LengthInBytes
        {
            get;
        }

        public abstract Command GenerateCommand(byte[] commandBytes);

        public abstract byte[] GenerateBytes(Command command);

        public abstract Command GenerateAckCommand();

        public abstract bool IsAckCommand(Command command);
    }
}
