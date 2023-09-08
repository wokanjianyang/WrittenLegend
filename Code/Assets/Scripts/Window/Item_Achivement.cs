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

    private AchievementConfig Config = null;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Active.onClick.AddListener(Active);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItem(AchievementConfig config, bool active)
    {
        this.Config = config;
    }

    private void Active()
    {
        if (this.Config == null)
        {
            return;
        }

        int current = 0;
        if (current >= Config.Condition)
        {
            GameProcessor.Inst.User.EventCenter.Raise(new UserAchievementEvent() { Id = Config.Id });
            return;
        }
        else
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您还没有完成", ToastType = ToastTypeEnum.Failure });
            return;
        }
    }
}

