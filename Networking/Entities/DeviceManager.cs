using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Interfaces;

namespace Networking.Entities
{
    static class DeviceManager
    {
        //Importing external C++ dll lib iphlpapi for network implementation
        [DllImport("iphlpapi.dll", ExactSpelling = true , SetLastError = true)]
        private static extern int SendARP(uint DestIP, uint SrcIP,
                                         byte[] pMacAddr, ref uint PhyAddrLen);

        //Prototype for managed call to external SendARP
        public static Tuple<byte[] , uint> SendARPMessage(string DestinationIP , string SourceIPFamily)
        {
            //Declare Tuple members
            byte[] macAddress  = new byte[6];
            uint macAddressLen = 6;
            
            try 
            {
                //IPAddress.Address is deprecated so i'm forced to do this mumbo-jumbo code
                uint destIP = BitConverter.ToUInt32(IPAddress.Parse(DestinationIP).GetAddressBytes() , 0);
                //IPAddress.Address is deprecated so i'm forced to do this mumbo-jumbo code
                uint srcIP = BitConverter.ToUInt32(IPAddress.Parse(SourceIPFamily).GetAddressBytes(), 0);

                int result = SendARP(destIP, srcIP, macAddress, ref macAddressLen);
            }
            catch(Exception e)
            {
                //TO-DO : Exception handling AND Log Error
            }
            return Tuple.Create(macAddress , macAddressLen);
        }

        public static int SendARPMessage(Device DestinationDevice, string SourceIPFamily)
        {
            //Declare Tuple members
            byte[] macAddress = new byte[6];
            uint macAddressLen = 6;

            try
            {
                //IPAddress.Address is deprecated so i'm forced to do this mumbo-jumbo code
                uint destIP = BitConverter.ToUInt32(IPAddress.Parse(DestinationDevice.IpAddress).GetAddressBytes(), 0);
                //IPAddress.Address is deprecated so i'm forced to do this mumbo-jumbo code
                uint srcIP = SourceIPFamily == null ? 0 :
                             BitConverter.ToUInt32(IPAddress.Parse(SourceIPFamily).GetAddressBytes(), 0);

                int result = SendARP(destIP, srcIP, macAddress, ref macAddressLen);
                if (result == 0) DestinationDevice.MacAddress = new PhysicalAddress(macAddress);
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }
    }
}
