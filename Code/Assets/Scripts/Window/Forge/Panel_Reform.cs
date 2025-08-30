using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Reform : MonoBehaviour
{
    public Transform Tran_Item_List;
    private ItemForge[] items;

    public Transform Tran_Attr_List;
    private StrenthAttrItem[] AttrList;

    public Text Reform_Txt_Fee;
    public Text Reform_Txt_Fee1;
    public Button Btn_Reform;

    private int Refine_Position = 1;

    private double UnitGold = 10000000000000000L;
    //private int ReformStoneFee = 1;
    // Start is called before the first frame update
    void Awake()
    {
        items = Tran_Item_List.GetComponentsInChildren<ItemForge>();
        Btn_Reform.onClick.AddListener(OnClick_Refine);

        AttrList = Tran_Attr_List.GetComponentsInChildren<StrenthAttrItem>();
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<EquipReformSelectEvent>(this.OnEquipReformSelectEvent);

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
            long level = user.GetReformLevel(position);

            items[i].Init(3, position, level, toggleGroup);
        }
    }

    private void ShowRefine()
    {
        User user = GameProcessor.Inst.User;

        long MaxLevel = user.GetReformLimit(Refine_Position);
        long currentLevel = user.GetReformLevel(Refine_Position);

        items[Refine_Position - 1].SetLevel(currentLevel);

        long nextLevel = currentLevel + 1;
        EquipReformFeeConfig feeConfig = EquipReformFeeConfigCategory.Instance.GetByLevel(nextLevel);

        if (feeConfig == null || nextLevel > MaxLevel)
        {
            Reform_Txt_Fee.text = "已满级";
            Reform_Txt_Fee1.text = "已满级";
            Btn_Reform.gameObject.SetActive(false);
        }
        else
        {
            long stoneCount = user.GetMaterialCount(ItemHelper.SpecialId_Reform_Stone);
            double needGold = feeConfig.GetFee(nextLevel); //京单位

            int needStoneCount = feeConfig.StoneFee;
            if (stoneCount > needStoneCount)
            {
                Reform_Txt_Fee.text = string.Format("需要改造石：<color={0}>{1}/{2}</color>", "#FFFF00", stoneCount, needStoneCount);
                Btn_Reform.gameObject.SetActive(true);
            }
            else
            {
                Reform_Txt_Fee.text = string.Format("需要改造石：<color={0}>{1}/{2}</color>", "#FF0000", stoneCount, needStoneCount);
                Btn_Reform.gameObject.SetActive(false);
            }

            double realNeedGold = UnitGold * needGold;

            if (user.MagicGold.Data >= realNeedGold)
            {
                Reform_Txt_Fee1.text = string.Format("需要金币：<color={0}>{1}</color>", "#FFFF00", StringHelper.FormatNumber(realNeedGold) );
                Btn_Reform.gameObject.SetActive(true);
            }
            else
            {
                Reform_Txt_Fee1.text = string.Format("需要金币：<color={0}>{1}</color>", "#FF0000", StringHelper.FormatNumber(realNeedGold));
                Btn_Reform.gameObject.SetActive(false);
            }
        }


        EquipReformConfig reformConfig = EquipReformConfigCategory.Instance.Get(Refine_Position);

        for (int i = 0; i < AttrList.Length; i++)
        {
            if (i < reformConfig.AttrList.Length && currentLevel >= reformConfig.RequireLevel[i])
            {
                int attrId = reformConfig.AttrList[i];

                long attrAdd = reformConfig.AttrValueList[i];
                long attrCurrent = reformConfig.GetAttr(currentLevel, i);

                AttrList[i].SetContent(attrId, attrCurrent, attrAdd);
                AttrList[i].gameObject.SetActive(true);
            }
            else
            {
                AttrList[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnEquipReformSelectEvent(EquipReformSelectEvent e)
    {
        this.Refine_Position = e.Position;
        ShowRefine();
    }

    private void OnClick_Refine()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = user.GetReformLevel(Refine_Position);

        long MaxLevel = user.GetReformLimit(Refine_Position);
        if (currentLevel >= MaxLevel)
        {
            //
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "改造等级满级了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long nextLevel = currentLevel + 1;
        EquipReformFeeConfig config = EquipReformFeeConfigCategory.Instance.GetByLevel(nextLevel);

        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Reform_Stone);

        if (materialCount < config.StoneFee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的改造石头", ToastType = ToastTypeEnum.Failure });
            return;
        }

        double needGold = config.GetFee(nextLevel); //京单位
        double realNeedGold = UnitGold * needGold;

        if (user.MagicGold.Data < realNeedGold)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的金币", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SubGold(realNeedGold);
        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Reform_Stone,
            Quantity = config.StoneFee
        });
        user.MagicEquipReform[Refine_Position].Data++;

        GameProcessor.Inst.UpdateInfo();
        ShowRefine();
    }


}

