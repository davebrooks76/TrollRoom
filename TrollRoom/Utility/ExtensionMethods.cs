using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrollRoom
{
    public static class ExtensionMethods
    {
        public static double Remap(this double value, double from1, double to1, double from2, double to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static BitArray ToBitArray(this byte[] byteArray)
        {
            return new BitArray((byte[])(Array)byteArray);
        }

        public static byte[] ToByteArray(this BitArray bitArray)
        {
            var byteArray = new byte[bitArray.Count / 8];
            bitArray.CopyTo(byteArray, 0);
            return byteArray;
        }

        public static string ToTestString(this Layout layout)
        {
            var coordinates = layout.Coordinates;
            var stringBuilder = new StringBuilder();
            for (var y = 64; y >= 0; y--)
            {
                if (y >= coordinates.Min() && y <= coordinates.Max())
                {
                    for (var x = 0; x <= 64; x++)
                    {
                        if (x >= coordinates.Min() && x <= coordinates.Max())
                        {
                            var symbol = ".";
                            for (var i = 0; i < layout.Map.Rooms.Count; i++)
                            {
                                var currentRoomX = coordinates[i * 2];
                                var currentRoomY = coordinates[i * 2 + 1];
                                if (currentRoomX == x && currentRoomY == y)
                                {
                                    symbol = layout.Map.Rooms[i].Name;
                                }
                            }
                            stringBuilder.Append(symbol);
                        }
                    }
                    stringBuilder.Append(Environment.NewLine);
                }
             }
            return stringBuilder.ToString();
        }
    }
}
