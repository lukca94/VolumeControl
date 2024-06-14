using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeControl
{
    static class GetSessionEnumerator
    {
        public static void GetAudioSessionEnumerator()
        {
            using (var sessionManager = GetDevice.GetDefaultAudioDevice())
            {
                using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                {
                    foreach (var session in sessionEnumerator)
                    {
                        Console.WriteLine(session);
                        using (var sessionControl = session.QueryInterface<AudioSessionControl2>()) //process name
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
    static class GetSession
    { 
    
    }
}
