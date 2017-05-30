using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrollRoom
{
    public class AngleLayoutScorer : ILayoutScorer
    {
        public double Score(Map map, Layout layout)
        {
            var coordinates = layout.Bits.ToByteArray();
            double maxOffset = 0;
            double totalOffset = 0;

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

                    // if the two points are the same, give the worst possible score for the exit
                    if (currentRoomX == destinationRoomX && currentRoomY == destinationRoomY)
                    {
                        maxOffset += 180;
                        totalOffset += 180;
                        continue;
                    }
                    
                    double degrees = GetAngleBetweenTwoPoints(
                        currentRoomX, currentRoomY,
                        destinationRoomX, destinationRoomY);

                    double degreesOffset = 0;
                    switch (exit.Direction)
                    {
                        case Direction.North:
                            degreesOffset = Math.Abs(90 - degrees);
                            break;
                        case Direction.Northeast:
                            degreesOffset = Math.Abs(45 - degrees);
                            break;
                        case Direction.East:
                            degreesOffset = Math.Abs(360 - degrees);
                            break;
                        case Direction.Southeast:
                            degreesOffset = Math.Abs(315 - degrees);
                            break;
                        case Direction.South:
                            degreesOffset = Math.Abs(270 - degrees);
                            break;
                        case Direction.Southwest:
                            degreesOffset = Math.Abs(225 - degrees);
                            break;
                        case Direction.West:
                            degreesOffset = Math.Abs(180 - degrees);
                            break;
                        case Direction.Northwest:
                            degreesOffset = Math.Abs(135 - degrees);
                            break;
                        case Direction.Down:
                            var offset_SE = Math.Abs(315 - degrees) > 180 ? Math.Abs(360 - Math.Abs(315 - degrees)) : Math.Abs(315 - degrees);
                            var offset_S = Math.Abs(270 - degrees) > 180 ? Math.Abs(360 - Math.Abs(270 - degrees)) : Math.Abs(270 - degrees);
                            var offset_SW = Math.Abs(225 - degrees) > 180 ? Math.Abs(360 - Math.Abs(225 - degrees)) : Math.Abs(225 - degrees);
                            degreesOffset = new[] {offset_SE, offset_S, offset_SW}.Min();
                            break;
                        case Direction.Up:
                            var offset_NE = Math.Abs(45 - degrees) > 180 ? Math.Abs(360 - Math.Abs(45 - degrees)) : Math.Abs(45 - degrees);
                            var offset_N = Math.Abs(90 - degrees) > 180 ? Math.Abs(360 - Math.Abs(90 - degrees)) : Math.Abs(90 - degrees);
                            var offset_NW = Math.Abs(135 - degrees) > 180 ? Math.Abs(360 - Math.Abs(135 - degrees)) : Math.Abs(135 - degrees);
                            degreesOffset = new[] { offset_NE, offset_N, offset_NW }.Min();
                            break;
                    }
                    if (degreesOffset > 180)
                        degreesOffset = Math.Abs(360 - degreesOffset);
                    maxOffset += 180;
                    totalOffset += degreesOffset;
                }
            }
            return 1 - totalOffset.Remap(0, maxOffset, 0, 1);
        }
        private double GetAngleBetweenTwoPoints(int x1, int y1, int x2, int y2)
        {
            return Math.Atan2(y2 - y1, x2 - x1) * 180.0 / Math.PI;
        }
    }
}
