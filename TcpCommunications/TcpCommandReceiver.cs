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
    public class TcpCommandReceiver : TcpCommandIOBase
    {
        public delegate void ToExecuteBeforeSendingACK(object[] parametersOfDelegate);

        /// <summary>
        /// should not be used. (use TcpConnectionManager.GetTcpCommandReceiver() instead)
        /// </summary>
        /// <param name="connection"></param>
        public TcpCommandReceiver(Socket connection, CommandConverter converter)
            : base(connection, connection.ReceiveBufferSize, converter)
        { }

        private bool IsTimeout(Exception e)
        {
            if (e is IOException && e.InnerException is SocketException)
            {
                if (((SocketException)e.InnerException).ErrorCode == 10060)
                    return true;
            }

            return false;
        }
        private void ReceiveTimedOut(object stateData)
        {
            lock (this)
            {
                hasTimedOut = true;
            }

            //this will cause the blocking call inside ReceiveMessageWithCorrectNumber to quit
            CloseSocket();
        }

        /// <summary>
        /// Throws IOException
        /// </summary>
        /// <returns></returns>
        private TcpCommandMessage ReceiveMessageWithCorrectNumber()
        {
            TcpCommandMessage receivedMessage;

            while (true)//loop until getting a message with the correct format and the correct number
            {
                try
                {
                    receivedMessage = SimpleReceive();//may cause a communication exception

                    if (receivedMessage.MessageNumber == MessageNumber)//correct format and number
                        return receivedMessage;
                }
                catch (ArgumentException)//incorrect format
                {
                    //do nothing just loop
                }
                catch (FormatException)//incorrect format
                {
                    //do nothing just loop
                }
            }
        }
        private void SendAck()
        {
            try
            {
                SimpleSend(messageGenerator.GenerateAckMessage(MessageNumber));
            }
            catch (Exception e)
            {
                ErrorCause = e;

                lock (this)
                {
                    if (hasBeenCancelled)
                        MessageStatus = TransmissionResult.Cancelled;
                    else
                        MessageStatus = TransmissionResult.AckError;
                }

                CloseSocket();
            }
        }

        /*Public Methods*/

        //receives a message with a specific number within a timeout period 
        //and if successful sends an ACK 
        //(within the timeout period mismatching messages will be discarded)
        public TransmissionResult ReceiveWithAck(int expectedMessageNumber, int timeoutInMilliseconds, TcpCommandMessage receivedMessage)
        {
            ReceiveWithoutAck(expectedMessageNumber, timeoutInMilliseconds, receivedMessage);

            if (MessageStatus != TransmissionResult.Successful)
                return MessageStatus;

            SendAck();

            return MessageStatus;
        }
        //receive a message with a specific number within a timeout period 
        //and if successful executes a method then sens
        public TransmissionResult ReceiveWithAck(int expectedMessageNumber, int timeoutInMilliseconds, TcpCommandMessage receivedMessage,
                                                ToExecuteBeforeSendingACK method, object[] methodParameters)
        {
            ReceiveWithoutAck(expectedMessageNumber, timeoutInMilliseconds, receivedMessage);

            if (MessageStatus != TransmissionResult.Successful)
                return MessageStatus;

            method(methodParameters);
            SendAck();

            return MessageStatus;
        }
        //receive a message without a specific number and without specifying a period 
        //and if successful send an ACK 
        //(a mismatching message will result in MessageStatus == TransmissionResult.MessageError)
        public TransmissionResult ReceiveWithAck(int timeoutInMilliseconds, TcpCommandMessage receivedMessage)
        {
            ReceiveWithoutAck(timeoutInMilliseconds, receivedMessage);

            if (MessageStatus != TransmissionResult.Successful)
                return MessageStatus;

            SendAck();

            return MessageStatus;
        }
        //receive a message with a specific number within a timeout period 
        //(within the timeout period mismatching messages will be discarded)
        public TransmissionResult ReceiveWithoutAck(int expectedMessageNumber, int timeoutInMilliseconds, TcpCommandMessage receivedMessage)
        {
            Initialize(expectedMessageNumber);
            TcpCommandMessage message;
            Timer ackTimoutTimer = null;

            try
            {
                ackTimoutTimer = new Timer(new TimerCallback(ReceiveTimedOut), null, timeoutInMilliseconds, Timeout.Infinite);
                message = ReceiveMessageWithCorrectNumber();
                MessageStatus = TransmissionResult.Successful;
                message.CopyTo(receivedMessage);

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
                        if (hasTimedOut)
                            MessageStatus = TransmissionResult.TimedOut;
                        else
                            MessageStatus = TransmissionResult.MessageError;
                }

                CloseSocket();

                return MessageStatus;
            }
            finally
            {
                ackTimoutTimer.Dispose();
            }
        }
        //receive a message without a specific number and without specifying a period
        //(a mismatching message will result in MessageStatus == TransmissionResult.MessageError)
        public TransmissionResult ReceiveWithoutAck(int timeoutInMilliseconds, TcpCommandMessage receivedMessage)
        {
            Initialize(-1);
            TcpCommandMessage message;
            int oldTimeout = IOStream.ReadTimeout;
            IOStream.ReadTimeout = timeoutInMilliseconds;

            try
            {
                message = SimpleReceive();
                MessageNumber = message.MessageNumber;
                IOStream.ReadTimeout = oldTimeout;
                MessageStatus = TransmissionResult.Successful;
                message.CopyTo(receivedMessage);

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
                        if (IsTimeout(e))
                            MessageStatus = TransmissionResult.TimedOut;
                        else
                            MessageStatus = TransmissionResult.MessageError;
                }

                CloseSocket();

                return MessageStatus;
            }
        }

    }
}

