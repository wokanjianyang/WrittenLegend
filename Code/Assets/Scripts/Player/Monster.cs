using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : APlayer
{
    public override void Load()
    {
        var prefab = Resources.Load<GameObject>("Prefab/Char/Enemy").transform;
        this.Transform = GameObject.Instantiate(prefab).transform;
        
        this.Camp = PlayerType.Enemy;
    }
}
