namespace Game
{
    public interface IBattleLife
    {
        public void OnBattleStart();

        public int Order { get; }
    }
}
