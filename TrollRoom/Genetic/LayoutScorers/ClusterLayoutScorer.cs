using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrollRoom.Genetic.LayoutScorers
{
    public class ClusterLayoutScorer
    {
        public double Score(Map map, Layout layout)
        {
            var coordinates = layout.Bits.ToByteArray();
            double maxDistance = 0;
            double distance = 0;

            for (var i = 0; i < map.Rooms.Count; i++)
            {
                var currentRoom = map.Rooms[i];
                var currentRoomX = coordinates[i * 2];
                var currentRoomY = coordinates[i * 2 + 1];

                distance += GetDistance(currentRoomX, currentRoomY, 16, 16);
                maxDistance += Math.Sqrt((128 * 128) + (128 * 128));
            }
            return 1 - distance.Remap(0, maxDistance, 0, 1);
        }
        private static double GetDistance(byte point1X, byte point1Y, byte point2X, byte point2Y)
        {
            //pythagorean theorem c^2 = a^2 + b^2
            //thus c = square root(a^2 + b^2)
            double a = (double)(point2X - point1X);
            double b = (double)(point2Y - point1Y);

            return Math.Sqrt(a * a + b * b);
        }
    }
}
