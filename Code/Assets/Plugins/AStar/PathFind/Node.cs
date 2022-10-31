using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PathFinc
{
    public class Node : IComparable<Node>
    {
        public enum NodeType
        {
            blockedTerrain = 0,
            easyTerrain    = 1,
            mediumTerrain  = 2,
            hardTerrain    = 3
        }

        public List<Node> adjacentNodes;
        public float      costsFromStart = Mathf.Infinity; //this way you can track hazardous terrain

        public float distanceFromStart = Mathf.Infinity;

        public Node previousNode;

        public float priority;

        public Node(int xPosition, int yPosition, NodeType nodeType)
        {
            PositionX = xPosition;
            PositionY = yPosition;
            Type      = nodeType;
        }

        public int PositionX { get; }

        public int PositionY { get; }

        public NodeType Type { get; }

        //TODO right now this represents only the obstruction by terrain - it could additionally represent the distance to enemies
        public float hazardValue => (float) Type;

        // used for sorting order priorityqueue
        public int CompareTo(Node other)
        {
            if (priority < other.priority) return -1;
            if (priority > other.priority) return 1;
            return 0;
        }
    }
}