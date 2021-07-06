using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace PR_Lab1
{
    public class Urls
    {
        private Object MatchLock = new object();
        private int Index { get; set; }
        private string[] Matches { get; set; }
        private CreateSocket CurrentSocket { get; set; }
        public Urls(MatchCollection matches, CreateSocket socket)
        {
            CurrentSocket = socket;
            Matches = matches.Cast<Match>()
                    .Select(MatchSelect)
                    .ToArray();
            Index = -1;
        }

        public string GetFileUrl()
        {
            if (Index + 1 >= Matches.Length)
            {
                Console.WriteLine("Sorry, there are no more images!");
                return null;
            }
            
            lock(MatchLock)
            {
                if (Index + 1 < Matches.Length)
                {
                    Index += 1;
                }

                return Matches.ElementAt(Index);
            }
        }

        public void DownloadImage()
        {
            string fileUrl = null;
            do
            {
                fileUrl = GetFileUrl();
                if (fileUrl != null) {
                    Console.WriteLine("Thread:{0}, file:{1}", Thread.CurrentThread.Name, FileName(fileUrl));
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(new Uri(fileUrl), @"C:\ASUS_X541U\UNIVER2018\SEMESTRUL_VI\PR\lab1images\" + FileName(fileUrl));
                    }

                }
            }
            while (fileUrl != null);
        }

/*        private string MatchSelect(Match m)
        {
            string value = m.Groups[1].Value;
            Uri uri = null;

            if (Uri.TryCreate(value, UriKind.Absolute, out uri))
            {
                return uri.PathAndQuery;
            } else
            {
                if (value.IndexOf("/") != 0)
                {
                    return '/' + value;
                }

                return value;
            }
        }*/

        private string MatchSelect(Match m)
        {
            string baseUrl = "";
            string value = m.Groups[1].Value;
            if (value.Contains("http://") || value.Contains("https://"))
            {
                return value;
            }
            if (CurrentSocket.Port == 443)
            {
                baseUrl = "https://";
            }
            else
            {
                baseUrl = "http://";
            }
            baseUrl += CurrentSocket.Server;

            if (value.IndexOf("/") == 0)
            {
                return baseUrl + value;
            }

            return baseUrl + '/' + value;
        }

        private string FileName(string url)
        {
            string substr = url.Substring(url.LastIndexOf('/') + 1);

            return Regex.Replace(substr, @"(\?|\|/)", "_");
        }
    }
}
