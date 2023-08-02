using System;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Com_AD : MonoBehaviour, IBattleLife
{
    [LabelText("金币收益次数")]
    public Text txt_Reward_Gold_Count;
    
    [LabelText("经验收益次数")]
    public Text txt_Reward_Exp_Count;

    [LabelText("副本券次数")]
    public Text txt_Reward_Copy_Ticket_Count;

    [LabelText("精炼石次数")]
    public Text txt_Reward_Stone_Count;

    [LabelText("经验加成次数")]
    public Text txt_Reward_Exp_Add;
    
    [LabelText("经验加成持续时间")]
    public Text txt_Reward_Exp_Time;
    
    [LabelText("金币加成次数")]
    public Text txt_Reward_Gold_Add;
    
    [LabelText("金币加成持续时间")]
    public Text txt_Reward_Gold_Time;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public void OnBattleStart()
    {

        
    }

    public int Order => (int)ComponentOrder.Dialog;

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClick_GoldCount()
    {
        GameProcessor.Inst.OnShowVideoAd("金币收益2小时","gold_count_2_hour", (giveReward) =>
        {
            if (giveReward)
            {
                //发放奖励
            }
            else
            {
                //不发奖励
            }
        });
    }

    public void OnClick_ExpCount()
    {
        GameProcessor.Inst.OnShowVideoAd("经验收益2小时","exp_count_2_hour", (giveReward) =>
        {
            if (giveReward)
            {
                //发放奖励
            }
            else
            {
                //不发奖励
            }
        });
    }
    
    public void OnClick_CopyTicketCount()
    {
        GameProcessor.Inst.OnShowVideoAd("副本挑战券","copy_ticket_count", (giveReward) =>
        {
            if (giveReward)
            {
                //发放奖励
            }
            else
            {
                //不发奖励
            }
        });
    }
    public void OnClick_StoneCount()
    {
        GameProcessor.Inst.OnShowVideoAd("精炼石100个","stone_count_100", (giveReward) =>
        {
            if (giveReward)
            {
                //发放奖励
            }
            else
            {
                //不发奖励
            }
        });
    }
    public void OnClick_ExpAdd()
    {
        GameProcessor.Inst.OnShowVideoAd("经验加成","exp_add_percent", (giveReward) =>
        {
            if (giveReward)
            {
                //发放奖励
            }
            else
            {
                //不发奖励
            }
        });
    }
    public void OnClick_GoldAdd()
    {
        GameProcessor.Inst.OnShowVideoAd("金币加成","gold_add_percent", (giveReward) =>
        {
            if (giveReward)
            {
                //发放奖励
            }
            else
            {
                //不发奖励
            }
        });
    }
}
