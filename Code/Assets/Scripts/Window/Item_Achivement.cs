using Game;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Item_Achivement : MonoBehaviour
{
    public Text Txt_Name;
    public Text Txt_Attr;
    public Text Txt_Des;
    public Text Txt_Progress;

    public Button Btn_Active;

    public AchievementConfig Config { get; set; }
    private long Progress;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Active.onClick.AddListener(Active);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItem(AchievementConfig config, long progress, bool active)
    {
        this.Config = config;
        this.Progress = progress;

        Txt_Name.text = config.Name;

        Txt_Des.text = string.Format(config.Memo, config.Condition);


        if (active)
        {
            if (config.RewardType == 1)
            {
                Txt_Attr.text = "成就属性:" + StringHelper.FormatAttrText(config.AttrId, config.AttrValue);
            }
            else if (config.RewardType == 2)
            {
                Txt_Attr.text = "成就属性:装备技能套装上限 -1";
            }
            else if (config.RewardType == 3)
            {
                Txt_Attr.text = "成就属性:装备分解精炼石数量 + " + config.AttrValue;
            }
            else if (config.RewardType == 4)
            {
                Txt_Attr.text = "成就属性:魂环碎片掉落数量 + " + config.AttrValue;
            }
            else if (config.RewardType == 5)
            {
                Txt_Attr.text = "成就属性:离线闯关寻怪时间 - " + config.AttrValue;
            }
            else if (config.RewardType == (int)AchievementRewardType.Skill)
            {
                Txt_Attr.text = "成就属性:技能出战栏 + " + config.AttrValue;
            }

            Txt_Progress.gameObject.SetActive(false);
            Btn_Active.gameObject.SetActive(false);
        }
        else
        {
            Txt_Attr.text = "成就属性: ？ ？ ？";

            if (progress >= config.Condition)
            {
                Txt_Progress.text = string.Format("<color=#{0}>{1}</color>", "D8CAB0", progress + "/" + config.Condition);
                Btn_Active.gameObject.SetActive(true);
            }
            else
            {
                Txt_Progress.text = string.Format("<color=#{0}>{1}</color>", "FF0000", progress + "/" + config.Condition);
                Btn_Active.gameObject.SetActive(false);
            }
        }
    }

    private void Active()
    {
        if (this.Config == null)
        {
            return;
        }

        if (Progress >= Config.Condition)
        {
            GameProcessor.Inst.EventCenter.Raise(new ActiveAchievementEvent() { Id = Config.Id });
            return;
        }
        else
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您还没有完成", ToastType = ToastTypeEnum.Failure });
            return;
        }
    }
}

