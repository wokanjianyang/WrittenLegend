using System.Collections.Generic;

namespace Game.NewAstar
{
    public class Grid
    {
        private readonly int height;
        private readonly int width;
        private readonly Node[][] nodes;
        
        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
            nodes = BuildNodes();
        }

        private Node[][] BuildNodes()
        {
            var nodes = new Node[height][];
            for (var i = 0; i < height; i++)
            {
                nodes[i] = new Node[width];
                for (var j = 0; j < width; j++) nodes[i][j] = new Node(j, i, false);
            }

            return nodes;
        }

        public void SetWalkableAt(int x, int y, bool walkable)
        {
            nodes[y][x].walkable = walkable;
        }

        public bool IsInside(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }


        public bool IsWalkableAt(int x, int y)
        {
            return IsInside(x, y) && nodes[y][x].walkable;
        }

        public Node GetNodeAt(int x, int y)
        {
            return IsInside(x, y) ? nodes[y][x] : null;
        }

        public List<Node> GetNeighbors(Node node, DiagonalMovement diagonalMovement)
        {
            var x = node.x;
            var y = node.y;
            var neighbors = new List<Node>();
            var s0 = false;
            //var d0        = false;
            var s1 = false;
            //var d1        = false;
            var s2 = false;
            //var d2        = false;
            var s3 = false;
            //var d3        = false;
            
            // ←
            if (IsWalkableAt(x - 1, y))
            {
                neighbors.Add(nodes[y][x - 1]);
            }
            // ↑
            if (IsWalkableAt(x, y - 1))
            {
                neighbors.Add(nodes[y - 1][x]);
            }

            // →
            if (IsWalkableAt(x + 1, y))
            {
                neighbors.Add(nodes[y][x + 1]);
            }

            // ↓
            if (IsWalkableAt(x, y + 1))
            {
                neighbors.Add(nodes[y + 1][x]);
            }
            
            return diagonalMovement == DiagonalMovement.Never ? neighbors : neighbors;

            // if (diagonalMovement == DiagonalMovement.OnlyWhenNoObstacles)
            // {
            //     d0 = s3 && s0;
            //     d1 = s0 && s1;
            //     d2 = s1 && s2;
            //     d3 = s2 && s3;
            // }
            // else if (diagonalMovement == DiagonalMovement.IfAtMostOneObstacle)
            // {
            //     d0 = s3 || s0;
            //     d1 = s0 || s1;
            //     d2 = s1 || s2;
            //     d3 = s2 || s3;
            // }
            // else if (diagonalMovement == DiagonalMovement.Always)
            // {
            //     d0 = true;
            //     d1 = true;
            //     d2 = true;
            //     d3 = true;
            // }

            // // ↖
            // if (d0 && IsWalkableAt(x - 1, y - 1)) neighbors.Add(nodes[y - 1][x - 1]);
            //
            // // ↗
            // if (d1 && IsWalkableAt(x + 1, y - 1)) neighbors.Add(nodes[y - 1][x + 1]);
            //
            // // ↘
            // if (d2 && IsWalkableAt(x + 1, y + 1)) neighbors.Add(nodes[y + 1][x + 1]);
            //
            // // ↙
            // if (d3 && IsWalkableAt(x - 1, y + 1)) neighbors.Add(nodes[y + 1][x - 1]);
        }


        public void ClearOpen()
        {
            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
            {
                nodes[i][j].opened = 0;
                nodes[i][j].closed = false;
                nodes[i][j].parent = null;
            }
        }
    }
}