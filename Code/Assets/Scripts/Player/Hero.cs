using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Unity.VisualScripting;

public class Hero : APlayer
{
    public override void Load()
    {
        var prefab = Resources.Load<GameObject>("Prefab/Char/Player");
        this.Transform = GameObject.Instantiate(prefab).transform;
        this.Camp = PlayerType.Hero;
    }
}
