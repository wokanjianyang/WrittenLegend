using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Spirit_Offline : MonoBehaviour
{
    public Text Txt_Content;

    public Button Btn_Close;
    public Button Btn_Ok;
    public Button Btn_Cancle;


    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Ok.onClick.AddListener(OnOk);
        Btn_Cancle.onClick.AddListener(OnCancle);
    }

    private void OnEnable()
    {
        this.Show();
    }


    public void Show()
    {
        User user = GameProcessor.Inst.User;

        if (user.SpiritOfflineFlag)
        {
            Btn_Cancle.gameObject.SetActive(true);
            Btn_Ok.gameObject.SetActive(false);
        }
        else
        {
            Btn_Cancle.gameObject.SetActive(false);
            Btn_Ok.gameObject.SetActive(true);
        }

        if (user.SpiritOfflineLog != null && user.SpiritOfflineLog.Count > 1)
        {
            int mapId = user.SpiritOfflineLog[1];
            int time = user.SpiritOfflineLog[2];
            int total = user.SpiritOfflineLog[3];

            SpiritCopyConfig config = SpiritCopyConfigCategory.Instance.Get(mapId);

            Txt_Content.text = string.Format("记录离线副本为：{0}，\n通关时间为{1}秒，通关积分为{2}", config.MapName, time, total);
        }
        else
        {
            Btn_Ok.gameObject.SetActive(false);
            Txt_Content.text = "还没有记录通关副本和时间，请先通关副本";
        }

    }



    public void OnOk()
    {
        Btn_Ok.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;
        if (user.SpiritOfflineLog == null || user.SpiritOfflineLog.Count != 3)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先通关副本", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.User.SpiritOfflineFlag = true;
        Btn_Cancle.gameObject.SetActive(true);

    }

    public void OnCancle()
    {
        GameProcessor.Inst.User.SpiritOfflineFlag = false;
        Btn_Cancle.gameObject.SetActive(false);
        Btn_Ok.gameObject.SetActive(true);
    }



    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
