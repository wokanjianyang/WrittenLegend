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

    public StrenthAttrItem[] AttrList;

    public Text Txt_Fee;

    public Button Btn_Ok;
    public Button Btn_Close;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

        AttrList = this.GetComponentsInChildren<StrenthAttrItem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        string account = GameProcessor.Inst.User.Account;
        if (account.Length > 0)
        {
            Btn_Ok.onClick.AddListener(OnClick_Ok);
        }
    }

    private void OnEnable()
    {
        this.Show();
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;

        long level = user.MagicLevel.Data;
        long cycle = user.Cycle.Data;
        long maxLevel = user.GetMaxLevel();

        Txt_Name.text = ConfigHelper.LayerChinaList[cycle] + "转";

        string color = level >= maxLevel ? "#FFFF00" : "#FF0000";
        Txt_Fee.text = string.Format("<color={0}>{1}</color> /{2}", color, level, maxLevel);

        if (level >= maxLevel && cycle < ConfigHelper.Cycle_Max && user.Account != "")
        {
            Btn_Ok.gameObject.SetActive(true);
        }
        else
        {
            Btn_Ok.gameObject.SetActive(false);
        }

        CycleConfig config = CycleConfigCategory.Instance.GetByCycle(cycle);
        CycleConfig nextConfig = CycleConfigCategory.Instance.GetByCycle(cycle + 1);

        int maxCount = nextConfig != null ? nextConfig.AttrIdList.Length : config.AttrIdList.Length;

        for (int i = 0; i < AttrList.Length; i++)
        {
            if (i < maxCount)
            {
                AttrList[i].gameObject.SetActive(true);

                int attrId = nextConfig != null ? nextConfig.AttrIdList[i] : config.AttrIdList[i];
                long bv = config != null && config.AttrValueList.Length > i ? config.AttrValueList[i] : 0;
                long nv = nextConfig != null ? nextConfig.AttrValueList[i] : bv;

                AttrList[i].SetContent(attrId, bv, nv - bv);
            }
            else
            {
                AttrList[i].gameObject.SetActive(false);
            }
        }

    }

    public void OnClick_Ok()
    {
        Btn_Ok.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        long level = user.MagicLevel.Data;
        long cycleLevel = user.GetMaxLevel();

        if (level < cycleLevel)
        {
            return;
        }

        if (user.Account != "")
        {
            user.Cycle.Data += 1;
        }
        user.MagicLevel.Data = 1;

        user.EventCenter.Raise(new SetPlayerLevelEvent { Cycle = user.Cycle.Data, Level = user.MagicLevel.Data });
        user.EventCenter.Raise(new UserAttrChangeEvent());

        this.Show();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
