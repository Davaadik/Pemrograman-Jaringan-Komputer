using System;
using System.Net;
using System.Threading;

namespace URIDemo
{
    class Program
    {
        public static Uri GetSimpleUri()
        {
            var builder = new UriBuilder();
            builder.Scheme = "http";
            builder.Host = "google.com";
            return builder.Uri;
        }

        static void Main(string[] args)
        {
            var simpleUri = GetSimpleUri();
            Console.WriteLine(simpleUri.ToString());

            Thread.Sleep(10000);
        }
    }
}
