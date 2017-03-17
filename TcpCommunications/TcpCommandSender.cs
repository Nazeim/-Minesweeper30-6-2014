using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

/// <summary>
/// The only thread-safe public method in this class is Cancel()
/// </summary>
/// 
namespace TcpCommunications
{
    public class TcpCommandSender : TcpCommandIOBase
    {
        readonly int ACK_MESSAGE_SIZE;

        /// <summary>
        /// Should not be used. (use TcpConnectionManager.GetTcpCommandSender() instead)
        /// </summary>
        /// <param name="connection"></param>
        public TcpCommandSender(Socket connection, CommandConverter converter)
            : base(connection, converter.LengthInBytes + TcpCommandMessage.MESSAGE_NUMBER_LENGTH, converter)
        {
            ACK_MESSAGE_SIZE = messageGenerator.GetAckMessageSize();
        }

        private void AckTimedOut(object stateData)
        {
            lock (this)
            {
                hasTimedOut = true;
            }

            //this will cause the blocking call inside ReceiveAck to quit
            CloseSocket();
        }

        //Discards all incoming messages except for the approperiate ACK message!!
        private void ReceiveAck()
        {
            TcpCommandMessage message;

            try
            {
                while (true)
                {
                    try
                    {
                        message = SimpleReceive();

                        if (messageGenerator.IsAckMessage(message, MessageNumber))
                        {
                            MessageStatus = TransmissionResult.Successful;
                            return;
                        }
                    }
                    catch (ArgumentException)
                    { }
                    catch (FormatException)
                    { }
                }
            }
            catch (Exception e)
            {
                ErrorCause = e;

                lock (this)
                {
                    if (hasBeenCancelled)
                        MessageStatus = TransmissionResult.Cancelled;
                    else
                        if (hasTimedOut)
                            MessageStatus = TransmissionResult.TimedOut;
                        else
                            MessageStatus = TransmissionResult.AckError;
                }

                CloseSocket();
            }
        }

        /*Public Methods*/

        //The timeout is associated with ACK receiving
        public TransmissionResult SendWithAck(TcpCommandMessage message, int timeoutInMilliseconds)
        {
            SendWithoutAck(message);

            if (MessageStatus != TransmissionResult.Successful)
            {
                return MessageStatus;
            }

            Timer ackTimoutTimer = new Timer(new TimerCallback(AckTimedOut), null, timeoutInMilliseconds, Timeout.Infinite);
            //Receiving ACK
            ReceiveAck();

            ackTimoutTimer.Dispose();

            return MessageStatus;
        }
        public TransmissionResult SendWithoutAck(TcpCommandMessage message)
        {
            //Initialization
            Initialize(message.MessageNumber);

            //Sending Message
            try
            {
                SimpleSend(message);
                MessageStatus = TransmissionResult.Successful;
                return MessageStatus;
            }
            catch (Exception e)
            {
                ErrorCause = e;

                lock (this)
                {
                    if (hasBeenCancelled)
                        MessageStatus = TransmissionResult.Cancelled;
                    else
                        MessageStatus = TransmissionResult.MessageError;
                }

                CloseSocket();

                return MessageStatus;
            }
        }
    }
}

