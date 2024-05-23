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
    public List<Item_Legacy> items;

    public List<StrenthAttrItem> LayerAttrList;

    public List<StrenthAttrItem> LevelAttrList;

    public Text Txt_Fee;

    public Button Btn_Ok;
    public Button Btn_Close;

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

        List<LegacyConfig> configs = LegacyConfigCategory.Instance.GetRoleList(suitId);

        for (int i = 1; i <= configs.Count; i++)
        {
            LegacyConfig config = configs[i];
            Item_Legacy box = items[i - 1];

            box.Init(config);

            int level = (int)fs[i].Data;
            box.SetLevel(level);
        }

        Item_Legacy currentItem = items.Where(m => m.toggle.isOn).FirstOrDefault();
        ShowItem(currentItem);
    }

    private void ShowItem(Item_Legacy currentItem)
    {
        //套装属性
        FashionSuitConfig suitConfig = FashionSuitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Id == CurrentSuit).FirstOrDefault();

        User user = GameProcessor.Inst.User;

        Dictionary<int, MagicData> fs = user.FashionData[CurrentSuit];

        long currentLevel = user.GetLegacyLevel(currentItem.Config.Id);
        long currentLayer = user.GetLegacyLayer(currentItem.Config.Id);

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


        //单件属性

        long total = user.GetItemMeterialCount(ItemHelper.SpecialId_Legacy_Stone);

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


    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
