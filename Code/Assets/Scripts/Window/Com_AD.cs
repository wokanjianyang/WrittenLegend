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

    public Text txt_Time;
    public Text txt_Error_Count;
    public Text txt_Rule;

    public Transform tran_FakeAD;

    public Text txt_FakeAD;

    private int CD_Time = 60;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        long time = TimeHelper.ClientNowSeconds() - GameProcessor.Inst.User.AdLastTime;
        txt_Time.text = "倒计时:" + Math.Max(0, CD_Time - time);
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
                case ADTypeEnum.ErrorCount:
                    this.txt_Error_Count.text = $"失败次数:{data.CurrentShowCount}/{data.MaxShowCount}";
                    break;
            }
        }


    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    private bool CheckErrorPlatform()
    {
        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.ErrorCount);
        if (data == null)
        {
            data = new ADData()
            {
                ADType = (int)ADTypeEnum.ErrorCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            };
            return false;
        }

        if (data.CurrentShowCount >= data.MaxShowCount)
        {
            return true;
        }

        return false;
    }

    private bool CheckCd()
    {
        long time = TimeHelper.ClientNowSeconds() - GameProcessor.Inst.User.AdLastTime;

        if (time > CD_Time)
        {
            GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
            return true;
        }

        return false;
    }

    public void OnClick_GoldCount()
    {
        if (!CheckCd())
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告CD间隔" + CD_Time + "S，请稍候", ToastType = ToastTypeEnum.Failure });
            return;
        }

        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.GoldCount);
        if (data == null)
        {
            GameProcessor.Inst.User.ADShowData.ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.GoldCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
        }

        if (data.CurrentShowCount >= data.MaxShowCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (CheckErrorPlatform())
        { //无法播放,直接给播白屏
            StartCoroutine(ShowFakeAD(() =>
            {
                RewardExpAndGold(false);
                data.CurrentShowCount++;
                this.UpdateAdData();
            }));
            return;
        }

        GameProcessor.Inst.OnShowVideoAd("金币和经验收益2小时", "gold_count_2_hour", (code) =>
         {
             if (code == (int)AdStateEnum.Reward)
             {
                 this.txt_FakeAD.text += "获得奖励";

                 RewardExpAndGold(true);
                 data.CurrentShowCount += 2;
                 this.UpdateAdData();
                 GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
             }
             else if (code == (int)AdStateEnum.NotSupport || code == (int)AdStateEnum.LoadFail)
             {
                 var errorData = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.ErrorCount);
                 errorData.CurrentShowCount++;
                 this.UpdateAdData();
                 GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告加载失败,请稍候再试", ToastType = ToastTypeEnum.Failure });
             }
             else
             {
                 //取消的,不处理
             }
         });
    }

    private void RewardExpAndGold(bool real)  //看的真广告还是假广告
    {
        User user = GameProcessor.Inst.User;

        //发放奖励
        long gold = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold);
        long exp = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp);
        int rate = real ? 2 : 1;

        gold = gold * 1440 * rate; //2小时/5 = 1440
        exp = exp * 1440 * rate; //2小时/5 = 1440

        user.AddExpAndGold(exp, gold);
        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", exp, gold, null)
        });

        if (!user.Record.Check())
        {
            return;
        }

        if (real)
        {
            user.Record.AddRecord(RecordType.AdReal, 1);
        }
        else
        {
            user.Record.AddRecord(RecordType.AdVirtual, 1);
        }
    }

    public void OnClick_ExpCount()
    {
        if (!CheckCd())
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告CD间隔" + CD_Time + "S，请稍候", ToastType = ToastTypeEnum.Failure });
            return;
        }

        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.ExpCount);
        if (data == null)
        {
            GameProcessor.Inst.User.ADShowData.ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.ExpCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
        }

        if (data.CurrentShowCount >= data.MaxShowCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (CheckErrorPlatform())
        { //无法播放,直接给播白屏
            StartCoroutine(ShowFakeAD(() =>
            {
                RewardBossTicket(false);
                data.CurrentShowCount++;
                this.UpdateAdData();
            }));
            return;
        }

        GameProcessor.Inst.OnShowVideoAd("BOSS挑战卷", "exp_count_2_hour", (code) =>
         {
             if (code == (int)AdStateEnum.Reward)
             {
                 RewardBossTicket(true);
                 data.CurrentShowCount += 2;
                 this.UpdateAdData();
                 GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
             }
             else if (code == (int)AdStateEnum.NotSupport || code == (int)AdStateEnum.LoadFail)
             {
                 var errorData = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.ErrorCount);
                 errorData.CurrentShowCount++;
                 this.UpdateAdData();
                 GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告加载失败,请稍候再试", ToastType = ToastTypeEnum.Failure });
             }
             else
             {
                 //取消的,不处理
             }
         });
    }
    private void RewardBossTicket(bool real)
    {
        User user = GameProcessor.Inst.User;
        //发放奖励
        int rate = real ? 2 : 1;


        Item item = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Boss_Ticket, rate * 2);

        List<Item> items = new List<Item>();
        items.Add(item);

        user.EventCenter.Raise(new HeroBagUpdateEvent()
        {
            ItemList = items
        });

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", 0, 0, items)
        });

        if (!user.Record.Check())
        {
            return;
        }

        if (real)
        {
            user.Record.AddRecord(RecordType.AdReal, 1);
        }
        else
        {
            user.Record.AddRecord(RecordType.AdVirtual, 1);
        }
    }

    public void OnClick_CopyTicketCount()
    {
        if (!CheckCd())
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告CD间隔" + CD_Time + "S，请稍候", ToastType = ToastTypeEnum.Failure });
            return;
        }

        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.CopyTicketCount);
        if (data == null)
        {
            GameProcessor.Inst.User.ADShowData.ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.CopyTicketCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
        }

        if (data.CurrentShowCount >= data.MaxShowCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (CheckErrorPlatform())
        { //无法播放,直接给播白屏
            StartCoroutine(ShowFakeAD(() =>
            {
                RewardCopyTicket(false);
                data.CurrentShowCount++;
                this.UpdateAdData();
            }));
            return;
        }

        GameProcessor.Inst.OnShowVideoAd("广告-副本挑战8次", "ticket_count_8", (code) =>
        {
            if (code == (int)AdStateEnum.Reward)
            {
                //发放奖励
                RewardCopyTicket(true);
                data.CurrentShowCount += 2;
                this.UpdateAdData();
                GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
            }
            else if (code == (int)AdStateEnum.NotSupport || code == (int)AdStateEnum.LoadFail)
            {
                var errorData = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.ErrorCount);
                errorData.CurrentShowCount++;
                this.UpdateAdData();
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告加载失败,请稍候再试", ToastType = ToastTypeEnum.Failure });
            }
            else
            {
                //取消的,不处理
            }
        });
    }

    private void RewardCopyTicket(bool real)
    {
        int rate = real ? 2 : 1;

        User user = GameProcessor.Inst.User;
        user.MagicCopyTikerCount.Data += 8 * rate;

        if (!user.Record.Check())
        {
            return;
        }

        if (real)
        {
            user.Record.AddRecord(RecordType.AdReal, 1);
        }
        else
        {
            user.Record.AddRecord(RecordType.AdVirtual, 1);
        }
    }

    public void OnClick_StoneCount()
    {
        if (!CheckCd())
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告CD间隔" + CD_Time + "S，请稍候", ToastType = ToastTypeEnum.Failure });
            return;
        }

        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.StoneCount);
        if (data == null)
        {
            GameProcessor.Inst.User.ADShowData.ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.StoneCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
        }

        if (data.CurrentShowCount >= data.MaxShowCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (CheckErrorPlatform())
        { //无法播放,直接给播白屏
            StartCoroutine(ShowFakeAD(() =>
            {
                RewardStone(false);
                data.CurrentShowCount++;
                this.UpdateAdData();
                GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
            }));
            return;
        }

        GameProcessor.Inst.OnShowVideoAd("精炼石", "stone_count_100", (code) =>
         {
             if (code == (int)AdStateEnum.Reward)
             {
                 RewardStone(true);
                 data.CurrentShowCount += 2;
                 this.UpdateAdData();
             }
             else if (code == (int)AdStateEnum.NotSupport || code == (int)AdStateEnum.LoadFail)
             {
                 var errorData = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.ErrorCount);
                 errorData.CurrentShowCount++;
                 this.UpdateAdData();
                 GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告加载失败,请稍候再试", ToastType = ToastTypeEnum.Failure });
             }
             else
             {
                 //取消的,不处理
             }
         });
    }
    private void RewardStone(bool real)
    {
        int rate = real ? 2 : 1;

        User user = GameProcessor.Inst.User;

        int MapNo = (user.MapId - ConfigHelper.MapStartId + 1);
        int stoneRate = (MapNo / 10) + 1;

        int refineStone = 600 * MapNo * stoneRate * rate;
        Item item = ItemHelper.BuildRefineStone(refineStone);

        List<Item> items = new List<Item>();
        items.Add(item);

        user.EventCenter.Raise(new HeroBagUpdateEvent()
        {
            ItemList = items
        });

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", 0, 0, items)
        });

        if (!user.Record.Check())
        {
            return;
        }

        if (real)
        {
            user.Record.AddRecord(RecordType.AdReal, 1);
        }
        else
        {
            user.Record.AddRecord(RecordType.AdVirtual, 1);
        }
    }

    public void OnClick_ExpAdd()
    {
        GameProcessor.Inst.OnShowVideoAd("经验加成", "exp_add_percent", (code) =>
         {
             if (code > 0)
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
        GameProcessor.Inst.OnShowVideoAd("金币加成", "gold_add_percent", (code) =>
         {
             if (code > 0)
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
        var duration = RandomHelper.RandomNumber(45, 60);
        for (int i = duration; i > 0; i--)
        {
            this.txt_FakeAD.text = $"再看{i}秒广告就发奖励";
            yield return new WaitForSeconds(1f);
        }
        this.tran_FakeAD.gameObject.SetActive(false);

        endCallback?.Invoke();
    }
}
