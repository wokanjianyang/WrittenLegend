using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.PathFinc
{
    public class MapData
    {
        public TileBase deadTerrain;
        public TileBase grassTerrain;

        [SerializeField] private int m_mapHeight;

        [SerializeField] private int m_mapWidth;

        public TileBase sandTerrain;


        //public TileBase blockedTerrain;
        //public TileBase easyTerrain;
        //public TileBase mediumTerrain;
        //public TileBase hardTerrain;

        private readonly Dictionary<TileBase, Node.NodeType> terrainDifficultyDict = new Dictionary<TileBase, Node.NodeType>();
        public           Tilemap                             tilemapTerrain;
        public           TileBase                            waterTerrain;

        public int MapWidth => m_mapWidth;

        public int MapHeight => m_mapHeight;

        private void DetermineMapBoundaries()
        {
            if (tilemapTerrain != null)
            {
                Vector3 tilemapSize = tilemapTerrain.size;
                m_mapWidth  = (int) tilemapSize.x;
                m_mapHeight = (int) tilemapSize.y;
                Debug.Log("The Size of the tilemap is " + m_mapWidth + " by " + m_mapHeight);
            }
            else
            {
                Debug.LogWarning("Tilemap is not set! Could not determine map height or width!");
            }
        }

        private void Awake()
        {
            DetermineMapBoundaries();
            SetupTerrainDifficulty();
            //GetNodeTypeAtPosition(15, 15);
        }

        private void SetupTerrainDifficulty()
        {
            if (deadTerrain == null || grassTerrain == null || sandTerrain == null || waterTerrain == null)
            {
                Debug.LogWarning("Could not setup terrain difficulty!");
                return;
            }

            terrainDifficultyDict.Add(deadTerrain, Node.NodeType.blockedTerrain);
            terrainDifficultyDict.Add(grassTerrain, Node.NodeType.easyTerrain);
            terrainDifficultyDict.Add(sandTerrain, Node.NodeType.mediumTerrain);
            terrainDifficultyDict.Add(waterTerrain, Node.NodeType.hardTerrain);
        }

        public Node.NodeType GetNodeTypeAtPosition(int xPosition, int yPosition)
        {
            var tile = tilemapTerrain.GetTile(new Vector3Int(xPosition, yPosition, 0));

            if (terrainDifficultyDict.ContainsKey(tile))
            {
                var nodeTypeTile = terrainDifficultyDict[tile];
                //Debug.Log("The tile "+ tile.ToString() + " at position " + xPosition + "," + yPosition + " is classified as type " + nodeTypeTile.ToString());

                return nodeTypeTile;
            }

            return Node.NodeType.blockedTerrain;
        }

        public int[,] CreateMap()
        {
            var map = new int[m_mapWidth, m_mapHeight];

            for (var x = 0; x < m_mapWidth; x++)
            {
                for (var y = 0; y < m_mapHeight; y++)
                {
                    map[x, y] = (int) GetNodeTypeAtPosition(x, y);
                }
            }

            return map;
        }
    }
}