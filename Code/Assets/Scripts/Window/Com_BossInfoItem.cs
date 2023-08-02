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

    public MapConfig mapConfig { get; set; }
    private BossConfig bossConfig;
    private long killTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        btn_Start.gameObject.SetActive(false);
        txt_Time.gameObject.SetActive(false);

        btn_Start.onClick.AddListener(OnClick_NavigateMap);
    }

    // Update is called once per frame
    void Update()
    {
        RefeshTime();
    }

    private void OnClick_NavigateMap()
    {
        var vm = this.GetComponentInParent<ViewMore>();
        vm.SelectMap(mapConfig.Id);
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

    public void SetKillTime(long killTime)
    {
        this.killTime = killTime;

        RefeshTime();
    }

    private void RefeshTime()
    {
        if (GameProcessor.Inst.isTimeError)
        {
            txt_Time.text = "99:99:99";
            btn_Start.gameObject.SetActive(false);
            txt_Time.gameObject.SetActive(true);
            return;
        }

        long refeshTime = TimeHelper.ClientNowSeconds() - killTime - mapConfig.BossInterval * 60;

        if (killTime == 0 || refeshTime >= 0)
        {
            btn_Start.gameObject.SetActive(true);
            txt_Time.gameObject.SetActive(false);
        }
        else
        {
            //œ‘ æµπº∆ ±
            txt_Time.text = TimeSpan.FromSeconds(refeshTime).ToString(@"hh\:mm\:ss");

            btn_Start.gameObject.SetActive(false);
            txt_Time.gameObject.SetActive(true);
        }
    }
}
