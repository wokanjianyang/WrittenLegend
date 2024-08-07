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

    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Text Txt_Time;
    public Button Btn_Close;

    List<Map_Pill_Item> items = new List<Map_Pill_Item>();

    // Start is called before the first frame update
    void Start()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        this.Init();
        //GameProcessor.Inst.EventCenter.AddListener<BossInfoEvent>(this.OnBossInfoEvent);
    }

    void OnEnable()
    {
        User user = GameProcessor.Inst.User;
        Txt_Time.text = (int)user.PillTime.Time.Data + "S";
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;
        user.PillTime.Check();

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Pill/Map_Pill_Item");

        List<MonsterPillConfig> list = MonsterPillConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        long cycle = user.Cycle.Data;

        for (int i = 0; i < cycle; i++)
        {
            BuildItem(list[i]);
        }
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
