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

    public Text Txt_Cost;

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
        int maxLevel = SelectPet.GetQuality() * 30;
        long currentLevel = SelectPet.PetLevel.Data;

        Txt_Level.text = "当前等级：" + currentLevel + "级（最高等级" + maxLevel + "级）";

        User user = GameProcessor.Inst.User;
        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Pet_Exp).Select(m => m.MagicNubmer.Data).Sum();
        Txt_Cost.text = "拥有口粮：" + stoneTotal;

        long fee = PetConfigCategory.Instance.GetPetFee(SelectPet.PetLevel.Data);
        ExpProgress.SetProgress(SelectPet.LevelExp.Data, fee);

        if (currentLevel >= maxLevel || stoneTotal <= 0)
        {
            Btn_OK.gameObject.SetActive(false);
        }
        else
        {
            Btn_OK.gameObject.SetActive(true);
        }
    }

    public void OnClick_Ok()
    {
        this.Btn_OK.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        long max = PetConfigCategory.Instance.GetPetFee(SelectPet.PetLevel.Data);
        long current = SelectPet.LevelExp.Data;

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Pet_Exp).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "口粮不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long fee = Math.Min(stoneTotal, max - current);
        fee = Math.Max(fee, 0);

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
