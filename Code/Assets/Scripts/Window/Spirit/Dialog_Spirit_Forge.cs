using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Spirit_Forge : MonoBehaviour
{
    public Text Txt_Name;
    public Text Txt_Fee;
    public Text Txt_Level;

    public Button Btn_Close;
    public Button Btn_Ok;

    public Transform tf_attr;
    private List<StrenthAttrItem> AtrrList;

    private int ConfigId = 0;
    private SpiritConfig Config = null;

    private int MaxLevel = 50;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        AtrrList = tf_attr.GetComponentsInChildren<StrenthAttrItem>(true).ToList();

        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Ok.onClick.AddListener(OnStrong);

    }

    public void Init(int id)
    {
        this.gameObject.SetActive(true);
        this.ConfigId = id;
        this.Config = SpiritConfigCategory.Instance.Get(id);
        this.Show();
    }

    public void Show()
    {
        this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), Config.Name);

        User user = GameProcessor.Inst.User;
        long currentLevel = user.GetSpiritLevel(ConfigId);

        this.Txt_Level.text = currentLevel + "级（满级" + MaxLevel + "级）";

        long nextLevel = currentLevel + 1;
        //Debug.Log("currentLevel show:" + currentLevel);



        if (currentLevel >= MaxLevel)
        {
            this.Txt_Fee.text = "已满级";
            this.Btn_Ok.gameObject.SetActive(false);
        }
        else
        {
            //Fee
            long materialCount = user.GetItemMeterialCount(this.Config.ItemId);
            long fee = this.GetFee(nextLevel);
            string color = materialCount >= fee ? "#FFFF00" : "#FF0000";

            Txt_Fee.gameObject.SetActive(true);
            Txt_Fee.text = string.Format("<color={0}>{1}</color>", color, Config.Name + "：" + materialCount + " /" + fee);
            this.Btn_Ok.gameObject.SetActive(true);
        }


        for (int i = 0; i < AtrrList.Count; i++)
        {
            StrenthAttrItem attrItem = AtrrList[i];

            if (i >= Config.AttrIdList.Length)
            {
                attrItem.gameObject.SetActive(false);
            }
            else
            {
                attrItem.gameObject.SetActive(true);

                double attrBase = currentLevel * Config.AttrValueList[i];

                attrItem.SetContent(Config.AttrIdList[i], attrBase, Config.AttrValueList[i]);
            }
        }
    }

    private long GetFee(long level)
    {
        if (level <= 100)
        {
            return Math.Min(100 - 5 + level * 5, 500);
        }
        else
        {
            return 500;
        }

    }

    public void OnStrong()
    {
        this.Btn_Ok.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        long currentLevel = user.GetSpiritLevel(ConfigId);
        long nextLevel = currentLevel + 1;

        long materialCount = user.GetItemMeterialCount(this.Config.ItemId);

        long fee = this.GetFee(nextLevel);

        if (materialCount < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的材料", ToastType = ToastTypeEnum.Failure });
            return;
        }

        user.UseItemMeterialCount(this.Config.ItemId, fee);
        user.SaveSpiritLevel(this.ConfigId, 1);

        GameProcessor.Inst.UpdateInfo();

        Show();
    }



    public void OnClick_Close()
    {
        Dialog_Spirit dialog = this.GetComponentInParent<Dialog_Spirit>();
        dialog.Refresh();

        this.gameObject.SetActive(false);
    }
}
