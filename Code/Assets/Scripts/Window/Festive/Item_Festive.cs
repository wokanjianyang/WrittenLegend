using Game;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item_Festive : MonoBehaviour
{
    public Text TargetName;

    public Text Txt_Title;

    public Text Txt_Cost_Title;
    public Text Txt_Cost_Content;

    public Text Txt_Limit_Title;
    public Text Txt_Limit_Content;

    public Button Btn_Ok;
    public Button Btn_Batch;
    private bool auto = true;

    private FestiveConfig Config { get; set; }

    private bool check = false;
    private int CurrentStep = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Ok.onClick.AddListener(OnClickOK);
        Btn_Batch.onClick.AddListener(OnClickBatch);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        this.Check();
    }

    public void SetData(FestiveConfig config)
    {
        this.Config = config;
        this.Init();
        this.Check();
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;
        int MaxCount = user.GetFestiveCount(Config.Id);

        TargetName.text = Config.TargetName;
        Txt_Title.text = Config.Title;
        Txt_Cost_Content.text = Config.Cost + " 个/次";
        Txt_Limit_Content.text = MaxCount + "/" + Config.Max;

        DropLimitConfig dropLimit = DropLimitConfigCategory.Instance.Get(1);

        if (DateTime.Now.Ticks > DateTime.Parse(dropLimit.StartDate).AddDays(-10).Ticks)
        {
            Btn_Batch.gameObject.SetActive(true);
            Btn_Ok.gameObject.SetActive(true);
        }
        else
        {
            Btn_Batch.gameObject.SetActive(false);
            Btn_Ok.gameObject.SetActive(false);
        }
    }

    private void Check()
    {
        if (Config == null)
        {
            return;
        }

        User user = GameProcessor.Inst.User;

        this.CurrentStep = user.GetFestiveStep();
        if (Config.Step > this.CurrentStep)
        {
            this.check = false;
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }

        this.check = true;

        long count = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();

        if (count < Config.Cost)
        {
            this.check = false;
        }

        int MaxCount = user.GetFestiveCount(Config.Id);
        if (MaxCount >= Config.Max)
        {
            this.check = false;
            Btn_Ok.gameObject.SetActive(false);
            Btn_Batch.gameObject.SetActive(false);

            if (auto)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }
    }



    public void OnClickOK()
    {
        if (!check)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.Check();

        if (!check)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足或已达上限", ToastType = ToastTypeEnum.Failure });
            return;
        }

        User user = GameProcessor.Inst.User;
        long total = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();

        int maxCount = Config.Max - user.GetFestiveCount(Config.Id);

        int count = 1;

        user.SaveFestiveCount(Config.Id, count);

        //材料
        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Chunjie,
            Quantity = Config.Cost * count
        });

        Item item = ItemHelper.BuildItem((ItemType)Config.TargetType, Config.TargetId, 1, Config.TargetCount * count);

        List<Item> list = new List<Item>();
        list.Add(item);
        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = list });

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Normal,
            Message = BattleMsgHelper.BuildGiftPackMessage("兑换节日奖励:", 0, 0, list)
        });

        GameProcessor.Inst.EventCenter.Raise(new FestiveUIFreshEvent() { });

        int MaxCount = user.GetFestiveCount(Config.Id);
        Txt_Limit_Content.text = MaxCount + "/" + Config.Max;

        //this.Check();
        //GameProcessor.Inst.SaveData();
    }

    public void OnClickBatch()
    {
        if (!check)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.Check();

        if (!check)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足或已达上限", ToastType = ToastTypeEnum.Failure });
            return;
        }

        User user = GameProcessor.Inst.User;
        long total = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();

        int maxCount = Config.Max - user.GetFestiveCount(Config.Id);

        int count = (int)(total / Config.Cost);
        count = Math.Min(count, maxCount);

        user.SaveFestiveCount(Config.Id, count);

        //材料
        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Chunjie,
            Quantity = Config.Cost * count
        });

        Item item = ItemHelper.BuildItem((ItemType)Config.TargetType, Config.TargetId, 1, Config.TargetCount * count);

        List<Item> list = new List<Item>();
        list.Add(item);
        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = list });

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Normal,
            Message = BattleMsgHelper.BuildGiftPackMessage("兑换节日奖励:", 0, 0, list)
        });

        GameProcessor.Inst.EventCenter.Raise(new FestiveUIFreshEvent() { });

        int MaxCount = user.GetFestiveCount(Config.Id);
        Txt_Limit_Content.text = MaxCount + "/" + Config.Max;
    }

    public void ChangeAuto(bool isOn)
    {
        this.auto = isOn;

        this.Check();
    }
}

