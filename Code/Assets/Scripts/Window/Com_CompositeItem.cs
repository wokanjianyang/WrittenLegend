using System.Collections;
using System.Collections.Generic;
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

    private SynthesisConfig config;

    public void SetData(SynthesisConfig data)
    {
        this.config = data;
        this.ItemName.text = data.TargetName;
        this.CostItemName.text = data.FromName;
        this.CostItemCount.text = $"(0/{data.Quantity})";
        this.CostItemGold.text = data.Commission.ToString();
        //TODO 道具数量，是否满足消耗，合成，根据道具品质修改边框和颜色
    }

    public void OnClick_Composite()
    {
        Log.Debug($"合成:{this.config.TargetName}");
    }
}
