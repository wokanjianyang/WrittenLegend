using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInfinite : MonoBehaviour, IBattleLife
{

    public Text Txt_Level;
    public Text Txt_Count;

    public ScrollRect sr_BattleMsg;

    [LabelText("退出")]
    public Button btn_Exit;


    private GameObject msgPrefab;
    private int msgId = 0;
    private List<Text> msgPool = new List<Text>();

    private long MapTime = 0;

    public int Order => (int)ComponentOrder.BattleRule;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Exit.onClick.AddListener(this.OnClick_Exit);
    }

    public void OnBattleStart()
    {
        this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_DropMsg");

        GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
        GameProcessor.Inst.EventCenter.AddListener<ShowInfiniteInfoEvent>(this.OnShowInfo);
        GameProcessor.Inst.EventCenter.AddListener<InfiniteStartEvent>(this.OnInfiniteStart);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);

        this.gameObject.SetActive(false);
    }


    public void OnInfiniteStart(InfiniteStartEvent e)
    {
        StartCopy();
    }

    private void StartCopy()
    {
        this.gameObject.SetActive(true);

        User user = GameProcessor.Inst.User;

        InfiniteRecord record = user.InfiniteData.GetCurrentRecord();

        if (record == null)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有了挑战次数", ToastType = ToastTypeEnum.Failure });
            return;
        }

        record.Count.Data--;

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("progress", record.Progress.Data);
        param.Add("count", record.Count.Data);

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.Infinite, this.transform, param);
        });
    }

    public void OnShowInfo(ShowInfiniteInfoEvent e)
    {
        Txt_Level.text = "挑战波数：" + e.Count;
        Txt_Count.text = "重生次数：" + e.PauseCount;
    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
        if (e.Type != RuleType.Infinite)
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
        if (e.Time == MapTime && e.Type == RuleType.Infinite)
        {
            this.Exit();
        }
    }

    private void OnClick_Exit()
    {
        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("退出会消耗一次重生次数,是否退出？", true, () =>
          {
              this.Exit();
          }, null);
    }

    private void Exit()
    {
        GameProcessor.Inst.OnDestroy();
        this.gameObject.SetActive(false);
        GameProcessor.Inst.EventCenter.Raise(new DefendEndEvent());
        GameProcessor.Inst.SetGameOver(PlayerType.Hero);
        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            var map = GameObject.Find("Canvas").GetComponentInChildren<ViewBattleProcessor>(true).transform;
            GameProcessor.Inst.LoadMap(RuleType.Normal, map, null);
        });
    }
}
