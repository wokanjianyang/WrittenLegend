using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Exchange : MonoBehaviour
{
    public ScrollRect sr_Panel;
    private GameObject ItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        this.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Exchange_Item");

        List<ExchangeConfig> list = ExchangeConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (var config in list)
        {
            var Item = GameObject.Instantiate(ItemPrefab);
            Item.transform.SetParent(sr_Panel.content);
            Item.gameObject.SetActive(true);

            Item_Exchange sc = Item.GetComponent<Item_Exchange>();
            sc.SetData(config);
        }
    }
}

