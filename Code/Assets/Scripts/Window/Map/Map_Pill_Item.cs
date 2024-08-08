using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Pill_Item : MonoBehaviour
{
    public Text Txt_Name;
    public Button Btn_Start;

    private MonsterPillConfig Config;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_NavigateMap);
    }


    private void OnClick_NavigateMap()
    {
        User user = GameProcessor.Inst.User;

        if (user.PillTime.Time.Data < 5)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "挑战时间不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        var dialog = this.GetComponentInParent<Map_Dialog_Pill>();
        dialog.gameObject.SetActive(false);

        var vm = this.GetComponentInParent<ViewMore>();
        vm.StartPill(Config.Id);
    }

    public void SetContent(MonsterPillConfig config)
    {
        this.Config = config;
        Txt_Name.text = config.MapName;
    }
}
