using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Interfaces;

namespace Networking.Entities
{
    class Device : IDevice
    {
        private string _name;
        private string _ipaddr;
        private PhysicalAddress _macaddr;
        private string _mac_manufacter;

        public string Name { get => _name; set => _name = value; }
        public string IpAddress { get => _ipaddr; set => _ipaddr = value; }
        public PhysicalAddress MacAddress { get => _macaddr; set => _macaddr = value; }
        public string Mac_Manufacter { get => _mac_manufacter; set => _mac_manufacter = value; }

        public Device()
        {
            _name    = String.Empty;
            _ipaddr  = String.Empty;
            _macaddr = null;
            _mac_manufacter = String.Empty;
        }

        public Device(string hostname) : this(hostname, null , null , null) { }

        public Device(string hostname , string ipaddress) : this(hostname , ipaddress , null , null) { }

        public Device(string hostname, string ipaddress, string MacAddress) : this(hostname, ipaddress, MacAddress, null) { }

        public Device(string hostname, string ipaddress, string MacAddress , string MacVendor)
        {
            _name           = hostname;
            _ipaddr         = ipaddress;
            _macaddr        = PhysicalAddress.Parse(MacAddress);
            _mac_manufacter = MacVendor;
        }
    }
}
