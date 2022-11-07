using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Monster : APlayer
    {
        public override void Load()
        {
            base.Load();
            
            this.Camp = PlayerType.Enemy;
        }
    }
}
