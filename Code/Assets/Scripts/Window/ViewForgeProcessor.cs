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
    public Toggle toggle_Strengthen;
    public Transform tran_EquiList;
    public Transform tran_AttrList;
    public Text Txt_Fee;
    public Button Btn_Strengthen;
    public Button Btn_Strengthen_Batch;
    private StrenthAttrItem[] AttrList;
    private StrenthBox[] StrengthenEquiList;
    private int SelectPosition = 1;

    public Toggle toggle_Compound;
    public ScrollRect sr_Left;
    public ScrollRect sr_Right;

    public Transform Refine;
    public Toggle toggle_Refine;
    public Transform Refine_Tran_EquiList;
    public Transform Refine_Tran_AttrList;
    public Text Refine_Txt_Fee;
    public Button Btn_Refine;

    public StrenthAttrItem Refine_Attr_Base;
    public StrenthAttrItem Refine_Attr_Quality;

    private RefineBox[] Refine_EquiList;
    private int Refine_Position = 1;

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
        Btn_Strengthen.onClick.AddListener(OnClick_Strengthen);
        Btn_Strengthen_Batch.onClick.AddListener(OnClick_Strengthen_Batch);

        this.toggle_Refine.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                this.ShowRefine();
            }
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

        Btn_Refine.onClick.AddListener(OnClick_Refine);

        Refine.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.ShowStrengthInfo();
    }

    public override void OnBattleStart()
    {
        base.OnBattleStart();

        GameProcessor.Inst.EventCenter.AddListener<EquipStrengthSelectEvent>(this.OnEquipStrengthSelectEvent);
        GameProcessor.Inst.EventCenter.AddListener<ChangeCompositeTypeEvent>(this.OnChangeCompositeTypeEvent);
        //GameProcessor.Inst.EventCenter.AddListener<CompositeUIFreshEvent>(this.OnCompositeUIFreshEvent);
        GameProcessor.Inst.EventCenter.AddListener<EquipRefineSelectEvent>(this.OnEquipRefineSelectEvent);

    }

    private void OnEquipStrengthSelectEvent(EquipStrengthSelectEvent e)
    {
        List<StrenthBox> list = StrengthenEquiList.Where(m => m.Active).ToList();
        foreach (var oldBox in list)
        {
            oldBox.SeSelect(false);
        }
        this.SelectPosition = e.Position;

        this.ShowStrengthInfo();
    }

    private void ShowStrengthInfo()
    {
        //Log.Debug("ShowStrengthInfo");

        User user = GameProcessor.Inst.User;

        long MaxLevel = user.GetStrengthLimit();

        foreach (var box in StrengthenEquiList)
        {
            int position = ((int)box.SlotType);

            if (user.MagicEquipStrength.TryGetValue(position, out MagicData strenthTemp))
            {
                box.SetLevel(strenthTemp.Data);
            }
            else
            {
                user.MagicEquipStrength[position] = new MagicData();
            }
        }

        long nextLevel = 1;

        if (user.MagicEquipStrength.TryGetValue(SelectPosition, out MagicData strengthData))
        {
            nextLevel = strengthData.Data + 1;
        }

        EquipStrengthConfig config = EquipStrengthConfigCategory.Instance.GetByPositioin(SelectPosition);

        EquipStrengthFeeConfig feeConfig = EquipStrengthFeeConfigCategory.Instance.GetByLevel(nextLevel);

        long levelAttr = LevelConfigCategory.GetLevelAttr(nextLevel);

        if (feeConfig == null || nextLevel > MaxLevel)
        {
            Txt_Fee.text = "已满级";
            Btn_Strengthen.gameObject.SetActive(false);
            Btn_Strengthen_Batch.gameObject.SetActive(false);
        }
        else
        {
            long fee = levelAttr * feeConfig.Fee;
            string color = user.MagicGold.Data >= fee ? "#FFFF00" : "#FF0000";
            Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, StringHelper.FormatNumber(fee));

            Btn_Strengthen.gameObject.SetActive(true);
            Btn_Strengthen_Batch.gameObject.SetActive(true);
        }

        for (int i = 0; i < AttrList.Length; i++)
        {
            if (i < config.AttrList.Length)
            {
                int attrId = config.AttrList[i];

                long attrAdd = config.AttrValueList[i] * nextLevel;
                long attrCurrent = config.AttrValueList[i] * levelAttr;

                AttrList[i].SetContent(attrId, attrCurrent, attrAdd);
                AttrList[i].gameObject.SetActive(true);
            }
            else
            {
                AttrList[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnClick_Strengthen()
    {
        User user = GameProcessor.Inst.User;

        long nextLevel = 1;

        if (user.MagicEquipStrength.TryGetValue(SelectPosition, out MagicData strengthData))
        {
            nextLevel = strengthData.Data + 1;
        }

        EquipStrengthFeeConfig config = EquipStrengthFeeConfigCategory.Instance.GetByLevel(nextLevel);

        long levelAttr = LevelConfigCategory.GetLevelAttr(nextLevel);
        long fee = levelAttr * config.Fee;

        if (user.MagicGold.Data < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的金币", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.MagicEquipStrength[SelectPosition].Data++;

        user.SubGold(fee);

        GameProcessor.Inst.UpdateInfo();

        ShowStrengthInfo();

        TaskHelper.CheckTask(TaskType.Strength, 1);

        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "强化成功", ToastType = ToastTypeEnum.Success });
    }

    private void OnClick_Strengthen_Batch()
    {
        User user = GameProcessor.Inst.User;

        long nextLevel = 1;

        if (user.MagicEquipStrength.TryGetValue(SelectPosition, out MagicData strengthData))
        {
            nextLevel = strengthData.Data + 1;
        }

        EquipStrengthFeeConfig config = EquipStrengthFeeConfigCategory.Instance.GetByLevel(nextLevel);

        int LimitLevel = user.GetStrengthLimit();
        long maxLevel = Math.Min(config.EndLevel, LimitLevel) - nextLevel + 1;

        long sl = 0;
        long feeTotal = 0;

        for (int i = 0; i < maxLevel; i++)
        {
            long levelAttr = LevelConfigCategory.GetLevelAttr(nextLevel + i);
            long fee = levelAttr * config.Fee;

            if (feeTotal + fee > user.MagicGold.Data)
            {
                break;
            }

            feeTotal += fee;
            sl++;
        }


        if (sl > 0)
        {
            user.MagicEquipStrength[SelectPosition].Data += sl;

            user.SubGold(feeTotal);

            GameProcessor.Inst.UpdateInfo();

            ShowStrengthInfo();

            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "一键强化成功", ToastType = ToastTypeEnum.Success });

            TaskHelper.CheckTask(TaskType.Strength, 1);
        }
        else
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不够", ToastType = ToastTypeEnum.Failure });
        }
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
    private void ShowRefine()
    {
        User user = GameProcessor.Inst.User;

        long MaxLevel = user.GetRefineLimit();

        foreach (var box in Refine_EquiList)
        {
            int position = ((int)box.SlotType);

            if (user.MagicEquipRefine.TryGetValue(position, out MagicData refineTemp))
            {
                box.SetLevel(refineTemp.Data);
            }
            else
            {
                user.MagicEquipRefine[position] = new MagicData();
            }

            if (position == Refine_Position)
            {
                box.SeSelect(true);
            }
        }

        user.MagicEquipRefine.TryGetValue(Refine_Position, out MagicData refineData);

        long currentLevel = refineData.Data;
        EquipRefineConfig currentConfig = EquipRefineConfigCategory.Instance.GetByLevel(currentLevel);

        long nextLevel = currentLevel + 1;
        EquipRefineConfig nextConfig = EquipRefineConfigCategory.Instance.GetByLevel(nextLevel);

        if (nextConfig == null || nextLevel > MaxLevel)
        {
            Refine_Txt_Fee.text = "已满级";
            Btn_Refine.gameObject.SetActive(false);
        }
        else
        {
            var materialCount = user.GetMaterialCount(ItemHelper.SpecialId_EquipRefineStone);

            string color = materialCount >= nextConfig.GetFee(nextLevel) ? "#FFFF00" : "#FF0000";

            Refine_Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, nextConfig.GetFee(nextLevel));
            Btn_Refine.gameObject.SetActive(true);
        }

        Refine_Attr_Base.gameObject.SetActive(false);
        Refine_Attr_Quality.gameObject.SetActive(false);

        if (nextConfig != null && nextConfig.GetBaseAttrPercent(nextLevel) > 0)
        {
            long currentAttrValue = currentConfig == null ? 0 : currentConfig.GetBaseAttrPercent(currentLevel);
            long nextAttrValue = nextConfig == null ? 0 : nextConfig.GetBaseAttrPercent(nextLevel);

            long attrRise = nextAttrValue - currentAttrValue;

            Refine_Attr_Base.gameObject.SetActive(true);
            Refine_Attr_Base.SetContent((int)AttributeEnum.EquipBaseIncrea, currentAttrValue, attrRise);
        }

        if (nextConfig != null && nextConfig.GetQualityAttrPercent(nextLevel) > 0)
        {
            long currentAttrValue = currentConfig == null ? 0 : currentConfig.GetQualityAttrPercent(currentLevel);
            long nextAttrValue = nextConfig == null ? 0 : nextConfig.GetQualityAttrPercent(nextLevel);

            long attrRise = nextAttrValue - currentAttrValue;

            Refine_Attr_Quality.gameObject.SetActive(true);
            Refine_Attr_Quality.SetContent((int)AttributeEnum.EquipRandomIncrea, currentAttrValue, attrRise);
        }
    }
    private void OnEquipRefineSelectEvent(EquipRefineSelectEvent e)
    {
        if (e.Position == Refine_Position)
        {
            return;
        }

        //̬
        RefineBox oldBox = Refine_EquiList.Where(m => ((int)m.SlotType) == Refine_Position).First();
        oldBox.SeSelect(false);

        this.Refine_Position = e.Position;
        ShowRefine();
    }
    private void OnClick_Refine()
    {
        User user = GameProcessor.Inst.User;
        user.MagicEquipRefine.TryGetValue(Refine_Position, out MagicData refineData);

        long MaxLevel = user.GetRefineLimit();
        if (refineData.Data >= MaxLevel)
        {
            //
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "精练等级满级了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long refineLevel = refineData.Data + 1;
        EquipRefineConfig config = EquipRefineConfigCategory.Instance.GetByLevel(refineLevel);

        var materialCount = user.GetMaterialCount(ItemHelper.SpecialId_EquipRefineStone);

        if (materialCount < config.GetFee(refineLevel))
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的精炼石", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.MagicEquipRefine[Refine_Position].Data++;

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_EquipRefineStone,
            Quantity = config.GetFee(refineLevel)
        });

        GameProcessor.Inst.UpdateInfo();

        ShowRefine();
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

        //var equipList = tran_EquiList.GetComponentsInChildren<SlotBox>();

        AttrList = tran_AttrList.GetComponentsInChildren<StrenthAttrItem>();
        foreach (var attrTxt in AttrList)
        {
            attrTxt.gameObject.SetActive(false);
        }
        StrengthenEquiList = tran_EquiList.GetComponentsInChildren<StrenthBox>();

        Refine_EquiList = Refine_Tran_EquiList.GetComponentsInChildren<RefineBox>();

        this.InitComposite();
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }
}
