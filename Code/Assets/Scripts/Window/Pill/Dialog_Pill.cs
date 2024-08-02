using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Pill : MonoBehaviour
{
    public Text Txt_Fee;
    public Text Txt_Point_Name;
    public Text Txt_Level_Name;

    public Transform Tf_Attr;
    public Transform Tf_Item;

    public Button Btn_Close;
    public Button Btn_Active;

    private Item_Pill[] ItemList;
    private StrenthAttrItem[] AtrrList;

    public int Order => (int)ComponentOrder.Dialog;

    private string[] PillNameList = new string[]{"手太阴","手阳明","足阳明","足太阴","手少阴","手太阳","足太阳","足少阴","手厥阴","手少阳","足少阳","足厥阴"
        ,"任脉","督脉","冲脉","带脉","阴跷脉","阳跷脉","阴维脉","阳维脉"};

    void Awake()
    {
        ItemList = Tf_Item.GetComponentsInChildren<Item_Pill>();
        AtrrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>();

        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Active.onClick.AddListener(OnStrong);
    }

    // Start is called before the first frame update
    void Start()
    {
        Show();
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = user.PillData.Data;
        //Debug.Log("currentLevel show:" + currentLevel);

        long PillLayer = (currentLevel / 2000);

        long p = currentLevel % 2000;

        long PillIndex = p / 100;
        long PillLevel = (p % 100) / 10 + 1;

        this.Txt_Point_Name.text = PillNameList[PillIndex + 1];
        this.Txt_Level_Name.text = ConfigHelper.LayerChinaList[PillLayer] + "阶" + PillLevel + "重";

        PillConfig config = PillConfigCategory.Instance.GetByLevel(currentLevel);

        //Fee
        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Pill);

        long fee = config.FeeRise * (PillLayer + 1);

        string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

        Txt_Fee.gameObject.SetActive(true);
        Txt_Fee.text = string.Format("<color={0}>消耗淬体丹:{1}/{2}</color>", color, fee, materialCount);

        Dictionary<int, long> attrDict = PillConfigCategory.Instance.ParseLevel(currentLevel);

        //Debug.Log(JsonConvert.SerializeObject(attrDict));

        int index = 0;
        foreach (var kv in attrDict)
        {
            StrenthAttrItem attrItem = AtrrList[index++];

            long rise = 0;
            if (config.AttrId == kv.Key)
            {
                rise = config.AttrValue * (PillLayer + 1);
            }

            attrItem.SetContent(kv.Key, kv.Value, rise);
        }

        long itemIndex = currentLevel % 10;
        for (int i = 0; i < ItemList.Length; i++)
        {
            if (i < itemIndex)
            {
                ItemList[i].Active(true);
            }
            else
            {
                ItemList[i].Active(false);
            }
        }
    }

    public void OnStrong()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = user.PillData.Data;
        long PillLayer = (currentLevel / 2000);

        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Pill);

        PillConfig config = PillConfigCategory.Instance.GetByLevel(currentLevel);
        long fee = config.FeeRise * (PillLayer + 1);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.PillData.Data++;

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Pill,
            Quantity = fee
        });

        Show();

        user.EventCenter.Raise(new UserAttrChangeEvent());
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
