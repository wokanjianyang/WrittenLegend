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
    public Text Text_Cost;
    public Text Text_Require;

    public Button Btn_OK;



    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

    }


    public void Show(int tid)
    {
        TalentConfig config = TalentConfigCategory.Instance.Get(tid);

        User user = GameProcessor.Inst.User;

        long level = user.GetTalentLevel(tid);

        Txt_Name.text = config.Name;
        Txt_Desc.text = string.Format(config.desc);
        Txt_Current.text = "当前等级:" + "";
        Txt_Next.text = "下一等级:" + "";

        Text_Cost.text = "需求天赋点:" + config.Fee;
        Text_Require.text = "前置天赋总等级:" + config.RequireId;

        if (level < config.MaxLevel)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false); ;
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
