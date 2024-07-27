using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace VolumeControl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MMDevice device = GetDevice.GetDefaultAudioDevice();
            
            var sessionEnumerator = GetSessionEnumerator.GetAudioSessionEnumerator(device);

            List<Session> sessions = new List<Session>();
            foreach (var session in sessionEnumerator)
            {
                sessions.Add(new Session(session));

                // PENIS
            }

            Group[] groups = Control.LoadGroups("GroupSave.json");

            Control.MainChoice(groups, sessions, device);
            
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            Control.SaveGroups(groups, "GroupSave.json");
            Console.Write("\n    Press anything to quit..."); 
            Console.ReadKey();
        }
    }
}
