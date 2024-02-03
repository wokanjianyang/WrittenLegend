using Game;
using Sirenix.OdinInspector;
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


    private FestiveConfig Config { get; set; }

    private bool check = false;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Ok.onClick.AddListener(OnClickOK);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
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

        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(ItemHelper.SpecialId_Chunjie);
    }

    private void Check()
    {
        if (Config == null)
        {
            return;
        }

        User user = GameProcessor.Inst.User;

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
            Btn_Ok.enabled = false;
        }
    }

    public void SetData(FestiveConfig config)
    {
        this.Config = config;
        this.Init();
        this.Check();
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
        user.SaveFestiveCount(Config.Id);

        //材料
        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Chunjie,
            Quantity = Config.Cost
        });

        Item item = ItemHelper.BuildItem((ItemType)Config.TargetType, Config.TargetId, 1, Config.TargetCount);

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
}

