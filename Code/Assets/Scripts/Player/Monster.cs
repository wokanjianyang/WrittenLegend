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
            
            var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/MonsterBox");
            var box = GameObject.Instantiate(boxPrefab).transform;
            box.SetParent(this.Transform);
            
            this.Camp = PlayerType.Enemy;
        }
    }
}
