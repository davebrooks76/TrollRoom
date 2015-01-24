using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TrollRoom
{
    public class LogFileParser
    {
        private Graph graph = new Graph();

        public Graph ParseFile(string logFilePath)
        {
            var reader = new System.IO.StreamReader(logFilePath);
            string line;
            List<string> commandSection = new List<string>();
            //start parsing file into command sections (i.e. a block that starts with > and ends before the next one)
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(">"))
                {
                    //we've encountered a new command, process previous section if any
                    if (commandSection.Any())
                    {
                        ProcessCommandSection(commandSection);
                        commandSection.Clear();
                    }
                }
                commandSection.Add(line);
            }
            return graph;
        }

        private void ProcessCommandSection(List<string> commandSection)
        {
            if (commandSection[0].StartsWith(">"))
            {
                string[] cardinalCommands = { "N", "S", "E", "W", "NE", "NW", "SE", "SW", "U", "D" };
                Cardinal cardinal;
                if (cardinalCommands.Contains(commandSection[0].Substring(1).ToUpperInvariant()) &&
                    !commandSection[1].Contains("."))
                { 
                    TryAddNode(commandSection[1], String.Concat(commandSection.Skip(2)));
                    Enum.TryParse(commandSection[0].Substring(1).ToUpperInvariant(), out cardinal);
                    TryAddEdge(cardinal);
                }
            }

            switch (commandSection[0])
            { 
                case ">look":
                    TryAddNode(commandSection[1], String.Concat(commandSection.Skip(2)));
                    break;
            }
        }

        private bool TryAddNode(string name, string description)
        {
            if (!graph.Nodes.Values.Any(n => n.Name == name))
            {
                var newNode = new Node(name, description);
                graph.SetPlayerCurrentNode(newNode);
                graph.Nodes.Add(newNode.Id, newNode);
                return true;
            }
            else if (graph.Nodes.Values.First(n => n.Name == name).Description != description)
            {
                if (ConfirmNewNode())
                {
                    var newNode = new Node(name, description);
                    graph.SetPlayerCurrentNode(newNode);
                    graph.Nodes.Add(newNode.Id, newNode);
                    return true;
                }
                else
                     graph.SetPlayerCurrentNode(graph.Nodes.Values.First(n => n.Name == name));
                
            }
            return false;
        }

        private void TryAddEdge(Cardinal direction)
        {
           if (graph.PlayerPreviousNode != null &&
                !graph.Edges.Any(e => e.Destination == graph.PlayerCurrentNode
                    && e.Origin == graph.PlayerPreviousNode))
            {
                graph.Edges.Add(new Edge(graph.PlayerPreviousNode, direction, graph.PlayerCurrentNode));
            }
        }

        private bool ConfirmNewNode()
        {
            //eventually this will allow the player to confirm whether the node should really be added or not.
            return true;
        }
        
    }
}
