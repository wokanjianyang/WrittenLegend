using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Shengxiao : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    public Button Btn_Close;

    public Toggle toggle_Auto;

    private GameObject ItemPrefab;
    List<Map_Shengxiao_Item> items = new List<Map_Shengxiao_Item>();

    // Start is called before the first frame update
    void Start()
    {

        Btn_Close.onClick.AddListener(OnClick_Close);
        this.Init();

        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            AppHelper.Shengxiao_Auto = isOn;
        });
    }

    private void OnEnable()
    {
        this.ShowItemMax();
    }


    private void ShowItemMax()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }

        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

        ShengxiaoGroup gp = user.GetShengxiaoGroup();

        ShengxiaoGroupItem item = gp.List.Where(m => m.Config.Count == 12).FirstOrDefault();

        int max = item.Count >= item.Config.Count && !ac ? item.Config.Quality - 5 : 0;

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetMax(max + 1);
        }
    }


    private void Init()
    {
        User user = GameProcessor.Inst.User;

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Map/Map_Shengxiao_Item");

        List<ShengxiaoCopyConfig> list = ShengxiaoCopyConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            BuildItem(list[i]);
        }

        this.ShowItemMax();
    }

    private void BuildItem(ShengxiaoCopyConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Map_Shengxiao_Item>();

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
