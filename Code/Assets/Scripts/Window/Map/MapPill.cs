using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPill : MonoBehaviour, IBattleLife
{
    public Text Txt_Name;
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
        GameProcessor.Inst.EventCenter.AddListener<ShowPillInfoEvent>(this.OnShowInfo);
        GameProcessor.Inst.EventCenter.AddListener<PillStartEvent>(this.OnStart);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);

        this.gameObject.SetActive(false);
    }


    public void OnStart(PillStartEvent e)
    {
        Debug.Log("PillStartEvent");

        this.gameObject.SetActive(true);

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("Layer", e.Layer);

        MonsterPillConfig config = MonsterPillConfigCategory.Instance.Get(e.Layer);
        Txt_Name.text = config.MapName;

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.Pill, this.transform, param);
        });
    }

    public void OnShowInfo(ShowPillInfoEvent e)
    {
        Txt_Count.text = "剩余挑战时间：" + e.Time;
    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
        if (e.Type != RuleType.Pill)
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
        if (e.Time == MapTime && e.Type == RuleType.Pill)
        {
            this.Exit();
        }
    }

    private void OnClick_Exit()
    {
        this.Exit();
    }

    private void Exit()
    {
        GameProcessor.Inst.OnDestroy();
        this.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new BattlerEndEvent() { Type = RuleType.Pill });

        GameProcessor.Inst.SetGameOver(PlayerType.Hero);
        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            var map = GameObject.Find("Canvas").GetComponentInChildren<ViewBattleProcessor>(true).transform;
            GameProcessor.Inst.LoadMap(RuleType.Normal, map, null);
        });
    }
}
