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

    public Toggle toggle_Exchange;
    public Panel_Exchange PanelExchange;

    public Toggle toggle_Devour;
    public Panel_Devour PanelDevour;

    public Toggle toggle_ExclusiveUp;

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
            this.ShowStrengthen(isOn);
        });

        this.toggle_Refine.onValueChanged.AddListener((isOn) =>
        {
            this.ShowRefine(isOn);
        });

        this.toggle_Exchange.onValueChanged.AddListener((isOn) =>
        {
            PanelExchange.gameObject.SetActive(isOn);
        });

        this.toggle_Devour.onValueChanged.AddListener((isOn) =>
        {
            this.ShowDevour(isOn);
        });

        this.toggle_Refresh.onValueChanged.AddListener((isOn) =>
        {
            this.ShowRefresh(isOn);
        });

        this.toggle_Grade.onValueChanged.AddListener((isOn) =>
        {
            this.ShowGrade(isOn);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        this.ShowStrengthen(true);
    }

    public override void OnBattleStart()
    {
        base.OnBattleStart();

        //GameProcessor.Inst.EventCenter.AddListener<ChangeCompositeTypeEvent>(this.OnChangeCompositeTypeEvent);
        //GameProcessor.Inst.EventCenter.AddListener<CompositeUIFreshEvent>(this.OnCompositeUIFreshEvent);
    }

    private void ShowStrengthen(bool isOn)
    {
        PanelStrengthen.gameObject.SetActive(isOn);
    }

    // Composite


    //private void ShowComposite() {
    //    for (int i = 0; i < sr_Right.content.childCount; i++)
    //    {
    //        Item_Composite com = sr_Right.content.GetChild(i).GetComponent<Item_Composite>();
    //        if (com != null)
    //        {
    //            com.Check();
    //        }
    //    }
    //}

    //private void OnCompositeUIFreshEvent(CompositeUIFreshEvent e)
    //{
    //    ShowComposite();
    //}


    // Refine
    private void ShowRefine(bool isOn)
    {
        PanelRefine.gameObject.SetActive(isOn);
    }

    private void ShowDevour(bool isOn)
    {
        PanelDevour.gameObject.SetActive(isOn);
    }

    private void ShowRefresh(bool isOn)
    {
        PanelRefresh.gameObject.SetActive(isOn);
    }

    private void ShowGrade(bool isOn)
    {
        PanelGrade.gameObject.SetActive(isOn);
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
