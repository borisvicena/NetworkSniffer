using PacketDotNet;

namespace project2;

public class PacketContext
{
    // Define properties for various packet types and metadata
    public required EthernetPacket EthernetPacket { get; init; }
    public IPPacket? IpPacket { get; init; }
    public TcpPacket? TcpPacket { get; init; }
    public UdpPacket? UdpPacket { get; init; }
    public ArpPacket? ArpPacket { get; init; }
    public NdpPacket? NdpPacket { get; init; }
    public IcmpV4Packet? Icmp4Packet { get; init; }
    public IcmpV6Packet? Icmp6Packet { get; init; }
    public IgmpPacket? IgmpPacket { get; init; }
    public int PacketLength { get; init; }
    public byte[]? Data { get; init; }
    public DateTime Timestamp { get; init; }
}