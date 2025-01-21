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

    public Text Txt_Metail_Name;
    public Text Txt_Metail_Count;

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

        User user = GameProcessor.Inst.User;
        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Pet_Exp).Select(m => m.MagicNubmer.Data).Sum();
        Txt_Metail_Count.text = stoneTotal + "";

        long fee = PetConfigCategory.Instance.GetPetFee(SelectPet.PetLevel.Data);
        ExpProgress.SetProgress(SelectPet.LevelExp.Data, fee);
    }

    public void OnClick_Ok()
    {
        this.Btn_OK.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        long max = PetConfigCategory.Instance.GetPetFee(SelectPet.PetLevel.Data);
        long current = SelectPet.LevelExp.Data;

        long fee = Math.Max(0, max - current);

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Pet_Exp).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "¿ÚÁ¸²»×ã", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Pet_Exp,
            Quantity = fee
        });


        SelectPet.AddExp(fee);

        this.Show();

        this.Btn_OK.gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        this.SelectPet = null;
        this.gameObject.SetActive(false);
    }
}
