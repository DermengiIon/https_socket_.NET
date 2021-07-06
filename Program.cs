using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace PR_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "utm.md";
            //string host = "me.utm.md";
            int port = 443;
            //int port = 80;

            int threadsNr = 4;
            CreateSocket socket = GetSocketByPort(host, port);

            Urls urls = new Urls(GetMatchCollection(socket.SendReceive(null)), socket);

            StartThreads(urls, threadsNr);

            Console.ReadLine();
        }

        static CreateSocket GetSocketByPort(string server, int port)
        {
            if (port == 443)
            {
                return new HttpsSocket(server, port);
            }

            return new HttpSocket(server, port);
        }

        static MatchCollection GetMatchCollection(string html)
        {
            string pattern = @"<img.+?src=[""'](.+?(.jpg|.png|.gif|.jpeg))[""'].*?>";
            Regex rgx = new Regex(pattern);

            return rgx.Matches(html);
        }

        static void StartThreads(Urls urls, int threadsNr)
        {
            Thread[] threads = new Thread[threadsNr];

            Thread.CurrentThread.Name = "main";

            for (int i = 0; i < threadsNr; i++)
            {
                Thread t = new Thread(new ThreadStart(urls.DownloadImage));
                t.Name = "T" + (i + 1).ToString();
                threads[i] = t;
            }

            for (int i = 0; i < threadsNr; i++)
            {
                Console.WriteLine("Thread {0} Alive: {1}", threads[i].Name, threads[i].IsAlive);
                threads[i].Start();
                Console.WriteLine("Thread {0} Alive: {1}", threads[i].Name, threads[i].IsAlive);
            }
        }
    }
}
