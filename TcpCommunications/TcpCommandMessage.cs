using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpCommunications
{
    public class TcpCommandMessage
    {
        public const int MESSAGE_NUMBER_LENGTH = 0;
        private int messageNumber;
        private Command command;
        byte[] parameters;

        public int MessageNumber
        {
            get { return messageNumber; }
            set
            {
                if (value >= -1)
                    messageNumber = value;
            }
        }
        public Command Command
        {
            get { return command; }
            set { command = value; }
        }
        public byte[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public TcpCommandMessage(int messageNumber,
                Command command, byte[] parameters)
        {
            MessageNumber = messageNumber;
            Command = command;
            Parameters = parameters;
        }
        public TcpCommandMessage(Command command, byte[] parameters)
            : this(-1, command, parameters)
        {

        }

        public TcpCommandMessage()
        { }
        public void CopyTo(TcpCommandMessage target)
        {
            target.MessageNumber = MessageNumber;
            target.Command = Command;
            target.Parameters = Parameters;
        }

        public override string ToString()
        {
            byte[] parameters = (Parameters == null) ? new byte[0] : Parameters;
            return string.Format("{0}:{1};{2}", MessageNumber, Command, Encoding.UTF8.GetString(parameters));
        }

    }
}
