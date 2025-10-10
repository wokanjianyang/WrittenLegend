using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Pill3 : MonoBehaviour
{
    public Text Txt_Fee;
    public Text Txt_Point_Name;
    public Text Txt_Level_Name;

    public Transform Tf_Attr;
    public Transform Tf_Item;

    public Button Btn_Active;
    public Button Btn_Active_Batch;

    private Item_Pill[] ItemList;
    private StrenthAttrItem[] AtrrList;

    public int Order => (int)ComponentOrder.Dialog;

    private string[] PillNameList = new string[]{"凡胎","启灵","灵台","入微","凝形","千念","入梦","刻印","神火","时光","众生","真灵"
        ,"‌归真","天命","万象","‌归墟","法则","‌因果","终焉","创世"};

    void Awake()
    {
        ItemList = Tf_Item.GetComponentsInChildren<Item_Pill>();
        AtrrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>();

        Btn_Active.onClick.AddListener(OnStrong);
        Btn_Active_Batch.onClick.AddListener(OnBatch);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        Show();
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = user.PillData3.Data;
        //Debug.Log("currentLevel show:" + currentLevel);

        long PillLayer = (currentLevel / 2000);

        long p = currentLevel % 2000;

        long PillIndex = p / 100;
        long PillLevel = (p % 100) / 10 + 1;

        this.Txt_Point_Name.text = PillNameList[PillIndex];
        this.Txt_Level_Name.text = StringHelper.GetChinaNumber(PillLayer) + "阶" + PillLevel + "重";

        //Debug.Log("Pill3Layer:" + PillLayer + " Pill3Level:" + PillLevel);

        PillConfig3 config = PillConfig3Category.Instance.GetByLevel(currentLevel);

        //Fee
        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Pill3);

        long fee = config.GetFee(PillLayer);

        string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

        Txt_Fee.gameObject.SetActive(true);

        if (PillLayer > ConfigHelper.PillMax2)
        {
            Txt_Fee.text = "修炼已满";
            Btn_Active.gameObject.SetActive(false);
        }
        else
        {
            Txt_Fee.text = string.Format("<color={0}>消耗炼神丹:{1}/{2}</color>", color, fee, materialCount);
            Btn_Active.gameObject.SetActive(true);
        }

        Dictionary<int, double> attrDict = PillConfig3Category.Instance.ParseLevel(currentLevel);

        //Debug.Log(JsonConvert.SerializeObject(attrDict));

        int index = 0;
        foreach (var kv in attrDict)
        {
            StrenthAttrItem attrItem = AtrrList[index++];

            double rise = 0;
            if (config.AttrId == kv.Key)
            {
                rise = config.AttrValue;
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

        strong();

        Show();

        user.EventCenter.Raise(new UserAttrChangeEvent());

        //GameProcessor.Inst.SaveData();
    }

    private bool strong()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = user.PillData3.Data;
        long PillLayer = (currentLevel / 2000);

        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Pill3);

        PillConfig3 config = PillConfig3Category.Instance.GetByLevel(currentLevel);
        long fee = config.GetFee(PillLayer);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return false;
        }

        user.PillData3.Data++;

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Pill3,
            Quantity = fee
        });

        return true;
    }

    private void OnBatch()
    {
        User user = GameProcessor.Inst.User;

        for (int i = 0; i < 100; i++)
        {
            if (!strong())
            {
                break;
            }
        }

        Show();

        user.EventCenter.Raise(new UserAttrChangeEvent());
    }
}
