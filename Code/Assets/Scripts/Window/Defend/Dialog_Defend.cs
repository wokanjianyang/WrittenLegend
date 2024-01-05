using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Defend : MonoBehaviour, IBattleLife
{
    public Button Btn_Start1;

    public Button Btn_Start2;

    public Button btn_FullScreen;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Start1.onClick.AddListener(() => { this.OnClick_Start(1); });
        Btn_Start2.onClick.AddListener(() => { this.OnClick_Start(2); });

        btn_FullScreen.onClick.AddListener(this.OnClick_Close);
    }

    void Update()
    {

    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenDefendEvent>(this.OnOpenDefend);
    }


    private void OnOpenDefend(OpenDefendEvent e)
    {
        this.gameObject.SetActive(true);
    }

    private void OnClick_Start(int level)
    {
        AppHelper.DefendLevel = level;

        User user = GameProcessor.Inst.User;
        DefendRecord record = user.DefendData.GetCurrentRecord();

        if (record == null)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "û������ս����", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new CloseViewMoreEvent());
        GameProcessor.Inst.EventCenter.Raise(new DefendStartEvent());
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}