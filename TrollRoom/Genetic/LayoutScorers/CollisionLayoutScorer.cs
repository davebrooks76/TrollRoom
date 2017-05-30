using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrollRoom
{
    public class CollisionLayoutScorer : ILayoutScorer
    {
        public double Score(Map map, Layout layout)
        {
            var totalRooms = map.Rooms.Count;
            var coordinates = layout.Bits.ToByteArray();
            
            var distinctCoordinates = new HashSet<KeyValuePair<int, int>>();

            for (var i = 0; i < totalRooms; i++)
            {
                var currentRoomX = coordinates[i * 2];
                var currentRoomY = coordinates[i * 2 + 1];
                distinctCoordinates.Add(new KeyValuePair<int, int>(currentRoomX, currentRoomY));
            }
            return 1 - Convert.ToDouble(totalRooms - distinctCoordinates.Count).Remap(0, totalRooms, 0, 1);
        }
    }
}
