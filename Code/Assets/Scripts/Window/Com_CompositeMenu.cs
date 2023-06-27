using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Com_CompositeMenu : MonoBehaviour
{
    public Text Name;

    private string compositeType;

    public void SetData(string t)
    {
        this.compositeType = t;
        this.Name.text = t;
    }

    public void OnClick_ChangeCompositeType()
    {
        GameProcessor.Inst.EventCenter.Raise(new ChangeCompositeTypeEvent()
        {
            CompositeType = this.compositeType
        });
    }
}
