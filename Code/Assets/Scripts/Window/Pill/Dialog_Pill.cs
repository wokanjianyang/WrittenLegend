using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Pill : MonoBehaviour, IBattleLife
{
    public Text Txt_Fee;
    public Text Txt_Level;
    public Text Txt_Layer;

    public Transform Tf_Attr;

    public Button Btn_Close;
    public Button Btn_Active;

    private List<Item_Pill> ItemList;
    private List<StrenthAttrItem> AtrrList;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Active.onClick.AddListener(OnStrong);

        Show();
    }

    public void OnBattleStart()
    {
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;
        long currentLevel = user.WingData.Data;
        long nextLevel = currentLevel + 1;
        //Debug.Log("currentLevel show:" + currentLevel);

        long MaxLevel = user.GetWingLimit();

        this.Txt_Level.text = "等级:" + currentLevel;
        if (currentLevel > 0)
        {
            this.Btn_Active.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_Active.gameObject.SetActive(true);
        }

        WingConfig currentConfig = WingConfigCategory.Instance.GetByLevel(currentLevel);
        WingConfig nextConfig = WingConfigCategory.Instance.GetByLevel(nextLevel);

        if (nextConfig == null || currentLevel >= MaxLevel)
        {
            this.Txt_Fee.text = "已满级";
        }
        else
        {
            //Fee
            long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Wing_Stone);
            long fee = nextConfig.GetFee(nextLevel);
            string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

            Txt_Fee.gameObject.SetActive(true);
            Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, "需要:" + fee + " 凤凰之羽");

        }

        WingConfig showConfig = nextConfig == null ? currentConfig : nextConfig;

        for (int i = 0; i < AtrrList.Count; i++)
        {
            StrenthAttrItem attrItem = AtrrList[i];

            if (i >= showConfig.AttrIdList.Length)
            {
                attrItem.gameObject.SetActive(false);
            }
            else
            {
                attrItem.gameObject.SetActive(true);
                long attrBase = currentConfig == null ? 0 : currentConfig.GetAttr(i, currentLevel);
                attrItem.SetContent(showConfig.AttrIdList[i], attrBase, showConfig.AttrRiseList[i]);
            }
        }
    }

    public void OnStrong()
    {
        User user = GameProcessor.Inst.User;

        long currentLevel = user.WingData.Data;
        long nextLevel = currentLevel + 1;

        long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Wing_Stone);

        WingConfig nextConfig = WingConfigCategory.Instance.GetByLevel(nextLevel);
        long fee = nextConfig.GetFee(nextLevel);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.WingData.Data = nextLevel;

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Wing_Stone,
            Quantity = fee
        });

        Show();

        GameProcessor.Inst.UpdateInfo();

        //Debug.Log("OnStrong :" + user.WingData.Data);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
