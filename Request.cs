using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_Lab1
{
    public class Request
    {
        public string Headers { get; set; }
        public Request(string server, string path)
        {
            if (path == null)
            {
                path = "/";
            }

            Headers =
                "GET "+ path + " HTTP/1.1\r\n" +
                "Host: " + server + "\r\n" +
                "Accept: */*\r\n" +
                "Accept-Encoding: gzip, deflate\r\n" +
                "Content-Language: en-US\r\n" +
                "Accept-Language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\r\n" +
                "Connection: close\r\n\r\n";
            
        }
    }
}
