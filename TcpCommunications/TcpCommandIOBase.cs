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
    public abstract class TcpCommandIOBase
    {
        public enum TransmissionResult
        {
            Unknown,//Initial state
            Successful,//If ACK is received successfully
            TimedOut,//If ACK is not received during the timeout period
            MessageError,//If communication error occurs when sending/receiving the message
            AckError,//If communication error occurs while receiving/sending for the ACKnowledgment
            Cancelled,//If user cancells a receiving
        }

        private NetworkStream ioStream;
        private Socket connection;
        private int messageNumber;
        private Exception errorCause;
        protected byte[] receiveBuffer;
        private TransmissionResult messageStatus;
        protected TcpMessageGenerator messageGenerator;
        protected bool hasBeenCancelled;
        protected bool hasTimedOut;

        protected TransmissionResult MessageStatus
        {
            get
            {
                return messageStatus;
            }
            set
            {
                Monitor.Enter(this);
                messageStatus = value;
                Monitor.Exit(this);
            }
        }

        /// <summary>
        /// changes the socket to operate on, and changes the associated NetworkStream.
        /// the new socket should be opened.
        /// should not be used (use TcpConnectionManager methods instead)
        /// </summary>
        internal Socket Connection
        {
            get { return connection; }
            set
            {
                if (value != null)
                {
                    connection = value;
                    IOStream = new NetworkStream(value);
                }
            }
        }

        /// <summary>
        /// current messageNumber or -1 if messageNumber is undetermined
        /// </summary>
        protected int MessageNumber
        {
            get { return messageNumber; }
            set
            {
                if (value >= -1)
                    messageNumber = value;
            }
        }
        public Exception ErrorCause
        {
            get { return errorCause; }

            protected set
            {
                lock (this)
                {
                    errorCause = value;
                }
            }
        }
        protected NetworkStream IOStream
        {
            get { return ioStream; }
            private set
            {
                ioStream = value;
            }
        }

        protected TcpCommandIOBase(Socket connection, int receiveBufferSize, CommandConverter commandConverter)
        {
            Connection = connection;
            receiveBuffer = new byte[receiveBufferSize];
            messageGenerator = new TcpMessageGenerator(commandConverter);
        }

        protected virtual void Initialize(int messageNumber)
        {
            ErrorCause = null;
            MessageNumber = messageNumber;
            MessageStatus = TransmissionResult.Unknown;
            hasBeenCancelled = false;
            hasTimedOut = false;
        }
        protected void SimpleSend(TcpCommandMessage message)
        {
            byte[] messageBytes = messageGenerator.GetBytes(message);
            IOStream.Write(messageBytes, 0, messageBytes.Length);
        }
        protected TcpCommandMessage SimpleReceive()
        {
            int bytesReceived = IOStream.Read(receiveBuffer, 0, receiveBuffer.Length);

            if (bytesReceived < 1)
                throw new IOException("the stream is closed by the remote side");

            return messageGenerator.GenerateMessage(receiveBuffer, bytesReceived);
        }
        protected void CloseSocket()
        {
            try
            {
                Connection.Close();
            }
            catch
            { }
        }

        /// <summary>
        /// Cancels the blocking method call if one exists. 
        /// This method causes the blocking call to return immediately whith the value
        /// TransmissionResult.Cancelled and causes the underlying Socket to close, so you must not 
        /// use this object and any other objects that rely on the same Socket afterwards 
        /// (all subsequent method calls will return with TransmissionResult.Cancelled)
        /// </summary>
        public void Cancel()
        {
            lock (this)
            {
                hasBeenCancelled = true;
            }

            CloseSocket();
        }
    }
}
