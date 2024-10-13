using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Wing : MonoBehaviour, IBattleLife
{
    public Text txt_Fee;
    public Text txt_Level;

    public Button Btn_Full;
    public Button Btn_Active;
    public Button Btn_Strong;

    public List<StrenthAttrItem> AtrrList;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Full.onClick.AddListener(OnClick_Close);
        Btn_Active.onClick.AddListener(OnStrong);
        Btn_Strong.onClick.AddListener(OnStrong);

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

        this.txt_Level.text = "等级:" + currentLevel;
        if (currentLevel > 0)
        {
            this.Btn_Active.gameObject.SetActive(false);
            this.Btn_Strong.gameObject.SetActive(true);
        }
        else
        {
            this.Btn_Active.gameObject.SetActive(true);
            this.Btn_Strong.gameObject.SetActive(false);
        }

        WingConfig currentConfig = WingConfigCategory.Instance.GetByLevel(currentLevel);
        WingConfig nextConfig = WingConfigCategory.Instance.GetByLevel(nextLevel);

        if (nextConfig == null || currentLevel >= MaxLevel)
        {
            this.Btn_Strong.gameObject.SetActive(false);
            this.txt_Fee.text = "已满级";
        }
        else
        {
            //Fee
            long materialCount = user.GetMaterialCount(ItemHelper.SpecialId_Wing_Stone);
            long fee = currentConfig.GetFee(nextLevel);
            string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

            txt_Fee.gameObject.SetActive(true);
            txt_Fee.text = string.Format("<color={0}>{1}</color>", color, "需要:" + fee + " 凤凰之羽");

        }

        WingConfig showConfig = currentConfig;

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

        GameProcessor.Inst.SaveData();
        //Debug.Log("OnStrong :" + user.WingData.Data);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
