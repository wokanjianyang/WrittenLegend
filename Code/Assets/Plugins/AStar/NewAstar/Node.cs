using UnityEngine;

namespace Game.NewAstar
{
    public class Node
    {
        public bool closed;


        public int f;
        public int g;
        public int h;

        public int opened = 0;

        public Node parent;
        public bool walkable;

        public int x;
        public int y;

        public Node(int x, int y, bool walkable)
        {
            this.x        = x;
            this.y        = y;
            this.walkable = walkable;
        }
        
        public Vector3Int ToVector3Int()
        {
            return new Vector3Int((int) x, (int) y, 0);
        }
    }
}