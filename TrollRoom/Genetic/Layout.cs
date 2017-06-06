using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrollRoom
{
    public class Layout
    {
        public Map Map;

        public Layout(Map map)
        {
            Map = map;
            Coordinates = new byte[map.Rooms.Count*2];
        }

        public byte[] Coordinates { get; set; }

        public double FitnessScore { get; set; }
    }
}
