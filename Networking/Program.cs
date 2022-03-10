using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Entities;
using Networking.Interfaces;

namespace Networking
{

    class Program
    { 
        static void GetAllNetworkInformations()
        {
            NetworkInterface[] NTadapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in NTadapters)
            {
                Console.WriteLine("--------------NETWORK INFORMATION CARD-----------------");
                Console.WriteLine($"ID Adapter : {adapter.Id}");
                Console.WriteLine($"Name Adapter :{adapter.Name}");
                Console.WriteLine($"Name Description :{adapter.Description}");
                Console.WriteLine($"Network InterfaceType :{adapter.NetworkInterfaceType.ToString()}");
                Console.WriteLine($"Physical Address (MAC) :{adapter.GetPhysicalAddress().ToString()}");
                var IpPropAdapter = adapter.GetIPProperties();
                if (IpPropAdapter.GetIPv4Properties() != null)
                {
                    var IPv4Porp = IpPropAdapter.GetIPv4Properties();
                    Console.WriteLine($"------------ IP PROPERTIES OF NETWORK CARD--------------");
                    Console.WriteLine($"Index :{IPv4Porp.Index}");
                    foreach (var ipv4 in IpPropAdapter.UnicastAddresses.Where
                     (
                                         ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                     )
                    {
                        Console.WriteLine($"IP Address :{ipv4.Address.ToString()}");
                        Console.WriteLine($"Subnet Mask  :{ipv4.IPv4Mask.ToString()}");
                        Console.Write($"Dns server Address : ");
                        foreach (var dns in IpPropAdapter.DnsAddresses.Where(dns => dns.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                            foreach (var byte_addr in dns.GetAddressBytes())
                                Console.Write($"Byte : {byte_addr.ToString()}");
                        Console.WriteLine("");
                        foreach (var gateway in IpPropAdapter.GatewayAddresses)
                            Console.WriteLine($"Gatey Address  :{gateway.Address}");
                    }
                }
            }
        }
        static void GetInformationFromIP(string ip_addr)
        {
            try
            {
                var result_of_resolve = Dns.GetHostEntry(ip_addr);
                Console.WriteLine($"---------------------------------------------");
                Console.WriteLine($"Resolved HostName = {result_of_resolve.HostName.ToString()}");
                Console.WriteLine($"Resolved IP = {result_of_resolve.AddressList.FirstOrDefault().ToString()}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"---------------------------------------------");
                Console.WriteLine($"Resolved HostName = {e.InnerException}");
                Console.WriteLine($"Resolved IP = {e.Message}");
            }
        }
        public static void ComplexPing(string ip_addr)
        {
            Ping pingSender = new Ping();

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            // Wait 10 seconds for a reply.
            int timeout = 10000;

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            PingOptions options = new PingOptions(128, true);

            // Send the request.
            var reply_async = pingSender.SendPingAsync(ip_addr, 2000, buffer, options);

            reply_async.Wait();

            var reply = reply_async.Result;

            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
            else
            {
                Console.WriteLine(reply.Status);
            }
        }

        static void Main(string[] args)
        {
            Device d1 = new Device(null, "10.50.1.251");
            GetAllNetworkInformations();
            GetInformationFromIP(d1.IpAddress);
            ComplexPing(d1.IpAddress);
            var result = DeviceManager.SendARPMessage(d1, "10.50.1.58");
        }
    }
}
