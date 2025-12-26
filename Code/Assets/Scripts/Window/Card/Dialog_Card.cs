using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Card : MonoBehaviour
{
    public Button btn_Close;
    public Button Btn_Batch;

    public Toggle toggle_Skip;

    private int SelectStage = 0;
    public List<Toggle> toggleStageList = new List<Toggle>();

    public Panel_Card panel1;
    public Panel_Card_Special panel2;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);
        this.Btn_Batch.onClick.AddListener(OnClick_Batch);

        for (int i = 0; i < toggleStageList.Count; i++)
        {
            int index = i;
            toggleStageList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangePanel(index);
            });
        }

        this.ChangePanel(0);

        User user = GameProcessor.Inst.User;
        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";
        if (ac)
        {
            toggleStageList[toggleStageList.Count - 1].gameObject.SetActive(false);
        }
    }

    private void ChangePanel(int index)
    {
        this.SelectStage = index;

        if (index == 4)
        {
            this.panel1.gameObject.SetActive(false);
            this.panel2.Show();
        }
        else
        {
            this.panel2.gameObject.SetActive(false);
            this.panel1.Show(this.SelectStage);
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

            this.panel1.Show(this.SelectStage);
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
