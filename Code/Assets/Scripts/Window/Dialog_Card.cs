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

    List<Item_Phantom> items = new List<Item_Phantom>();

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item_Phantom");

        Init();
    }

    private void Init()
    {
        List<PhantomConfig> configs = PhantomConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Phantom>();

            items.Add(com);

            com.SetContent(configs[i], 0);
            com.gameObject.SetActive(false);


            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;
        }
    }

    public int Order => (int)ComponentOrder.Dialog;


    public void UpadateItem()
    {
        User user = GameProcessor.Inst.User;

        Dictionary<int, int> recordList = user.PhantomRecord;

        List<PhantomConfig> configs = PhantomConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (PhantomConfig config in configs)
        {
            recordList.TryGetValue(config.Id, out int lv);
            if (lv == 0)
            {
                lv = 1;
                recordList[config.Id] = lv;
            }

            Item_Phantom com = items.Where(m => m.ConfigId == config.Id).FirstOrDefault();
            if (com != null)
            {
                com.gameObject.SetActive(true);
                com.SetContent(config, lv);
            }
        }
    }
    
    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
