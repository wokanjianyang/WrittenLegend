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
        int Count = user.GetFestiveCount(Config.Id);

        TargetName.text = Config.TargetName;
        Txt_Title.text = Config.Title;
        Txt_Cost_Content.text = Config.Cost + " 个/次";
        Txt_Limit_Content.text = Count + "/" + Config.Max;

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

        int MaxCount = Config.Max;

        long count = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();

        string color = "#00FF00";

        if (count < MaxCount)
        {
            color = "#FF0000";
            this.check = false;
        }

        //CommissionCount.text = string.Format("<color={0}>({1}/{2})</color>", color, count, MaxCount);
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
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }


        GameProcessor.Inst.EventCenter.Raise(new FestiveUIFreshEvent() { });
    }

    public void OnUIFresh(FestiveUIFreshEvent e)
    {
        this.Check();
    }
}

