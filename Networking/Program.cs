using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        foreach (var gateway in IpPropAdapter.GatewayAddresses)
                            Console.WriteLine($"Gatey Address  :{gateway.Address}");
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            GetAllNetworkInformations();
            GetInformationFromIP("127.0.0.1");
            Console.ReadLine();
        }

        static void GetInformationFromIP(string ip_addr)
        {
            var result_of_resolve = Dns.GetHostEntry(ip_addr);
            Console.WriteLine($"---------------------------------------------");
            Console.WriteLine($"Resolved HostName = {result_of_resolve.HostName.ToString()}");
            Console.WriteLine($"Resolved IP = {result_of_resolve.HostName.ToString()}");

        }
    }
}
