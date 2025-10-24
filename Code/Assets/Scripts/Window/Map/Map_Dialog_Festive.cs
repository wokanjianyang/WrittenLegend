using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Festive : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    public Button Btn_Close;

    public Text Txt_Count;

    private GameObject ItemPrefab;
    List<Map_Festive_Item> items = new List<Map_Festive_Item>();

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

        int maxId = user.FestiveMapData.Record;

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetMax(maxId);
        }

        ShowCount();
    }

    public void ShowCount() {
        User user = GameProcessor.Inst.User;
        Txt_Count.text = "Ê£ÓàÌôÕ½´ÎÊý£º" + user.FestiveMapData.Number.Data;
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        user.FestiveMapData.Check();

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Map/Map_Festive_Item");

        List<FestiveCopyConfig> list = FestiveCopyConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            BuildItem(list[i]);
        }

        this.ShowItemMax();
    }

    private void BuildItem(FestiveCopyConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Map_Festive_Item>();

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
