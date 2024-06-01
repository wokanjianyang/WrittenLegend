using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Ring : MonoBehaviour
{
    public Transform Tran_Item_List;
    public List<Item_Ring> items;

    public Text Txt_Desc;

    public List<StrenthAttrItem> AttrList;

    public Text Txt_Metail;
    public Text Txt_Fee;

    public Button Btn_Ok;
    public Button Btn_Close;
    public Text Txt_OK;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Ok.onClick.AddListener(OnClick_Ok);

        this.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            items[i].toggle.onValueChanged.AddListener((isOn) =>
            {
                ShowItem(item);
            });
        }

        User user = GameProcessor.Inst.User;
        List<RingConfig> configs = RingConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
        for (int i = 0; i < configs.Count; i++)
        {
            RingConfig config = configs[i];
            long level = user.GetRingLevel(config.Id);
            items[i].SetContent(level);
        }

        Item_Ring currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        ShowItem(currentItem);
    }

    private void Init()
    {
        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        User user = GameProcessor.Inst.User;

        List<RingConfig> configs = RingConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            RingConfig config = configs[i];
            Item_Ring box = items[i];

            box.Init(toggleGroup, config);
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void ShowItem(Item_Ring currentItem)
    {
        User user = GameProcessor.Inst.User;

        RingConfig config = currentItem.Config;

        long currentLevel = user.GetRingLevel(config.Id);

        currentItem.SetContent(currentLevel);

        //attr
        for (int i = 0; i < AttrList.Count; i++)
        {
            if (i < config.AttrIdList.Length)
            {
                AttrList[i].gameObject.SetActive(true);
                AttrList[i].SetContent(config.AttrIdList[i], config.AttrValueList[i], config.AttrRiseList[i]);
            }
            else
            {
                AttrList[i].gameObject.SetActive(false);
            }
        }

        long total = user.GetBagItemCount(config.ItemId);
        long needNumber = GetNeedNumber(currentLevel);

        string color = total >= needNumber ? "#FFFF00" : "#FF0000";

        Txt_Metail.text = "消耗" + config.Name + "";
        Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, total, needNumber);

        if (config.SkillId > 0)
        {
            SkillData skillData = new SkillData(config.SkillId, 0);
            SkillPanel sp = new SkillPanel(skillData, null, null, true);

            Txt_Desc.text = sp.SkillData.SkillConfig.Name + "Lv." + currentLevel + " : " + sp.Desc;
        }
        else
        {
            Txt_Desc.text = config.Desc;
        }


        if (total >= needNumber)
        {
            Btn_Ok.gameObject.SetActive(true);
            if (currentLevel <= 0)
            {
                Txt_OK.text = "激活";
            }
            else
            {
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
        Item_Ring currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        RingConfig config = currentItem.Config;

        User user = GameProcessor.Inst.User;
        long currentLevel = user.GetRingLevel(config.Id);

        long total = user.GetBagItemCount(config.ItemId);
        long needCount = GetNeedNumber(currentLevel);

        if (total < needCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料数量不足" + needCount + "个", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Ring,
            ItemId = config.ItemId,
            Quantity = needCount
        });
        user.AddRingLevel(config.Id);

        this.ShowItem(currentItem);
        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
