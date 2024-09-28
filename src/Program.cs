using System.Net.NetworkInformation;
using CommandLine;
using SharpPcap;
using PacketDotNet;

namespace project2;

internal static class Sniffer
{ 
    // Initialize CancellationTokenSource
    private static readonly CancellationTokenSource Cts = new();

    // Main method to parse command line arguments and run the sniffer
    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunSniffer)
            .WithNotParsed(HandleParseError);
    }

    // Method to handle parsing errors
    private static void HandleParseError(IEnumerable<Error> errors)
    {
        Console.WriteLine("Failed to parse command line arguments.");
    }

    // Method to run the sniffer with given options
    private static void RunSniffer(Options options)
    {
        // Get the network device to sniff on
        var device = GetDevice(options);
        if (device == null) return;

        // Open the device in promiscuous mode and set up event handlers
        device.Open(DeviceModes.Promiscuous);
        SetupDeviceHandlers(device, options);

        // Start capturing packets
        StartCapture(device);
    }
    
    // Method to list available network devices
    private static void ListDevices()
    {
        foreach (var dev in NetworkInterface.GetAllNetworkInterfaces())
        {
            Console.WriteLine($"{dev.Name}");
        }
    }
    
    // Method to start capturing packets on a device
    private static void StartCapture(ILiveDevice device)
    {
        try
        {
            // Start capturing packets and wait for cancellation
            device.StartCapture();
            Console.WriteLine("Press Ctrl+C to stop...");
            while (!Cts.Token.IsCancellationRequested)
            {
                Thread.Sleep(1000);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation cancelled by user or packet count reached.");
        }
        finally
        {
            // Stop capturing and close the device
            device.StopCapture();
            device.Close();
        }
    }

    // Method to get the specified network device
    private static ILiveDevice? GetDevice(Options options)
    {
        var devices = CaptureDeviceList.Instance;
        if (devices.Count < 1)
        {
            Console.WriteLine("No devices were found on this machine.");
            return null;
        }
        
        // If no interface specified, list available devices
        if (string.IsNullOrEmpty(options.Interface))
        {
            ListDevices();
            return null;
        }
        
        // Find the device by name
        var device = devices.First(dev => dev.Name.Contains(options.Interface));
        if (device != null) return device;
        
        Console.WriteLine("Device not found.");
        return null;
    }
    
    // Method to set up event handlers for packet arrival and cancellation
    private static void SetupDeviceHandlers(ILiveDevice device, Options options)
    {
        device.OnPacketArrival += (_, e) =>
        {
            ProcessPacket(e, options, device);
        };

        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            Cts.Cancel();
        };
    }
    
    // Method to process each captured packet
    private static void ProcessPacket(PacketCapture e, Options options, ILiveDevice device)
    {
        var rawCapture = e.GetPacket();
        if (rawCapture == null)
            return;
        
        var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
        if (packet == null)
            return;
        
        // Handle the packet
        PacketHandler.HandlePacket(packet, rawCapture, options, device, Cts);
    }
}
