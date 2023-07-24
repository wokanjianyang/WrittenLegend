using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
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


    private Dictionary<string, List<CompositeConfig>> allCompositeDatas= new Dictionary<string, List<CompositeConfig>>();

    // Start is called before the first frame update
    void Start()
    {
        Btn_Strengthen.onClick.AddListener(OnClick_Strengthen);
        Btn_Strengthen_Batch.onClick.AddListener(OnClick_Strengthen_Batch);

        //var equipList = tran_EquiList.GetComponentsInChildren<SlotBox>();

        AttrList = tran_AttrList.GetComponentsInChildren<StrenthAttrItem>();
        foreach (var attrTxt in AttrList)
        {
            attrTxt.gameObject.SetActive(false);
        }
        StrengthenEquiList = tran_EquiList.GetComponentsInChildren<StrenthBox>();

        Refine_EquiList =  Refine_Tran_EquiList.GetComponentsInChildren<RefineBox>();

        this.toggle_Strengthen.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                Log.Debug("打开强化界面");
                this.ShowStrengthInfo();
            }
        });
        this.toggle_Compound.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                Log.Debug("打开合成界面");
                this.ShowComposite();
            }
        });
        this.toggle_Refine.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                Log.Debug("打开精练界面");
                this.ShowRefine();
            }
        });

        Btn_Refine.onClick.AddListener(OnClick_Refine);

        Refine.gameObject.SetActive(false);
    }

    public override void OnBattleStart()
    {
        base.OnBattleStart();

        GameProcessor.Inst.EventCenter.AddListener<EquipStrengthSelectEvent>(this.OnEquipStrengthSelectEvent);
        GameProcessor.Inst.EventCenter.AddListener<ChangeCompositeTypeEvent>(this.OnChangeCompositeTypeEvent);
        GameProcessor.Inst.EventCenter.AddListener<CompositeUIFreshEvent>(this.OnCompositeUIFreshEvent);
        GameProcessor.Inst.EventCenter.AddListener<EquipRefineSelectEvent>(this.OnEquipRefineSelectEvent);
        
        this.ShowStrengthInfo();
        this.InitComposite();
    }

    private void OnEquipStrengthSelectEvent(EquipStrengthSelectEvent e)
    {
        Debug.Log("SelectPosition:" + e.Position);

        if (e.Position == SelectPosition) {
            return;
        }

        //̬
        StrenthBox oldBox = StrengthenEquiList.Where(m => ((int)m.SlotType) == SelectPosition).First();
        oldBox.SeSelect(false);

        this.SelectPosition = e.Position;
        ShowStrengthInfo();
    }

    private void ShowStrengthInfo()
    {
        User user = GameProcessor.Inst.User;

        user.EquipStrength.TryGetValue(SelectPosition, out long strengthLevel);

        EquipStrengthConfig config = EquipStrengthConfigCategory.Instance.GetByPositioin(SelectPosition);

        EquipStrengthFeeConfig feeConfig = EquipStrengthFeeConfigCategory.Instance.GetByLevel(strengthLevel + 1);

        long fee = feeConfig.Fee;

        string color = user.Gold >= fee ? "#FFFF00" : "#FF0000";

        Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, fee);


        for (int i = 0; i < AttrList.Length; i++)
        {
            if (i < config.AttrList.Length)
            {
                int attrId = config.AttrList[i];

                string attrAdd = config.AttrValueList[i] + "";
                string attrCurrent = config.AttrValueList[i] * strengthLevel + "";
                string attrName = PlayerHelper.PlayerAttributeMap[((AttributeEnum)attrId).ToString()];

                AttrList[i].SetContent(attrName, attrCurrent, attrAdd);
                AttrList[i].gameObject.SetActive(true);
            }
            else
            {
                AttrList[i].gameObject.SetActive(false);
            }
        }


        foreach (var box in StrengthenEquiList)
        {
            int position = ((int)box.SlotType);
            user.EquipStrength.TryGetValue(position, out long level);

            box.SetLevel(level);

            if (position == SelectPosition)
            {
                box.SeSelect(true);
            }
        }
    }

    private void OnClick_Strengthen()
    {
        User user = GameProcessor.Inst.User;
        user.EquipStrength.TryGetValue(SelectPosition, out long strengthLevel);

        if (strengthLevel >= user.Level)
        {
            // 改为无限强化
            //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "强化等级不能超过人物等级", Parent = tran_AttrList });
            //return;
        }

        EquipStrengthFeeConfig config = EquipStrengthFeeConfigCategory.Instance.GetByLevel(strengthLevel + 1);

        long fee = config.Fee;

        if (user.Gold < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的金币", Parent = tran_AttrList });
            return;
        }

        user.EquipStrength[SelectPosition] = strengthLevel + 1;

        user.AddExpAndGold(0, -fee);

        GameProcessor.Inst.UpdateInfo();

        ShowStrengthInfo();

        //Debug.Log("Strengthen Success");
    }
    private void OnClick_Strengthen_Batch()
    {
        User user = GameProcessor.Inst.User;
        user.EquipStrength.TryGetValue(SelectPosition, out long strengthLevel);

        //
        EquipStrengthFeeConfig config = EquipStrengthFeeConfigCategory.Instance.GetByLevel(strengthLevel + 1);

        long maxLevel = config.EndLevel - strengthLevel;
        long realLevel = user.Gold / config.Fee;

        long sl = Math.Min(maxLevel, realLevel);

        if (sl > 0)
        {
            user.EquipStrength[SelectPosition] = strengthLevel + sl;

            user.AddExpAndGold(0, -config.Fee * sl);

            GameProcessor.Inst.UpdateInfo();

            ShowStrengthInfo();
            Debug.Log("batch strenthen " + sl + " level");
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

        var compositeItemPrefab = sr_Right.content.GetChild(0);
        compositeItemPrefab.gameObject.SetActive(false);
        foreach (var config in firstCompositeList)
        {
            var compositeItem = GameObject.Instantiate(compositeItemPrefab);
            compositeItem.transform.SetParent(sr_Right.content);
            compositeItem.gameObject.SetActive(true);

            var com = compositeItem.GetComponent<Com_CompositeItem>();
            com.SetData(config);
        }
    }

    private void ShowComposite() {
        for (int i = 0; i < sr_Right.content.childCount; i++)
        {
            Com_CompositeItem com = sr_Right.content.GetChild(i).GetComponent<Com_CompositeItem>();
            if (com != null)
            {
                com.Refresh();
            }
        }
    }

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
                Com_CompositeItem com = null;
                if (i < sr_Right.content.childCount - 1)
                {
                    com = sr_Right.content.GetChild(i + 1).GetComponent<Com_CompositeItem>();
                    com.gameObject.SetActive(true);
                }
                else
                {
                    var compositeItem = GameObject.Instantiate(compositeItemPrefab);
                    compositeItem.transform.SetParent(sr_Right.content);
                    compositeItem.gameObject.SetActive(true);

                    com = compositeItem.GetComponent<Com_CompositeItem>();
                }
                com.SetData(config);
            }
            else
            {
                sr_Right.content.GetChild(i+1).gameObject.SetActive(false);
            }
        }

        sr_Right.verticalNormalizedPosition = 0;
    }

    private void OnCompositeUIFreshEvent(CompositeUIFreshEvent e)
    {
        ShowComposite();
    }


    // Refine
    private void ShowRefine()
    {
        User user = GameProcessor.Inst.User;
        user.EquipRefine.TryGetValue(Refine_Position, out int refineLevel);

        foreach (var box in Refine_EquiList)
        {
            int position = ((int)box.SlotType);
            user.EquipRefine.TryGetValue(position, out int level);
            box.SetLevel(level);

            if (position == Refine_Position)
            {
                box.SeSelect(true);
            }
        }

        EquipRefineConfig currentConfig = EquipRefineConfigCategory.Instance.GetByLevel(refineLevel);

        EquipRefineConfig nextConfig = EquipRefineConfigCategory.Instance.GetByLevel(refineLevel + 1);

        if (nextConfig != null)
        {
            var materialCount = user.GetMaterialCount(ItemHelper.SpecialId_EquipRefineStone);

            string color = materialCount >= nextConfig.Fee ? "#FFFF00" : "#FF0000";

            Refine_Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, nextConfig.Fee);
        }

        EquipRefineConfig showConfig = currentConfig == null ? nextConfig : currentConfig;


        Refine_Attr_Base.gameObject.SetActive(false);
        Refine_Attr_Quality.gameObject.SetActive(false);


        if (nextConfig.BaseAttrPercent > 0)
        {
            long currentAttrValue = currentConfig == null ? 0 : currentConfig.BaseAttrPercent;
            long nextAttrValue = nextConfig == null ? 0 : nextConfig.BaseAttrPercent;

            string attrName = PlayerHelper.PlayerAttributeMap[AttributeEnum.EquipBaseIncrea.ToString()];
            string attrCurrent = currentAttrValue == 0 ? "" : currentAttrValue + "%";
            string attrAdd = "+" + (nextAttrValue - currentAttrValue) + "%";

            Refine_Attr_Base.gameObject.SetActive(true);
            Refine_Attr_Base.SetContent(attrName, attrCurrent, attrAdd);
        }

        if (nextConfig.QualityAttrPercent > 0)
        {
            long currentAttrValue = currentConfig == null ? 0 : currentConfig.QualityAttrPercent;
            long nextAttrValue = nextConfig == null ? 0 : nextConfig.QualityAttrPercent;

            string attrName = PlayerHelper.PlayerAttributeMap[AttributeEnum.EquipQualityIncrea.ToString()];
            string attrCurrent = currentAttrValue == 0 ? "" : currentAttrValue + "%";
            string attrAdd = "+" + (nextAttrValue - currentAttrValue) + "%";

            Refine_Attr_Quality.gameObject.SetActive(true);
            Refine_Attr_Quality.SetContent(attrName, attrCurrent, attrAdd);
        }
    }
    private void OnEquipRefineSelectEvent(EquipRefineSelectEvent e)
    {
        Debug.Log("Refine Position:" + e.Position);

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
        user.EquipRefine.TryGetValue(Refine_Position, out int refineLevel);

        if (refineLevel >= user.Level)
        {
            //
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "精练等级不能超过人物等级", Parent = Refine_Tran_AttrList });
            return;
        }

        EquipRefineConfig config = EquipRefineConfigCategory.Instance.GetByLevel(refineLevel + 1);

        var materialCount = user.GetMaterialCount(ItemHelper.SpecialId_EquipRefineStone);

        if (materialCount < config.Fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的精炼石", Parent = Refine_Tran_AttrList });
            return;
        }

        user.EquipRefine[Refine_Position] = refineLevel + 1;

        GameProcessor.Inst.EventCenter.Raise(new MaterialUseEvent()
        {
            MaterialId = ItemHelper.SpecialId_EquipRefineStone,
            Quantity = config.Fee
        });

        GameProcessor.Inst.UpdateInfo();

        ShowRefine();

        Debug.Log("Refine Success");
    }

    protected override bool CheckPageType(ViewPageType page)
    {
        return page == ViewPageType.View_Forge;
    }


}
