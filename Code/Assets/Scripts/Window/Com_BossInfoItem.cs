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
    public Button btn_MapName;
    public Text txt_Time;
    public Text txt_Statu;

    private MapConfig mapConfig;
    private BossConfig bossConfig;
    private long killTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        txt_Statu.gameObject.SetActive(false);
        txt_Time.gameObject.SetActive(false);

        btn_MapName.onClick.AddListener(OnClick_NavigateMap);
    }

    // Update is called once per frame
    void Update()
    {
        RefeshTime();
    }

    private void OnClick_NavigateMap()
    {
        GameProcessor.Inst.EventCenter.Raise(new ChangeMapEvent() { MapId = mapConfig.Id });
    }

    public void SetContent(MapConfig mapConfig, BossConfig bossConfig, long killTime)
    {
        this.mapConfig = mapConfig;
        this.bossConfig = bossConfig;
        this.killTime = killTime;

        txt_MapName.text = mapConfig.Name;
        txt_BossName.text = bossConfig.Name;

        RefeshTime();
    }

    private void RefeshTime()
    {
        long refeshTime = TimeHelper.ClientNowSeconds() - killTime - mapConfig.BossInterval * 60;

        if (killTime == 0 || refeshTime >= 0)
        {
            txt_Statu.gameObject.SetActive(true);
            txt_Time.gameObject.SetActive(false);
        }
        else
        {
            //œ‘ æµπº∆ ±
            txt_Time.text = TimeSpan.FromSeconds(refeshTime).ToString(@"hh\:mm\:ss");

            txt_Statu.gameObject.SetActive(false);
            txt_Time.gameObject.SetActive(true);
        }
    }
}
