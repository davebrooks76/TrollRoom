using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrollRoom
{
    // A map is an abstract *concept* of a game map.  It has rooms, which have exits, which each have a direction (e.g. N, S, E etc.), and a destination room.
    // It does not contain any positioning information for the rooms.
    public class Map
    {
        public Map()
        {
            Rooms = new Dictionary<int, Room>();
        }

        public Dictionary<int, Room> Rooms { get; set; }
    }
}
