using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class ViewForgeProcessor : AViewPage
{
    public Toggle toggle_Equip;
    public Toggle toggle_Exclusive;
    public Toggle toggle_Compound;

    public Panel_Compound PanelCompound;

    public Transform Nav_Equip;
    public Transform Nav_Exclusive;

    public Toggle toggle_Refine;
    public Panel_Refine PanelRefine;

    public Toggle toggle_Strengthen;
    public Panel_Strengthen PanelStrengthen;

    public Toggle toggle_Refresh;
    public Panel_Refresh PanelRefresh;

    public Toggle toggle_Grade;
    public Panel_Grade PanelGrade;

    public Toggle toggle_Grade_Golden;
    public Panel_Grade_Golden PanelGradeGolden;

    public Toggle toggle_Hone;
    public Panel_Hone PanelHone;

    public Toggle toggle_Exchange;
    public Panel_Exchange PanelExchange;

    public Toggle toggle_Devour;
    public Panel_Devour PanelDevour;

    public Toggle toggle_ExclusiveUp;
    public Panel_Exclusive_Up PanelExclusiveUp;

    private void Awake()
    {
        this.toggle_Equip.onValueChanged.AddListener((isOn) =>
        {
            this.Nav_Equip.gameObject.SetActive(isOn);
        });
        this.toggle_Exclusive.onValueChanged.AddListener((isOn) =>
        {
            this.Nav_Exclusive.gameObject.SetActive(isOn);
        });
        this.toggle_Compound.onValueChanged.AddListener((isOn) =>
        {
            this.PanelCompound.Show(isOn);
        });

        this.toggle_Strengthen.onValueChanged.AddListener((isOn) =>
        {
            PanelStrengthen.gameObject.SetActive(isOn);
        });

        this.toggle_Refine.onValueChanged.AddListener((isOn) =>
        {
            PanelRefine.gameObject.SetActive(isOn);
        });

        this.toggle_Exchange.onValueChanged.AddListener((isOn) =>
        {
            PanelExchange.gameObject.SetActive(isOn);
        });

        this.toggle_Devour.onValueChanged.AddListener((isOn) =>
        {
            PanelDevour.gameObject.SetActive(isOn);
        });

        this.toggle_Refresh.onValueChanged.AddListener((isOn) =>
        {
            PanelRefresh.gameObject.SetActive(isOn);
        });

        this.toggle_Grade.onValueChanged.AddListener((isOn) =>
        {
            PanelGrade.gameObject.SetActive(isOn);
        });

        this.toggle_Grade_Golden.onValueChanged.AddListener((isOn) =>
        {
            PanelGradeGolden.gameObject.SetActive(isOn);
        });

        this.toggle_Hone.onValueChanged.AddListener((isOn) =>
        {
            PanelHone.gameObject.SetActive(isOn);
        });

        this.toggle_ExclusiveUp.onValueChanged.AddListener((isOn) =>
        {
            PanelExclusiveUp.gameObject.SetActive(isOn);
        });
    }

    public override void OnBattleStart()
    {
        base.OnBattleStart();
    }

    protected override bool CheckPageType(ViewPageType page)
    {
        return page == ViewPageType.View_Forge;
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }
}
