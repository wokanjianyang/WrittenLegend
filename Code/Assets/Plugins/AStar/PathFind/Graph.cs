using System.Collections.Generic;
using UnityEngine;

namespace Game.PathFinc
{
    public class Graph
    {
        //int[,] mapData;

        private readonly Vector2[] allDirections;
        private readonly float     m_distanceUnitsDiagonal = 1.4f; //~pythagoras of root(1^2+1^2)

        private readonly float m_distanceUnitsStraight = 1f;
        private          int   m_height;

        private int     m_width;
        private Node[,] nodes;

        private Graph()
        {
        }

        public Graph(int[,] mapData, Vector2[] allDirections)
        {
            this.allDirections = allDirections;
            Init(mapData);
        }

        private void Init(int[,] mapData)
        {
            m_width  = mapData.GetLength(0);
            m_height = mapData.GetLength(1);

            nodes = new Node[m_width, m_height];

            for (var x = 0; x < m_width; x++)
            for (var y = 0; y < m_height; y++)
            {
                var newNodeType = (Node.NodeType) mapData[x, y];
                nodes[x, y] = new Node(x, y, newNodeType);
            }

            for (var x = 0; x < m_width; x++)
            for (var y = 0; y < m_height; y++)
                nodes[x, y].adjacentNodes = GetAdjacentNodesAtPosition(x, y);
        }

        public Node GetNodeAtPosition(int xPosition, int yPosition)
        {
            return nodes[xPosition, yPosition];
        }

        public bool IsWithinBounds(int xPosition, int yPosition)
        {
            return xPosition >= 0 && xPosition < m_width && yPosition >= 0 && yPosition < m_height;
        }

        private List<Node> GetAdjacentNodesAtPosition(int xPosition, int yPosition)
        {
            var adjacentNodes = new List<Node>();

            foreach (var dir in allDirections)
            {
                var newX = xPosition + (int) dir.x;
                var newY = yPosition + (int) dir.y;

                if (IsWithinBounds(newX, newY) && nodes[newX, newY] != null && nodes[newX, newY].Type != Node.NodeType.blockedTerrain) adjacentNodes.Add(nodes[newX, newY]);
            }

            return adjacentNodes;
        }

        /*
        * to find the shortest distance, you first go diagonal steps which are equal to the smaller absolute delta (x xor y),
        * then you go the remaining steps in a straight line.
        */
        public float DetermineDistanceBetweenNodes(Node nodeFrom, Node nodeTo)
        {
            var absDeltaX = Mathf.Abs(nodeFrom.PositionX - nodeTo.PositionX);
            var absDeltaY = Mathf.Abs(nodeFrom.PositionY - nodeTo.PositionY);

            int stepsDiagonal;
            int stepsStraight;

            if (absDeltaX >= absDeltaY)
            {
                stepsDiagonal = absDeltaY;
                stepsStraight = absDeltaX - absDeltaY;
            }
            else
            {
                stepsDiagonal = absDeltaX;
                stepsStraight = absDeltaY - absDeltaX;
            }

            return m_distanceUnitsDiagonal * stepsDiagonal + m_distanceUnitsStraight * stepsStraight;
        }

        /*
         * Rougher, slightly more performant estimation of distance
         * see https://en.wikipedia.org/wiki/Taxicab_geometry
         */
        public float DetermineManhattanDistanceBetweenNodes(Node nodeFrom, Node nodeTo)
        {
            var absDeltaX = Mathf.Abs(nodeFrom.PositionX - nodeTo.PositionX);
            var absDeltaY = Mathf.Abs(nodeFrom.PositionY - nodeTo.PositionY);

            return absDeltaX + absDeltaY;
        }
    }
}