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

    public Text Txt_Layer;
    public Text Txt_Name_Layer;
    public Text Txt_Cost_Layer;

    public Button Btn_Close;
    public Button Btn_OK;
    public Button Btn_OK_Batch;

    public Button Btn_OK_Layer;

    private Pet SelectPet;

    private int PetQualityRate = 30;
    private int PetSpeicalRate = 20;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_OK.onClick.AddListener(OnClick_Ok);
        Btn_OK_Batch.onClick.AddListener(OnClick_Ok_Batch);

        Btn_OK_Layer.onClick.AddListener(OnClick_Ok_Layer);
    }


    public void Open(Pet pet)
    {
        this.SelectPet = pet;
        this.gameObject.SetActive(true);

        this.Show();
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;
        int psg = user.GetPetSpeicalGroupLevel();

        int maxLevel = SelectPet.GetQuality() * PetQualityRate + psg * PetSpeicalRate;
        long currentLevel = SelectPet.PetLevel.Data;

        Txt_Level.text = "当前等级：" + currentLevel + "级（最高等级" + maxLevel + "级）";

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

        long maxLayer = currentLevel / 20 + 1;
        long currentLayer = SelectPet.PetLayer.Data;

        Txt_Layer.text = "当前等阶：" + currentLayer + "阶（最高等阶" + maxLayer + "阶）";

        int quanlity = SelectPet.GetQuality();
        int materailId = ItemHelper.Specail_Pet_Layer[quanlity - 5];

        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(materailId);

        long haveCount = user.GetMaterialCount(materailId);
        long needCount = PetConfigCategory.Instance.GetPetLayerFee(currentLayer);

        Txt_Name_Layer.text = itemConfig.Name;
        Txt_Cost_Layer.text = haveCount + "/" + needCount;

        if (currentLayer >= maxLayer || haveCount < needCount)
        {
            Btn_OK_Layer.gameObject.SetActive(false);
        }
        else
        {
            Btn_OK_Layer.gameObject.SetActive(true);
        }
    }

    public void OnClick_Ok()
    {
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_OK_Batch.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        int psg = user.GetPetSpeicalGroupLevel();

        long max = PetConfigCategory.Instance.GetPetFee(SelectPet.PetLevel.Data);
        long current = SelectPet.LevelExp.Data;

        long currentLevel = SelectPet.PetLevel.Data;
        int maxLevel = SelectPet.GetQuality() * PetQualityRate + psg * PetSpeicalRate;
        if (currentLevel >= maxLevel)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "不能超过最大等级", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Pet_Exp).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "口粮不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long fee = Math.Min(stoneTotal, max - current);

        //Debug.Log("pet fee:" + fee);

        if (fee <= 0)
        {
            SelectPet.AddExp(0);
        }
        else
        {
            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = ItemHelper.SpecialId_Pet_Exp,
                Quantity = fee
            });

            SelectPet.AddExp(fee);
        }

        this.Show();

        user.EventCenter.Raise(new UserAttrChangeEvent());

        this.Btn_OK.gameObject.SetActive(true);
        this.Btn_OK_Batch.gameObject.SetActive(true);
    }

    public void OnClick_Ok_Batch()
    {
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_OK_Batch.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        int psg = user.GetPetSpeicalGroupLevel();

        for (int i = 0; i < 50; i++)
        {
            long max = PetConfigCategory.Instance.GetPetFee(SelectPet.PetLevel.Data);
            long current = SelectPet.LevelExp.Data;
            long currentLevel = SelectPet.PetLevel.Data;
            int maxLevel = SelectPet.GetQuality() * PetQualityRate + psg * PetSpeicalRate;

            if (currentLevel >= maxLevel)
            {
                //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "不能超过最大等级", ToastType = ToastTypeEnum.Failure });
                break;
            }

            long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Pet_Exp).Select(m => m.MagicNubmer.Data).Sum();
            if (stoneTotal <= 0)
            {
                //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "口粮不足", ToastType = ToastTypeEnum.Failure });
                break;
            }

            long fee = Math.Min(stoneTotal, max - current);

            //Debug.Log("pet fee:" + fee);

            if (fee <= 0)
            {
                SelectPet.AddExp(0);
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                {
                    Type = ItemType.Material,
                    ItemId = ItemHelper.SpecialId_Pet_Exp,
                    Quantity = fee
                });

                SelectPet.AddExp(fee);
            }
        }

        this.Show();

        user.EventCenter.Raise(new UserAttrChangeEvent());

        this.Btn_OK.gameObject.SetActive(true);
        this.Btn_OK_Batch.gameObject.SetActive(true);
    }

    public void OnClick_Ok_Layer()
    {
        int quality = SelectPet.GetQuality();
        if (quality < 5)
        {
            return;
        }

        this.Btn_OK_Layer.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        long max = SelectPet.PetLevel.Data / 20 + 1;
        long current = SelectPet.PetLayer.Data;

        if (current >= max)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满阶", ToastType = ToastTypeEnum.Failure });
            return;
        }

        int materilId = ItemHelper.Specail_Pet_Layer[quality - 5];
        long fee = PetConfigCategory.Instance.GetPetLayerFee(current);

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == materilId).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal < fee)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = materilId,
            Quantity = fee
        });

        SelectPet.PetLayer.Data++;

        this.Show();

        user.EventCenter.Raise(new UserAttrChangeEvent());

        this.Btn_OK_Layer.gameObject.SetActive(true);
    }
    public void OnClick_Close()
    {
        this.SelectPet = null;
        this.gameObject.SetActive(false);

        Panel_Pet panel_Pet = this.GetComponentInParent<Panel_Pet>();
        panel_Pet.Show();
    }
}
