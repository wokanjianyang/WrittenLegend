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

    //List<Item_Card> items = new List<Item_Card>();

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item_Card");

        Init();
    }

    private void Init()
    {
        List<CardConfig> configs = CardConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        User user = GameProcessor.Inst.User;

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Card>();

            if (!user.CardData.ContainsKey(configs[i].Id)) {
                user.CardData[configs[i].Id] = new Game.Data.MagicData();
            }

            com.SetContent(configs[i], user.CardData[configs[i].Id].Data);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;
        }
    }

    public int Order => (int)ComponentOrder.Dialog;
    
    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
