using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrollRoom
{
    public class Map
    {
        public Map()
        {
            Rooms = new Dictionary<int, Room>();
        }

        public Dictionary<int, Room> Rooms { get; set; }
    }
}
