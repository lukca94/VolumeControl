using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeControl
{
    static class Control
    {
        public static Session ChooseSession(List<Session> sessionsList)
        {
            for (int i = 0; i < sessionsList.Count(); i++)
            {
                Console.WriteLine($"{i + 1}. {sessionsList[i].name}");
            }
            while (true)
            {
                Console.Write("\n    Choose a process (1,2,3,...) or Exit (0): ");
                string input = Console.ReadLine();
                try
                {
                    int sessionIndex = Int32.Parse(input);
                    if (sessionIndex < 0 || sessionIndex > sessionsList.Count())
                    {
                        Console.Write("\n    Invalid input, try again: ");
                        continue;
                    }
                    if (sessionIndex == 0)
                        return null;
                    else
                    {
                        Console.WriteLine();
                        return sessionsList[sessionIndex - 1];

                    }
                }
                catch (Exception)
                {
                    Console.Write("\n    Invalid input, try again: ");
                    continue;
                }
            }
        }
        public static void SessionActions(Session currentSession)
        {
            while (true)
            {
                Console.Write($"    Choose an operation for process {currentSession.name}:\nVolume up (1), Volume down (2), Toggle mute (3) or Exit (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();
                if (input == "1")
                {
                    Control.VolumeUp(currentSession, 5);
                    continue;
                }
                else if (input == "2")
                {
                    Control.VolumeDown(currentSession, 5);
                    continue;
                }
                else if (input == "3")
                {
                    Control.Mute(currentSession);
                    continue;
                }
                else if (input == "0")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }
            }
        }
        public static void VolumeUp(Session session, float increment) //increment inputed in %s
        {
            increment = increment / 100;
            if (session.volume.MasterVolume + increment > 1)
                session.volume.MasterVolume = 1;
            else
                session.volume.MasterVolume = session.volume.MasterVolume + increment;
            Console.WriteLine("Current Volume is: " + session.volume.MasterVolume * 100 + "%\n");
        }
        public static void VolumeDown(Session session, float increment) //increment inputed in %
        {
            increment = increment / 100;
            if (session.volume.MasterVolume - increment < 0)
                session.volume.MasterVolume = 0;
            else
                session.volume.MasterVolume = session.volume.MasterVolume - increment;
            Console.WriteLine("Current Volume is: " + session.volume.MasterVolume * 100 + "%\n");
        }
        public static void Mute(Session session) //mute toggle
        {
            if (session.volume.IsMuted)
            {
                session.volume.IsMuted = false;
                Console.WriteLine("Process is now unmuted.");
            }
            else
            {
                session.volume.IsMuted = true;
                Console.WriteLine("Process is now muted.");
            }
        }
    }
}
