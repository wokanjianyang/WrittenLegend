using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Talent_Detail : MonoBehaviour
{

    public Button Btn_Close;

    public Text Txt_Name;
    public Text Txt_Desc;
    public Text Txt_Current;
    public Text Txt_Next;
    public Text Txt_Cost;
    public Text Txt_Require;

    public Button Btn_OK;

    private int Tid = 0;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_OK.onClick.AddListener(OnClick_Ok);
    }

    public void Open(int tid)
    {
        this.gameObject.SetActive(true);

        this.Tid = tid;
        this.Show();
    }

    public void Show()
    {
        TalentConfig config = TalentConfigCategory.Instance.Get(this.Tid);

        User user = GameProcessor.Inst.User;

        long total = user.TalentExp.Data / 10000;
        long use = user.TalentData.Select(m => m.Value.Data).Sum();


        long level = user.GetTalentLevel(this.Tid);
        double attrVal = level * config.AttrValue;

        Txt_Name.text = config.Name;
        Txt_Desc.text = string.Format(config.desc, attrVal);
        Txt_Current.text = "等级：" + level + "/" + config.MaxLevel;
        Txt_Next.text = "升级提高：" + "" + StringHelper.FormatAttrValueText(config.AttrId, attrVal);

        Txt_Cost.text = "需求天赋点：" + config.Fee;
        Txt_Require.text = "前置天赋总等级：" + config.RequireId;

        if (level < config.MaxLevel && config.Fee < (total - use))
        {
            Btn_OK.gameObject.SetActive(true);
        }
        else
        {
            Btn_OK.gameObject.SetActive(false); ;
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClick_Ok()
    {
        TalentConfig config = TalentConfigCategory.Instance.Get(this.Tid);

        User user = GameProcessor.Inst.User;

        long total = user.TalentExp.Data / 10000;
        long use = user.TalentData.Select(m => m.Value.Data).Sum();


        long level = user.GetTalentLevel(this.Tid);

        if (level < config.MaxLevel && config.Fee < (total - use))
        {
            user.AddTalentLevel(Tid);

            this.Show();
        }
    }
}
