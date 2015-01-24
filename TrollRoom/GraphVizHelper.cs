using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TrollRoom
{
    public class GraphVizHelper
    {
        public static void WriteGraphVizDocument(List<Node> nodes, List<Edge> edges)
        {
            var fileName = "test.gv";
            var lines = new List<string>();
            var stream = System.IO.File.Create(fileName);
            

        }
    }
}
