using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace TcpCommunications
{
    public abstract class TcpConnectionManager
    {
        protected Socket socket;
        protected TcpCommandSender sender;
        protected TcpCommandReceiver receiver;
        protected CommandConverter converter;

        public bool IsSocketOpen()
        {
            return (socket != null && socket.Connected);
        }

        /// <summary>
        /// Calls Cancel() for both TcpCommandSender and TcpCommandReceiver
        /// </summary>
        public void CloseSocket()
        {
            try
            {
                if (sender != null)
                    sender.Cancel();

                if (receiver != null)
                    receiver.Cancel();

                if (socket != null)
                    socket.Close();
            }
            catch
            { }
        }

        //should be called after a connection has been established
        public TcpCommandReceiver GetTcpCommandReceiver()
        {
            if (!IsSocketOpen())
                throw new IOException("the underlying socket is closed");
            if (receiver == null)
                return new TcpCommandReceiver(socket, converter);
            return receiver;
        }

        //should be called after a connection has been established
        public TcpCommandSender GetTcpCommandSender()
        {
            if (!IsSocketOpen())
                throw new IOException("the underlying socket is closed");

            return sender;
        }
    }

}