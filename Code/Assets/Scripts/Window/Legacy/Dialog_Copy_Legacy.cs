using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Copy_Legacy : MonoBehaviour
{
    public Text Txt_Count;
    public Text Txt_Time;
    public Button btn_Close;

    public List<Item_Copy_Legacy> ItemList;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);

        this.Init();
    }

    void OnEnable()
    {

    }

    void Update()
    {
        RefeshTime();
    }

    private void RefeshTime()
    {
        if (GameProcessor.Inst == null || GameProcessor.Inst.User == null)
        {
            return;
        }

        User user = GameProcessor.Inst.User;

        if (user.LegacyTicketTime == 0)
        {
            user.LegacyTicketTime = TimeHelper.ClientNowSeconds();
        }

        long now = TimeHelper.ClientNowSeconds();
        long dieTime = now - user.LegacyTicketTime;

        int TicketCd = ConfigHelper.LegacyTicketCd;

        if (dieTime >= TicketCd)
        {
            int count = (int)(dieTime / TicketCd);
            user.LegacyTicketTime += count * TicketCd;

            if (count >= ConfigHelper.LegacyTiketMax)  //离线最高可以获取100次
            {
                count = ConfigHelper.LegacyTiketMax;
            }
            if (user.LegacyTikerCount.Data + count > ConfigHelper.LegacyTiketMax) //次数超过200次，时间不能累计
            {
                count = Math.Max(0, (int)(ConfigHelper.LegacyTiketMax - user.LegacyTikerCount.Data));
            }

            user.LegacyTikerCount.Data += count;

            dieTime = now - user.LegacyTicketTime;
        }

        //显示倒计时
        Txt_Count.text = user.LegacyTikerCount.Data + "";
        long refeshTime = TicketCd - dieTime;
        Txt_Time.text = TimeSpan.FromSeconds(refeshTime).ToString(@"hh\:mm\:ss");
    }

    public void Init()
    {
        this.gameObject.SetActive(true);

        List<LegacyMapConfig> configs = LegacyMapConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            ItemList[i].Init(configs[i]);
        }

        this.Show();
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
