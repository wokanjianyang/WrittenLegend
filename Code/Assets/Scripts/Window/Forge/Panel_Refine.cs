using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Refine : MonoBehaviour
{
    public Transform Tran_Item_List;
    private ItemForge[] items;

    public StrenthAttrItem Refine_Attr_Base;
    public StrenthAttrItem Refine_Attr_Quality;
    public StrenthAttrItem Refine_Attr_Strenth;

    public Text Refine_Txt_Fee;
    public Button Btn_Refine;

    private int Refine_Position = 1;

    // Start is called before the first frame update
    void Awake()
    {
        items = Tran_Item_List.GetComponentsInChildren<ItemForge>();
        Btn_Refine.onClick.AddListener(OnClick_Refine);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<EquipRefineSelectEvent>(this.OnEquipRefineSelectEvent);

        this.Init();
        this.ShowRefine();
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        for (int i = 0; i < items.Count(); i++)
        {
            int position = i + 1;
            long level = user.GetRefineLevel(position);

            items[i].Init(position, level, toggleGroup);
        }
    }

    private void ShowRefine()
    {
        User user = GameProcessor.Inst.User;
        long MaxLevel = user.GetRefineLimit();
        long currentLevel = user.GetRefineLevel(Refine_Position);

        items[Refine_Position - 1].SetLevel(currentLevel);

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


            long currentStrenthValue = currentConfig == null ? 0 : currentConfig.GetStengthPercent(currentLevel);
            long nextStrenthValue = nextConfig == null ? 0 : nextConfig.GetStengthPercent(nextLevel);
            long strenthRise = nextStrenthValue - currentStrenthValue;
            Refine_Attr_Strenth.gameObject.SetActive(true);
            Refine_Attr_Strenth.SetContent((int)AttributeEnum.EquipStrengthIncrea, currentStrenthValue, strenthRise);
        }
    }

    private void OnEquipRefineSelectEvent(EquipRefineSelectEvent e)
    {
        this.Refine_Position = e.Position;
        ShowRefine();
    }

    private void OnClick_Refine()
    {
        User user = GameProcessor.Inst.User;
        long currentLevel = user.GetRefineLevel(Refine_Position);

        long MaxLevel = user.GetRefineLimit();
        if (currentLevel >= MaxLevel)
        {
            //
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "精练等级满级了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long refineLevel = currentLevel + 1;
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


}

