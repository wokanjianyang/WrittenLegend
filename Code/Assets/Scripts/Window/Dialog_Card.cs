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
    public Button Btn_Batch;

    private int SelectStage = 0;
    public List<Toggle> toggleStageList = new List<Toggle>();

    private List<Item_Card> items = new List<Item_Card>();

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);
        this.Btn_Batch.onClick.AddListener(OnClick_Batch);

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
        User user = GameProcessor.Inst.User;
        if (user.Cycle.Data >= 1)
        {
            Btn_Batch.gameObject.SetActive(true);
        }
        else
        {
            Btn_Batch.gameObject.SetActive(false);
        }

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
                items[i].Show();
            }
            else
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnClick_Batch()
    {
        User user = GameProcessor.Inst.User;

        foreach (var cardItem in user.CardData)
        {
            int cardId = cardItem.Key;
            long cardLevel = cardItem.Value.Data;

            CardConfig config = CardConfigCategory.Instance.Get(cardId);
            int itemId = config.RiseId;

            long total = user.GetItemMeterialCount(itemId);

            long upLevel = config.CalUpLevel(cardLevel, total, out long useNumber);

            if (upLevel > 0)
            {
                user.UseItemMeterialCount(itemId, useNumber);
                user.SaveCardLevel(cardId, upLevel);

                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = config.Name + "使用" + useNumber + "个材料成功提升" + upLevel + "级", ToastType = ToastTypeEnum.Success });
            }
        }

        this.Show();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
