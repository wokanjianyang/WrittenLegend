using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Relic : MonoBehaviour, IBattleLife
{
    public Button Btn_Full;
    public Toggle toggle1;
    public Toggle toggle2;

    public Panel_Relic panel_Relic;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Full.onClick.AddListener(OnClick_Close);

        toggle1.onValueChanged.AddListener((isOn) =>
        {
            this.ShowPanel(1);
        });

        toggle2.onValueChanged.AddListener((isOn) =>
        {
            this.ShowPanel(2);
        });

        this.ShowPanel(1);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<RelicShowEvent>(this.OnShow);
    }

    public void OnShow(RelicShowEvent e)
    {
        this.gameObject.SetActive(true);
    }
    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    private void ShowPanel(int id)
    {
        panel_Relic.ChangePanel(id);
    }
}
