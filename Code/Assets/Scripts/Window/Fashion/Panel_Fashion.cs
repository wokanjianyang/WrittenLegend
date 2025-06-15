using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Fashion : MonoBehaviour
{
    public Transform Tf_Nav;
    private List<Toggle> toggles;

    public Transform Tf_Items;
    private List<Item_Fashion> items;

    public Transform Tf_Attr;
    private List<StrenthAttrItem> ItemAttrList;

    public Transform Tf_SuitAttr;
    private List<StrenthAttrItem> SuitAttrList;

    public Text Txt_Fee;
    public Button Btn_Ok;

    public Button Btn_Batch;

    private int CountMax = 8;

    public int Order => (int)ComponentOrder.Dialog;

    private int CurrentSuit = 0;

    private void Awake()
    {
        toggles = Tf_Nav.GetComponentsInChildren<Toggle>().ToList();
        items = Tf_Items.GetComponentsInChildren<Item_Fashion>().ToList();
        ItemAttrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>().ToList();
        SuitAttrList = Tf_SuitAttr.GetComponentsInChildren<StrenthAttrItem>().ToList();

        Btn_Ok.onClick.AddListener(OnClick_Ok);
        Btn_Batch.onClick.AddListener(OnClick_Batch);
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

        ShowSuit(1);
    }

    public void Show(int type)
    {
        this.gameObject.SetActive(true);

        if (type == 0)
        {
            for (int i = 0; i < toggles.Count; i++)
            {

                if (i < 6)
                {
                    toggles[i].gameObject.SetActive(true);
                }
                else
                {
                    toggles[i].gameObject.SetActive(false);
                }

            }

            this.ShowSuit(1);
        }
        else if (type == 1)
        {
            for (int i = 0; i < toggles.Count; i++)
            {

                if (i < 6)
                {
                    toggles[i].gameObject.SetActive(false);
                }
                else
                {
                    toggles[i].gameObject.SetActive(true);
                }
            }

            this.ShowSuit(7);
        }
    }

    private void ShowSuit(int suitId)
    {
        this.CurrentSuit = suitId;

        User user = GameProcessor.Inst.User;

        if (!user.FashionData.ContainsKey(suitId))
        {
            Dictionary<int, MagicData> nfs = new Dictionary<int, MagicData>();
            for (int i = 1; i <= CountMax; i++)
            {
                nfs[i] = new MagicData();
            }
            user.FashionData[suitId] = nfs;
        }

        Dictionary<int, MagicData> fs = user.FashionData[suitId];

        List<FashionConfig> configs = FashionConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SuitId == suitId).ToList();

        for (int i = 1; i <= CountMax; i++)
        {
            FashionConfig config = configs.Where(m => m.Part == i).FirstOrDefault();

            Item_Fashion box = items[i - 1];

            box.Init(i, config);

            int level = (int)fs[i].Data;
            box.SetLevel(level);
        }

        Item_Fashion currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();

        ShowItem(currentItem);
    }

    private void ShowItem(Item_Fashion currentItem)
    {
        //套装属性
        FashionSuitConfig suitConfig = FashionSuitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Id == CurrentSuit).FirstOrDefault();

        User user = GameProcessor.Inst.User;

        Dictionary<int, MagicData> fs = user.FashionData[CurrentSuit];

        int currentLevel = (int)fs[currentItem.Part].Data;

        currentItem.SetLevel(currentLevel);

        if (currentLevel >= suitConfig.MaxLevel)
        {
            Btn_Ok.gameObject.SetActive(false);
        }
        else
        {
            Btn_Ok.gameObject.SetActive(true);
        }

        int suitLevel = (int)fs.Select(m => m.Value.Data).Min();

        for (int i = 0; i < SuitAttrList.Count; i++)
        {
            if (i < suitConfig.AttrIdList.Length)
            {
                SuitAttrList[i].gameObject.SetActive(true);

                long attrValue = suitConfig.GetAttrValue(i, suitLevel);
                SuitAttrList[i].SetContent(suitConfig.AttrIdList[i], attrValue, suitConfig.AttrRiseList[i]);
            }
            else
            {
                SuitAttrList[i].gameObject.SetActive(false);
            }
        }

        //单件属性
        FashionConfig config = currentItem.Config;
        for (int i = 0; i < ItemAttrList.Count; i++)
        {
            if (config.AttrIdList.Count() > i)
            {
                long ab1 = 0;
                long ar1 = config.AttrRiseList[i];

                if (currentLevel > 0)
                {
                    ab1 = config.AttrRiseList[i] * (currentLevel);
                }
                else
                {
                    ar1 = config.AttrValueList[i];
                }

                ItemAttrList[i].SetContent(config.AttrIdList[i], ab1, ar1);
                ItemAttrList[i].gameObject.SetActive(true);
            }
            else
            {
                ItemAttrList[i].gameObject.SetActive(false);
            }
        }

        long total = user.GetItemMeterialCount(config.ItemId);
        int needCount = CalNeedCount(currentLevel);

        string color = total >= needCount ? "#FFFF00" : "#FF0000";

        Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, currentItem.Config.Name + " * " + total, needCount);

        if (total >= needCount)
        {
            Btn_Ok.gameObject.SetActive(true);
        }
        else
        {
            Btn_Ok.gameObject.SetActive(false);
        }
    }

    private int CalNeedCount(int currentLevel)
    {
        return Math.Min(currentLevel + 1, 20);
    }

    public void OnClick_Ok()
    {
        User user = GameProcessor.Inst.User;

        Item_Fashion currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        Dictionary<int, MagicData> fs = user.FashionData[CurrentSuit];

        int currentLevel = (int)fs[currentItem.Part].Data;

        int atLevel = user.GetFashionLimit();

        FashionSuitConfig suitConfig = FashionSuitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Id == CurrentSuit).FirstOrDefault();

        if (currentLevel >= suitConfig.MaxLevel + atLevel)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已满级", ToastType = ToastTypeEnum.Failure });
            return;
        }

        FashionConfig config = currentItem.Config;

        long total = user.GetItemMeterialCount(config.ItemId);

        int needCount = CalNeedCount(currentLevel);

        if (total < needCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = config.Name + "数量不足" + needCount + "个", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.UseItemMeterialCount(config.ItemId, needCount);

        fs[currentItem.Part].Data++;

        this.ShowItem(currentItem);
        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
    }

    private void OnClick_Batch()
    {
        User user = GameProcessor.Inst.User;

        foreach (var kv in user.FashionData)
        {
            int suitId = kv.Key;
            foreach (var fv in kv.Value)
            {
                int part = fv.Key;
                int currentLevel = (int)fv.Value.Data;

                FashionConfig config = FashionConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SuitId == suitId && m.Part == part).FirstOrDefault();

                long total = user.GetItemMeterialCount(config.ItemId);

                int needCount = CalNeedCount(currentLevel);

                int atLevel = user.GetFashionLimit();

                if (total >= needCount && currentLevel < 20 + atLevel)
                {
                    //开始升级

                    user.UseItemMeterialCount(config.ItemId, needCount);
                    user.FashionData[suitId][part].Data++;
                }
            }

        }

        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        this.ShowSuit(CurrentSuit);
    }
}
