using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Travel : MonoBehaviour
{
    public Text Txt_MapName;

    public Button btn_Start;

    private int MapId;


    // Start is called before the first frame update
    void Start()
    {
        btn_Start.onClick.AddListener(OnClick_NavigateMap);
    }


    private void OnClick_NavigateMap()
    {
        GameProcessor.Inst.EventCenter.Raise(new PetStartTravelEvent() { MapId = this.MapId });
    }

    public void Init(MapConfig mapConfig)
    {
        this.MapId = mapConfig.Id;
        Txt_MapName.text = mapConfig.Name;
    }
}
