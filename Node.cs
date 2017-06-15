using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace steep_hill_climbing
{
    public class Node
    {
        public Node(int[,] move)
        {
            _move = move;
        }

        List<Node> children = new List<Node>();
        private Node _parent;
        private int[,] _move;
        public void AddChild(Node child)
        {
            child.Parent = this;
            children.Add(child);
        }

        public Node Parent
        {
            set { _parent = value; }
            get { return _parent; }
        }

        public int[,] Move
        {
            set { _move = value; }
            get { return _move; }
        }
    }
}
