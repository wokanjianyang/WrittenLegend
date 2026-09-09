using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Abyss : MonoBehaviour
{
    public Button btn_Close;

    public ScrollRect sr_Boss;

    private int Type = 1;

    public Transform tf_tgs;

    private List<Toggle> toggles;

    private List<Item_Spirit> items = new List<Item_Spirit>();

    public Dialog_Spirit_Forge DialogSpiritForge;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        toggles = tf_tgs.GetComponentsInChildren<Toggle>().ToList();

        this.btn_Close.onClick.AddListener(OnClick_Close);
        this.Init();

        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i + 1;


            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                this.ShowPanel(index);
            });

        }

        this.ShowPanel(Type);
    }

    public void Refresh()
    {
        foreach (Item_Spirit item in items)
        {
            item.Show();
        }
    }

    private void Init()
    {
        foreach (var sp in items)
        {
            GameObject.Destroy(sp.gameObject);
        }
        items.Clear();

        List<SpiritConfig> configs = SpiritConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        GameObject ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Spirit/Item_Spirit");
        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Spirit>();

            com.SetContent(configs[i]);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }
    }

    private void ShowPanel(int type)
    {
        this.Type = type;

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Refresh(type);
        }
    }

    public void ShowForge(int id)
    {
        DialogSpiritForge.Init(id);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
