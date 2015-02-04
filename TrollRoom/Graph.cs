using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TrollRoom
{
    public class Graph
    {
        private Dictionary<Guid, Node> nodes = new Dictionary<Guid, Node>();
        private List<Edge> edges = new List<Edge>();

        public Node PlayerCurrentNode { get; set; }
        public Node PlayerPreviousNode { get; set; }

        public Dictionary<Guid, Node> Nodes { get { return nodes; } set { nodes = value; } }
        public List<Edge> Edges { get { return edges; } set { edges = value; } }

        private List<BitArray> bits = new List<BitArray>();
        public List<BitArray> Bits 
        { get { return bits; } 
            set 
            {
                bits = value;
                Decode();
            }
        }

        public Node GetNode(string name, string description)
        {
            return Nodes.Values.First(n => n.Name == name && n.Description == description);
        }

        public void SetPlayerCurrentNode(Node node)
        { 
            if (this.PlayerCurrentNode != null)
                this.PlayerPreviousNode = this.PlayerCurrentNode;
            this.PlayerCurrentNode = node;
        }

        private void Decode()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                Nodes.Values.ElementAt(i).Rank = getIntFromBitArray(bits[i * 2]);
                Nodes.Values.ElementAt(i).Order = getIntFromBitArray(bits[i * 2 + 1]);
            }
        }

        private int getIntFromBitArray(BitArray bitArray)
        {
            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];

        }

        public double Fitness { get; set; }

        public void CalculateFitness(int generation)
        {
            double orientationScore = 0;
            double alignmentScore = 0;
            double proximityScore = 0;
            foreach (var edge in Edges)
            {
                switch (edge.Direction)
                {
                    case Cardinal.N:
                            alignmentScore += (1.0 / (Math.Abs(edge.Destination.Order - edge.Origin.Order) + 1));
                            proximityScore += (1.0 / (edge.Destination.Rank - edge.Origin.Rank));
                        break;
                    case Cardinal.S:
                            alignmentScore += (1.0 / (Math.Abs(edge.Destination.Order - edge.Origin.Order) + 1));
                            proximityScore += (1.0 / (edge.Origin.Rank - edge.Destination.Rank));
                        break;
                    case Cardinal.E:
                            alignmentScore += (1.0 / (Math.Abs(edge.Destination.Rank - edge.Origin.Rank) + 1));
                            proximityScore += (1.0 / (edge.Destination.Order - edge.Origin.Order));
                        break;
                    case Cardinal.W:
                            alignmentScore += (1.0 / (Math.Abs(edge.Destination.Rank - edge.Origin.Rank) + 1));
                            proximityScore += (1.0 / (edge.Origin.Order - edge.Destination.Order));
                        break;
                }
            }
            Fitness = alignmentScore;
            //Fitness = alignmentScore + (proximityScore * (generation/50));
        }

        public bool IsValid
        {
            get
            {
                foreach (var edge in Edges)
                {
                    switch (edge.Direction)
                    {
                        case Cardinal.N:
                            if (edge.Destination.Rank <= edge.Origin.Rank)
                            {
                                return false;
                            }
                            break;
                        case Cardinal.S:
                            if (edge.Destination.Rank >= edge.Origin.Rank)
                            {
                                return false;
                            }
                            break;
                        case Cardinal.E:
                            if (edge.Destination.Order <= edge.Origin.Order)
                            {
                                return false;
                            }
                            break;
                        case Cardinal.W:
                            if (edge.Destination.Order >= edge.Origin.Order)
                            {
                                return false;
                            }
                            break;
                    }
                }
                return true;
            }
        }

        public void Collapse()
        { 
            var orders = new List<int>();
            var ranks = new List<int>();
            foreach (var node in Nodes)
            { 
             
            }
        }

        public Graph Copy()
        {
            var graphCopy = new Graph();
            foreach (var node in Nodes.Values)
            {
                var newNode = node.Copy();
                graphCopy.Nodes.Add(newNode.Id, newNode);
            }
            foreach (var edge in Edges)
            {
                Node origin;
                Node destination;
                graphCopy.Nodes.TryGetValue(edge.Origin.Id, out origin);
                graphCopy.Nodes.TryGetValue(edge.Destination.Id, out destination);

                var edgeCopy = new Edge(origin, edge.Direction, destination);
                graphCopy.Edges.Add(edgeCopy);
            }
            if (PlayerCurrentNode != null)
                graphCopy.PlayerCurrentNode = graphCopy.Nodes.Values.First(x => x.Id == PlayerCurrentNode.Id);
            if (PlayerPreviousNode != null)
                graphCopy.PlayerPreviousNode = graphCopy.Nodes.Values.First(x => x.Id == PlayerPreviousNode.Id);

            return graphCopy;
        }

        public override string ToString()
        {
            var maxRank = nodes.Values.Max(x => x.Rank);
            var maxOrder = nodes.Values.Max(x => x.Order);
            var stringBuilder = new StringBuilder();

            for (var i = maxRank; i >= 0; i--)
            {
                for (var j = 0; j <= maxOrder; j++)
                { 
                    var node = nodes.Values.FirstOrDefault(x=>x.Rank == i && x.Order == j);
                    stringBuilder.Append(node == null? " " : node.Name);  
                }
                stringBuilder.Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }
    }
}
