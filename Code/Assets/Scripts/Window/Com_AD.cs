using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Data;
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

    [LabelText("装备副本次数")]
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
    
    public Transform tran_FakeAD;

    public Text txt_FakeAD;

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
        this.UpdateAdData();

        this.gameObject.SetActive(true);
    }

    public void UpdateAdData()
    {
        var @enums = Enum.GetValues(typeof(ADTypeEnum));
        foreach (ADTypeEnum @enum in @enums)
        {
            var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(@enum);
            if (data == null)
            {
                continue;
            }
            switch (@enum)
            {
                case ADTypeEnum.GoldCount:
                    this.txt_Reward_Gold_Count.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";
                    break;
                case ADTypeEnum.ExpCount:
                    this.txt_Reward_Exp_Count.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                    break;
                case ADTypeEnum.CopyTicketCount:
                    this.txt_Reward_Copy_Ticket_Count.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                    break;
                case ADTypeEnum.StoneCount:
                    this.txt_Reward_Stone_Count.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                    break;
                case ADTypeEnum.ExpAdd:
                    this.txt_Reward_Exp_Add.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                    break;
                case ADTypeEnum.ExpTime:
                    this.txt_Reward_Exp_Time.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                    break;
                case ADTypeEnum.GoldAdd:
                    this.txt_Reward_Gold_Add.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                    break;
                case ADTypeEnum.GoldTime:
                    this.txt_Reward_Gold_Time.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                    break;
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClick_GoldCount()
    {
        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.GoldCount);
        if (data == null)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return;
        }
        GameProcessor.Inst.OnShowVideoAd("金币收益2小时", "gold_count_2_hour", (giveReward) =>
         {

             if (giveReward)
             {
                 RewardGold();
                 data.CurrentShowCount++;
                 this.UpdateAdData();
             }
             else
             {
                 //不发奖励
                 StartCoroutine(ShowFakeAD(() =>
                  {
                      RewardGold();
                      data.CurrentShowCount++;
                      this.UpdateAdData();
                  }));
             }
         });
    }
    private void RewardGold() {
        User user = GameProcessor.Inst.User;
        //发放奖励
        long gold = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold);

        gold = gold * 1440; //2小时/5 = 1440

        user.AddExpAndGold(0, gold);
        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", 0, gold)
        });
    }

    public void OnClick_ExpCount()
    {
        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.ExpCount);
        if (data == null)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return;
        }
        GameProcessor.Inst.OnShowVideoAd("经验收益2小时", "exp_count_2_hour", (giveReward) =>
         {
             if (giveReward)
             {
                 RewardExp();
                 data.CurrentShowCount++;
                 this.UpdateAdData();
             }
             else
             {
                 //不发奖励
                 StartCoroutine(ShowFakeAD(() =>
                 {
                     RewardExp();
                     data.CurrentShowCount++;
                     this.UpdateAdData();
                 }));
             }
         });
    }
    private void RewardExp() {
        User user = GameProcessor.Inst.User;
        //发放奖励
        long exp = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp);

        exp = exp * 1440; //2小时/5 = 1440

        user.AddExpAndGold(exp, 0);
        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", exp, 0)
        });
    }

    public void OnClick_CopyTicketCount()
    {
        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.CopyTicketCount);
        if (data == null)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.OnShowVideoAd("经验收益2小时", "exp_count_2_hour", (giveReward) =>
        {
            Debug.Log("广告-副本挑战8次-完成");
            if (giveReward)
            {
                //发放奖励
                RewardCopyTicket();
                data.CurrentShowCount++;
                this.UpdateAdData();
            }
            else
            {
                //不发奖励
                StartCoroutine(ShowFakeAD(() =>
                {
                    RewardCopyTicket();
                    data.CurrentShowCount++;
                    this.UpdateAdData();
                }));

            }
        });
    }

    private void RewardCopyTicket() {
        User user = GameProcessor.Inst.User;
        user.CopyTikerCount += 8;
    }

    public void OnClick_StoneCount()
    {
        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.StoneCount);
        if (data == null)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.OnShowVideoAd("精炼石", "stone_count_100", (giveReward) =>
         {
             if (giveReward)
             {
                 RewardStone();
                 data.CurrentShowCount++;
                 this.UpdateAdData();
             }
             else
             {
                 StartCoroutine(ShowFakeAD(() =>
                 {
                     RewardStone();
                     data.CurrentShowCount++;
                     this.UpdateAdData();
                 }));
                 //不发奖励
             }
         });
    }
    private void RewardStone()
    {
        User user = GameProcessor.Inst.User;

        int MapNo = (user.MapId - ConfigHelper.MapStartId + 1);
        int rate = (MapNo / 10) + 1;

        int refineStone = 100 * MapNo * rate;
        Item item = ItemHelper.BuildRefineStone(refineStone);

        List<Item> items = new List<Item>();
        items.Add(item);

        user.EventCenter.Raise(new HeroBagUpdateEvent()
        {
            ItemList = items
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

    private IEnumerator ShowFakeAD(Action endCallback)
    {
        this.tran_FakeAD.gameObject.SetActive(true);
        var duration = 5;
        for (int i = duration; i > 0; i--)
        {
            this.txt_FakeAD.text = $"再看{i}秒广告就发奖励";
            yield return new WaitForSeconds(1f);
        }
        this.tran_FakeAD.gameObject.SetActive(false);

        endCallback?.Invoke();
    }
}
