using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Class representing the state of a Socket that is opened using the Network
    /// code in the Snake game.
    /// </summary>
    public class SocketState
    {
        public Action<SocketState> callMe;
        private Socket socket;
        private int id;
        private byte[] messageBuffer = new byte[2048];
        private StringBuilder sb = new StringBuilder();

        /// <summary>
        /// Constructor of a SocketState object.
        /// </summary>
        /// <param name="_socket"></param>
        /// <param name="_ID"></param>
        public SocketState(Socket _socket, int _ID)
        {
            socket = _socket;
            id = _ID;
        }

        /// <summary>
        /// Property containing a socket object.
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
            set { socket = value; }
        }

        /// <summary>
        /// Property containing a byte[] that is a messageBuffer.
        /// </summary>
        public byte[] MessageBuffer
        {
            get { return messageBuffer; }
            set { messageBuffer = value; }
        }

        /// <summary>
        /// Property containing a StringBuilder.
        /// </summary>
        public StringBuilder SB
        {
            get { return sb; }
        }

        public int ID
        {
            get { return id; }
        }
    }
}
