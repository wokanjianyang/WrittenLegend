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

    public List<Toggle> tgLevelList;
    private int LevelCount = 35; //每个难度多少个
    private int ShowCount = 10; //隐藏的时候显示多少个

    private int MaxLayer = -1;
    private int SelectLayer = -1;

    List<Com_BossInfoItem> items = new List<Com_BossInfoItem>();

    private void Awake()
    {
        this.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
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
            this.Show();
        });

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            int index = i;
            tgLevelList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangeLevel(index);
            });
        }
    }

    void OnEnable()
    {
        this.toggle_Auto.isOn = GameProcessor.Inst.EquipCopySetting_Auto;
    }

    void Update()
    {
        RefeshTime();
    }

    public void OnBattleStart()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_BossInfo");
        GameProcessor.Inst.EventCenter.AddListener<BossInfoEvent>(this.OnBossInfoEvent);
    }

    private void Init()
    {
        List<MapConfig> list = MapConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (MapConfig config in list)
        {
            BuildItem(config);
        }
    }
    private void BuildItem(MapConfig config)
    {
        BossConfig bossConfig = BossConfigCategory.Instance.Get(config.BoosId);

        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Com_BossInfoItem>();

        com.SetContent(config, bossConfig);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }


    private void ChangeLevel(int layer)
    {
        this.SelectLayer = layer;
        this.Show();
    }

    private void Show()
    {
        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }

        int MapId = GameProcessor.Inst.User.MapId;

        if (this.MaxLayer < 0)
        {
            this.MaxLayer = (MapId - ConfigHelper.MapStartId) / 35;
            tgLevelList[MaxLayer].isOn = true;
            this.SelectLayer = this.MaxLayer;
        }

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            if (i <= MaxLayer)
            {
                tgLevelList[i].gameObject.SetActive(true);
            }
            else
            {
                tgLevelList[i].gameObject.SetActive(false);
            }
        }

        int count = MapConfigCategory.Instance.GetAll().Where(m => m.Value.Id <= MapId).Count();

        int startIndex = this.SelectLayer * LevelCount;
        int endIndex = startIndex + Math.Min(LevelCount, count - startIndex) - 1;

        int j = 0;
        for (int i = endIndex; i >= startIndex; i--)
        {
            if (j < ShowCount)
            {
                items[i].gameObject.SetActive(true);
            }
            else
            {
                items[i].gameObject.SetActive(!toggle_Hide.isOn);
            }
            j++;
        }
    }

    public int Order => (int)ComponentOrder.Dialog;

    private void OnBossInfoEvent(BossInfoEvent e)
    {
        this.gameObject.SetActive(true);
        this.Show();
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


        int CopyTicketCd = ConfigHelper.CopyTicketCd - user.GetArtifactValue(ArtifactType.EquipTicketCd);
        if (user.IsDz())
        {
            CopyTicketCd = CopyTicketCd / 5;
        }
        CopyTicketCd = Math.Max(CopyTicketCd, ConfigHelper.CopyTicketCdMin);

        if (dieTime >= CopyTicketCd)
        {
            int count = (int)(dieTime / CopyTicketCd);
            user.CopyTicketTime += count * CopyTicketCd;

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
        long refeshTime = CopyTicketCd - dieTime;
        txt_boss_time.text = TimeSpan.FromSeconds(refeshTime).ToString(@"hh\:mm\:ss");
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
