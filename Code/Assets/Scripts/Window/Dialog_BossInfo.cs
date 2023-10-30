using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_BossInfo : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Text txt_boss_count;
    public Text txt_boss_time;

    public Toggle toggle_Rate;
    public Toggle toggle_Auto;
    public Toggle toggle_Hide;

    List<Com_BossInfoItem> items = new List<Com_BossInfoItem>();

    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);

        //ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item_BossInfo");

        toggle_Rate.onValueChanged.AddListener((isOn) =>
        {
            GameProcessor.Inst.EquipCopySetting_Rate = isOn;
        });

        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            GameProcessor.Inst.EquipCopySetting_Auto = isOn;
        });

        toggle_Hide.onValueChanged.AddListener((isOn) =>
        {
            this.ShowItem(isOn);
        });
    }

    void Update()
    {
        RefeshTime();
    }

    public void OnBattleStart()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item_BossInfo");
        GameProcessor.Inst.EventCenter.AddListener<BossInfoEvent>(this.OnBossInfoEvent);
    }

    public int Order => (int)ComponentOrder.Dialog;

    private void OnBossInfoEvent(BossInfoEvent e)
    {
        this.gameObject.SetActive(true);

        this.UpadateItem();

        this.ShowItem(toggle_Hide.isOn);
    }

    private void BuildItem(int MapId)
    {
        MapConfig mapConfig = MapConfigCategory.Instance.Get(MapId);
        BossConfig bossConfig = BossConfigCategory.Instance.Get(mapConfig.BoosId);

        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Com_BossInfoItem>();

        com.SetContent(mapConfig, bossConfig);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    public void UpadateItem()
    {
        User user = GameProcessor.Inst.User;

        List<int> list = MapConfigCategory.Instance.GetAll().Select(m=>m.Value.Id).ToList();

        foreach (int MapId in list)
        {
            if (MapId <= user.MapId)
            {
                var item = items.Where(m => m.mapConfig.Id == MapId).FirstOrDefault();
                if (item == null)
                {
                    BuildItem(MapId);
                }
            }
        }
    }

    private void RefeshTime()
    {
        if (GameProcessor.Inst.isTimeError || GameProcessor.Inst.isCheckError)
        {
            txt_boss_count.text = "-99";
            txt_boss_time.text = "99:99:99";
            return;
        }

        User user = GameProcessor.Inst.User;

        if (user.CopyTicketTime == 0)
        {
            user.CopyTicketTime = TimeHelper.ClientNowSeconds();
        }

        long now = TimeHelper.ClientNowSeconds();
        long dieTime = now - user.CopyTicketTime;


        if (dieTime >= ConfigHelper.CopyTicketCd)
        {
            int count = (int)(dieTime / ConfigHelper.CopyTicketCd);
            user.CopyTicketTime += count * ConfigHelper.CopyTicketCd;

            if (count >= ConfigHelper.CopyTicketFirstCount)  //离线最高可以获取100次
            {
                count = ConfigHelper.CopyTicketFirstCount;
            }
            if (user.MagicCopyTikerCount.Data + count > ConfigHelper.CopyTicketMax) //次数超过200次，时间不能累计
            {
                count = Math.Max(0, (int)(ConfigHelper.CopyTicketMax - user.MagicCopyTikerCount.Data));
            }

            user.MagicCopyTikerCount.Data += count;

            dieTime = now - user.CopyTicketTime;
        }

        //显示倒计时
        txt_boss_count.text = user.MagicCopyTikerCount.Data + "";
        long refeshTime = ConfigHelper.CopyTicketCd - dieTime;
        txt_boss_time.text = TimeSpan.FromSeconds(refeshTime).ToString(@"hh\:mm\:ss");
    }

    private void ShowItem(bool hide)
    {
        int hideCount = items.Count - 10;

        for (int i = 0; i < items.Count; i++)
        {
            if (i < hideCount)
            {
                items[i].gameObject.SetActive(!hide);
            }
            else {
                items[i].gameObject.SetActive(true);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
