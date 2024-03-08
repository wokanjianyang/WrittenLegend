using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Metal : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Button btn_Close;

    List<Item_Metal> items = new List<Item_Metal>();

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/More/Item_Metal");

        this.Init();
    }

    public void Init()
    {
        List<MetalConfig> configs = MetalConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        User user = GameProcessor.Inst.User;

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            Item_Metal com = item.GetComponentInChildren<Item_Metal>();

            var md = user.MetalData;
            int key = configs[i].Id;

            if (!md.ContainsKey(key))
            {
                md[key] = new Game.Data.MagicData();
            }

            com.SetContent(configs[i], md[key].Data);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        List<MetalConfig> configs = MetalConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        User user = GameProcessor.Inst.User;

        for (int i = 0; i < configs.Count; i++)
        {
            Item_Metal com = items.Where(m => m.Config.Id == configs[i].Id).FirstOrDefault();

            if (com != null)
            {
                var md = user.MetalData;
                int key = configs[i].Id;

                if (!md.ContainsKey(key))
                {
                    md[key] = new Game.Data.MagicData();
                }
                com.SetContent(configs[i], md[key].Data);
            }
        }
    }

    public int Order => (int)ComponentOrder.Dialog;

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
