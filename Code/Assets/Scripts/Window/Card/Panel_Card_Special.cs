using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Card_Special : MonoBehaviour
{
    public Text txt_Fee;
    public Text Txt_Group;
    public Button Btn_Active;

    public Transform Tf_Ring;
    public Transform Tf_Attr;

    private List<Item_Card_Special> ItemList;
    private List<StrenthAttrItem> AttrList;

    private int Rid = 0;
    private int SelectId = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnStrong);

        ItemList = Tf_Ring.GetComponentsInChildren<Item_Card_Special>().ToList();

        AttrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>().ToList();

        foreach (Item_Card_Special item in ItemList)
        {
            item.AddListener(SelectItem);
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        this.Init();

        User user = GameProcessor.Inst.User;

        int groupLevel = user.GetCardSpecialGroupLevel(); //user.GetRelicGroupLevel(Rid);

        double groupValue = groupLevel * 1;

        //double nextValue = groupValue + 1;

        string color = groupValue >= 0 ? "#D8CAB0" : "#4D4D4d";

        string des = string.Format("仙鉴玄心【{0}级】： 低于暗金的图鉴，额外增加的{0}%属性", groupLevel, groupValue);

        this.Txt_Group.text = string.Format("<color={0}>{1}</color>", color, des);
    }

    private void Init()
    {
        Btn_Active.gameObject.SetActive(false);

        List<CardSpecialConfig> list = CardSpecialConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            ItemList[i].SetContent(list[i]);
        }

        if (this.SelectId == 0)
        {
            this.SelectId = list[0].Id;
        }

        SelectItem(this.SelectId);
    }

    private void SelectItem(int id)
    {
        Debug.Log("select card item id:" + id);

        this.SelectId = id;
        this.Btn_Active.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        int level = user.GetCardSpecialLevel(id);

        CardSpecialConfig config = CardSpecialConfigCategory.Instance.Get(id);

        for (int i = 0; i < AttrList.Count; i++)
        {
            if (i >= config.AttrIdList.Length)
            {
                AttrList[i].gameObject.SetActive(false);
            }
            else
            {
                AttrList[i].gameObject.SetActive(true);

                double attrValue = config.GetAttrValue(i, level);
                AttrList[i].SetContent(config.AttrIdList[i], attrValue, config.AttrRiseList[i]);
            }
        }

        int fee = config.GetFee(level);

        long materialCount = user.GetItemMeterialCount(config.ItemId);
        string color = materialCount >= fee ? "#FFFF00" : "#FF0000";
        txt_Fee.text = string.Format("<color={0}>{1}</color>", color, config.Name + ":" + materialCount + "/ " + fee);

        if (materialCount >= fee)
        {
            this.Btn_Active.gameObject.SetActive(true);
        }
    }


    public void OnStrong()
    {
        this.Btn_Active.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        CardSpecialConfig config = CardSpecialConfigCategory.Instance.Get(this.SelectId);
        long materialCount = user.GetItemMeterialCount(config.ItemId);

        int level = user.GetCardSpecialLevel(this.SelectId);

        long fee = config.GetFee(level);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.UseItemMeterialCount(config.ItemId, fee);

        user.SaveCardSpecialLevel(this.SelectId, 1);

        GameProcessor.Inst.UpdateInfo();

        Show();
    }
}
