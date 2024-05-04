using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class DialogFashion : MonoBehaviour, IBattleLife
{
    public List<Toggle> toggles;
    public List<ItemFashion> items;

    public List<StrenthAttrItem> ItemAttrList;

    public Text Txt_Suit_Name;
    public StrenthAttrItem SuitAttr;

    public Text Txt_Fee;
    public Button Btn_Ok;
    public Button Btn_Close;
    public Text Txt_Ok;

    private int CountMax = 8;

    public int Order => (int)ComponentOrder.Dialog;

    private int CurrentSuit = 0;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Ok.onClick.AddListener(OnClick_Ok);
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

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenFashionDialogEvent>(this.OpenFashionDialog);
        //throw new NotImplementedException();
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

            ItemFashion box = items[i - 1];

            box.Init(i, config);

            int level = (int)fs[i].Data;
            box.SetLevel(level);
        }

        ItemFashion currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();

        ShowItem(currentItem);
    }

    private void ShowItem(ItemFashion currentItem)
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
            Txt_Ok.text = currentLevel > 0 ? "升级" : "激活";
        }

        int suitLevel = (int)fs.Select(m => m.Value.Data).Min();

        string attrName = StringHelper.FormatAttrValueName(suitConfig.AttrId);
        long ab = 0;
        long ar = suitConfig.AttrRise;
        if (suitLevel > 0)
        {
            ab = suitConfig.AttrValue + suitConfig.AttrRise * (suitLevel - 1);
        }
        else
        {
            ar = suitConfig.AttrValue;
        }

        SuitAttr.SetContent(suitConfig.AttrId, ab, ar);

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

        string color = total >= currentLevel + 1 ? "#FFFF00" : "#FF0000";

        Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, currentItem.Config.Name + " * " + (currentLevel + 1), total);

        if (total >= currentLevel + 1)
        {
            Btn_Ok.gameObject.SetActive(true);
        }
        else
        {
            Btn_Ok.gameObject.SetActive(false);
        }
    }

    private void OpenFashionDialog(OpenFashionDialogEvent e)
    {
        this.gameObject.SetActive(true);
    }

    public void OnClick_Ok()
    {
        User user = GameProcessor.Inst.User;

        ItemFashion currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        Dictionary<int, MagicData> fs = user.FashionData[CurrentSuit];

        int currentLevel = (int)fs[currentItem.Part].Data;

        int atLevel = user.GetArtifactValue(ArtifactType.FashionLimit);

        FashionSuitConfig suitConfig = FashionSuitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Id == CurrentSuit).FirstOrDefault();

        if (currentLevel >= suitConfig.MaxLevel + atLevel)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已满级", ToastType = ToastTypeEnum.Failure });
            return;
        }

        FashionConfig config = currentItem.Config;

        long total = user.GetItemMeterialCount(config.ItemId);

        int needCount = currentLevel + 1;

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

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
