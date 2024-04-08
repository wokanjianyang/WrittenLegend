using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPhantom : MonoBehaviour, IBattleLife
{
    [LabelText("地图名称")]
    public Text Txt_Name;

    public Text Txt_Time;

    public ScrollRect sr_BattleMsg;

    [LabelText("退出")]
    public Button btn_Exit;

    private GameObject msgPrefab;
    private List<Text> msgPool = new List<Text>();
    private int msgId = 0;

    private int PhantomId = 0;
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
        GameProcessor.Inst.EventCenter.AddListener<PhantomStartEvent>(this.OnPhantomStart);
        GameProcessor.Inst.EventCenter.AddListener<ShowPhantomInfoEvent>(this.OnShowPhantomInfoEvent);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);

        this.gameObject.SetActive(false);
    }


    public void OnPhantomStart(PhantomStartEvent e)
    {
        this.PhantomId = e.PhantomId;
        this.gameObject.SetActive(true);

        this.MapTime = TimeHelper.ClientNowSeconds();

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("PhantomId", PhantomId);
        param.Add("MapTime", MapTime);

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.Phantom, this.transform, param);
        });

        PhantomConfig config = PhantomConfigCategory.Instance.Get(PhantomId);

        Txt_Name.text = config.Name;
    }

    public void OnShowPhantomInfoEvent(ShowPhantomInfoEvent e)
    {
        Txt_Time.text = "剩余时间：" + e.Time;
    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
        if (e.Type != RuleType.Phantom)
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
        if (e.Time == MapTime && e.Type == RuleType.Phantom)
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
        GameProcessor.Inst.EventCenter.Raise(new PhantomEndEvent());
        GameProcessor.Inst.SetGameOver(PlayerType.Hero);
        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            var map = GameObject.Find("Canvas").GetComponentInChildren<ViewBattleProcessor>(true).transform;
            GameProcessor.Inst.LoadMap(RuleType.Normal, map, null);
        });
    }
}
