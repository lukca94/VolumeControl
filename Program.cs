using System;
using System.Collections.Generic;


namespace VolumeControl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sessionEnumerator = GetSessionEnumerator.GetAudioSessionEnumerator();

            List<Session> sessionsList = new List<Session>();

            foreach (var session in sessionEnumerator)
            {
                sessionsList.Add(new Session(session));
            }

            while (true)
            {
                var currentSession = Control.ChooseSession(sessionsList);
                if (currentSession != null)
                {
                    Control.SessionActions(currentSession);
                }
                else { break; }
            }

            Console.WriteLine("\n    Press anything to quit...");
            Console.ReadKey();
        }
    }
}
