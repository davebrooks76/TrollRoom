using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrollRoom
{
    public partial class Form1 : Form
    {
        private Graph graph = new Graph();

        public Form1()
        {
            InitializeComponent();
            InitializeNodes();
            InitializeEdges();

            var parser = new LogFileParser();
            //graph = parser.ParseFile(@"C:\zork1\DATA\ZORK1.log");
            var geneticLayout = new GeneticLayout();
            graph = geneticLayout.FindBestGraphLayout(graph);
            foreach (var node in graph.Nodes.Values)
            { 
                textBox1.Text += (node.Name + " " + node.Rank + " " + node.Order + Environment.NewLine);
            }
            textBox1.Text += Environment.NewLine;
            textBox1.Text += Environment.NewLine;
            textBox1.Text += graph.ToString();
            textBox1.Text += Environment.NewLine;
            textBox1.Text += graph.Fitness;
        }

        private void InitializeNodes()
        {
            var nodeNames = new string[] { "A", "B", "C", "D", "E", "F", "G" };
            foreach (var name in nodeNames)
            {
                var node = new Node(name, "test");
                graph.Nodes.Add(node.Id, node);
            }
        }

        private void InitializeEdges()
        {
            graph.Edges.Add(new Edge(graph.GetNode("A", "test"), Cardinal.N, graph.GetNode("B", "test")));
            graph.Edges.Add(new Edge(graph.GetNode("B", "test"), Cardinal.E, graph.GetNode("C", "test")));
            graph.Edges.Add(new Edge(graph.GetNode("C", "test"), Cardinal.N, graph.GetNode("D", "test")));
            graph.Edges.Add(new Edge(graph.GetNode("D", "test"), Cardinal.W, graph.GetNode("E", "test")));
            graph.Edges.Add(new Edge(graph.GetNode("E", "test"), Cardinal.E, graph.GetNode("A", "test")));
            graph.Edges.Add(new Edge(graph.GetNode("E", "test"), Cardinal.N, graph.GetNode("F", "test")));
            graph.Edges.Add(new Edge(graph.GetNode("F", "test"), Cardinal.W, graph.GetNode("G", "test")));
        }  
    }
}
