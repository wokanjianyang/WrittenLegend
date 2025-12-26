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
    public Transform Nav_Other;

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

    public Toggle toggle_Grade_Hundun;
    public Panel_Grade_Hundun PanelGradeHundun;

    public Toggle toggle_Hone;
    public Panel_Hone PanelHone;

    public Toggle toggle_Exchange;
    public Panel_Exchange PanelExchange;

    public Toggle toggle_Devour;
    public Panel_Devour PanelDevour;

    public Toggle toggle_ExclusiveUp;
    public Panel_Exclusive_Up PanelExclusiveUp;

    public Toggle toggle_ExclusiveUpGolden;
    public Panel_Exclusive_Up_Golden PanelExclusiveUpGolden;

    public Toggle toggle_ExclusiveGold;
    public Panel_Devour_Golden PanelDevourGolden;

    public Toggle toggle_ExclusiveDark;
    public Panel_Devour_Dark PanelDevourDark;

    public Toggle toggle_GradeSpecail;
    public Panel_Grade_Specail PanelGradeSpecail;

    public Toggle toggle_Other;

    public Toggle toggle_Shengxiao;
    public Panel_Shengxiao_Up PanelShengxiao;

    public Toggle toggle_Shengxiao_Grade;
    public Panel_Shengxiao_Grade PanelShengxiaoGrade;

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
        this.toggle_Other.onValueChanged.AddListener((isOn) =>
        {
            this.Nav_Other.gameObject.SetActive(isOn);
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

        this.toggle_Grade_Hundun.onValueChanged.AddListener((isOn) =>
        {
            PanelGradeHundun.gameObject.SetActive(isOn);
        });

        this.toggle_Hone.onValueChanged.AddListener((isOn) =>
        {
            PanelHone.gameObject.SetActive(isOn);
        });

        this.toggle_ExclusiveUp.onValueChanged.AddListener((isOn) =>
        {
            PanelExclusiveUp.gameObject.SetActive(isOn);
        });

        this.toggle_ExclusiveUpGolden.onValueChanged.AddListener((isOn) =>
        {
            PanelExclusiveUpGolden.gameObject.SetActive(isOn);
        });

        this.toggle_ExclusiveGold.onValueChanged.AddListener((isOn) =>
        {
            PanelDevourGolden.gameObject.SetActive(isOn);
        });

        this.toggle_ExclusiveDark.onValueChanged.AddListener((isOn) =>
        {
            PanelDevourDark.gameObject.SetActive(isOn);
        });


        this.toggle_GradeSpecail.onValueChanged.AddListener((isOn) =>
        {
            //PanelGradeSpecail.gameObject.SetActive(isOn);
        });


        this.toggle_Stone.onValueChanged.AddListener((isOn) =>
        {
            PanelStone.gameObject.SetActive(isOn);
        });

        this.toggle_Shengxiao.onValueChanged.AddListener((isOn) =>
        {
            PanelShengxiao.gameObject.SetActive(isOn);
        });


        this.toggle_Shengxiao_Grade.onValueChanged.AddListener((isOn) =>
        {
            PanelShengxiaoGrade.gameObject.SetActive(isOn);
        });
    }

    void OnEnable()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }
        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

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

        if (user.Cycle.Data > 3 && !ac)
        {
            toggle_Other.gameObject.SetActive(true);
            toggle_Stone.gameObject.SetActive(true);
        }
        else
        {
            toggle_Other.gameObject.SetActive(false);
            toggle_Stone.gameObject.SetActive(false);
        }


        if (user.MapId >= 1130)
        {
            toggle_ExclusiveGold.gameObject.SetActive(true);
            toggle_ExclusiveUpGolden.gameObject.SetActive(true);
        }
        else
        {
            toggle_ExclusiveGold.gameObject.SetActive(false);
            toggle_ExclusiveUpGolden.gameObject.SetActive(false);
        }

        if (user.MapId >= 1169)
        {
            toggle_ExclusiveDark.gameObject.SetActive(true);
        }
        else
        {
            toggle_ExclusiveDark.gameObject.SetActive(false);
        }

        if (user.MapId >= 1175)
        {
            toggle_Grade_Hundun.gameObject.SetActive(true);
        }
        else
        {
            toggle_Grade_Hundun.gameObject.SetActive(false);
        }

        if (user.Cycle.Data >= 10)
        {
            toggle_Shengxiao.gameObject.SetActive(true);
        }
        else
        {
            toggle_Shengxiao.gameObject.SetActive(false);
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
