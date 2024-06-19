using CSCore.CoreAudioAPI;
using System.Collections.Generic;

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
    public class Session
    {
        public readonly AudioSessionControl2 control;
        public readonly SimpleAudioVolume volume; 
        public string name;
        public int id;
        //icon, mute state?, group?, device?
                
        public Session(AudioSessionControl session) //contains all info and parameters for an audio session
        {
            control = session.QueryInterface<AudioSessionControl2>();//rozepsat na id a name?
            volume = session.QueryInterface<SimpleAudioVolume>();
            id = control.ProcessID;
            name = control.Process.ToString();
            int openBracketIndex = name.LastIndexOf('(')+1;
            int closeBracketIndex = name.LastIndexOf(')');
            name = name.Substring(openBracketIndex, closeBracketIndex - openBracketIndex);
            name = name[0].ToString().ToUpper()+name.Substring(1);
        }
    }
}
