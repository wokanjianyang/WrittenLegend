using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Pet_Forge : MonoBehaviour
{
    public HP_Progress ExpProgress;

    public Text Txt_Level;

    public Item_Metail_Need Item_Metail_Need;
    public Button Btn_Close;
    public Button Btn_OK;

    private Pet SelectPet;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_OK.onClick.AddListener(OnClick_Ok);
    }


    public void Open(Pet pet)
    {
        this.SelectPet = pet;
        this.gameObject.SetActive(true);

        this.Show();
    }

    private void Show()
    {
        Txt_Level.text = SelectPet.PetLevel.Data + "¼¶";
        Txt_Level.color = ColorHelper.GetColorByQuality(SelectPet.GetQuality());

        int upCount = 100;

        Item_Metail_Need.SetContent(ItemHelper.SpecialId_Pet_Exp, upCount);
        ExpProgress.SetProgress(SelectPet.LevelExp.Data, upCount);
    }

    public void OnClick_Ok()
    {
        this.Btn_OK.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        int upCount = 100;

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Pet_Exp).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal < upCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "²ÄÁÏ²»×ã", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.Show();

        this.Btn_OK.gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        this.SelectPet = null;
        this.gameObject.SetActive(false);
    }
}
