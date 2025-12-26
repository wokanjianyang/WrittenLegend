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
        this.doTime += Time.unscaledDeltaTime;

        if (doTime > 0.5)
        {
            this.doTime = 0;
            this.Show();
        }
    }


    private void Show()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }

        long refeshTime = user.WorldData.Ticket - TimeHelper.ClientNowSeconds() + 86400 * 10;

        //Debug.Log("refeshTime:" + refeshTime);

        long day = refeshTime / 86400;
        refeshTime = refeshTime - day * 86400;

        string dayText = (day > 0 ? day + "天" : "");
        if (refeshTime > 0)
        {
            Txt_Des.text = "重置时间倒计时：" + dayText + TimeSpan.FromSeconds(refeshTime).ToString(@"hh\:mm\:ss");
        }
        else
        {
            Txt_Des.text = "刷新中...";
            if (user.WorldData.Check())
            {
                GameProcessor.Inst.SaveData();
                GameProcessor.Inst.SaveNetData();
            }
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
        var item = GameObject.Instantiate(ItemPrefab);
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
