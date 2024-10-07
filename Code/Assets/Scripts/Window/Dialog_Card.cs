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

    public Toggle toggle_Skip;

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
        Btn_Batch.gameObject.SetActive(true);

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

        long totalUp = 0;

        foreach (var cardItem in user.CardData)
        {
            int cardId = cardItem.Key;
            long cardLevel = cardItem.Value.Data;

            if (cardId == 1999998 && toggle_Skip.isOn)
            {
                continue;
            }

            CardConfig config = CardConfigCategory.Instance.Get(cardId);
            int itemId = config.RiseId;

            long limitLevel = user.GetCardLimit(config);

            long total = user.GetItemMeterialCount(itemId);

            long upLevel = config.CalUpLevel(cardLevel, total, limitLevel, out long useNumber);

            if (upLevel > 0)
            {
                user.UseItemMeterialCount(itemId, useNumber);
                user.SaveCardLevel(cardId, upLevel);

                totalUp += upLevel;
                //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = config.Name + "使用" + useNumber + "个材料成功提升" + upLevel + "级", ToastType = ToastTypeEnum.Success });
            }
        }

        if (totalUp > 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "一键升级成功，总共提高" + totalUp + "级", ToastType = ToastTypeEnum.Success });
            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
            this.Show();
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
