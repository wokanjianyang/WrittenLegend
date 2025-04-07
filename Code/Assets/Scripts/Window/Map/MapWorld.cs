using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapWorld : MonoBehaviour, IBattleLife
{
    public Text Txt_Name;
    public Text Txt_Time;

    public ScrollRect sr_BattleMsg;

    [LabelText("退出")]
    public Button btn_Exit;


    private GameObject msgPrefab;
    private int msgId = 0;
    private List<Text> msgPool = new List<Text>();

    private long MapTime = 14;

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
        GameProcessor.Inst.EventCenter.AddListener<ShowWorldInfoEvent>(this.OnShowInfo);
        GameProcessor.Inst.EventCenter.AddListener<WorldStartEvent>(this.OnStart);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);

        this.gameObject.SetActive(false);
    }


    public void OnStart(WorldStartEvent e)
    {
        this.gameObject.SetActive(true);

        int layer = GameProcessor.Inst.User.WorldData.GetLayer(e.Id);

        if (layer > ConfigHelper.MaxWorld)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已通关，请等下次", ToastType = ToastTypeEnum.Failure });
            return;
        }

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("MapId", e.Id);
        param.Add("Layer", layer);

        WorldConfig config = WorldConfigCategory.Instance.Get(e.Id);

        Txt_Name.text = config.MapName;
        Txt_Time.text = layer + "轮";

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.World, this.transform, param);
        });
    }

    public void OnShowInfo(ShowWorldInfoEvent e)
    {
        if (e.Layer < 5)
        {
            Txt_Time.text = "下波出怪时间：" + (e.Time);
        }
        else
        {
            Txt_Time.text = "击杀所有怪物通关";
        }
    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
        if (e.Type != RuleType.World)
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
        if (e.Time == MapTime && e.Type == RuleType.World)
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

        GameProcessor.Inst.EventCenter.Raise(new BattlerEndEvent() { Type = RuleType.World });

        GameProcessor.Inst.SetGameOver(PlayerType.Hero);
        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            var map = GameObject.Find("Canvas").GetComponentInChildren<ViewBattleProcessor>(true).transform;
            GameProcessor.Inst.LoadMap(RuleType.Normal, map, null);
        });
    }
}
