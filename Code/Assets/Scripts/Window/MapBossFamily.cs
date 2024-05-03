using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBossFamily : MonoBehaviour, IBattleLife
{

    public Text Txt_Count;

    public ScrollRect sr_BattleMsg;

    [LabelText("退出")]
    public Button btn_Exit;

    public Button btn_Stop;
    public Text txt_Stop;

    private GameObject msgPrefab;
    private int msgId = 0;
    private List<Text> msgPool = new List<Text>();

    private long MapTime = 0;
    private int MapLevel = 0;
    private int MapRate = 0;

    public int Order => (int)ComponentOrder.BattleRule;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Exit.onClick.AddListener(this.OnClick_Exit);
        this.btn_Stop.onClick.AddListener(this.OnClick_Stop);
    }

    public void OnBattleStart()
    {
        this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_DropMsg");

        GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
        GameProcessor.Inst.EventCenter.AddListener<BossFamilyStartEvent>(this.OnBossFamilyStart);
        GameProcessor.Inst.EventCenter.AddListener<ShowBossFamilyInfoEvent>(this.ShowBossFamilyInfo);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);
        GameProcessor.Inst.EventCenter.AddListener<AutoStartBossFamily>(this.OnAutoStart);

        this.gameObject.SetActive(false);
    }


    public void OnBossFamilyStart(BossFamilyStartEvent e)
    {
        this.MapLevel = e.Level;
        this.MapRate = e.Rate;
        StartCopy();
    }

    public void OnAutoStart(AutoStartBossFamily e)
    {
        User user = GameProcessor.Inst.User;

        long bossTicket = user.GetMaterialCount(ItemHelper.SpecialId_Boss_Ticket);

        if (bossTicket < this.MapRate)
        {
            Debug.Log("Boss MapRate:" + this.MapRate);
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的BOSS挑战卷", ToastType = ToastTypeEnum.Failure });
            return;
        }

        GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
        {
            Type = ItemType.Material,
            ItemId = ItemHelper.SpecialId_Boss_Ticket,
            Quantity = this.MapRate
        });

        long newTicket = user.GetMaterialCount(ItemHelper.SpecialId_Boss_Ticket);
        if (newTicket >= bossTicket)
        {
            GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
        }

        user.MagicRecord[AchievementSourceType.BossFamily].Data += this.MapRate;

        StartCopy();
    }

    private void StartCopy()
    {
        this.gameObject.SetActive(true);
        if (GameProcessor.Inst.EquipBossFamily_Auto)
        {
            txt_Stop.text = "自动中...";
        }
        else {
            txt_Stop.text = "不自动";
        }

        this.MapTime = TimeHelper.ClientNowSeconds();

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("MapTime", MapTime);
        param.Add("MapLevel", this.MapLevel);
        param.Add("MapRate", this.MapRate);

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.BossFamily, this.transform, param);
        });
    }

    public void ShowBossFamilyInfo(ShowBossFamilyInfoEvent e)
    {
        Txt_Count.text = "剩余BOSS：" + e.Count;
    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
        if (e.Type != RuleType.BossFamily)
        {
            return;
        }

        msgId++;
        Text txt_msg = null;
        if (this.sr_BattleMsg.content.childCount > 50)
        {
            txt_msg = msgPool[0];
            msgPool.RemoveAt(0);
            txt_msg.transform.SetAsLastSibling();
        }
        else
        {
            var msg = GameObject.Instantiate(this.msgPrefab);
            msg.transform.SetParent(this.sr_BattleMsg.content);
            msg.transform.localScale = Vector3.one;

            var m = msg.GetComponent<Text>();

            txt_msg = m;
        }
        msgPool.Add(txt_msg);

        txt_msg.gameObject.name = $"msg_{msgId}";
        txt_msg.text = e.Message;
        this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
    }

    private void OnBattleLoseEvent(BattleLoseEvent e)
    {
        if (e.Time == MapTime && e.Type == RuleType.BossFamily)
        {
            this.Exit();
        }
    }

    private void OnClick_Exit()
    {
        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认退出？", true, () =>
         {
             this.Exit();
         }, null);
    }
    private void OnClick_Stop()
    {
        if (GameProcessor.Inst.EquipBossFamily_Auto)
        {
            GameProcessor.Inst.EquipBossFamily_Auto = false;
            txt_Stop.text = "不自动";
        }
        else
        {
            GameProcessor.Inst.EquipBossFamily_Auto = true;
            txt_Stop.text = "自动中...";
        }
    }

    private void Exit()
    {
        GameProcessor.Inst.OnDestroy();
        this.gameObject.SetActive(false);
        GameProcessor.Inst.EventCenter.Raise(new BossFamilyEndEvent());
        GameProcessor.Inst.SetGameOver(PlayerType.Hero);
        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            var map = GameObject.Find("Canvas").GetComponentInChildren<ViewBattleProcessor>(true).transform;
            GameProcessor.Inst.LoadMap(RuleType.Normal, map, null);
        });
    }
}
