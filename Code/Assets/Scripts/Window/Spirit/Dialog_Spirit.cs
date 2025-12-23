using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Spirit : MonoBehaviour
{
    public Button btn_Close;

    public ScrollRect sr_Boss;

    private int Type = 1;

    private List<Item_Spirit> items = new List<Item_Spirit>();

    public Dialog_Spirit_Forge DialogSpiritForge;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);
        this.Init(1);
    }

    public void Refresh()
    {
        foreach (Item_Spirit item in items) {
            item.Show();
        }
    }

    private void Init(int type)
    {
        foreach (var sp in items)
        {
            GameObject.Destroy(sp.gameObject);
        }
        items.Clear();

        List<SpiritConfig> configs = SpiritConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Type == type).ToList();

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

    public void ShowForge(int id)
    {
        DialogSpiritForge.Init(id);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
