using UnityEngine;

namespace Game
{
    public class HalfLife:MonoBehaviour,IPlayer
    {
        public int DeadCount = 0;

        public int DeadRoundNum = 0;
        public APlayer SelfPlayer { get; set; }
        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
            
            this.SelfPlayer.EventCenter.AddListener<PlayerDeadEvent>(OnPlayerDeadEvent);
        }

        private void OnPlayerDeadEvent(PlayerDeadEvent e)
        {
            this.DeadCount++;
            this.DeadRoundNum = e.RoundNum;
        }

        public bool IsCanRevive()
        {
            return this.SelfPlayer.RoundCounter - this.DeadRoundNum > this.DeadCount;
        }
    }
}
