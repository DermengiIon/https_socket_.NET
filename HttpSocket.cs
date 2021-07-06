using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PR_Lab1
{
    public class HttpSocket: CreateSocket
    {
        public HttpSocket(string server, int port): base(server, port)
        {
        }
        public override string SendReceive(string path)
        {
            Byte[] bytesSent = Encoding.ASCII.GetBytes(new Request(Server, path).Headers);
            string page = "";
            using (Socket s = ConnectSocket())
            {
                if (s == null)
                {
                    return ("Connection failed");
                }

                // Send request to the server.
                s.Send(bytesSent, bytesSent.Length, 0);

                page = ReadRespMessage(s, null);
                

                // Close the client connection.
                s.Close();
                Console.WriteLine("Client closed.");
            }

            return page;
        }

    }
}
