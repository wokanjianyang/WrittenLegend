using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Com_BossInfoItem : MonoBehaviour
{
    public Text txt_BossName;
    public Text txt_MapName;
    public Text txt_Time;
    //public Text txt_MapName;

    public Button btn_Start;
    public Text txt_Start;

    public MapConfig mapConfig { get; set; }
    private BossConfig bossConfig;

    // Start is called before the first frame update
    void Start()
    {
        btn_Start.gameObject.SetActive(true);
        txt_Time.gameObject.SetActive(false);

        btn_Start.onClick.AddListener(OnClick_NavigateMap);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    RefeshTime();
    //}

    private void OnClick_NavigateMap()
    {
        int ticket = GameProcessor.Inst.EquipCopySetting_Rate ? 5 : 1;

        if (GameProcessor.Inst.User.MagicCopyTikerCount.Data < ticket)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "挑战次数不够了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        var vm = this.GetComponentInParent<ViewMore>();
        vm.SelectMap(mapConfig.Id, ticket);
    }

    public void SetContent(MapConfig mapConfig, BossConfig bossConfig)
    {
        this.mapConfig = mapConfig;
        this.bossConfig = bossConfig;

        txt_MapName.text = mapConfig.Name;
        if (mapConfig.Memo != "")
        {
            txt_MapName.text += "(" + mapConfig.Memo + ")";
        }

        txt_BossName.text = bossConfig.Name;

        if (GameProcessor.Inst.isTimeError || GameProcessor.Inst.isCheckError)
        {
            btn_Start.gameObject.SetActive(false);
            txt_Time.gameObject.SetActive(true);
            txt_Time.text = "99:99:99";
        }
        else
        {
            txt_Start.text = "挑战";
        }

        //RefeshTime();
    }

    //public void SetKillTime(long killTime)
    //{
    //    this.killTime = killTime;

    //    RefeshTime();
    //}

    //private void RefeshTime()
    //{
    //    if (GameProcessor.Inst.isTimeError)
    //    {
    //        txt_Time.text = "99:99:99";
    //        btn_Start.gameObject.SetActive(false);
    //        txt_Time.gameObject.SetActive(true);
    //        return;
    //    }

    //    long dieTime = TimeHelper.ClientNowSeconds() - killTime;

    //    long count = Math.Min(dieTime / (mapConfig.BossInterval * 60), 5);

    //    txt_Start.text = "挑战(" + count + "次)";

    //    if (count > 0)
    //    {
    //        btn_Start.gameObject.SetActive(true);
    //        txt_Time.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        //显示倒计时
    //        long refeshTime = mapConfig.BossInterval * 60 - dieTime;
    //        txt_Time.text = TimeSpan.FromSeconds(refeshTime).ToString(@"hh\:mm\:ss");

    //        btn_Start.gameObject.SetActive(false);
    //        txt_Time.gameObject.SetActive(true);
    //    }
    //}
}
