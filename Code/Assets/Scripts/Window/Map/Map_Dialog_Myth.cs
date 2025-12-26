using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Myth : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    public Button Btn_Close;

    private GameObject ItemPrefab;
    List<Map_Myth_Item> items = new List<Map_Myth_Item>();

    // Start is called before the first frame update
    void Start()
    {

        Btn_Close.onClick.AddListener(OnClick_Close);
        this.Init();
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

        int max = user.MythData.GetMax();

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetMax(max);
        }
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;
        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

        user.MythData.Check();

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Map/Map_Myth_Item");

        List<MythConfig> list = MythConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            if (ac && i >= 10)
            {
            }
            else
            {
                BuildItem(list[i]);
            }
        }

        this.ShowItemMax();
    }

    private void BuildItem(MythConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Map_Myth_Item>();

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
