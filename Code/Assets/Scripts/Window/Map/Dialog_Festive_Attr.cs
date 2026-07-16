using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Festive_Attr : MonoBehaviour
{
    public Text txt_Fee;
    public Text txt_Level;

    public Button Btn_Full;
    public Button Btn_Strong;

    public Transform tf_attr;
    private List<StrenthAttrItem> AttrList;

    private int GroupId = 1;
    private int MaxLevel = 120;

    public Transform Tf_Nav;
    private List<Toggle> toggles;

    public Image Img_Bg;

    private int[] mcList = { 4113, 4114 };

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        AttrList = tf_attr.GetComponentsInChildren<StrenthAttrItem>(true).ToList();
        toggles = Tf_Nav.GetComponentsInChildren<Toggle>().ToList();

        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i + 1;

            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                this.ShowCycle(index);
            });
        }

        Btn_Full.onClick.AddListener(OnClick_Close);
        Btn_Strong.onClick.AddListener(OnStrong);

        //Show();
    }

    private void OnEnable()
    {
        this.Show();
    }

    Color[] cls = new Color[] { Color.white, Color.red };

    private void ShowCycle(int cycle)
    {
        this.GroupId = cycle;

        this.Img_Bg.color = cls[cycle - 1];

        this.Show();
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;
        long currentLevel = user.GetFestiveAttrLevel(GroupId);
        long nextLevel = currentLevel + 1;

        this.txt_Level.text = "等级:" + currentLevel;

        List<FestiveAttrConfig> configs = FestiveAttrConfigCategory.Instance.GetList(GroupId, nextLevel);

        int mcId = mcList[GroupId - 1];
        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(mcId);


        if (currentLevel >= MaxLevel)
        {
            this.Btn_Strong.gameObject.SetActive(false);
            this.txt_Fee.text = "已满级";
        }
        else
        {
            //Fee
            long materialCount = user.GetMaterialCount(mcId);
            long fee = GetFee(nextLevel);

            string color = materialCount >= fee ? "#FFFF00" : "#FF0000";
            txt_Fee.gameObject.SetActive(true);
            txt_Fee.text = string.Format("<color={0}>{3}:{2}/{1}</color>", color, fee, materialCount, itemConfig.Name);

            if (materialCount >= fee)
            {
                this.Btn_Strong.gameObject.SetActive(true);
            }
            else
            {
                this.Btn_Strong.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < AttrList.Count; i++)
        {
            if (i >= configs.Count)
            {
                AttrList[i].gameObject.SetActive(false);
            }
            else
            {
                AttrList[i].gameObject.SetActive(true);

                FestiveAttrConfig config = configs[i];

                double attrValue = currentLevel >= config.StartLevel ? config.AttrValue * currentLevel : 0;

                AttrList[i].SetContent(config.AttrId, attrValue, config.AttrValue);
            }
        }
    }

    private long GetFee(long nextLevel)
    {
        return nextLevel * 5;
    }

    public void OnStrong()
    {
        this.Btn_Strong.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        long currentLevel = user.GetFestiveAttrLevel(GroupId);
        long nextLevel = currentLevel + 1;

        if (currentLevel >= MaxLevel)
        {
            return;
        }

        int mcId = mcList[GroupId - 1];

        long materialCount = user.GetMaterialCount(mcId);

        long fee = GetFee(nextLevel);
        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.SaveFestiveAttrLevel(GroupId);

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = mcId,
            Quantity = fee
        });

        Show();

        GameProcessor.Inst.UpdateInfo();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
