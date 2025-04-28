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

    public Toggle toggle_Reform;
    public Panel_Reform PanelReform;

    public Toggle toggle_Strengthen;
    public Panel_Strengthen PanelStrengthen;

    public Toggle toggle_Refresh;
    public Panel_Refresh PanelRefresh;

    public Toggle toggle_Grade;
    public Panel_Grade PanelGrade;

    public Toggle toggle_Grade_Golden;
    public Panel_Grade_Golden PanelGradeGolden;

    public Toggle toggle_Grade_Dark;
    public Panel_Grade_Dark PanelGradeDark;

    public Toggle toggle_Hone;
    public Panel_Hone PanelHone;

    public Toggle toggle_Exchange;
    public Panel_Exchange PanelExchange;

    public Toggle toggle_Devour;
    public Panel_Devour PanelDevour;

    public Toggle toggle_ExclusiveUp;
    public Panel_Exclusive_Up PanelExclusiveUp;

    public Toggle toggle_GradeSpecail;
    public Panel_Grade_Specail PanelGradeSpecail;

    public Toggle toggle_Stone;
    public Panel_Stone PanelStone;

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

        this.toggle_Reform.onValueChanged.AddListener((isOn) =>
        {
            PanelReform.gameObject.SetActive(isOn);
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


        this.toggle_Grade_Dark.onValueChanged.AddListener((isOn) =>
        {
            PanelGradeDark.gameObject.SetActive(isOn);
        });

        this.toggle_Hone.onValueChanged.AddListener((isOn) =>
        {
            PanelHone.gameObject.SetActive(isOn);
        });

        this.toggle_ExclusiveUp.onValueChanged.AddListener((isOn) =>
        {
            PanelExclusiveUp.gameObject.SetActive(isOn);
        });

        this.toggle_GradeSpecail.onValueChanged.AddListener((isOn) =>
        {
            //PanelGradeSpecail.gameObject.SetActive(isOn);
        });


        this.toggle_Stone.onValueChanged.AddListener((isOn) =>
        {
            PanelStone.gameObject.SetActive(isOn);
        });
    }

    void OnEnable()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }

        if (user.Cycle.Data > 0)
        {
            toggle_Reform.gameObject.SetActive(true);
            toggle_Grade_Golden.gameObject.SetActive(true);
        }
        else
        {
            toggle_Reform.gameObject.SetActive(false);
            toggle_Grade_Golden.gameObject.SetActive(false);
        }

        if (user.MapId >= 1070)
        {
            toggle_Equip.gameObject.SetActive(true);
        }
        else
        {
            toggle_Equip.gameObject.SetActive(false);
        }

        if (user.Cycle.Data > 3)
        {
            toggle_Stone.gameObject.SetActive(true);
        }
        else
        {
            toggle_Stone.gameObject.SetActive(false);
        }
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
