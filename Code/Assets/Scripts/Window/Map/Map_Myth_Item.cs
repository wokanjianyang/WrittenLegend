using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Myth_Item : MonoBehaviour
{
    public Text Txt_Name;
    public Button Btn_Start;
    public Text Txt_Over;

    private MythConfig Config;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_NavigateMap);
    }

    private void OnEnable()
    {
        if (this.Config != null)
        {
            this.Show();
        }
    }


    private void Show()
    {
        User user = GameProcessor.Inst.User;

        if (user.MythData.GetOver(Config.Id))
        {
            Txt_Over.gameObject.SetActive(true);
            Btn_Start.gameObject.SetActive(false);
        }
    }

    private void OnClick_NavigateMap()
    {
        User user = GameProcessor.Inst.User;

        if (user.MythData.GetOver(this.Config.Id))
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已通过，请等下周", ToastType = ToastTypeEnum.Failure });
            return;
        }

        var dialog = this.GetComponentInParent<Map_Dialog_Myth>();
        dialog.gameObject.SetActive(false);

        var vm = this.GetComponentInParent<ViewMore>();
        vm.StartMyth(Config.Id);
    }

    public void SetContent(MythConfig config)
    {
        this.Config = config;
        Txt_Name.text = config.MapName;

        this.Show();
    }

    public void SetMax(int maxId)
    {
        if (Config.Id - 1 <= maxId)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
