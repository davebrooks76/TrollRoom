using System.Collections.Generic;

namespace TrollRoom
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Exit> Exits { get; set; }        
    }

    public class Exit
    {
        public Direction Direction { get; set; }
        public Room Destination { get; set; }
    }

    public enum Direction
    {
        North,
        Northeast,
        East,
        Southeast,
        South,
        Southwest,
        West,
        Northwest,
        Up,
        Down
    }
}