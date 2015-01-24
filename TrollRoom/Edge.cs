using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrollRoom
{
    public class Edge
    {
        public Node Origin { get; set; }
        public Node Destination { get; set; }
        public Cardinal Direction { get; set; }

        public Edge(Node origin, Cardinal direction, Node destination)
        {
            this.Origin = origin;
            this.Destination = destination;
            this.Direction = direction;
        }
    }

    public enum Cardinal
    { 
        N, S, E, W, NE, NW, SE, SW, U, D
    }
}
