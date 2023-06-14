using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ViewForgeProcessor : AViewPage
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override bool CheckPageType(ViewPageType page)
    {
        return page == ViewPageType.View_Forge;
    }

    public override void OnBattleStart()
    {
        base.OnBattleStart();
    }
}
