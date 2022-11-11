namespace Game
{
    abstract public class ASkill : IPlayer
    {
        protected float Value { get; private set; } = 0;
        public void Do(params int[] tids)
        {
            this.PlayAnimation(tids);
        }

        virtual public void PlayAnimation(params int[] tids)
        {
            
        }

        virtual public float CalcFormula(APlayer player,float ratio)
        {
            return 0;
        }

        public APlayer SelfPlayer { get; set; }
        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
        }
    }
}
