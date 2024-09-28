using CommandLine;

namespace project2;

public class Options
{
    [Option('i', "interface", Required = false, HelpText = "Specify the network interface")]
    public string? Interface { get; set; }

    [Option('p', "port", Required = false, HelpText = "Specify the port")]
    public int? Port { get; set; }
    
    [Option("port-source", Required = false, HelpText = "Specify the source port.")]
    public int? PortSource { get; set; }
    
    [Option("port-destination", Required = false, HelpText = "Specify the destination port.")]
    public int? PortDestination { get; set; }

    [Option('t', "tcp", Required = false, HelpText = "Filter TCP packets")]
    public bool Tcp { get; set; }

    [Option('u', "udp", Required = false, HelpText = "Filter UDP packets")]
    public bool Udp { get; set; }
    
    [Option("arp", Required = false, HelpText = "Display only ARP frames")]
    public bool Arp { get; set; }
    
    [Option("ndp", Required = false, HelpText = "Display only NDP packets")]
    public bool Ndp { get; set; }
    
    [Option("icmp4", Required = false, HelpText = "Display only ICMPv4 packets.")]
    public bool Icmp4 { get; set; }
    
    [Option("icmp6", Required = false, HelpText = "Display only ICMPv6 echo request/response.")]
    public bool Icmp6 { get; set; }
    
    [Option("igmp", Required = false, HelpText = "Display only IGMP packets.")]
    public bool Igmp { get; set; }
    
    [Option("mld", Required = false, HelpText = "Display only MLD packets.")]
    public bool Mld { get; set; }

    [Option('n', Required = false, Default = 1, HelpText = "Number of packets to capture")]
    public int Num { get; set; }
}