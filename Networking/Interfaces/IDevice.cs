using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Interfaces
{
    interface IDevice
    {
        string Name { get; set; }
        string IpAddress { get; set; }
        PhysicalAddress MacAddress { get; set; }
        string Mac_Manufacter { get; set; }
    }
}
