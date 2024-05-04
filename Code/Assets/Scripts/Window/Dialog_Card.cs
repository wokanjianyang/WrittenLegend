using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Card : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Button btn_Close;

    private int SelectStage = 0;
    public List<Toggle> toggleStageList = new List<Toggle>();

    private List<Item_Card> items = new List<Item_Card>();

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Card");

        for (int i = 0; i < toggleStageList.Count; i++)
        {
            int index = i;
            toggleStageList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangePanel(index);
            });
        }

        this.Init();
    }

    public void Init()
    {
        this.gameObject.SetActive(true);

        List<CardConfig> configs = CardConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Card>();

            com.SetContent(configs[i]);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }

        this.Show();
    }

    public int Order => (int)ComponentOrder.Dialog;

    private void ChangePanel(int index)
    {
        this.SelectStage = index;
        this.Show();
    }

    private void Show()
    {
        this.gameObject.SetActive(true);

        for (int i = 0; i < items.Count; i++)
        {
            if (this.SelectStage == items[i].Config.Stage)
            {
                items[i].gameObject.SetActive(true);
            }
            else
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
