using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace PR_Lab1
{
    public class HttpsSocket : CreateSocket
    {
        public HttpsSocket(string server, int port) : base(server, port)
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
                    Console.WriteLine("Connection failed");
                    return ("Connection failed");
                }

                SslStream sslStream = new SslStream(new NetworkStream(s), false);
                
                try
                {
                    sslStream.AuthenticateAsClient(Server);
                }
                catch (AuthenticationException e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                    if (e.InnerException != null)
                    {
                        Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                    }
                    Console.WriteLine("Authentication failed - closing the connection.");
                    s.Close();
                    return "";
                }

                // Send request.
                sslStream.Write(bytesSent);
                sslStream.Flush();

                // Read message from the server.
                page = ReadRespMessage(null, sslStream);

                // Close the client connection.
                s.Close();
                Console.WriteLine("Client closed.");
            }
                
            return page;
        }
    }
}
