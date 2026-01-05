using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Cycle : MonoBehaviour
{
    public Text Txt_Name;
    public Text Txt_Desc;

    public Toggle toggle_Type1;
    public Toggle toggle_Type2;
    public Toggle toggle_Type3;
    public Toggle toggle_Type4;

    private StrenthAttrItem[] AttrList;

    public Text Txt_Fee;

    public Button Btn_Ok;
    public Button Btn_Close;
    public Text Txt_Ok;

    private string[] BtnName = { "轮回", "练气", "修仙", "成圣" };

    public int Order => (int)ComponentOrder.Dialog;

    private int Type = 0;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

        toggle_Type1.onValueChanged.AddListener((isOn) =>
        {
            this.Show(0);
        });

        toggle_Type2.onValueChanged.AddListener((isOn) =>
        {
            this.Show(1);
        });

        toggle_Type3.onValueChanged.AddListener((isOn) =>
        {
            this.Show(2);
        });
        toggle_Type4.onValueChanged.AddListener((isOn) =>
        {
            this.Show(3);
        });

        AttrList = this.GetComponentsInChildren<StrenthAttrItem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        User user = GameProcessor.Inst.User;

        string account = user.Account;
        long day = (TimeHelper.ClientNowSeconds() - user.First_Create_Time) / 86400 + 1;

        if (account.Length > 0 || user.GetLimitId() >= 1030)
        {
            Btn_Ok.onClick.AddListener(OnClick_Ok);
        }

        if (GameProcessor.Inst.User.Cycle.Data < 10)
        {
            toggle_Type2.gameObject.SetActive(false);
        }

        if (GameProcessor.Inst.User.Cycle.Data < 20)
        {
            toggle_Type3.gameObject.SetActive(false);
        }
        if (GameProcessor.Inst.User.Cycle.Data < 30)
        {
            toggle_Type4.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        this.Show(this.Type);
    }

    private void Show(int type)
    {
        this.Type = type;
        this.Txt_Ok.text = BtnName[type];

        User user = GameProcessor.Inst.User;

        long cycle = user.Cycle.Data;
        long maxCycle = (type + 1) * 10;

        if (cycle >= maxCycle)
        {
            CycleConfig maxConfig = CycleConfigCategory.Instance.GetByCycle(type, maxCycle);
            Txt_Name.text = ConfigHelper.CycleList[maxCycle];
            Txt_Fee.text = "已满";
            Btn_Ok.gameObject.SetActive(false);

            for (int i = 0; i < AttrList.Length; i++)
            {
                if (i < maxConfig.AttrIdList.Length)
                {
                    AttrList[i].gameObject.SetActive(true);

                    int attrId = maxConfig.AttrIdList[i];
                    long bv = maxConfig.AttrValueList[i];

                    AttrList[i].SetContent(attrId, bv, 0);
                }
                else
                {
                    AttrList[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            CycleConfig currentConfig = CycleConfigCategory.Instance.GetByCycle(type, cycle);
            CycleConfig nextConfig = CycleConfigCategory.Instance.GetByCycle(type, cycle + 1);

            long level = user.MagicLevel.Data;

            Txt_Name.text = ConfigHelper.CycleList[cycle];
            long RequireLevel = user.GetMaxLevel();

            string color = level >= RequireLevel ? "#FFFF00" : "#FF0000";
            Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, level, RequireLevel);

            if (level >= RequireLevel && cycle < ConfigHelper.Cycle_Max)
            {
                Btn_Ok.gameObject.SetActive(true);
            }
            else
            {
                Btn_Ok.gameObject.SetActive(false);
            }

            int maxCount = nextConfig != null ? nextConfig.AttrIdList.Length : currentConfig.AttrIdList.Length;

            for (int i = 0; i < AttrList.Length; i++)
            {
                if (i < maxCount)
                {
                    int attrId = nextConfig != null ? nextConfig.AttrIdList[i] : currentConfig.AttrIdList[i];
                    long bv = currentConfig != null && currentConfig.AttrValueList.Length > i ? currentConfig.AttrValueList[i] : 0;
                    long nv = nextConfig != null ? nextConfig.AttrValueList[i] : bv;

                    AttrList[i].SetContent(attrId, bv, nv - bv);
                    AttrList[i].gameObject.SetActive(true);
                }
                else
                {
                    AttrList[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnClick_Ok()
    {
        Btn_Ok.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;
        long RequireLevel = user.GetMaxLevel();

        long level = user.MagicLevel.Data;
        long cycle = user.Cycle.Data;
        CycleConfig nextConfig = CycleConfigCategory.Instance.GetByCycle(this.Type, cycle + 1);

        if (level < RequireLevel)
        {
            return;
        }

        user.Cycle.Data += 1;
        user.MagicLevel.Data = 1;

        user.EventCenter.Raise(new SetPlayerLevelEvent { Cycle = user.Cycle.Data, Level = user.MagicLevel.Data });
        user.EventCenter.Raise(new UserAttrChangeEvent());

        this.Show(this.Type);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
