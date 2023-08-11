using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Com_CompositeItem : MonoBehaviour
{
    public Text ItemName;
    public Transform tran_Item;

    public Text CostItemName;
    public Text CostItemCount;
    public Text CostItemGold;

    private CompositeConfig config;

    public void SetData(CompositeConfig data)
    {
        this.config = data;

        User user = GameProcessor.Inst.User;

        //TODO 合成数量问题
        //int count = user.Bags.Where(m => (int)m.Item.Type == config.FromItemType && m.Item.ConfigId == config.FromId).Select(m => m.Number).Sum();
        int count = user.Bags.Where(m => (int)m.Item.Type == config.FromItemType && m.Item.ConfigId == config.FromId).Count();

        this.ItemName.text = config.TargetName;
        this.CostItemName.text = config.FromName;

        string color = count >= config.Quantity ? "#00FF00" : "#FF0000";

        this.CostItemCount.text = string.Format("<color={0}>({1}/{2})</color>", color, count, config.Quantity); ;

        this.CostItemGold.text = data.Commission.ToString();
        //TODO 道具数量，是否满足消耗，合成，根据道具品质修改边框和颜色
    }

    public void OnClick_Composite()
    {
        User user = GameProcessor.Inst.User;
        int count = user.Bags.Where(m => (int)m.Item.Type == config.FromItemType && m.Item.ConfigId == config.FromId).Count();
        //TODO 合成数量问题

        if (count < config.Quantity || user.Gold < config.Commission)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足" });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new CompositeEvent()
        {
            Config = config
        });

        Log.Debug($"合成:{this.config.TargetName}");
    }

    public void Refresh()
    {
        if (config == null)
        {
            return;
        }
        User user = GameProcessor.Inst.User;

        int count = user.Bags.Where(m => (int)m.Item.Type == config.FromItemType && m.Item.ConfigId == config.FromId).Select(m => m.Number).Sum();

        this.ItemName.text = config.TargetName;
        this.CostItemName.text = config.FromName;

        string color = count >= config.Quantity ? "#00FF00" : "#FF0000";

        this.CostItemCount.text = string.Format("<color={0}>({1}/{2})</color>", color, count, config.Quantity); ;
    }
}
