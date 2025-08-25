using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Halidom : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Button btn_Reset;
    public Button btn_Close;

    private int SelectStage = 1;
    public List<Toggle> toggleStageList = new List<Toggle>();

    private List<Item_Halidom> items = new List<Item_Halidom>();

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Close.onClick.AddListener(OnClick_Close);
        this.btn_Reset.onClick.AddListener(OnClickReset);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Halidom");

        for (int i = 0; i < toggleStageList.Count; i++)
        {
            int index = i + 1;
            toggleStageList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangePanel(index);
            });
        }

        Init();

        this.ChangePanel(SelectStage);
    }

    private void Init()
    {
        List<HalidomConfig> configs = HalidomConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        User user = GameProcessor.Inst.User;

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            Item_Halidom com = item.GetComponentInChildren<Item_Halidom>();

            long level = user.GetHalidomLevel(configs[i].Id);
            com.SetContent(configs[i], level);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }
    }

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
            if (this.SelectStage == items[i].Config.Layer)
            {
                items[i].gameObject.SetActive(true);
            }
            else
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickReset()
    {

        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认花费1垓金币重生遗物到宇阶？", true,
        () =>
        {

            User user = GameProcessor.Inst.User;

            if (user.MagicGold.Data <= ConfigHelper.RestoreGold * 20000.0)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足1垓", ToastType = ToastTypeEnum.Failure });
                return;
            }

            user.SubGold(ConfigHelper.RestoreGold * 20000.0);

            int total = 0;

            foreach (var sp in user.HalidomData)
            {
                if (sp.Value.Data > 8)
                {
                    total += HalidomConfigCategory.Instance.GetRestoreFee(sp.Value.Data);

                    sp.Value.Data = 8;
                }
            }

            List<Item> newList = new List<Item>();
            Item item = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Halidom_Chip, total);
            newList.Add(item);

            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = newList });

            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "重生一共获得遗物粉尘" + total + "个", ToastType = ToastTypeEnum.Success });

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Refresh();
            }

            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        }, () =>
        {
        });
    }


    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
