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

    private int MapId;


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
        User user = GameProcessor.Inst.User;
        int Rate = user.GetArtifactValue(ArtifactType.EquipBattleRate) + 5;

        int ticket = GameProcessor.Inst.EquipCopySetting_Rate ? Rate : 1;

        if (GameProcessor.Inst.User.MagicCopyTikerCount.Data < ticket)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "挑战次数不够了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        var vm = this.GetComponentInParent<ViewMore>();
        vm.SelectMap(MapId, ticket);
    }

    public void SetContent(MapConfig mapConfig)
    {
        this.MapId = mapConfig.Id;

        txt_MapName.text = mapConfig.Name;
        if (mapConfig.Memo != "")
        {
            txt_MapName.text += "(" + mapConfig.Memo + ")";
        }

        txt_BossName.text = mapConfig.LevelRequired + "";
    }
}
