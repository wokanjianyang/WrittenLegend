using UnityEngine;

namespace Game
{
    public class Hero : APlayer
    {
        public override void Load()
        {
            base.Load();
            this.Camp = PlayerType.Hero;
        }
    }
    
}
