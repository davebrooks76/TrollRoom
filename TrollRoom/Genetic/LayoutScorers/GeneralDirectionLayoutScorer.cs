using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrollRoom.Genetic.LayoutScorers
{
    public class GeneralDirectionLayoutScorer : ILayoutScorer
    {
        public double Score(Map map, Layout layout)
        {
            var coordinates = layout.Bits.ToByteArray();
            double correctExits = 0;
            double totalExits = 0;

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
                    totalExits++;

                    switch (exit.Direction)
                    {
                        case Direction.North:
                            if (destinationRoomY > currentRoomY)
                                correctExits++;
                            break;
                        case Direction.Northeast:
                            if (destinationRoomY > currentRoomY &&
                                destinationRoomX > currentRoomX)
                                correctExits++;
                            break;
                        case Direction.East:
                            if (destinationRoomX > currentRoomX)
                                correctExits++;
                            break;
                        case Direction.Southeast:
                            if (destinationRoomY < currentRoomY &&
                                destinationRoomX > currentRoomX)
                                correctExits++;
                            break;
                        case Direction.South:
                            if (destinationRoomY < currentRoomY)
                                correctExits++;
                            break;
                        case Direction.Southwest:
                            if (destinationRoomY < currentRoomY &&
                                destinationRoomX < currentRoomX)
                                correctExits++;
                            break;
                        case Direction.West:
                            if (destinationRoomX < currentRoomX)
                                correctExits++;
                            break;
                        case Direction.Northwest:
                            if (destinationRoomY > currentRoomY &&
                                destinationRoomX < currentRoomX)
                                correctExits++;
                            break;
                        case Direction.Down:
                            if (destinationRoomY < currentRoomY)
                                correctExits++;
                            break;
                        case Direction.Up:
                            if (destinationRoomY > currentRoomY)
                                correctExits++;
                            break;
                    }
                }
            }
            return correctExits.Remap(0, totalExits, 0, 1);
        }
    }
}
