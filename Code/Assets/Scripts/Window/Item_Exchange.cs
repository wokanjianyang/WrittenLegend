using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item_Exchange : MonoBehaviour
{
    public Text TargetName;
    public Text FromName;
    public Text FromCount;
    public Text CommissionName;
    public Text CommissionCount;

    public Button Btn_Ok;

    private List<Text> TxtNameList = new List<Text>();
    private List<Text> TxtCountList = new List<Text>();

    private ExchangeConfig Config { get; set; }

    private bool check = false;

    // Start is called before the first frame update
    void Awake()
    {
        TxtNameList.Add(FromName);
        TxtNameList.Add(CommissionName);

        TxtCountList.Add(FromCount);
        TxtCountList.Add(CommissionCount);

        Btn_Ok.onClick.AddListener(OnClickOK);

        GameProcessor.Inst.EventCenter.AddListener<ExchangeUIFreshEvent>(this.OnExchangeUIFresh);
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
        TargetName.text = Config.TargetName.Insert(2, "\n");

        string color = QualityConfigHelper.GetQualityColor(5);
        TxtNameList[0].text = string.Format("<color=#{0}>{1}</color>", color, "任意橙专属");

        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(Config.ItemId);
        TxtNameList[1].text = string.Format("<color=#{0}>{1}</color>", color, itemConfig.Name);
    }

    private void Check()
    {
        if (Config == null)
        {
            return;
        }

        User user = GameProcessor.Inst.User;

        this.check = true;

        for (int i = 0; i <= 1; i++)
        {
            int MaxCount = 1;
            long count = 0;

            if (i == 0)
            {
                count = user.Bags.Where(m => m.Item.Type == ItemType.Exclusive && m.Item.GetQuality() >= 5 && !m.Item.IsLock).Count();
                MaxCount = 1;
            }
            else
            {
                count = user.GetMaterialCount(ItemHelper.SpecialId_Exclusive_Stone);
                MaxCount = Config.ItemCount;
            }

            string color = "#00FF00";

            if (count < MaxCount)
            {
                color = "#FF0000";
                this.check = false;
            }

            TxtCountList[i].text = string.Format("<color={0}>({1}/{2})</color>", color, count, MaxCount);
        }
    }

    public void SetData(ExchangeConfig config)
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

        GameProcessor.Inst.EventCenter.Raise(new ExchangeEvent() { Config = Config });
    }

    public void OnExchangeUIFresh(ExchangeUIFreshEvent e)
    {
        this.Check();
    }
}

