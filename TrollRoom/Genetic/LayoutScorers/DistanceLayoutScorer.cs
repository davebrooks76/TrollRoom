using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrollRoom.Genetic.LayoutScorers
{
    public class DistanceLayoutScorer : ILayoutScorer
    {
        private int bitsLength;
        public DistanceLayoutScorer(int bitsLength)
        {
            this.bitsLength = bitsLength;
        }

        public double Score(Map map, Layout layout)
        {
            var coordinates = layout.Coordinates;
            double maxDistance = 0;
            double distance = 0;

            for (var i = 0; i < map.Rooms.Count; i++)
            {
                var currentRoom = map.Rooms[i];
                var currentRoomX = coordinates[i * 2];
                var currentRoomY = coordinates[i * 2 + 1]; 

                foreach (var exit in currentRoom.Exits)
                {
                    var destinationRoom = exit.Destination;
                    var destinationRoomX = coordinates[destinationRoom.Id * 2];
                    var destinationRoomY = coordinates[destinationRoom.Id * 2 + 1];

                    var distanceToAdd = GetDistance(currentRoomX, currentRoomY, destinationRoomX, destinationRoomY);
                    if (distanceToAdd == 0)
                    {
                        //collision
                        distance += 10;
                    }
                    distance += GetDistance(currentRoomX, currentRoomY, destinationRoomX, destinationRoomY);

                    maxDistance += Math.Sqrt(Math.Pow(2, bitsLength + 6));
                }
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
