using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

namespace VolumeControl
{
    static class Control
    {
        public static void MainChoice(Group[] groups, List<Session> sessions, MMDevice device)
        {
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
                        Control.GroupActions(groups, sessions, device);
                    }
                    else if (choice == 2) //process control
                    {
                        while (true)
                        {
                            var currentSession = Control.ChooseSession(sessions);
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
        }
        public static void DeviceVolume(MMDevice device)
        {
            float increment = 5;
            increment = increment / 100;
            var volume = AudioEndpointVolume.FromDevice(device);
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.WriteLine($"\n    Choose an operation for Device:\n");
                Console.Write("    Volume up (1), Volume down (2), Toggle mute (3) or Return (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();   
                try
                {
                    int choice = Int32.Parse(input);
                    if (choice < 0 || choice > 3)
                    {
                        Console.WriteLine("\nInvalid input...\n ");
                        continue;
                    }
                    else if (input == "1")
                    {
                        if (volume.MasterVolumeLevelScalar + increment > 1)
                            volume.MasterVolumeLevelScalar = 1;
                        else
                            volume.MasterVolumeLevelScalar = float.Parse(Math.Round(volume.MasterVolumeLevelScalar + increment, 2).ToString());
                        Console.WriteLine($"Current volume of Device is: {volume.MasterVolumeLevelScalar * 100}%\n");
                        continue;
                    }
                    else if (input == "2")
                    {
                        if (volume.MasterVolumeLevelScalar - increment < 0)
                            volume.MasterVolumeLevelScalar = 0;
                        else
                            volume.MasterVolumeLevelScalar = float.Parse(Math.Round(volume.MasterVolumeLevelScalar - increment, 2).ToString());
                        Console.WriteLine($"Current volume of Device is: {volume.MasterVolumeLevelScalar * 100}%\n");
                        continue;
                    }
                    else if (input == "3")
                    {
                        if (volume.IsMuted)
                        {
                            volume.IsMuted = false;
                            Console.WriteLine($"Device is now unmuted.\n");
                        }
                        else
                        {
                            volume.IsMuted = true;
                            Console.WriteLine($"Device is now muted.\n");
                        }
                        continue;
                    }
                    else if (input == "0")
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid input...\n ");
                    continue;
                }
            }
        }
        public static List<Session> ActiveSessionsFromGroup(Group[] groups, int groupIndex, List<Session> sessionsList)
        {
            List<Session> activeSessions = new List<Session>();
            for (int i = 0; i < groups[groupIndex].members.Count(); i++)
            {
                for (int j = 0; j < sessionsList.Count(); j++)
                {
                    if (groups[groupIndex].members[i] == sessionsList[j].name)
                        activeSessions.Add(sessionsList[j]);
                }
            }
            return activeSessions;
        }
        public static void GroupVolumeControl(Group[] groups, int groupIndex, List<Session> sessionsList)
        {
            while (true)
            {
                List<Session> activeSessions = ActiveSessionsFromGroup(groups, groupIndex, sessionsList);
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.WriteLine($"\n    Choose an operation for group {groups[groupIndex].Name}:\n");
                Console.Write("    Volume up (1), Volume down (2), Toggle mute (3) or Return (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    int choice = Int32.Parse(input);
                    if (choice < 0 || choice > 3)
                    {
                        Console.WriteLine("\nInvalid input...\n ");
                        continue;
                    }
                    else if (input == "1")
                    {
                        foreach(var currentSession in activeSessions)
                        {
                            VolumeUp(currentSession, 5);
                        }
                        continue;
                    }
                    else if (input == "2")
                    {
                        foreach (var currentSession in activeSessions)
                        {
                            VolumeDown(currentSession, 5);
                        }
                        continue;
                    }
                    else if (input == "3")
                    {
                        foreach (var currentSession in activeSessions)
                        {
                            Mute(currentSession);
                        }
                        continue;
                    }
                    else if (input == "0")
                    {
                        break;
                    }
                    else
                    {
                        GroupVolumeControl(groups, choice - 1, sessionsList);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid input...\n ");
                    continue;
                }
            }
        }
        public static void GroupVolumeControlChoose(Group[] groups, List<Session> sessionsList, MMDevice device)
        {
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\n    Groups:\n");
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine($"{i + 1}. {groups[i].Name}");
                    if (groups[i].Members.Count == 0)
                        Console.WriteLine("  - No members");
                    else
                    {
                        for (int j = 0; j < groups[i].Members.Count; j++)
                        {
                            Console.WriteLine($"  - {groups[i].Members[j]}");
                        }
                    }
                }
                Console.WriteLine("5. Device");
                Console.Write("\n    Pick a group to Control (1,2,3,4,5) or Return (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    int choice = Int32.Parse(input);
                    if (choice < 0 || choice > 5)
                    {
                        Console.WriteLine("\nInvalid input...\n ");
                        continue;
                    }
                    else if(choice == 5)
                    {
                        DeviceVolume(device);
                    }
                    else if (choice == 0)
                    {
                        break;
                    }
                    else
                    {
                        GroupVolumeControl(groups, choice - 1, sessionsList);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nInvalid input...\n ");
                    Console.Write(e.ToString()); 
                    continue;
                }

            }
        }
        public static void GroupMembersEditAction(Group[] groups, int groupIndex)
        {
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                if (groups[groupIndex].Members.Count != 0)
                {
                    Console.WriteLine($"\n    Members of group {groups[groupIndex].Name}:\n");
                    for (int j = 0; j < groups[groupIndex].Members.Count; j++)
                    {
                        Console.WriteLine($"{j+1}. {groups[groupIndex].Members[j]}");
                    }
                }
                
                Console.WriteLine($"\n    Choose an action for group {groups[groupIndex].Name}:\n");
                Console.Write("    Add a member (1), Remove a member (2) or Return (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    int choice = Int32.Parse(input);
                    if (choice < 0 || choice > 2)
                    {
                        Console.WriteLine("\nInvalid input...\n ");
                        continue;
                    }
                    else if (choice == 0)
                    {
                        break;
                    }
                    else if (choice == 1) // Add a member
                    {
                        Console.Write("    Type a new member's name: ");
                        string newMember = Console.ReadLine();
                        Console.WriteLine();
                        groups[groupIndex].Members.Add(newMember);
                    }
                    else if (choice == 2) // Remove a member
                    {
                        if (groups[groupIndex].Members.Count == 0)
                        {
                            Console.WriteLine("Group has no members to remove.\n");
                            continue;
                        }
                        else
                        {
                            while(true)
                            {
                                Console.Write("    Choose a member to remove (1,2,3,...) or Return (0): ");
                                input = Console.ReadLine();
                                Console.WriteLine();
                                try
                                {
                                    choice = Int32.Parse(input);
                                    if (choice < 0 || choice > groups[groupIndex].Members.Count)
                                    {
                                        Console.WriteLine("\nInvalid input...\n ");
                                        break;
                                    }
                                    else if (choice == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        groups[groupIndex].Members.RemoveAt(choice-1);
                                        break;
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("\nInvalid input...\n ");
                                    break;
                                }
                            }
                            
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid input...\n ");
                    continue;
                }
            }
        }
        public static void GroupMembersEdit(Group[] groups)
        {
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\n    Groups:\n");
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine($"{i + 1}. {groups[i].Name}");
                    if (groups[i].Members.Count == 0)
                        Console.WriteLine("  - No members");
                    else
                    {
                        for (int j = 0; j < groups[i].Members.Count; j++)
                        {
                            Console.WriteLine($"  - {groups[i].Members[j]}");
                        }
                    }
                }
                Console.Write("\n    Pick a group to edit (1,2,3,4) or Return (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();
                
                try
                {
                    int choice = Int32.Parse(input);
                    if (choice < 0 || choice > 4)
                    {
                        Console.WriteLine("\nInvalid input...\n ");
                        continue;
                    }
                    else if (choice == 0)
                    {
                        break;
                    }
                    else
                    {
                        GroupMembersEditAction(groups, choice-1);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid input...\n ");
                    continue;
                }
                
            }
        }
        public static void GroupNameChange(Group[] groups)
        {
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\n    Groups:\n");
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine($"{i + 1}. {groups[i].Name}");
                }
                Console.Write("\n    Pick a group (1,2,3,4) or Return (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    int choice = Int32.Parse(input);
                    if (choice < 0 || choice > 4)
                    {
                        Console.WriteLine("\nInvalid input...\n ");
                        continue;
                    }
                    else if (choice == 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.Write("    Type a new name: ");
                        string newName = Console.ReadLine();
                        Console.WriteLine();
                        groups[choice - 1].Name = newName;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid input...\n ");
                    continue;
                }
            }
        }
        public static void GroupEdit(Group[] groups)
        {
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.Write("\n    Choose between Group members edit (1), Group name change (2) or Return (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    int choice = Int32.Parse(input);
                    if (choice < 0 || choice > 2)
                    {
                        Console.WriteLine("\nInvalid input...\n ");
                        continue;
                    }
                    else if (choice == 0)
                    {
                        break;
                    }
                    else if (choice == 1) // Group members edit
                    {
                        GroupMembersEdit(groups);
                    }
                    else if (choice == 2) // Group name change
                    {
                        GroupNameChange(groups);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid input...\n ");
                    continue;
                }
            }
        }
        public static void GroupActions(Group[] groups, List<Session> sessionsList, MMDevice device)
        {
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.Write("\n    Choose between Volume control (1), Group edit (2) or Return (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    int choice = Int32.Parse(input);
                    if (choice < 0 || choice > 2)
                    {
                        Console.WriteLine("\nInvalid input...\n ");
                        continue;
                    }
                    else if (choice == 0)
                    {
                        break;
                    }
                    else if (choice == 1) // Volume control
                    {
                        GroupVolumeControlChoose(groups,sessionsList,device);
                    }
                    else if (choice == 2) // Group edit
                    {
                        GroupEdit(groups);    
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid input...\n ");
                    continue;
                }
            }
        }
        public static void NewGroups(string filePath)
        {
            string[] defaultGroupNames = new string[4];
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\n    There are no groups created. Create groups:\n");
            Console.Write("    Enter name of the first group: ");
            defaultGroupNames[0] = Console.ReadLine();
            Console.Write("    Enter name of the second group: ");
            defaultGroupNames[1] = Console.ReadLine();
            Console.Write("    Enter name of the third group: ");
            defaultGroupNames[2] = Console.ReadLine();
            Console.Write("    Enter name of the fourth group: ");
            defaultGroupNames[3] = Console.ReadLine();
            Console.WriteLine();

            var groups = new Group[4];
            for (int i = 0; i < defaultGroupNames.Length; i++)
            {
                groups[i] = new Group { Name = defaultGroupNames[i] };
            }

            string jsonString = JsonSerializer.Serialize(groups, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString );
        }
        public static Group[] LoadGroups(string fileName)
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VolumeControl");
            Directory.CreateDirectory(appDataPath); // Ensure directory exists
            string filePath = Path.Combine(appDataPath, fileName);

            if (File.Exists(filePath)==false) //create group
            {
                NewGroups(filePath);
            }
            string jsonString = File.ReadAllText(filePath);
            Group[] groups = JsonSerializer.Deserialize<Group[]>(jsonString);
            return groups;
        }
        public static void SaveGroups(Group[] groups, string fileName)
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VolumeControl");
            Directory.CreateDirectory(appDataPath); // Ensure directory exists
            string filePath = Path.Combine(appDataPath, fileName);

            string jsonString = JsonSerializer.Serialize(groups, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        }
        public static Session ChooseSession(List<Session> sessionsList)
        {
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            Console.WriteLine();
            for (int i = 0; i < sessionsList.Count(); i++)
            {
                Console.WriteLine($"{i + 1}. {sessionsList[i].name}");
            }
            while (true)
            {
                Console.Write("\n    Choose a process (1,2,3,...) or Return (0): ");
                string input = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    int sessionIndex = Int32.Parse(input);
                    if (sessionIndex < 0 || sessionIndex > sessionsList.Count())
                    {
                        Console.WriteLine("Invalid input...\n ");
                        continue;
                    }
                    else if (sessionIndex == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return sessionsList[sessionIndex - 1];
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input...\n ");
                    continue;
                }
            }
        }
        public static void SessionActions(Session currentSession)
        {
            while (true)
            {
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.WriteLine($"\n    Choose an operation for process {currentSession.name}:\n");
                Console.Write("    Volume up (1), Volume down (2), Toggle mute (3) or Return (0): ");
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
                    Console.WriteLine("Invalid input...\n");
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
                session.volume.MasterVolume = float.Parse(Math.Round(session.volume.MasterVolume + increment, 2).ToString());
            Console.WriteLine($"Current volume of {session.name} is: {session.volume.MasterVolume * 100}%\n");
        }
        public static void VolumeDown(Session session, float increment) //increment inputed in %
        {
            increment = increment / 100;
            if (session.volume.MasterVolume - increment < 0)
                session.volume.MasterVolume = 0;
            else
                session.volume.MasterVolume = float.Parse( Math.Round( session.volume.MasterVolume - increment,2).ToString());
            Console.WriteLine($"Current volume of {session.name} is: {session.volume.MasterVolume * 100}%\n");
        }
        public static void Mute(Session session) //mute toggle
        {
            if (session.volume.IsMuted)
            {
                session.volume.IsMuted = false;
                Console.WriteLine($"{session.name} is now unmuted.\n");
            }
            else
            {
                session.volume.IsMuted = true;
                Console.WriteLine($"{session.name} is now muted.\n");
            }
        }
    }
}
