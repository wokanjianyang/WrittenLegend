using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Compound : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_Left;
    public ScrollRect sr_Right;

    private Dictionary<string, List<CompositeConfig>> allCompositeDatas = new Dictionary<string, List<CompositeConfig>>();

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Start()
    {
        this.InitComposite();
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ChangeCompositeTypeEvent>(this.OnChangeCompositeTypeEvent);
    }

    private void InitComposite()
    {
        var allDatas = CompositeConfigCategory.Instance.GetAll();
        foreach (var kvp in allDatas)
        {
            this.allCompositeDatas.TryGetValue(kvp.Value.Type, out var list);
            if (list == null)
            {
                list = new List<CompositeConfig>();
            }

            list.Add(kvp.Value);

            this.allCompositeDatas[kvp.Value.Type] = list;
        }

        var menuItemPrefab = sr_Left.content.GetChild(0);
        menuItemPrefab.gameObject.SetActive(false);
        foreach (var kvp in this.allCompositeDatas)
        {
            var menuItem = GameObject.Instantiate(menuItemPrefab.gameObject);
            menuItem.transform.SetParent(sr_Left.content);
            menuItem.transform.localScale = Vector3.one;
            menuItem.gameObject.SetActive(true);

            var com = menuItem.GetComponent<Com_CompositeMenu>();
            com.SetData(kvp.Key);
        }

        var firstCompositeList = this.allCompositeDatas.First().Value;

        var compositeItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Composite");
        foreach (var config in firstCompositeList)
        {
            var compositeItem = GameObject.Instantiate(compositeItemPrefab);
            compositeItem.transform.SetParent(sr_Right.content);
            compositeItem.transform.localScale = Vector3.one;
            compositeItem.gameObject.SetActive(true);

            var com = compositeItem.GetComponent<Item_Composite>();
            com.SetData(config);
        }
    }

    internal void Show(bool isOn)
    {
        this.gameObject.SetActive(isOn);
    }

    private void OnChangeCompositeTypeEvent(ChangeCompositeTypeEvent e)
    {
        var compositeList = this.allCompositeDatas[e.CompositeType];

        var compositeItemPrefab = sr_Right.content.GetChild(0);
        compositeItemPrefab.gameObject.SetActive(false);

        var total = sr_Right.content.childCount - 1;
        var max = Mathf.Max(total, compositeList.Count);
        for (var i = 0; i < max; i++)
        {
            if (i < compositeList.Count)
            {
                var config = compositeList[i];
                Item_Composite com = null;
                if (i < sr_Right.content.childCount - 1)
                {
                    com = sr_Right.content.GetChild(i + 1).GetComponent<Item_Composite>();
                    com.gameObject.SetActive(true);
                }
                else
                {
                    var compositeItem = GameObject.Instantiate(compositeItemPrefab);
                    compositeItem.transform.SetParent(sr_Right.content);
                    compositeItem.gameObject.SetActive(true);

                    com = compositeItem.GetComponent<Item_Composite>();
                }
                com.SetData(config);
            }
            else
            {
                sr_Right.content.GetChild(i + 1).gameObject.SetActive(false);
            }
        }

        //sr_Right.horizontalNormalizedPosition = 0;
    }

}

