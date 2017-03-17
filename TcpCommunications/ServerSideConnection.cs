using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Timers;
using System.Net;

namespace TcpCommunications
{
    public class ServerSideConnection : TcpConnectionManager
    {
        private TcpListener server;
        private bool isDisposed;
        private int receiveBufferSize;
        private Timer timeoutTimer;

        public ServerSideConnection(int port, int receiveBufferSize, 
            CommandConverter converter)
        {
            this.receiveBufferSize = receiveBufferSize;
            this.converter = converter;
            server = new TcpListener(IPAddress.Loopback, port);
            isDisposed = false;
        }

        // this is a blocking call and it terminates when an incoming connection is
        // initiated -or- when an exception occurs
        // returns true if a connection is initiated otherwise returns false
        public bool WaitForIncomingConnection()
        {
            try
            {
                if (isDisposed)
                    return false;

                server.Start();
                socket = server.AcceptSocket();
                socket.ReceiveBufferSize = receiveBufferSize;

                if (sender == null || receiver == null)
                {
                    sender = new TcpCommandSender(socket, converter);
                    receiver = new TcpCommandReceiver(socket, converter);
                }
                else
                {
                    sender.Connection = socket;
                    receiver.Connection = socket;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WaitForIncomingConnection(int timeout)
        {
            timeoutTimer = new Timer(timeout);
            timeoutTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            return WaitForIncomingConnection();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                server.Stop();
            }
            catch
            { }
            finally
            {
                timeoutTimer.Stop();
            }
        }

        public void StopServer()
        {
            try
            {
                isDisposed = true;
                base.CloseSocket();
                server.Stop();
            }
            catch
            {
            }
        }
    }
}


