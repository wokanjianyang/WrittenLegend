namespace Game.NewAstar
{
    public class HeapNode
    {
        public HeapNode(Node node)
        {
            this.node = node;
//            this.ExpectedCost = expectedCost;            
        }

        public Node node { get; }

//        public float ExpectedCost { get; set; }                
        public HeapNode Next { get; set; }
    }
}