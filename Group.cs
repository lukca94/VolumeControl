using System.Collections.Generic;

namespace VolumeControl
{
    public class Group //"Music", "Games", "Video", "Communications"
    {
        public List<string> members = new List<string>();
        public string Name { get; set; }
        public List<string> Members { get { return members; } set { members = value; } }

        
    }
}
