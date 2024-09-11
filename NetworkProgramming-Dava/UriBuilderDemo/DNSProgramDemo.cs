using System;
using System.Net;
using System.Threading;

namespace UriBuilderDemo
{
    class DNSProgramDemo
    {
        static void Main(string[] args)
        {            
            
            var domainEntry = Dns.GetHostEntry("facebook.com");
  
            Console.WriteLine(domainEntry.HostName);
            foreach (var ip in domainEntry.AddressList)
            {
                Console.WriteLine(ip);
            }

            domainEntry = Dns.GetHostEntry(Dns.GetHostName());
            Console.WriteLine(domainEntry.HostName);
            foreach (var ip in domainEntry.AddressList)
            {
                Console.WriteLine(ip);
            }

            IPAddress ipadd = IPAddress.Parse("142.251.10.95");
            var ipEntry = Dns.GetHostEntry(ipadd);
            foreach (var ip in ipEntry.AddressList)
            {
                Console.WriteLine(ip);
            }

            Thread.Sleep(10000);

        }
    }
}
