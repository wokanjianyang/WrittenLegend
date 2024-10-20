using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Legacy : MonoBehaviour, IBattleLife
{
    public List<Toggle> toggles;

    public Transform Tran_Item_List;
    public List<Item_Legacy> items;

    public List<Text> TxtPowerList;

    public List<StrenthAttrItem> LayerAttrList;

    public List<StrenthAttrItem> LevelAttrList;

    public Text Txt_Fee;
    public Text Txt_Fee2;

    public Button Btn_Ok;
    public Button Btn_Layer_Up;

    public Button Btn_Close;

    private int CountMax = 8;

    public int Order => (int)ComponentOrder.Dialog;

    private int CurrentSuit = 0;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Ok.onClick.AddListener(OnClick_Ok);
        Btn_Layer_Up.onClick.AddListener(OnClick_Layer_Up);


        ToggleGroup toggleGroup = Tran_Item_List.GetComponent<ToggleGroup>();

        for (int i = 0; i < items.Count(); i++)
        {
            items[i].Init(toggleGroup);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i + 1;
            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                ShowSuit(index);
            });
        }

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            items[i].toggle.onValueChanged.AddListener((isOn) =>
            {
                ShowItem(item);
            });
        }

        this.ShowSuit(1);
    }

    void OnEnable()
    {
        if (CurrentSuit > 0)
        {
            this.ShowSuit(CurrentSuit);
        }
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenLegacyDialogEvent>(this.OpenDialog);
    }

    private void OpenDialog(OpenLegacyDialogEvent e)
    {
        this.gameObject.SetActive(true);
    }


    private void ShowSuit(int suitId)
    {
        this.CurrentSuit = suitId;

        User user = GameProcessor.Inst.User;

        List<LegacyConfig> configs = LegacyConfigCategory.Instance.GetRoleList(suitId);

        for (int i = 0; i < configs.Count; i++)
        {
            LegacyConfig config = configs[i];
            Item_Legacy box = items[i];

            box.Change(config);

            long layer = user.GetLegacyLayer(config.Id);
            long level = user.GetLegacyLevel(config.Id);

            box.SetContent(layer, level);
        }

        Item_Legacy currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        ShowItem(currentItem);
    }

    private void ShowItem(Item_Legacy currentItem)
    {
        User user = GameProcessor.Inst.User;

        LegacyConfig config = currentItem.Config;

        long currentLevel = user.GetLegacyLevel(config.Id);
        long currentLayer = user.GetLegacyLayer(config.Id);

        currentItem.SetContent(currentLayer, currentLevel);

        //layer
        for (int i = 0; i < LayerAttrList.Count; i++)
        {
            if (i < config.LayerIdList.Length)
            {
                LayerAttrList[i].gameObject.SetActive(true);
                LayerAttrList[i].SetContent(config.LayerIdList[i], config.GetLayerAttr(i, currentLayer), 0);
            }
            else
            {
                LayerAttrList[i].gameObject.SetActive(false);
            }
        }

        //power
        for (int i = 0; i < config.PowerList.Length; i++)
        {
            TxtPowerList[i].text = config.PowerList[i] * currentLayer + "";
        }

        //level
        for (int i = 0; i < LevelAttrList.Count; i++)
        {
            if (i < config.AttrIdList.Length)
            {
                LevelAttrList[i].gameObject.SetActive(true);
                LevelAttrList[i].SetContent(config.AttrIdList[i], config.GetLevelAttr(i, currentLevel), config.AttrRiseList[i]);
            }
            else
            {
                LevelAttrList[i].gameObject.SetActive(false);
            }
        }

        long total = user.GetMaterialCount(ItemHelper.SpecialId_Legacy_Stone);
        long needNumber = GetNeedNumber(currentLevel);

        string color = total >= needNumber + 1 ? "#FFFF00" : "#FF0000";

        Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, needNumber, total);

        if (total >= needNumber)
        {
            Btn_Ok.gameObject.SetActive(true);
        }
        else
        {
            Btn_Ok.gameObject.SetActive(false);
        }

        long point = user.LegacyPoint.Data;
        long needLayerNumber = GetNeedLayerNumber(currentLayer, config);
        color = point >= needLayerNumber + 1 ? "#FFFF00" : "#FF0000";
        Txt_Fee2.text = string.Format("<color={0}>{1}</color> /{2}", color, needLayerNumber, point);

        if (point >= needLayerNumber)
        {
            Btn_Layer_Up.gameObject.SetActive(true);
        }
        else
        {
            Btn_Layer_Up.gameObject.SetActive(false);
        }

    }

    private long GetNeedNumber(long level)
    {
        return (level + 1) * 5;
    }

    private long GetNeedLayerNumber(long layer, LegacyConfig config)
    {
        return 200 * config.RecoveryNubmer;
    }

    public void OnClick_Ok()
    {
        Btn_Ok.gameObject.SetActive(false);
        Btn_Layer_Up.gameObject.SetActive(false);

        Item_Legacy currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        LegacyConfig config = currentItem.Config;

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

        GameProcessor.Inst.SaveData();
    }


    public void OnClick_Layer_Up()
    {
        Btn_Ok.gameObject.SetActive(false);
        Btn_Layer_Up.gameObject.SetActive(false);

        Item_Legacy currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        LegacyConfig config = currentItem.Config;

        User user = GameProcessor.Inst.User;
        long currentLayer = user.GetLegacyLayer(config.Id);

        long total = user.LegacyPoint.Data;
        long needCount = GetNeedLayerNumber(currentLayer, config);

        if (total < needCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "祝福值不够", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.LegacyPoint.Data -= needCount;
        user.SaveLegacyLayer(config.Id, (int)(currentLayer + 1));

        this.ShowItem(currentItem);
        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

        GameProcessor.Inst.SaveData();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
