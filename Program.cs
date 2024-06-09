using System;
using System.Linq;
using System.Threading;
using CSCore.CoreAudioAPI;
using System.Diagnostics;


namespace VolumeControl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    foreach (var session in sessionEnumerator)
                    {
                        using (var simpleVolume = session.QueryInterface<SimpleAudioVolume>())
                        using (var sessionControl = session.QueryInterface<AudioSessionControl2>())
                        {
                            Console.WriteLine(sessionControl.Process.ToString());

                            Console.WriteLine(simpleVolume.MasterVolume);
                            if (sessionControl.Process.ToString() == "System.Diagnostics.Process (chrome)")
                                simpleVolume.MasterVolume = simpleVolume.MasterVolume + 0.05f; //fixnout max min range
                            //Console.WriteLine(sessionControl.Process);
                            //Console.WriteLine(sessionControl.ProcessID);
                            
                        
                        }
                    }
                }
            }

            Console.ReadKey();
        }

        private static AudioSessionManager2 GetDefaultAudioSessionManager2(DataFlow dataFlow)
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(dataFlow, Role.Multimedia))
                {
                    Debug.WriteLine("DefaultDevice: " + device.FriendlyName);
                    var sessionManager = AudioSessionManager2.FromMMDevice(device);
                    return sessionManager;
                }
            }
        }
    }
}
