using System.Collections.Generic;
using UnityEngine;

namespace Game.PathFinc
{
    public class Pathfinder
    {
        public enum SearchAlgorithm
        {
            BreadthFirst,
            Dijkstra,
            GreedyBestFirst,
            AStar
        }

        private Vector2[] allDirections = {new Vector2(0f, 1f), new Vector2(1f, 0f), new Vector2(0f, -1f), new Vector2(-1f, 0f)};

        private bool       isComplete;
        private List<Node> m_exploredNodes;

        private PriorityQueue<Node> m_frontierNodes;
        private Node                m_goalNode;
        private Graph               m_graph;

        private int        m_iterations;
        private List<Node> m_pathNodes;
        private Node       m_startNode;

        public void Init(int[,] map, Vector2[] allDirections)
        {
            m_graph = new Graph(map, allDirections);
        }

        public List<Node> GetPath(Vector3Int startpos, Vector3Int goalpos, SearchAlgorithm algorithm = SearchAlgorithm.AStar)
        {
            //重置
            m_startNode                   = m_graph.GetNodeAtPosition(startpos.x, startpos.y);
            m_startNode.distanceFromStart = 0;
            m_startNode.costsFromStart    = 0;
            m_goalNode                    = m_graph.GetNodeAtPosition(goalpos.x, goalpos.y);
            //
            m_frontierNodes = new PriorityQueue<Node>();
            m_frontierNodes.Enqueue(m_startNode);
            m_exploredNodes = new List<Node>();
            m_pathNodes     = new List<Node>();
            isComplete      = false;
            m_iterations    = 0;

            //搜索
            while (!isComplete)
                if (m_frontierNodes.Count > 0)
                {
                    var currentNode = m_frontierNodes.Dequeue();
                    m_iterations++;

                    if (!m_exploredNodes.Contains(currentNode)) m_exploredNodes.Add(currentNode);

                    if (SearchAlgorithm.BreadthFirst.Equals(algorithm))
                        ExpandFrontierBreadthFirst(currentNode);
                    else if (SearchAlgorithm.Dijkstra.Equals(algorithm))
                        ExpandFrontierDijkstra(currentNode);
                    else if (SearchAlgorithm.GreedyBestFirst.Equals(algorithm))
                        ExpandFrontierGreedyBestFirst(currentNode);
                    else if (SearchAlgorithm.AStar.Equals(algorithm)) ExpandFrontierAStar(currentNode);

                    if (m_frontierNodes.Contains(m_goalNode))
                    {
                        m_pathNodes = GetPathNodes(m_goalNode);
                        isComplete  = true;
                        Debug.Log("Path has been found in " + m_iterations                 + " iterations!");
                        Debug.Log("Path distance "          + m_goalNode.distanceFromStart + "!");
                        Debug.Log("Path costs "             + m_goalNode.costsFromStart    + "!");
                    }
                }
                else
                {
                    isComplete = true;
                    Debug.Log("No Path has been found. " + m_iterations + " iterations.");
                }

            return m_pathNodes;
        }

        private void ExpandFrontierBreadthFirst(Node node)
        {
            if (node == null) return;

            for (var i = 0; i < node.adjacentNodes.Count; i++)
                if (!m_exploredNodes.Contains(node.adjacentNodes[i]) && !m_frontierNodes.Contains(node.adjacentNodes[i]))
                {
                    //START BLOCK the values are only included for debugging purposes, they are not used for priority - comment out for more performance
                    var distanceToAdjacent   = m_graph.DetermineDistanceBetweenNodes(node, node.adjacentNodes[i]);
                    var newDistanceFromStart = node.distanceFromStart + distanceToAdjacent;
                    var newCostsFromStart    = node.costsFromStart    + distanceToAdjacent + node.hazardValue;
                    node.adjacentNodes[i].distanceFromStart = newDistanceFromStart;
                    node.adjacentNodes[i].costsFromStart    = newCostsFromStart;
                    //END BLOCK

                    node.adjacentNodes[i].previousNode = node;

                    //this way it still works with priorityqueue - because it just counts up
                    node.adjacentNodes[i].priority = m_exploredNodes.Count;
                    //Debug.Log("Explored nodes count: "+ m_exploredNodes.Count);

                    m_frontierNodes.Enqueue(node.adjacentNodes[i]);
                }
        }

        private void ExpandFrontierDijkstra(Node node)
        {
            if (node == null) return;

            for (var i = 0; i < node.adjacentNodes.Count; i++)
                if (!m_exploredNodes.Contains(node.adjacentNodes[i]))
                {
                    var distanceToAdjacent   = m_graph.DetermineDistanceBetweenNodes(node, node.adjacentNodes[i]);
                    var newDistanceFromStart = node.distanceFromStart + distanceToAdjacent;
                    var newCostsFromStart    = node.costsFromStart    + distanceToAdjacent + node.hazardValue; //this way the terraincosts get included

                    //if (float.IsPositiveInfinity(node.adjacentNodes[i].distanceFromStart)
                    //        || distanceFromStart < node.adjacentNodes[i].distanceFromStart)
                    if (float.IsPositiveInfinity(node.adjacentNodes[i].costsFromStart) || newCostsFromStart < node.adjacentNodes[i].costsFromStart)

                    {
                        node.adjacentNodes[i].previousNode      = node;
                        node.adjacentNodes[i].distanceFromStart = newDistanceFromStart;
                        node.adjacentNodes[i].costsFromStart    = newCostsFromStart;
                    }

                    if (!m_frontierNodes.Contains(node.adjacentNodes[i]))
                    {
                        //node.adjacentNodes[i].priority = node.adjacentNodes[i].distanceFromStart;
                        node.adjacentNodes[i].priority = node.adjacentNodes[i].costsFromStart;
                        m_frontierNodes.Enqueue(node.adjacentNodes[i]);
                    }
                }
        }

        private void ExpandFrontierGreedyBestFirst(Node node)
        {
            if (node == null) return;

            for (var i = 0; i < node.adjacentNodes.Count; i++)
                if (!m_exploredNodes.Contains(node.adjacentNodes[i]) && !m_frontierNodes.Contains(node.adjacentNodes[i]))
                {
                    //START BLOCK the values are only included for debugging purposes, they are not used for priority - comment out for more performance
                    var distanceToAdjacent   = m_graph.DetermineDistanceBetweenNodes(node, node.adjacentNodes[i]);
                    var newDistanceFromStart = node.distanceFromStart + distanceToAdjacent;
                    var newCostsFromStart    = node.costsFromStart    + distanceToAdjacent + node.hazardValue;
                    node.adjacentNodes[i].distanceFromStart = newDistanceFromStart;
                    node.adjacentNodes[i].costsFromStart    = newCostsFromStart;
                    //END BLOCK
                    node.adjacentNodes[i].previousNode = node;
                    if (m_graph != null)
                        //node.adjacentNodes[i].priority = m_graph.DetermineDistanceBetweenNodes(node.adjacentNodes[i], m_goalNode);
                        node.adjacentNodes[i].priority = m_graph.DetermineManhattanDistanceBetweenNodes(node.adjacentNodes[i], m_goalNode);

                    m_frontierNodes.Enqueue(node.adjacentNodes[i]);
                }
        }


        //see https://en.wikipedia.org/wiki/A*_search_algorithm
        private void ExpandFrontierAStar(Node node)
        {
            if (node == null) return;

            for (var i = 0; i < node.adjacentNodes.Count; i++)
                if (!m_exploredNodes.Contains(node.adjacentNodes[i]))
                {
                    var distanceToAdjacent   = m_graph.DetermineDistanceBetweenNodes(node, node.adjacentNodes[i]);
                    var newDistanceFromStart = node.distanceFromStart + distanceToAdjacent;
                    var newCostsFromStart    = node.costsFromStart    + distanceToAdjacent + node.hazardValue; //this way the terraincosts get included

                    if (float.IsPositiveInfinity(node.adjacentNodes[i].costsFromStart) || newCostsFromStart < node.adjacentNodes[i].costsFromStart)

                    {
                        node.adjacentNodes[i].previousNode      = node;
                        node.adjacentNodes[i].distanceFromStart = newDistanceFromStart;
                        node.adjacentNodes[i].costsFromStart    = newCostsFromStart;
                    }

                    if (!m_frontierNodes.Contains(node.adjacentNodes[i]) && m_graph != null)
                    {
                        var distanceToGoal = m_graph.DetermineDistanceBetweenNodes(node.adjacentNodes[i], m_goalNode);

                        //AStar: f = g + h
                        node.adjacentNodes[i].priority = node.adjacentNodes[i].costsFromStart + distanceToGoal;
                        m_frontierNodes.Enqueue(node.adjacentNodes[i]);
                    }
                }
        }

        private List<Node> GetPathNodes(Node endNode)
        {
            var path = new List<Node>();

            if (endNode == null) return path;

            var currentNode = endNode;

            while (currentNode.previousNode != null)
            {
                path.Insert(0, currentNode);
                currentNode = currentNode.previousNode;
            }

            return path;
        }
    }
}