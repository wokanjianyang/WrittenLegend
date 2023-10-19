using Game;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDefend : MonoBehaviour, IBattleLife
{

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
        this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item_DropMsg");

        GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
        GameProcessor.Inst.EventCenter.AddListener<ShowDefendInfoEvent>(this.OnShowDefendInfo);
        GameProcessor.Inst.EventCenter.AddListener<DefendStartEvent>(this.OnDefendStart);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);

        this.gameObject.SetActive(false);
    }


    public void OnDefendStart(DefendStartEvent e)
    {
        StartCopy();
    }

    private void StartCopy()
    {
        this.gameObject.SetActive(true);


        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("progress", 1);
        param.Add("hp", 1000);

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.Defend, this.transform, param);
        });
    }

    public void OnShowDefendInfo(ShowDefendInfoEvent e)
    {
        Txt_Count.text = "进攻波数：" + e.Count;
    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
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
        if (e.Time == MapTime)
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
