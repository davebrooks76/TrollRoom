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
        private byte[] bytes;

        public Layout(Map map)
        {
            Map = map;
        }

        public BitArray Bits { get; set; }

        public double FitnessScore { get; set; }

        public byte[] ToByteArray()
        {
            if (this.bytes == null)
            {
                bytes = new byte[Map.Rooms.Count * 2];
                 
            }
            return bytes;
        }
    }
}
