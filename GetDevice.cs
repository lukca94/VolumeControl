using CSCore.CoreAudioAPI;
using System;
using System.Linq;

namespace VolumeControl
{
    static class GetDevice
    {    
        public static MMDevice GetAudioDevice() //Get all active devices and choose one
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var devices = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    for (int i = 0; i < devices.Count(); i++)
                    {
                        var device = devices[i];
                        Console.WriteLine($"{i + 1}. {device.FriendlyName}");
                    }
                    Console.Write("\n    Choose a device (1,2,3,...): ");
                    
                    while (true) 
                    {
                        string input = Console.ReadLine();
                        try
                        {
                            int deviceIndex = Int32.Parse(input);
                            if (deviceIndex <= 0 || deviceIndex > devices.Count())
                            {
                                Console.Write("\nInvalid input...\n ");
                                continue;
                            }
                            Console.WriteLine();
                            return devices[deviceIndex - 1];
                        }
                        catch (Exception)
                        {
                            Console.Write("\nInvalid input...\n ");
                            continue;
                        }
                    }
                }
            }
        }
        public static MMDevice GetDefaultAudioDevice() //Get default active device
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                
            Console.WriteLine($"Default device: {device.FriendlyName}\n");
            
            return device;
        }
    }
}
