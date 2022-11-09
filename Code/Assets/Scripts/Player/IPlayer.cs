namespace Game
{
    public interface IPlayer
    {
        public APlayer SelfPlayer { get; set; }
        public void SetParent(APlayer player);
    }
}
