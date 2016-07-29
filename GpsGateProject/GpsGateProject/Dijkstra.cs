using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GpsGateProject
{
    public static class Dijkstra
    {
        private static bool firstRun = true;
        private static List<Node> nodes = new List<Node>();
        private static Node[,] nodeArray;

        public static List<Node> Run(Point p1, Point p2, int width, int height)
        {
            lock (nodes)
            {

                // based on https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
                // set up map if first run
                #region map init
                if (firstRun)
                {
                    nodeArray = new Node[width, height];
                    // set up map
                    // width * height nodes each with 4 neighbors
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            nodeArray[i, j] = new Node(i, j);
                            if (j > 0)
                            {
                                nodeArray[i, j].Neighbors[(int)Direction.UP] = nodeArray[i, j - 1];
                                nodeArray[i, j - 1].Neighbors[(int)Direction.DOWN] = nodeArray[i, j];
                            }
                            if (i > 0)
                            {
                                nodeArray[i, j].Neighbors[(int)Direction.LEFT] = nodeArray[i - 1, j];
                                nodeArray[i - 1, j].Neighbors[(int)Direction.RIGHT] = nodeArray[i, j];
                            }
                            nodes.Add(nodeArray[i, j]);
                        }
                    }
                    firstRun = false;
                }
                else
                {
                    // Set distances to "infinite" and reset bestFrom and visited
                    foreach (Node item in nodes)
                    {
                        item.distance = Int32.MaxValue;
                        item.bestFrom = null;
                        item.visited = false;
                    }
                }
                #endregion

                Node startNode = nodeArray[Convert.ToInt32(p1.X), Convert.ToInt32(p1.Y)];
                Node endNode = nodeArray[Convert.ToInt32(p2.X), Convert.ToInt32(p2.Y)];

                if (!nodes.Contains(startNode))
                {
                    throw new StartAlreadyPartOfPathException();
                }
                if (!nodes.Contains(endNode))
                {
                    throw new EndAlreadyPartOfPathException();
                }
                // Find the shortest path
                bool done = false;
                List<Node> notInfinites = new List<Node>();
                startNode.distance = 0;
                notInfinites.Add(startNode);
                // Traverse the graph
                while (!done)
                {
                    //Node nextNode = notInfinites.Find((n) => n.distance == notInfinites.Min((n2) => n2.distance));
                    Node nextNode = notInfinites.First();
                    foreach (Node n in nextNode.Neighbors)
                    {
                        // set distance to neighbor
                        if (n != null && n.distance > nextNode.distance + 1)
                        {
                            //if (n.bestFrom == null)
                            //{   
                            //    notInfinites.Add(n);
                            //}
                            n.distance = nextNode.distance + 1;
                            n.bestFrom = nextNode;
                            notInfinites.Remove(n);
                            bool inserted = false;
                            for (int i = 0; i < notInfinites.Count; i++)
                            {
                                if (notInfinites[i].distance > n.distance)
                                {
                                    notInfinites.Insert(i, n);
                                    inserted = true;
                                    break;
                                }
                            }
                            if (!inserted)
                            {
                                notInfinites.Add(n);
                            }
                        }
                    }
                    nextNode.visited = true;
                    notInfinites.Remove(nextNode);
                    if (endNode.visited)
                    {
                        // done, found a path
                        done = true;
                    }
                    if (notInfinites.Count == 0)
                    {
                        // done, no path
                        done = true;
                        throw new NoSuchPathException();
                    }
                }
                // Remove path nodes from map
                List<Node> path = new List<Node>();
                Node pathNode = endNode;
                while (pathNode.bestFrom != null)
                {
                    path.Add(pathNode);
                    nodes.Remove(pathNode);
                    foreach (Node n in nodeArray[pathNode.X, pathNode.Y].Neighbors)
                    {
                        if (n != null)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (n.Neighbors[i] == pathNode)
                                {
                                    n.Neighbors[i] = null;
                                }
                            }
                        }
                    }
                    pathNode = pathNode.bestFrom;
                }

                return path;
            }
        }
    }
}
