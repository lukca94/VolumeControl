using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


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
            Group[] groups = Control.LoadGroups("GroupSave.json");
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.Write("\n    Choose between Group control (1), Process control (2) or Exit (0): "); //add settings (increment, remove files?,..)
                string input = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    int choice = Int32.Parse(input);
                    if (choice < 0 || choice > 2)
                    {
                        Console.WriteLine("Invalid input...\n");
                        continue;
                    }
                    else if (choice == 0)
                    {
                        break;
                    }
                    else if (choice == 1) //group control
                    {
                        Control.GroupActions(groups, sessionsList);
                    }
                    else if (choice == 2) //process control
                    {
                        while (true)
                        {
                            var currentSession = Control.ChooseSession(sessionsList);
                            if (currentSession != null)
                            {
                                Control.SessionActions(currentSession);
                            }
                            else { break; }
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input...\n");
                    continue;
                }
            }
            
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            Control.SaveGroups(groups, "GroupSave.json");
            Console.Write("\n    Press anything to quit..."); 
            Console.ReadKey();
        }
    }
}
