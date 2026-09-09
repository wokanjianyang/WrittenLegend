using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Abyss_Forge : MonoBehaviour
{
    public Text Txt_Name;
    public Text Txt_Fee;
    public Text Txt_Level;

    public Button Btn_Close;
    public Button Btn_Ok;

    public Transform tf_attr;
    private List<StrenthAttrItem> AtrrList;

    private int ConfigId = 0;
    private AbyssConfig Config = null;

    private int MaxLevel = 100;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        AtrrList = tf_attr.GetComponentsInChildren<StrenthAttrItem>(true).ToList();

        Btn_Close.onClick.AddListener(OnClick_Close);
    }

    public void Init(int id)
    {
        this.gameObject.SetActive(true);
        this.ConfigId = id;
        this.Config = AbyssConfigCategory.Instance.Get(id);
        this.Show();
    }

    public void Show()
    {
        this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), Config.Name);

        User user = GameProcessor.Inst.User;
        AbyssData data = user.GetAbyss(ConfigId);
        long currentLevel = data.GetLevel();

        this.Txt_Level.text = currentLevel + "级（满级" + MaxLevel + "级）";

        long nextLevel = currentLevel + 1;
        //Debug.Log("currentLevel show:" + currentLevel);



        if (currentLevel >= MaxLevel)
        {
            this.Txt_Fee.text = "已满级";
            //this.Btn_Ok.gameObject.SetActive(false);
        }
        else
        {
            //Fee
            long fee = nextLevel * nextLevel;

            Txt_Fee.gameObject.SetActive(true);
            Txt_Fee.text = string.Format("下一级需求：<color={0}>{1}</color>", "#FF0000", Config.Name + "（" + data.Total.Data + " /" + fee+ "）");
            //this.Btn_Ok.gameObject.SetActive(true);
        }


        for (int i = 0; i < AtrrList.Count; i++)
        {
            StrenthAttrItem attrItem = AtrrList[i];

            if (i >= Config.AtrIdList.Length)
            {
                attrItem.gameObject.SetActive(false);
            }
            else
            {
                attrItem.gameObject.SetActive(true);

                double attrBase = currentLevel * Config.AtrVueList[i];

                attrItem.SetContent(Config.AtrIdList[i], attrBase, Config.AtrVueList[i]);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
