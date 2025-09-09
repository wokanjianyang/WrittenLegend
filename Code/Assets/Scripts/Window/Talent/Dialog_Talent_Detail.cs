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
    public Button Btn_OK_Batch;

    private int Tid = 0;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_OK.onClick.AddListener(OnClick_Ok);
        Btn_OK_Batch.onClick.AddListener(OnClick_Ok_Batch);
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

        long totalPoint = user.TalentExp.Data / 10000;
        long usedPoint = user.TalentPoint;
        long enablePoint = totalPoint - usedPoint;

        long totalLevel = user.TalentData.Select(m => m.Value.Data).Sum();
        long level = user.GetTalentLevel(this.Tid);
        double attrVal = config.GetAttrValue(level);

        Txt_Name.text = config.Name;
        Txt_Desc.text = string.Format(config.desc, StringHelper.FormatNumber(attrVal));
        Txt_Current.text = "等级：" + level + "/" + config.MaxLevel;

        if (level > 0)
        {
            Txt_Next.text = "升级提高：" + "" + config.RiseValue + config.RiseUnit;
        }
        else
        {
            Txt_Next.text = "激活提高：" + "" + config.AttrValue + config.RiseUnit;
        }

        string color = config.Fee <= enablePoint ? "#00FF00" : "#FF0000";
        Txt_Cost.text = string.Format("需求天赋点：<color={0}>{1} /{2}</color>", color, enablePoint, config.Fee);

        color = totalLevel >= config.RequireLevel ? "#00FF00" : "#FF0000";
        Txt_Require.text = string.Format("前置天赋总等级：<color={0}>{1} /{2}</color>", color, totalLevel, config.RequireLevel);

        if (level < config.MaxLevel && config.RequireLevel <= totalLevel && config.Fee <= enablePoint)
        {
            Btn_OK.gameObject.SetActive(true);
            Btn_OK_Batch.gameObject.SetActive(true);
        }
        else
        {
            Btn_OK.gameObject.SetActive(false);
            Btn_OK_Batch.gameObject.SetActive(false);
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClick_Ok()
    {
        Btn_OK.gameObject.SetActive(false);
        Btn_OK_Batch.gameObject.SetActive(false);

        TalentConfig config = TalentConfigCategory.Instance.Get(this.Tid);

        User user = GameProcessor.Inst.User;

        long total = user.TalentExp.Data / 10000;
        long use = user.TalentPoint;


        long level = user.GetTalentLevel(this.Tid);

        if (level < config.MaxLevel && config.Fee <= (total - use))
        {
            user.AddTalentLevel(Tid, config.Fee);

            this.Show();

            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

            Dialog_Talent parent = this.GetComponentInParent<Dialog_Talent>();
            parent.Refresh();
        }
    }

    public void OnClick_Ok_Batch()
    {
        Btn_OK.gameObject.SetActive(false);
        Btn_OK_Batch.gameObject.SetActive(false);

        TalentConfig config = TalentConfigCategory.Instance.Get(this.Tid);

        User user = GameProcessor.Inst.User;

        int level = (int)user.GetTalentLevel(this.Tid);

        long total = user.TalentExp.Data / 10000;

        for (int i = level; i < config.MaxLevel; i++)
        {
            if (config.Fee <= (total - user.TalentPoint))
            {
                user.AddTalentLevel(Tid, config.Fee);
            }
            else
            {
                break;
            }
        }

        this.Show();

        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

        Dialog_Talent parent = this.GetComponentInParent<Dialog_Talent>();
        parent.Refresh();
    }
}
