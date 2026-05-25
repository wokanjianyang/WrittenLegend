using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_World : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    public Button Btn_Close;

    public Text Txt_Des;

    public Toggle toggle_Auto;

    private GameObject ItemPrefab;
    private GameObject ItemPrefab1;
    List<Item_World> items = new List<Item_World>();

    // Start is called before the first frame update
    void Start()
    {

        Btn_Close.onClick.AddListener(OnClick_Close);
        this.Init();
        this.Show();

        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            GameProcessor.Inst.World_Auto = isOn;
        });
    }

    private float doTime = 0;
    private void Update()
    {
    }


    private void Show()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }

        Txt_Des.text = "每周日凌晨0点刷新";
        if (user.WorldData.Check())
        {
            GameProcessor.Inst.SaveData();
            GameProcessor.Inst.SaveNetData();
        }
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;
        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

        if (user.WorldData.Check())
        {
            GameProcessor.Inst.SaveData();
            GameProcessor.Inst.SaveNetData();
        }

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Map/Item_World");
        ItemPrefab1 = Resources.Load<GameObject>("Prefab/Window/Map/Item_World_1");

        long cycle = user.Cycle.Data;

        List<WorldConfig> list = WorldConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Cycle <= cycle).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            if (ac && i > 0)
            {
            }
            else
            {
                BuildItem(list[i]);
            }
        }
    }

    private void BuildItem(WorldConfig config)
    {
        var item = GameObject.Instantiate(config.Id <= 5 ? ItemPrefab : ItemPrefab1);

        var com = item.GetComponent<Item_World>();

        com.SetContent(config);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
