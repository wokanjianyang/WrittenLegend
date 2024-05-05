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

    public Button Btn_Read1;
    public Button Btn_Read2;
    public Button Btn_Read3;
    public Button Btn_Read4;

    public Toggle toggle_Fail;

    public Toggle toggle_Skip;
    public Text txt_Skip;

    public Text txt_Time;
    public Text txt_Error_Count;
    public Text txt_Rule;

    public Transform tran_FakeAD;

    public Text txt_FakeAD;

    private int CD_Time = 0;

    private int Time_Success = 30;
    private int Time_Error = 3;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Read1.onClick.AddListener(() => { ReadAd(1); });
        Btn_Read2.onClick.AddListener(() => { ReadAd(2); });
        Btn_Read3.onClick.AddListener(() => { ReadAd(3); });
        Btn_Read4.onClick.AddListener(() => { ReadAd(4); });
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
                //case ADTypeEnum.ExpAdd:
                //    this.txt_Reward_Exp_Add.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                //    break;
                //case ADTypeEnum.ExpTime:
                //    this.txt_Reward_Exp_Time.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                //    break;
                //case ADTypeEnum.GoldAdd:
                //    this.txt_Reward_Gold_Add.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                //    break;
                //case ADTypeEnum.GoldTime:
                //    this.txt_Reward_Gold_Time.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";

                //    break;
                case ADTypeEnum.ErrorCount:
                    this.txt_Error_Count.text = $"失败次数:{data.CurrentShowCount}/{data.MaxShowCount}";
                    break;
            }
        }

        User user = GameProcessor.Inst.User;
        int skipCount = user.AdData.GetSkipCount();
        txt_Skip.text = skipCount + "";

        if (skipCount > 0)
        {
            toggle_Skip.gameObject.SetActive(true);

            txt_Skip.gameObject.SetActive(true);
        }
        else
        {
            toggle_Skip.gameObject.SetActive(false);
            txt_Skip.gameObject.SetActive(false);
        }
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
                MaxShowCount = 20
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

    private bool CheckCount(int type)
    {
        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus((ADTypeEnum)type);
        if (data == null)
        {
            GameProcessor.Inst.User.ADShowData.ADDatas.Add(new ADData()
            {
                ADType = type,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
        }

        if (data.CurrentShowCount >= data.MaxShowCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return false;
        }

        return true;
    }

    public void DisableButton()
    {
        Btn_Read1.gameObject.SetActive(false);
        Btn_Read2.gameObject.SetActive(false);
        Btn_Read3.gameObject.SetActive(false);
        Btn_Read4.gameObject.SetActive(false);
    }

    public IEnumerator EnableButton()
    {
        yield return new WaitForSeconds(3f);

        Btn_Read1.gameObject.SetActive(true);
        Btn_Read2.gameObject.SetActive(true);
        Btn_Read3.gameObject.SetActive(true);
        Btn_Read4.gameObject.SetActive(true);
    }

    private void ReadAd(int type)
    {
        DisableButton();
        GameProcessor.Inst.StartCoroutine(EnableButton());

        //
        if (!CheckCount(type))
        {
            return;
        }

        User user = GameProcessor.Inst.User;

        if (user.IsDz())
        {
            RewardAd(type, true);
            return;
        }

        if (toggle_Skip.isOn)
        {
            int skipCount = user.AdData.GetSkipCount();

            if (skipCount > 0)
            {
                //使用跳过次数
                user.AdData.Use();
                RewardAd(type, true);
                return;
            }
        }

        //
        if (!CheckCd())
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告CD间隔" + CD_Time + "S，请稍候", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (CheckErrorPlatform() && toggle_Fail.isOn)
        {   //无法播放,直接给播白屏
            StartCoroutine(ShowFakeAD(() =>
            {
                RewardAd(type, false);
            }));
            return;
        }

        string des = "";
        string action = "";
        switch (type)
        {
            case 1:
                des = "金币和经验收益2小时";
                action = "gold_count_2_hour";
                break;
            case 2:
                des = "BOSS挑战卷";
                action = "exp_count_2_hour";
                break;
            case 3:
                des = "副本挑战卷";
                action = "ticket_count_8";
                break;
            case 4:
                des = "精炼石";
                action = "stone_count_100";
                break;
            default:
                break;
        }

        GameProcessor.Inst.OnShowVideoAd(des, action, (code) =>
        {
            if (code == (int)AdStateEnum.Reward)
            {
                this.txt_FakeAD.text += "获得奖励";

                RewardAd(type, true);

                GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
                this.CD_Time = this.Time_Success;
            }
            else if (code == (int)AdStateEnum.NotSupport || code == (int)AdStateEnum.LoadFail)
            {
                ErrorAd();

                GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
                this.CD_Time = this.Time_Error;
            }
            else
            {
                //取消的,不处理
            }
        });
    }


    public void RewardAd(int type, bool real)
    {
        User user = GameProcessor.Inst.User;

        int rate = real ? 2 : 1;
        int dzRate = user.GetDzRate();

        var data = user.ADShowData?.GetADShowStatus((ADTypeEnum)type);

        if (data.CurrentShowCount >= 6)
        {
            return;
        }

        data.CurrentShowCount += rate;

        if (!user.Record.Check())
        {
            return;
        }

        switch (type)
        {
            case 1:
                RewardExpAndGold(rate * dzRate);
                break;
            case 2:
                RewardBossTicket(rate * dzRate);
                break;
            case 3:
                RewardCopyTicket(rate * dzRate);
                break;
            case 4:
                RewardStone(rate * dzRate);
                break;
            default:
                break;
        }

        if (real)
        {
            user.Record.AddRecord(RecordType.AdReal, 1 * dzRate);
        }
        else
        {
            user.Record.AddRecord(RecordType.AdVirtual, 1 * dzRate);
        }

        this.UpdateAdData();
    }

    private void ErrorAd()
    {
        var errorData = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(ADTypeEnum.ErrorCount);
        errorData.CurrentShowCount++;
        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告加载失败,请稍候再试", ToastType = ToastTypeEnum.Failure });

        this.UpdateAdData();
    }


    private void RewardExpAndGold(int rate)  //看的真广告还是假广告
    {
        User user = GameProcessor.Inst.User;

        //发放奖励
        long gold = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold);
        long exp = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp);

        int atRate = user.GetArtifactValue(ArtifactType.ExpGoldAd);

        gold = gold * 2160 * rate; //3小时/5 = 2160
        exp = exp * 2160 * rate; //3小时/5 = 2160

        gold = gold + gold / 100 * atRate;
        exp = exp + exp / 100 * atRate;

        user.AddExpAndGold(exp, gold);
        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", exp, gold, null)
        });
    }

    private void RewardBossTicket(int rate)
    {
        User user = GameProcessor.Inst.User;

        int atRate = user.GetArtifactValue(ArtifactType.BossTicketAd);

        Item item = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Boss_Ticket, rate * (4 + atRate));

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
    }

    private void RewardCopyTicket(int rate)
    {
        User user = GameProcessor.Inst.User;

        int atRate = user.GetArtifactValue(ArtifactType.EquipTicketAd);
        int atCount = (8 + atRate) * rate;

        user.MagicCopyTikerCount.Data += atCount;

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励-副本次数:" + atCount + "次", 0, 0, null)
        });
    }

    private void RewardStone(int rate)
    {
        User user = GameProcessor.Inst.User;
        int atRate = user.GetArtifactValue(ArtifactType.EquipStoneAd);

        int MapNo = (user.MapId - ConfigHelper.MapStartId + 1);
        int stoneRate = (MapNo / 2) + 1;

        stoneRate = stoneRate + stoneRate * atRate / 100;

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

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
