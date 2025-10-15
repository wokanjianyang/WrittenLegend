using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Pill : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;

    public Text Txt_Desc;

    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Text Txt_Time;
    public Button Btn_Close;

    List<Map_Pill_Item> items = new List<Map_Pill_Item>();

    private string[] desc = new string[] { "此副本怪物，拥有低攻击，高刷新频率，高防御，高减伤，高抗暴，高回血，高生命，固定掉落，享受连爆，每次轮回解锁一个新难度，每次进入扣除3S，" +
        "\n累计最大时长为6000S，达到不再恢复时间。"
        , "10转开启炼气挑战，挑战时间全部共享，每次固定刷10个怪，全部打完扣除600S时间并且获得奖励"
        , "20转开启炼神挑战，挑战时间全部共享，每次固定刷10个怪，全部打完扣除600S时间并且获得奖励" };

    // Start is called before the first frame update
    void Start()
    {
        toggle1.onValueChanged.AddListener((isOn) =>
        {
            this.ShowPanel(1);
        });

        toggle2.onValueChanged.AddListener((isOn) =>
        {
            this.ShowPanel(2);
        });

        toggle3.onValueChanged.AddListener((isOn) =>
        {
            this.ShowPanel(3);
        });

        Btn_Close.onClick.AddListener(OnClick_Close);
        this.Init(1);

        User user = GameProcessor.Inst.User;
        if (user.Cycle.Data < 10)
        {
            toggle2.gameObject.SetActive(false);
        }

        if (user.Cycle.Data < 20)
        {
            toggle3.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        User user = GameProcessor.Inst.User;
        Txt_Time.text = (int)user.PillTime.Time.Data + "S";
    }

    private void Init(int index)
    {
        Debug.Log("init panel:" + index);

        User user = GameProcessor.Inst.User;
        user.PillTime.Check(user.Cycle.Data);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Pill/Map_Pill_Item");

        long cycle = user.Cycle.Data;
        List<MonsterPillConfig> list = MonsterPillConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Type == index && m.RequireCycle <= cycle).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            BuildItem(list[i]);
        }
    }

    private void ShowPanel(int index)
    {
        Txt_Desc.text = desc[index - 1];

        foreach (Map_Pill_Item sp in items)
        {
            GameObject.Destroy(sp.gameObject);
        }

        items.Clear();

        this.Init(index);

    }

    private void BuildItem(MonsterPillConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Map_Pill_Item>();

        com.SetContent(config);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    public void OnClick_Close()
    {
        Debug.Log("close pill dialog");
        this.gameObject.SetActive(false);
    }
}
