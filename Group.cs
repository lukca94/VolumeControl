using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeControl
{
    internal class Group //"Music", "Games", "Video", "Communications"
    {
        public string name;
        public List<string> members;

        public Group(string name)
        {
            this.name = name;
        }
    }
}
