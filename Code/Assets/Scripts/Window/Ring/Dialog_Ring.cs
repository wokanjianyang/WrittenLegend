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

    public Text Txt_Fee;

    public Button Btn_Ok;
    public Button Btn_Close;

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

        long total = user.GetMaterialCount(ItemHelper.SpecialId_Legacy_Stone);
        long needNumber = GetNeedNumber(currentLevel);

        string color = total >= needNumber + 1 ? "#FFFF00" : "#FF0000";

        Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, total, needNumber);
        Txt_Desc.text = config.Desc;

        if (total >= needNumber)
        {
            Btn_Ok.gameObject.SetActive(true);
        }
        else
        {
            Btn_Ok.gameObject.SetActive(false);
        }
    }

    private long GetNeedNumber(long level)
    {
        return (level + 1) * 5;
    }

    public void OnClick_Ok()
    {
        Item_Ring currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        RingConfig config = currentItem.Config;

        User user = GameProcessor.Inst.User;
        long currentLevel = user.GetLegacyLevel(config.Id);

        long total = user.GetMaterialCount(ItemHelper.SpecialId_Legacy_Stone);
        long needCount = GetNeedNumber(currentLevel);

        if (total < needCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料数量不足" + needCount + "个", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Legacy_Stone,
            Quantity = needCount
        });
        user.SaveLegacyLevel(config.Id);

        this.ShowItem(currentItem);
        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
