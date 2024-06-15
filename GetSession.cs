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
        public static IEnumerable<AudioSessionControl> GetAudioSessionEnumerator()
        {
            using (var sessionManager = GetDevice.GetDefaultAudioDevice()) //Change method for default or pick
            {
                var sessionEnumerator = sessionManager.GetSessionEnumerator();
                return sessionEnumerator;
            }
        }
    }
    static class GetSession
    { 
    
    }
}
