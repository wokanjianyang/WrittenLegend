using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Pill : MonoBehaviour, IBattleLife
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Text Txt_Time;

    private int LevelCount = 35; //每个难度多少个
    private int ShowCount = 10; //隐藏的时候显示多少个

    private int MaxLayer = -1;
    private int SelectLayer = -1;

    private int Rate = 5;

    List<Com_BossInfoItem> items = new List<Com_BossInfoItem>();

    private void Awake()
    {
        this.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {

    }

    void Update()
    {

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
      
    }



    private void OnBossInfoEvent(BossInfoEvent e)
    {
        this.gameObject.SetActive(true);
        this.Show();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
