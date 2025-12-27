using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Festive_Item : MonoBehaviour
{
    public Text Txt_Name;
    public Button Btn_Start;
    public Button Btn_Auto;
    public Text Txt_Over;

    private FestiveCopyConfig Config;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Start.onClick.AddListener(OnClick_NavigateMap);
        Btn_Auto.onClick.AddListener(OnClick_Auto);
    }

    private void OnEnable()
    {
        if (this.Config != null)
        {
            this.Show();
        }
    }


    private void Show()
    {
        int maxId = GameProcessor.Inst.User.FestiveMapData01.Record;

        if (this.Config.Id - 1 == maxId)
        {
            this.Btn_Start.gameObject.SetActive(true);
            this.Btn_Auto.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_Start.gameObject.SetActive(false);
            this.Btn_Auto.gameObject.SetActive(true);
        }
    }

    private void OnClick_NavigateMap()
    {
        User user = GameProcessor.Inst.User;

        if (user.FestiveMapData01.Number.Data <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有挑战次数了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        var dialog = this.GetComponentInParent<Map_Dialog_Festive>();
        dialog.gameObject.SetActive(false);

        var vm = this.GetComponentInParent<ViewMore>();
        vm.StartFestive(Config.Id);
    }

    private void OnClick_Auto()
    {
        User user = GameProcessor.Inst.User;

        if (user.FestiveMapData01.Number.Data <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有挑战次数了", ToastType = ToastTypeEnum.Failure });
            return;
        }


        List<Item> items = new List<Item>();
        FestiveCopyConfig mythConfig = FestiveCopyConfigCategory.Instance.Get(this.Config.Id);

        //非首通
        for (int i = 0; i < mythConfig.ItemIdList.Length; i++)
        {
            items.Add(ItemHelper.BuildItem((ItemType)mythConfig.ItemType[i], mythConfig.ItemIdList[i], 1, mythConfig.ItemQuantity[i]));
        }

        user.FestiveMapData01.Number.Data -= 1;

        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

        string message = "节日副本" + mythConfig.MapName + "扫荡奖励";
        GameProcessor.Inst.EventCenter.Raise(new ShowDropEvent() { Message = message, Items = items });

        var dialog = this.GetComponentInParent<Map_Dialog_Festive>();
        dialog.ShowCount();
    }


    public void SetContent(FestiveCopyConfig config)
    {
        this.Config = config;

        Txt_Name.text = config.MapName;

        this.Show();
    }

    public void SetMax(int maxId)
    {
        if (Config.Id - 1 <= maxId)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
