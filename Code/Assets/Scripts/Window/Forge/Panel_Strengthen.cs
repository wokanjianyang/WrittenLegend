using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Strengthen : MonoBehaviour
{
    public Transform Tran_Item_List;
    private ItemForge[] items;

    public Transform tran_AttrList;
    private StrenthAttrItem[] AttrList;

    public Text Txt_Fee;
    public Button Btn_Strengthen;
    public Button Btn_Strengthen_Batch;

    private int SelectPosition = 1;

    // Start is called before the first frame update
    void Awake()
    {
        items = Tran_Item_List.GetComponentsInChildren<ItemForge>();

        AttrList = tran_AttrList.GetComponentsInChildren<StrenthAttrItem>();
        foreach (var attrTxt in AttrList)
        {
            attrTxt.gameObject.SetActive(false);
        }

        Btn_Strengthen.onClick.AddListener(OnClick_Strengthen);
        Btn_Strengthen_Batch.onClick.AddListener(OnClick_Strengthen_Batch);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<EquipStrengthSelectEvent>(this.OnEquipStrengthSelectEvent);

        this.Init();
        this.ShowStrengthInfo();
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        for (int i = 0; i < items.Count(); i++)
        {
            int position = i + 1;
            long level = user.GetStrengthLevel(position);

            items[i].Init(position, level, toggleGroup);
        }
    }

    private void OnEquipStrengthSelectEvent(EquipStrengthSelectEvent e)
    {
        this.SelectPosition = e.Position;
        this.ShowStrengthInfo();
    }


    private void ShowStrengthInfo()
    {
        //Log.Debug("ShowStrengthInfo");

        User user = GameProcessor.Inst.User;
        long MaxLevel = user.GetStrengthLimit();
        long currentLevel = user.GetStrengthLevel(SelectPosition);
        long refineStrenthPercetn = user.GetRefineStrenthPercetn(SelectPosition);

        items[SelectPosition - 1].SetLevel(currentLevel);

        long nextLevel = currentLevel + 1;

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

                AttrList[i].SetContent(attrId, attrCurrent, refineStrenthPercetn, attrAdd);
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


}

