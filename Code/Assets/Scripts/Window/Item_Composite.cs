using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item_Composite : MonoBehaviour
{
    public Text TargetName;
    public Text FromName;
    public Text FromCount;
    public Text CommissionName;
    public Text CommissionCount;

    public Button Btn_Ok;

    private List<Text> TxtNameList = new List<Text>();
    private List<Text> TxtCountList = new List<Text>();

    private CompositeConfig Config { get; set; }

    private bool check = false;

    // Start is called before the first frame update
    void Awake()
    {
        TxtNameList.Add(FromName);
        TxtNameList.Add(CommissionName);

        TxtCountList.Add(FromCount);
        TxtCountList.Add(CommissionCount);

        Btn_Ok.onClick.AddListener(OnClickOK);

        GameProcessor.Inst.EventCenter.AddListener<CompositeUIFreshEvent>(this.OnUIFresh);
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

        for (int i = 0; i < Config.ItemIdList.Length; i++)
        {
            string color = QualityConfigHelper.GetQualityColor(Config.ItemQualityList[i]);

            if (Config.ItemTypeList[i] == (int)ItemType.Equip)
            {
                EquipConfig equipConfig = EquipConfigCategory.Instance.Get(Config.ItemIdList[i]);
                TxtNameList[i].text = string.Format("<color=#{0}>{1}</color>", color, equipConfig.Name); 
            }
            else
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(Config.ItemIdList[i]);
                TxtNameList[i].text = itemConfig.Name;
            }
        }
    }

    private void Check()
    {
        if (Config == null)
        {
            return;
        }

        User user = GameProcessor.Inst.User;

        this.check = true;

        for (int i = 0; i < Config.ItemIdList.Length; i++)
        {
            int quality = Config.ItemQualityList[i];
            int MaxCount = Config.ItemCountList[i];

            long count = user.Bags.Where(m => (int)m.Item.Type == Config.ItemTypeList[i] && m.Item.ConfigId == Config.ItemIdList[i]).Select(m => m.MagicNubmer.Data).Sum();

            string color = "#00FF00";

            if (count < MaxCount)
            {
                color = "#FF0000";
                this.check = false;
            }

            TxtCountList[i].text = string.Format("<color={0}>({1}/{2})</color>", color, count, MaxCount);
        }
    }

    public void SetData(CompositeConfig config)
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

        GameProcessor.Inst.EventCenter.Raise(new CompositeEvent() { Config = Config });
    }

    public void OnUIFresh(CompositeUIFreshEvent e)
    {
        this.Check();
    }
}

