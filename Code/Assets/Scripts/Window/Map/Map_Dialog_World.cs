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

    public Toggle toggle_Auto;

    private GameObject ItemPrefab;
    List<Item_World> items = new List<Item_World>();

    // Start is called before the first frame update
    void Start()
    {

        Btn_Close.onClick.AddListener(OnClick_Close);
        this.Init();

        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            GameProcessor.Inst.World_Auto = isOn;
        });
    }

    private void OnEnable()
    {
        this.Show();
    }   


    private void Show()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }


        for (int i = 0; i < items.Count; i++)
        {
            //items[i].SetMax(max);
        }
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        //user.MythData.Check();

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Map/Item_World");

        List<WorldConfig> list = WorldConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            BuildItem(list[i]);
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
