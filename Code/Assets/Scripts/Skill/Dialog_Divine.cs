using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Divine : MonoBehaviour, IBattleLife
{
    public Transform Tran_Item_List;
    private List<Item_Divine> items;

    public Text Txt_Desc;

    public StrenthAttrItem AttrItem;

    public Text Txt_Metail;
    public Text Txt_Fee;

    public Button Btn_Ok;
    public Button Btn_Close;
    public Button Btn_Restore;
    public Text Txt_OK;

    private int SkillId = 0;
    SkillData skillData = null;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Ok.onClick.AddListener(OnClick_Ok);
        Btn_Restore.onClick.AddListener(OnClick_Restore);

        items = this.GetComponentsInChildren<Item_Divine>().ToList();

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            items[i].toggle.onValueChanged.AddListener((isOn) =>
            {
                ShowItem(item);
            });
        }

        this.Init();
    }

    private void Init()
    {
        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        User user = GameProcessor.Inst.User;

        List<SkillDivineConfig> configs = SkillDivineConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            SkillDivineConfig config = configs[i];
            Item_Divine box = items[i];

            box.Init(toggleGroup, config);
        }
    }


    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenDivineEvent>(this.OnOpenDivine);
    }

    private void OnOpenDivine(OpenDivineEvent e)
    {
        this.gameObject.SetActive(true);
        this.SkillId = e.SkillId;

        this.Show();
    }

    private void Show()
    {
        Btn_Restore.gameObject.SetActive(true);

        User user = GameProcessor.Inst.User;
        List<SkillDivineConfig> configs = SkillDivineConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        skillData = user.SkillList.Where(m => m.SkillId == SkillId).FirstOrDefault();

        for (int i = 0; i < configs.Count; i++)
        {
            SkillDivineConfig config = configs[i];

            long level = skillData.GetDivineItemLevel(config.Id);
            items[i].SetContent(level);
        }

        Item_Divine currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        ShowItem(currentItem);
    }

    public string formatSkillAttrName(int SkillAttrId)
    {

        if (SkillAttrId == 1)
        {
            return "技能系数倍率";
        }
        else if (SkillAttrId == 2)
        {
            return "技能攻击加成";
        }
        else if (SkillAttrId == 3)
        {
            return "技能终伤加成";
        }

        return "";
    }

    private void ShowItem(Item_Divine currentItem)
    {
        if (skillData == null)
        {
            return; //not init;
        }

        User user = GameProcessor.Inst.User;

        SkillDivineConfig config = currentItem.Config;

        long currentLevel = skillData.GetDivineItemLevel(config.Id);
        currentItem.SetContent(currentLevel);

        //attr
        AttrItem.SetContent(formatSkillAttrName(config.SkillAttrId), config.SkillAttrValue * currentLevel + "%", "+" + config.SkillAttrValue + "%");

        long total = user.GetBagItemCount(config.ItemId);
        long needNumber = GetNeedNumber(currentLevel);

        string color = total >= needNumber ? "#FFFF00" : "#FF0000";

        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(config.ItemId);

        Txt_Metail.text = "消耗" + itemConfig.Name + "";
        Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, total, needNumber);

        SkillDivineAttrConfig divineAttrConfig = SkillDivineAttrConfigCategory.Instance.GetBySkillId(this.SkillId);
        long divineMax = currentLevel * divineAttrConfig.Param;
        Txt_Desc.text = "完成神技所有阶段之后，获得神技效果：" + string.Format(divineAttrConfig.Desc, divineMax);

        if (total >= needNumber && currentLevel < 4)
        {
            if (currentLevel <= 0)
            {
                Btn_Ok.gameObject.SetActive(true);
                Txt_OK.text = "激活";
            }
            else
            {
                Btn_Ok.gameObject.SetActive(true);
                Txt_OK.text = "升级";
            }
        }
        else
        {
            Btn_Ok.gameObject.SetActive(false);
        }
    }

    private long GetNeedNumber(long level)
    {
        return (level + 1);
    }

    public void OnClick_Ok()
    {
        Item_Divine currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        SkillDivineConfig config = currentItem.Config;

        User user = GameProcessor.Inst.User;
        long currentLevel = skillData.GetDivineItemLevel(config.Id);

        long total = user.GetBagItemCount(config.ItemId);
        long needCount = GetNeedNumber(currentLevel);

        if (total < needCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料数量不足" + needCount + "个", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = config.ItemId,
            Quantity = needCount
        });

        skillData.AddDivineItemLevel(config.Id);

        this.ShowItem(currentItem);
        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        GameProcessor.Inst.User.EventCenter.Raise(new SkillShowEvent());
    }

    public void OnClick_Restore()
    {
        Btn_Restore.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        if (user.MagicGold.Data <= ConfigHelper.RestoreGold)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足5000兆", ToastType = ToastTypeEnum.Failure });
            return;
        }

        int haveCount = user.GetBagIdleCount(4);
        if (haveCount < 10)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料包裹空格不足10个", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("重生消耗5000兆金币，其他材料全额返回。是否确认？", true,
        () =>
        {
            this.Restore();
        }, () =>
        {

        });
    }

    private void Restore()
    {
        User user = GameProcessor.Inst.User;
        skillData = user.SkillList.Where(m => m.SkillId == SkillId).FirstOrDefault();

        List<Item> newList = new List<Item>();
        foreach (var kv in skillData.DivineData)
        {
            long level = kv.Value.Data;
            if (level > 0)
            {
                SkillDivineConfig config = SkillDivineConfigCategory.Instance.Get(kv.Key);
                int count = (int)MathHelper.GetSequence1(level);

                newList.Add(ItemHelper.BuildMaterial(config.ItemId, count));
                //Debug.Log(kv.Key + " :" + kv.Value);
            }
        }

        skillData.DivineData.Clear();
        user.SubGold(ConfigHelper.RestoreGold);

        //生成新的
        user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = newList });

        this.Show();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
