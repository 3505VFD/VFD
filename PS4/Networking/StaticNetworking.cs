using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public static class StaticNetworking
    {
        public delegate void callbackFunction(SocketState state);

        /// <summary>
        /// Gets the bytes sent and ends the send.
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                int bytesSent = socket.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Makes the initial connection to the server with the given hostname.
        /// </summary>
        /// <param name="callbackFunction"></param>
        /// <param name="hostname"></param>
        /// <returns></returns>
        public static Socket ConnectToServer(Action<SocketState> callbackFunction, string hostname)
        {
            IPHostEntry ipHostInfo;
            IPAddress ipAddress = IPAddress.None;

            try
            {
                ipHostInfo = Dns.GetHostEntry(hostname);
                bool foundIPV4 = false;

                foreach (IPAddress addr in ipHostInfo.AddressList)
                {
                    if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                    {
                        foundIPV4 = true;
                        ipAddress = addr;
                        break;
                    }
                }

                // Didn't find any IPV4 addresses
                if (!foundIPV4)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid Address: " + hostname);
                }


            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("using IP");
                ipAddress = IPAddress.Parse(hostname);
            }

            //Open a socket
            Socket socket = new Socket(SocketType.Stream, ProtocolType.IP);
            SocketState state = new SocketState(socket, -1);
            state.callMe = callbackFunction;

            //Use begin connect method, use it to communicate with ConnectedToServer
            socket.BeginConnect(ipAddress, 2112, ConnectedToServer, state);

            //Return the socket
            return socket;
        }

        /// <summary>
        /// What to do once connected to server.
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectedToServer(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;
            state.Socket.EndConnect(ar);
            state.callMe(state);
            state.Socket.BeginReceive(state.MessageBuffer, 0, state.MessageBuffer.Length - 1, SocketFlags.None, ReceiveCallback, state);
        }

        /// <summary>
        /// Gets the data from the server to be used.
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            SocketState state = (SocketState)ar.AsyncState;
            Socket socket = state.Socket;

            int bytesRead = socket.EndReceive(ar);
            string theMessage = "";

            if (bytesRead > 0)
            {
                theMessage = Encoding.UTF8.GetString(state.MessageBuffer, 0, bytesRead);

                if (!theMessage.EndsWith("\n"))
                {
                    state.SB.Append(theMessage);
                    state.callMe(state);
                }
                else
                {
                    state.callMe(state);
                }
            }

            socket.BeginReceive(state.MessageBuffer, 0, state.MessageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
        }

        /// <summary>
        /// Called when the Client's "View" code requests more data.
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state)
        {
            state.Socket.BeginReceive(state.MessageBuffer, 0, state.MessageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public static void Send(Socket socket, string data)
        {
            //Convert data into bytes, then send to server
            byte[] toBytes = Encoding.ASCII.GetBytes(data);
            try
            {
                socket.BeginSend(toBytes, 0, toBytes.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Runs a loop while waiting for a new client to connect to the server.
        /// </summary>
        /// <param name="callMe"></param>
        public static void ServerAwaitingClientLoop(Action<SocketState> callMe)
        {
            TcpListener listener = new TcpListener(2112);
            SocketListener socketListener = new SocketListener(listener, callMe);
            listener.BeginAcceptSocket(AcceptNewClient, socketListener);
        }

        /// <summary>
        /// Accepts the connection request that the server receives from a client.
        /// </summary>
        /// <param name="ar"></param>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            SocketListener listener = (SocketListener)ar.AsyncState;
            Socket socket = listener.Listener.EndAcceptSocket(ar);
            SocketState state = new SocketState(socket, -1);
            listener.CallMe(state);

            listener.Listener.BeginAcceptSocket(AcceptNewClient, listener);
        }
    }

    public class SocketListener
    {
        private TcpListener listener;
        private Action<SocketState> callMe;

        public SocketListener(TcpListener _listener, Action<SocketState> _callMe)
        {
            listener = _listener;
            callMe = _callMe;
        }

        public TcpListener Listener
        {
            get { return listener; }
        }

        public Action<SocketState> CallMe
        {
            get { return callMe; }
        }
    }
}
