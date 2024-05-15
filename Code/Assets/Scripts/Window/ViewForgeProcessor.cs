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
    public Toggle toggle_Compound;
    public ScrollRect sr_Left;
    public ScrollRect sr_Right;

    public Toggle toggle_Refine;
    public Panel_Refine PanelRefine;

    public Toggle toggle_Strengthen;
    public Panel_Strengthen PanelStrengthen;

    public Toggle toggle_Exchange;
    public Panel_Exchange PanelExchange;

    public Toggle toggle_Devour;
    public Panel_Devour PanelDevour;

    public Toggle toggle_Refresh;
    public Panel_Refresh PanelRefresh;

    public Toggle toggle_Grade;
    public Panel_Grade PanelGrade;

    private Dictionary<string, List<CompositeConfig>> allCompositeDatas = new Dictionary<string, List<CompositeConfig>>();

    private void Awake()
    {
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

        GameProcessor.Inst.EventCenter.AddListener<ChangeCompositeTypeEvent>(this.OnChangeCompositeTypeEvent);
        //GameProcessor.Inst.EventCenter.AddListener<CompositeUIFreshEvent>(this.OnCompositeUIFreshEvent);
    }

    private void ShowStrengthen(bool isOn)
    {
        PanelStrengthen.gameObject.SetActive(isOn);
    }

    // Composite
    private void InitComposite()
    {
        var allDatas = CompositeConfigCategory.Instance.GetAll();
        foreach (var kvp in allDatas)
        {
            this.allCompositeDatas.TryGetValue(kvp.Value.Type, out var list);
            if (list == null)
            {
                list = new List<CompositeConfig>();
            }

            list.Add(kvp.Value);

            this.allCompositeDatas[kvp.Value.Type] = list;
        }

        var menuItemPrefab = sr_Left.content.GetChild(0);
        menuItemPrefab.gameObject.SetActive(false);
        foreach (var kvp in this.allCompositeDatas)
        {
            var menuItem = GameObject.Instantiate(menuItemPrefab.gameObject);
            menuItem.transform.SetParent(sr_Left.content);
            menuItem.gameObject.SetActive(true);

            var com = menuItem.GetComponent<Com_CompositeMenu>();
            com.SetData(kvp.Key);
        }

        var firstCompositeList = this.allCompositeDatas.First().Value;

        var compositeItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Composite");
        foreach (var config in firstCompositeList)
        {
            var compositeItem = GameObject.Instantiate(compositeItemPrefab);
            compositeItem.transform.SetParent(sr_Right.content);
            compositeItem.gameObject.SetActive(true);

            var com = compositeItem.GetComponent<Item_Composite>();
            com.SetData(config);
        }
    }

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

    private void OnChangeCompositeTypeEvent(ChangeCompositeTypeEvent e)
    {
        var compositeList = this.allCompositeDatas[e.CompositeType];

        var compositeItemPrefab = sr_Right.content.GetChild(0);
        compositeItemPrefab.gameObject.SetActive(false);

        var total = sr_Right.content.childCount - 1;
        var max = Mathf.Max(total, compositeList.Count);
        for (var i = 0; i < max; i++)
        {
            if (i < compositeList.Count)
            {
                var config = compositeList[i];
                Item_Composite com = null;
                if (i < sr_Right.content.childCount - 1)
                {
                    com = sr_Right.content.GetChild(i + 1).GetComponent<Item_Composite>();
                    com.gameObject.SetActive(true);
                }
                else
                {
                    var compositeItem = GameObject.Instantiate(compositeItemPrefab);
                    compositeItem.transform.SetParent(sr_Right.content);
                    compositeItem.gameObject.SetActive(true);

                    com = compositeItem.GetComponent<Item_Composite>();
                }
                com.SetData(config);
            }
            else
            {
                sr_Right.content.GetChild(i + 1).gameObject.SetActive(false);
            }
        }

        //sr_Right.horizontalNormalizedPosition = 0;
    }

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

        this.InitComposite();
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }
}
