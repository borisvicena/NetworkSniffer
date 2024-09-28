using System.Globalization;
using System.Net.NetworkInformation;
using System.Text;

namespace project2;

public static class PrintPacket
{
    // Method to print generic packet details
    public static void GenericPacketDetails(PacketContext context)
    {
        PrintProtocolName("Network Traffic Analysis");
        Console.WriteLine($"{"Timestamp:", -20}{GetTimeStamp(context.Timestamp), -40}");
        PrintEthernetPacketDetails(context);
        PrintIpPacketDetails(context);
        PrintPacketBytes(context.Data);
    }

    // Method to print protocol name
    private static void PrintProtocolName(string name)
    {
        var title = name + " - Packet Details";
        Console.WriteLine($"{new string('-', title.Length)}");
        Console.WriteLine(title);
        Console.WriteLine($"{new string('-', title.Length)}");
    }
    
    // Method to print IP packet details
    private static void PrintIpPacketDetails(PacketContext context)
    {
        Console.WriteLine($"{"Source IP:", -20}{context.IpPacket?.SourceAddress, -40}");
        Console.WriteLine($"{"Destination IP:", -20}{context.IpPacket?.DestinationAddress, -40}");
        Console.WriteLine($"{"Protocol:", -20}{context.IpPacket?.Protocol, -40}");
    }
    
    // Method to print Ethernet packet details
    private static void PrintEthernetPacketDetails(PacketContext context)
    {
        Console.WriteLine($"{"Source MAC:", -20}{FormatMacAddress(context.EthernetPacket.SourceHardwareAddress), -40}");
        Console.WriteLine($"{"Destination MAC:", -20}{FormatMacAddress(context.EthernetPacket.DestinationHardwareAddress), -40}");
        Console.WriteLine($"{"Ethernet Type:", -20}{context.EthernetPacket.Type, -40}");
        Console.WriteLine($"{"Frame Length:", -20}{context.PacketLength} bytes");
    }
    
    // Method to print TCP packet details
    public static void Tcp(PacketContext context)
    {
        PrintProtocolName("TCP (Transmission Control Protocol)");
        Console.WriteLine($"{"Timestamp:", -20}{GetTimeStamp(context.Timestamp), -40}");
        PrintEthernetPacketDetails(context);
        PrintIpPacketDetails(context);
        Console.WriteLine($"{"Source Port:", -20}{context.TcpPacket?.SourcePort, -40}");
        Console.WriteLine($"{"Destination Port:", -20}{context.TcpPacket?.DestinationPort, -40}");
        Console.WriteLine($"{"Flags:", -20}{$"(URG: {context.TcpPacket?.Urgent}, ACK: {context.TcpPacket?.Acknowledgment}, PSH: {context.TcpPacket?.Push}, RST: {context.TcpPacket?.Reset}, SYN: {context.TcpPacket?.Synchronize}, FIN: {context.TcpPacket?.Finished})", -40}");
        PrintPacketBytes(context.Data);
    }

    // Method to print UDP packet details
    public static void Udp(PacketContext context)
    {
        PrintProtocolName("UDP (Used Datagram Protocol)");
        Console.WriteLine($"{"Timestamp:", -20}{GetTimeStamp(context.Timestamp), -40}");
        PrintEthernetPacketDetails(context);
        PrintIpPacketDetails(context);
        Console.WriteLine($"{"Source Port:", -20}{context.UdpPacket?.SourcePort, -40}");
        Console.WriteLine($"{"Destination Port:", -20}{context.UdpPacket?.DestinationPort, -40}");
        PrintPacketBytes(context.Data);
    }
    
    // Method to print ARP packet details
    public static void Arp(PacketContext context)
    {
        PrintProtocolName("ARP (Address Resolution Protocol)");
        Console.WriteLine($"{"Timestamp:", -20}{GetTimeStamp(context.Timestamp), -40}");
        PrintEthernetPacketDetails(context);
        Console.WriteLine($"{"Operation:",-20}{context.ArpPacket?.Operation,-40}");
        Console.WriteLine($"{"Protocol Type:", -20}{context.ArpPacket?.ProtocolAddressType}");
        Console.WriteLine($"{"Source IP:",-20}{context.ArpPacket?.SenderProtocolAddress,-40}");
        Console.WriteLine($"{"Destination IP:",-20}{context.ArpPacket?.TargetProtocolAddress,-40}");
        PrintPacketBytes(context.Data);
    }

    // Method to print NDP packet details
    public static void Ndp(PacketContext context)
    {
        PrintProtocolName("NDP (Neighbor Discovery Protocol)");
        Console.WriteLine($"{"Timestamp:", -20}{GetTimeStamp(context.Timestamp), -40}");
        PrintEthernetPacketDetails(context);
        PrintIpPacketDetails(context);
        PrintPacketBytes(context.Data);
    }
    
    // Method to print ICMPv4 packet details
    public static void Icmp4(PacketContext context)
    {
        PrintProtocolName("ICMPv4 (Internet Control Message Protocol Version 4)");
        Console.WriteLine($"{"Timestamp:", -20}{GetTimeStamp(context.Timestamp), -40}");
        PrintEthernetPacketDetails(context);
        PrintIpPacketDetails(context);
        Console.WriteLine($"{"Type:", -20}{context.Icmp4Packet?.TypeCode, -40}");
        PrintPacketBytes(context.Data);
    }
    
    // Method to print ICMPv6 packet details
    public static void Icmp6(PacketContext context)
    {
        PrintProtocolName("ICMPv6 (Internet Control Message Protocol Version 6)");
        Console.WriteLine($"{"Timestamp:", -20}{GetTimeStamp(context.Timestamp), -40}");
        PrintEthernetPacketDetails(context);
        PrintIpPacketDetails(context);
        Console.WriteLine($"{"Type:", -20}{context.Icmp6Packet?.Type, -40}");
        PrintPacketBytes(context.Data);
    }
    
    // Method to print IGMP packet details
    public static void Igmp(PacketContext context)
    {
        PrintProtocolName("IGMP (Internet Group Management Protocol)");
        Console.WriteLine($"{"Timestamp:", -20}{GetTimeStamp(context.Timestamp), -40}");
        PrintEthernetPacketDetails(context);
        PrintIpPacketDetails(context);
        Console.WriteLine($"{"Type:", -20}{context.IgmpPacket?.Type, -40}");
        PrintPacketBytes(context.Data);
    }
    
    // Method to print MLD packet details
    public static void Mld(PacketContext context)
    {
        PrintProtocolName("MLD (Multicast Listener Discovery)");
        Console.WriteLine($"{"Timestamp:", -20}{GetTimeStamp(context.Timestamp), -40}");
        PrintEthernetPacketDetails(context);
        PrintIpPacketDetails(context);
        Console.WriteLine($"{"Type:", -20}{context.Icmp6Packet?.Type, -40}");
        PrintPacketBytes(context.Data);
    }
    
    // Method to print packet bytes
    private static void PrintPacketBytes(byte[]? bytes)
    {
        Console.WriteLine("Bytes offset:");
        for (var i = 0; i < bytes?.Length; i += 16)
        {
            var hexPart = new StringBuilder();
            var asciiPart = new StringBuilder();

            for (var j = 0; j < 16; j++)
            {
                if (i + j < bytes.Length)
                {
                    var b = bytes[i + j];
                    hexPart.Append($"{b:X2} ");
                    asciiPart.Append(b is >= 32 and <= 126 ? (char)b : '.');
                }
                else
                {
                    hexPart.Append("   ");
                }
            }
            
            Console.WriteLine($"0x{i:X4}   {hexPart} {asciiPart}");
        }
    }
    
    // Method to get timestamp string
    private static string GetTimeStamp(DateTime dateTime)
    {
        // Return the formatted date string
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
    }

    // Method to format MAC address
    private static string FormatMacAddress(PhysicalAddress address)
    {
        // Return the formatted MAC address
        return string.Join(":", address.GetAddressBytes().Select(b => b.ToString("X2")));
    }
}