using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_SoulRing : MonoBehaviour, IBattleLife
{
    public Button Btn_Full;
    public Toggle toggle_Ring;
    public Toggle toggle_Bone;

    public Panel_SoulRing panel_SoulRing;
    public Panel_SoulBone panel_SoulBone;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Full.onClick.AddListener(OnClick_Close);

        toggle_Ring.onValueChanged.AddListener((isOn) =>
        {
            panel_SoulRing.gameObject.SetActive(isOn);
        });

        toggle_Bone.onValueChanged.AddListener((isOn) =>
        {
            panel_SoulBone.gameObject.SetActive(isOn);
        });
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowSoulRingEvent>(this.OnShowSoulRingEvent);
    }

    public void OnShowSoulRingEvent(ShowSoulRingEvent e)
    {
        this.gameObject.SetActive(true);
    }
    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
