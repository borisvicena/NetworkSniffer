using PacketDotNet;
using SharpPcap;

namespace project2;

internal static class PacketHandler
{
    // Initialize packet count
    private static int _packetCount;
    
    // Method to handle each packet
    public static void HandlePacket(Packet packet, RawCapture rawCapture, Options options, ILiveDevice device, CancellationTokenSource cts)
    {
        // Check if packet is EthernetPacket
        if (packet is not EthernetPacket ethernetPacket) return;

        // Create packet context
        var context = CreatePacketContext(ethernetPacket, rawCapture);
        
        // Handle packet according to specified protocols
        var packetHandled = HandleProtocols(context, options);
        
        // Check if packet count limit reached, cancel capture if needed
        if (packetHandled && ++_packetCount >= options.Num)
        {
            cts.Cancel();
            device.StopCapture();
        }
    }
    
    // Method to create packet context
    private static PacketContext CreatePacketContext(EthernetPacket ethernetPacket, RawCapture rawCapture)
    {
        return new PacketContext
        {
            EthernetPacket = ethernetPacket,
            IpPacket = ethernetPacket.PayloadPacket as IPPacket,
            TcpPacket = ethernetPacket.Extract<TcpPacket>(),
            UdpPacket = ethernetPacket.Extract<UdpPacket>(),
            ArpPacket = ethernetPacket.Extract<ArpPacket>(),
            NdpPacket = ethernetPacket.Extract<NdpPacket>(),
            Icmp4Packet = ethernetPacket.Extract<IcmpV4Packet>(),
            Icmp6Packet = ethernetPacket.Extract<IcmpV6Packet>(),
            IgmpPacket = ethernetPacket.Extract<IgmpPacket>(),
            PacketLength = rawCapture.PacketLength,
            Data = rawCapture.Data,
            Timestamp = rawCapture.Timeval.Date
        };
    }
    
    // Method to handle specified protocols
    private static bool HandleProtocols(PacketContext context, Options options)
    {
        // Handle ARP packets
        if (context.ArpPacket != null && options.Arp)
        {
            PrintPacket.Arp(context);
            return true;
        }

        // Handle IP packets
        if (context.IpPacket != null)
        {
            return HandleIpSubProtocols(context, options);
        }

        return false;
    }
    
    // Method to handle IP protocols
    private static bool HandleIpSubProtocols(PacketContext context, Options options)
    {
        var packetHandled = false;
        var isProtocolSpecified = AnyProtocolOptionSet(options);
        
        // Handle TCP packets
        if (context.TcpPacket != null && options.Tcp && IsPortMatch(context.TcpPacket.SourcePort, context.TcpPacket.DestinationPort, options) && !packetHandled)
        {
            PrintPacket.Tcp(context);
            packetHandled = true;
        }
        // Handle UDP packets
        if (context.UdpPacket != null && options.Udp && IsPortMatch(context.UdpPacket.SourcePort, context.UdpPacket.DestinationPort, options) && !packetHandled)
        {
            PrintPacket.Udp(context);
            packetHandled = true;
        }
        // Handle MLD packets
        if (context.Icmp6Packet != null && options.Mld && IsMldPacket(context.Icmp6Packet) && !packetHandled)
        {
            PrintPacket.Mld(context);
            packetHandled = true;
        }
        // Handle NDP packets
        if (context.NdpPacket != null && options.Ndp && !packetHandled)
        {
            PrintPacket.Ndp(context);
            packetHandled = true;
        }
        // Handle ICMPv4 packets
        if (context.Icmp4Packet != null && options.Icmp4 && !packetHandled)
        {
            PrintPacket.Icmp4(context);
            packetHandled = true;
        }
        // Handle ICMPv6 packets
        if (context.Icmp6Packet != null && options.Icmp6 && !packetHandled)
        {
            // Check if the packet is an ICMPv6 echo request or response
            if (context.Icmp6Packet.Type is IcmpV6Type.EchoReply or IcmpV6Type.EchoRequest)
            {
                PrintPacket.Icmp6(context);
                packetHandled = true;
            }
        }
        // Handle IGMP packets
        if (context.IgmpPacket != null && options.Igmp && !packetHandled)
        {
            PrintPacket.Igmp(context);
            packetHandled = true;
        }
        // Handle other packets
        if (!isProtocolSpecified && !packetHandled)
        {
            PrintPacket.GenericPacketDetails(context);
            packetHandled = true;
        }
        return packetHandled;
    }

    // Method to check if port matches with specified port
    private static bool IsPortMatch(int srcPort, int dstPort, Options options)
    {
        return
            (options.Port.HasValue && (srcPort == options.Port.Value || dstPort == options.Port.Value)) ||
            (options.PortSource.HasValue && srcPort == options.PortSource.Value) ||
            (options.PortDestination.HasValue && dstPort == options.PortDestination.Value) ||
            (!options.Port.HasValue && !options.PortSource.HasValue && !options.PortDestination.HasValue);
    }
    
    // Method to check if protocol was set
    private static bool AnyProtocolOptionSet(Options options)
    {
        return options.Arp || options.Tcp || options.Udp || options.Ndp ||
               options.Icmp4 || options.Icmp6 || options.Igmp || options.Mld;
    }

    // Method to check for MLD packets
    private static bool IsMldPacket(IcmpV6Packet mldPacket)
    {
        return mldPacket.Type == IcmpV6Type.MulticastListenerQuery ||
               mldPacket.Type == IcmpV6Type.MulticastListenerReport ||
               mldPacket.Type == IcmpV6Type.MulticastListenerDone ||
               mldPacket.Type == IcmpV6Type.Version2MulticastListenerReport;
    }
}
