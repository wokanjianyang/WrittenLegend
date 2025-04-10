using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Relic : MonoBehaviour
{
    public Text txt_Fee;
    public Text Txt_Group;
    public Button Btn_Active;

    public Transform Tf_Ring;
    public Transform Tf_Attr;

    private List<Item_Relic> ItemList;
    private List<StrenthAttrItem> AttrList;

    private int Rid = 0;
    private int SelectId = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Active.onClick.AddListener(OnStrong);

        ItemList = Tf_Ring.GetComponentsInChildren<Item_Relic>().ToList();

        AttrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>().ToList();

        foreach (Item_Relic item in ItemList)
        {
            item.AddListener(SelectItem);
        }
    }

    public void Show(int id)
    {
        this.Rid = id;

        this.Init();

        //Debug.Log("show panel :" + id);

        RelicGroupConfig groupConfig = RelicGroupConfigCategory.Instance.Get(Rid);

        //double attrValue = groupConfig

        this.Txt_Group.text = string.Format(groupConfig.Des, 1);
    }

    private void Init()
    {
        Btn_Active.gameObject.SetActive(false);

        List<RelicConfig> list = RelicConfigCategory.Instance.GetListByType(Rid);

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
        //Debug.Log("select item:" + i);

        this.SelectId = id;
        this.Btn_Active.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        int level = user.GetRelicLevel(id);

        RelicConfig config = RelicConfigCategory.Instance.Get(id);

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

        long materialCount = user.GetMaterialCount(config.ItemId);
        string color = materialCount >= fee ? "#FFFF00" : "#FF0000";
        txt_Fee.text = string.Format("<color={0}>{1}</color>（每10级多1个）", color, config.Name + ":" + materialCount + "/ " + fee);

        if (materialCount >= fee)
        {
            this.Btn_Active.gameObject.SetActive(true);
        }
    }


    public void OnStrong()
    {
        this.Btn_Active.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        RelicConfig config = RelicConfigCategory.Instance.Get(this.SelectId);
        long materialCount = user.GetMaterialCount(config.ItemId);

        int level = user.GetRelicLevel(this.SelectId);

        long fee = config.GetFee(level);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = config.ItemId,
            Quantity = fee
        });

        user.AddRelicLevel(this.SelectId);

        GameProcessor.Inst.UpdateInfo();

        Show(this.Rid);
    }
}
