using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Steed_Forge : MonoBehaviour
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

    private Steed SelectSteed;

    private int MaxLevel = 100;
    private int SpeicalRate = 20;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_OK.onClick.AddListener(OnClick_Ok);
        //Btn_OK_Batch.onClick.AddListener(OnClick_Ok_Batch);

        Btn_OK_Layer.onClick.AddListener(OnClick_Ok_Layer);
    }


    public void Open(Steed steed)
    {
        this.SelectSteed = steed;
        this.gameObject.SetActive(true);

        this.Show();
    }

    private void Show()
    {
        User user = GameProcessor.Inst.User;

        int maxLevel = MaxLevel;
        long currentLevel = SelectSteed.SteedLevel.Data;

        Txt_Level.text = "当前等级：" + currentLevel + "级（最高等级" + maxLevel + "级）";

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Steed_Exp).Select(m => m.MagicNubmer.Data).Sum();
        Txt_Cost.text = "拥有兽粮：" + stoneTotal;

        long fee = SteedConfigCategory.Instance.GetFee(SelectSteed.SteedLevel.Data);
        ExpProgress.SetProgress(SelectSteed.LevelExp.Data, fee);

        if (currentLevel >= maxLevel || stoneTotal <= 0)
        {
            Btn_OK.gameObject.SetActive(false);
            //Btn_OK_Batch.gameObject.SetActive(false);
        }
        else
        {
            Btn_OK.gameObject.SetActive(true);
            //Btn_OK_Batch.gameObject.SetActive(true);
        }

        long maxLayer = currentLevel / 50 + 1;
        long currentLayer = SelectSteed.SteedLayer.Data;

        Txt_Layer.text = "当前等阶：" + currentLayer + "阶（最高等阶" + maxLayer + "阶）";

        int quality = SelectSteed.GetQuality();
        int materilId = SteedConfigCategory.Instance.GetLayerId(quality); ;

        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(materilId);

        long haveCount = user.GetMaterialCount(materilId);
        long needCount = SteedConfigCategory.Instance.GetLayerFee(currentLayer);

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

        long max = SteedConfigCategory.Instance.GetFee(SelectSteed.SteedLevel.Data);
        long current = SelectSteed.LevelExp.Data;

        long currentLevel = SelectSteed.SteedLevel.Data;
        int maxLevel = MaxLevel;
        if (currentLevel >= maxLevel)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "不能超过最大等级", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Steed_Exp).Select(m => m.MagicNubmer.Data).Sum();
        if (stoneTotal <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "兽粮不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        long fee = Math.Min(stoneTotal, max - current);

        //Debug.Log("steed fee:" + fee);

        if (fee <= 0)
        {
            SelectSteed.AddExp(0);
        }
        else
        {
            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = ItemHelper.SpecialId_Steed_Exp,
                Quantity = fee
            });

            SelectSteed.AddExp(fee);
        }

        this.Show();

        user.EventCenter.Raise(new UserAttrChangeEvent());

        //this.Btn_OK.gameObject.SetActive(true);
        //this.Btn_OK_Batch.gameObject.SetActive(true);
    }

    public void OnClick_Ok_Batch()
    {
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_OK_Batch.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        int psg = 0;

        for (int i = 0; i < 50; i++)
        {
            long max = SteedConfigCategory.Instance.GetFee(SelectSteed.SteedLevel.Data);
            long current = SelectSteed.LevelExp.Data;
            long currentLevel = SelectSteed.SteedLevel.Data;
            int maxLevel = this.MaxLevel;

            if (currentLevel >= maxLevel)
            {
                //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "不能超过最大等级", ToastType = ToastTypeEnum.Failure });
                break;
            }

            long stoneTotal = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Steed_Exp).Select(m => m.MagicNubmer.Data).Sum();
            if (stoneTotal <= 0)
            {
                //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "兽粮不足", ToastType = ToastTypeEnum.Failure });
                break;
            }

            long fee = Math.Min(stoneTotal, max - current);

            //Debug.Log("steed fee:" + fee);

            if (fee <= 0)
            {
                SelectSteed.AddExp(0);
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                {
                    Type = ItemType.Material,
                    ItemId = ItemHelper.SpecialId_Steed_Exp,
                    Quantity = fee
                });

                SelectSteed.AddExp(fee);
            }
        }

        this.Show();

        user.EventCenter.Raise(new UserAttrChangeEvent());

        //this.Btn_OK.gameObject.SetActive(true);
        //this.Btn_OK_Batch.gameObject.SetActive(true);
    }

    public void OnClick_Ok_Layer()
    {
        int quality = SelectSteed.GetQuality();
        if (quality < 5)
        {
            return;
        }

        this.Btn_OK_Layer.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        long max = SelectSteed.SteedLevel.Data / 20 + 1;
        long current = SelectSteed.SteedLayer.Data;

        if (current >= max)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满阶", ToastType = ToastTypeEnum.Failure });
            return;
        }

        int materilId = SteedConfigCategory.Instance.GetLayerId(quality);
        long fee = SteedConfigCategory.Instance.GetLayerFee(current);

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

        SelectSteed.SteedLayer.Data++;

        this.Show();

        user.EventCenter.Raise(new UserAttrChangeEvent());

        this.Btn_OK_Layer.gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        this.SelectSteed = null;
        this.gameObject.SetActive(false);

        Panel_Steed panel = this.GetComponentInParent<Panel_Steed>();
        panel.Show();
    }
}
