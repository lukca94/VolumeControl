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
            using (var sessionManager = GetDevice.GetAudioDevice())
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    foreach (var session in sessionEnumerator)
                    {
                        using (var sessionControl = session.QueryInterface<AudioSessionControl2>()) // process name
                        using (var simpleVolume = session.QueryInterface<SimpleAudioVolume>()) //process volume level
                        {
                            Console.WriteLine(sessionControl.Process.ToString()); //write process name
                            Console.WriteLine(sessionControl.ProcessID.ToString());
                            Console.WriteLine(simpleVolume.MasterVolume); //write process volume level

                            //if (sessionControl.Process.ToString() == "System.Diagnostics.Process (chrome)")
                            //    simpleVolume.MasterVolume = simpleVolume.MasterVolume + 0.05f; //fixnout max min range

                        }
                    }
                }
            }
            Console.ReadKey();
        }


    }
}
