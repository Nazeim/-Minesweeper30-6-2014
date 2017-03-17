using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TcpCommunications
{
    public class ClientSideConnection : TcpConnectionManager
    {
        TcpClient client;
        int receiveBufferSize;
        int sendBufferSize;

        public ClientSideConnection(int receiveBufferSize, int sendBufferSize, 
            CommandConverter commandConverter)
        {
            this.receiveBufferSize = receiveBufferSize;
            this.sendBufferSize = sendBufferSize;
            this.converter = commandConverter;
        }

        public bool Connect(string serverIpAddress, int port)
        {
            try
            {
                client = new TcpClient();
                client.SendBufferSize = sendBufferSize;
                client.ReceiveBufferSize = receiveBufferSize;
                client.NoDelay = true;
                socket = client.Client;
                client.Connect(serverIpAddress, port);

                if (receiver == null || sender == null)
                {
                    receiver = new TcpCommandReceiver(socket, converter);
                    sender = new TcpCommandSender(socket, converter);
                }
                else
                {
                    receiver.Connection = socket;
                    sender.Connection = socket;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// This method calls CloseSocket() method and closes the underlying TcpClient
        /// </summary>
        public void Disconnect()
        {
            try
            {
                base.CloseSocket();
                client.Close();
            }
            catch
            { }
        }
    }
}
