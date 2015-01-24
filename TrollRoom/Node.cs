using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TrollRoom
{
    public class Node
    {
        private Guid _id;

        public Guid Id { get { return _id; } private set { _id = value; } }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }
        
        public int Order { get; set; }

        public Node()
        { }

        public Node(string name, string description)
        {
            this._id = Guid.NewGuid();
            this.Name = name;
            this.Description = description;
            this.Rank = 0;
            this.Order = 0;
        }

        public Node Copy()
        {
            var nodeCopy = new Node();
            nodeCopy.Id = _id;
            nodeCopy.Name = Name;
            nodeCopy.Description = Description;
            nodeCopy.Rank = Rank;
            nodeCopy.Order = Order;
            return nodeCopy;
        }
    }
}
