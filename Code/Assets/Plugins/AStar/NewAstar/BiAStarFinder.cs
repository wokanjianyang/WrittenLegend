using System;
using System.Collections.Generic;

namespace Game.NewAstar
{
    public class BiAStarFinder
    {
        private readonly bool             allowDiagonal;
        private readonly DiagonalMovement diagonalMovement;
        private readonly bool             dontCrossCorners;
        private readonly int              weight = 1;

        public BiAStarFinder(bool allowDiagonal, bool dontCrossCorners, int weight = 1)
        {
            this.allowDiagonal    = allowDiagonal;
            this.dontCrossCorners = dontCrossCorners;
            this.weight           = weight;

            if (!this.allowDiagonal)
            {
                diagonalMovement = DiagonalMovement.Never;
            }
            else
            {
                if (this.dontCrossCorners)
                    diagonalMovement = DiagonalMovement.OnlyWhenNoObstacles;
                else
                    diagonalMovement = DiagonalMovement.IfAtMostOneObstacle;
            }
        }


        public List<Position> FindPath(int startX, int startY, int endX, int endY, Grid grid)
        {
            int Cmp(Node nodeA, Node nodeB)
            {
                return nodeA.f - nodeB.f;
            }

            var startOpenList = new Heap<Node>(Cmp);
            var endOpenList   = new Heap<Node>(Cmp);
            grid.ClearOpen();
            var        startNode = grid.GetNodeAt(startX, startY);
            var        endNode   = grid.GetNodeAt(endX, endY);
            var        BY_START  = 1;
            var        BY_END    = 2;
            List<Node> neighbors;
            var        i  = 0;
            var        j  = 0;
            var        l  = 0;
            var        x  = 0;
            var        y  = 0;
            var        ng = 0;
            Node       neighbor;

            startNode.g = 0;
            startNode.f = 0;
            startOpenList.Push(startNode);
            startNode.opened = BY_START;

            endNode.g = 0;
            endNode.f = 0;
            endOpenList.Push(endNode);
            endNode.opened = BY_END;
            Node node;
            
            while (!startOpenList.Empty() && !endOpenList.Empty())
            {
                node        = startOpenList.Pop();
                node.closed = true;
                neighbors   = grid.GetNeighbors(node, diagonalMovement);
                l           = neighbors.Count;
                for (i = 0; i < l; i++)
                {
                    neighbor = neighbors[i];
                    if (neighbor.closed) continue;

                    if (neighbor.opened == BY_END) return BiBacktrace(node, neighbor);

                    x  = neighbor.x;
                    y  = neighbor.y;
                    ng = node.g + (x - node.x == 0 || y - node.y == 0 ? 10 : 15);
                    if (neighbor.opened == 0 || ng < neighbor.g)
                    {
                        neighbor.g      = ng;
                        neighbor.h      = neighbor.h != 0 ? neighbor.h : weight * Manhattan(Math.Abs(x - endX), Math.Abs(y - endY));
                        neighbor.f      = neighbor.g + neighbor.h;
                        neighbor.parent = node;

                        if (neighbor.opened == 0)
                        {
                            startOpenList.Push(neighbor);
                            neighbor.opened = BY_START;
                        }
                        else
                        {
                            startOpenList.UpdateItem(neighbor);
                        }
                    }
                }
            }

            return new List<Position>();
        }

        private int Manhattan(int dx, int dy)
        {
            return dx + dy;
        }


        private List<Position> BiBacktrace(Node nodeA, Node nodeB)
        {
            var pathb = Backtrace(nodeB);
            pathb.Reverse();
            var patha = Backtrace(nodeA);
            patha.AddRange(pathb);
            return patha;
        }

        private List<Position> Backtrace(Node node)
        {
            var path = new List<Position> {new Position(node.x, node.y)};
            while (node.parent != null)
            {
                node = node.parent;
                path.Add(new Position(node.x, node.y));
            }

            path.Reverse();
            return path;
        }
    }
}