using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Drawing;

namespace PR_Lab1
{
    public class CreateSocket
    {
        public string Server { get; set; }
        public int Port { get; set; }

        protected CreateSocket(string server, int port)
        {
            Server = server;
            Port = port;
        }

        protected Socket ConnectSocket()
        {
            Socket s = null;
            IPHostEntry hostEntry = null;

            hostEntry = Dns.GetHostEntry(Server);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, Port);
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return s;
        }

        protected string ReadRespMessage(Socket socket, SslStream sslStream)
        {
            // Read the  message sent by the server.
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                if (sslStream != null)
                {
                    bytes = sslStream.Read(buffer, 0, buffer.Length);
                }
                else
                {
                   bytes = socket.Receive(buffer, buffer.Length, 0);
                }
                // Use Decoder class to convert from bytes to UTF8
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
            } while (bytes != 0);

            return messageData.ToString();
        }

        public virtual string SendReceive(string path) { return "SendReceive Not Defined"; }
    }
}
