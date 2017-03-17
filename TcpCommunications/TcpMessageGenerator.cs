using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpCommunications
{
    public class TcpMessageGenerator
    {
        private CommandConverter commandConverter;

        public TcpMessageGenerator(CommandConverter converter)
        {
            commandConverter = converter;
        }
        public int GetMessageLengthInBytes(TcpCommandMessage message)
        {
            return TcpCommandMessage.MESSAGE_NUMBER_LENGTH + commandConverter.LengthInBytes + message.Parameters.Length;
        }
        public byte[] GetBytes(TcpCommandMessage message)
        {
            if (message.Parameters == null)
                message.Parameters = new byte[0];

            byte[] result = new byte[GetMessageLengthInBytes(message)];
            int counter;
            byte[] messageNumberInBytes = ByteArrayConverter.ToArrayOfBytes(message.MessageNumber, TcpCommandMessage.MESSAGE_NUMBER_LENGTH);
            byte[] commandInBytes = commandConverter.GenerateBytes(message.Command);

            for (counter = 0; counter < TcpCommandMessage.MESSAGE_NUMBER_LENGTH; counter++)
                result[counter] = messageNumberInBytes[counter];

            for (counter = 0; counter < commandConverter.LengthInBytes; counter++)
                result[TcpCommandMessage.MESSAGE_NUMBER_LENGTH + counter] = commandInBytes[counter];

            for (counter = 0; counter < message.Parameters.Length; counter++)
                result[TcpCommandMessage.MESSAGE_NUMBER_LENGTH + commandConverter.LengthInBytes + counter] = message.Parameters[counter];

            return result;
        }
        public TcpCommandMessage GenerateMessage(byte[] message, int bytesReceived)
        {
            //<messageNumber-fixed size><CommandCode-fixed size><parameters-fixed size>
            TcpCommandMessage result;

            if (bytesReceived < TcpCommandMessage.MESSAGE_NUMBER_LENGTH + commandConverter.LengthInBytes)
                throw new FormatException("message length is less than expected");

            byte[] messageNumberInBytes = new byte[TcpCommandMessage.MESSAGE_NUMBER_LENGTH];
            byte[] commandInBytes = new byte[commandConverter.LengthInBytes];
            byte[] parameters = new byte[bytesReceived - (TcpCommandMessage.MESSAGE_NUMBER_LENGTH + commandConverter.LengthInBytes)];

            Array.Copy(message, 0, messageNumberInBytes, 0, messageNumberInBytes.Length);
            Array.Copy(message, messageNumberInBytes.Length, commandInBytes, 0, commandInBytes.Length);
            Array.Copy(message, messageNumberInBytes.Length + commandInBytes.Length, parameters, 0, parameters.Length);

            int messageNumber = ByteArrayConverter.ToInt32(messageNumberInBytes);
            Command command = commandConverter.GenerateCommand(commandInBytes);
            result = new TcpCommandMessage(messageNumber, command, parameters);

            return result;
        }
        public bool IsAckMessage(TcpCommandMessage message)
        {
            if (message.Parameters.Length == 0 && commandConverter.IsAckCommand(message.Command))
                return true;
            else
                return false;
        }
        public bool IsAckMessage(TcpCommandMessage message,int messageNumber)
        {
            return (IsAckMessage(message) && (messageNumber == message.MessageNumber));
        }
        public TcpCommandMessage GenerateAckMessage(int messageNumber)
        {
            return new TcpCommandMessage(messageNumber, commandConverter.GenerateAckCommand(), null);
        }
        public TcpCommandMessage GenerateAckMessage(TcpCommandMessage originalMessage)
        {
            return GenerateAckMessage(originalMessage.MessageNumber);
        }
        public int GetAckMessageSize()
        {
            return TcpCommandMessage.MESSAGE_NUMBER_LENGTH + commandConverter.LengthInBytes;
        }
    }
}
