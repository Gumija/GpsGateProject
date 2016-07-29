using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GpsGateProject
{
    public class Node
    {
        public Node(int x, int y)
        {
            X = x;
            Y = y;
            visited = false;
            distance = Int32.MaxValue;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int distance { get; set; }
        public bool visited { get; set; }
        public Node bestFrom { get; set; }
        public Node[] Neighbors { get; set; } = new Node[4]; // Up, Right, Down, Left
    }

    enum Direction { UP = 0, RIGHT, DOWN, LEFT }
}
