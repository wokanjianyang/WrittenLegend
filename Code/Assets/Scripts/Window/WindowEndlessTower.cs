using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowEndlessTower : MonoBehaviour, IBattleLife
{
    [Title("ÎÞ¾¡Ëþ")]
    [LabelText("ÍË³ö")]
    public Button btn_Exit;

    [LabelText("µôÂä")]
    public ScrollRect sr_BattleMsg;

    [LabelText("µØÍ¼Ãû³Æ")]
    public Text txt_FloorName;

    public Text TxtMc1;

    public Text TxtMc2;

    public Text TxtMc3;

    public Text TxtMc4;

    public Text TxtMc5;

    private bool isViewMapShowing = false;

    private GameObject msgPrefab;
    private List<Text> msgPool = new List<Text>();
    private int msgId = 0;

    private int CopyMapId = 0;

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
        GameProcessor.Inst.EventCenter.AddListener<StartCopyEvent>(this.OnStartCopy);
        GameProcessor.Inst.EventCenter.AddListener<ShowCopyInfoEvent>(this.OnShowCopyInfoEvent);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);
        //ShowMapInfo();
    }
    private void ShowMapInfo()
    {
        User user = GameProcessor.Inst.User;
        if (user != null)
        {
            MapConfig config = MapConfigCategory.Instance.Get(CopyMapId);
            txt_FloorName.text = config.Name;
        }
    }

    public void OnStartCopy(StartCopyEvent e)
    {
        this.CopyMapId = e.MapId;
        this.gameObject.SetActive(true);

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.Tower, 0, CopyMapId, this.transform);
        });

        ShowMapInfo();
    }

    public void OnShowCopyInfoEvent(ShowCopyInfoEvent e)
    {
        TxtMc1.text = "Ê£ÓàÐ¡¹Ö£º" + e.Mc1;
        TxtMc2.text = "Ê£Óà¾«Ó¢£º" + e.Mc2;
        TxtMc3.text = "Ê£ÓàÍ·Ä¿£º" + e.Mc3;
        TxtMc4.text = "Ê£ÓàÊ×Áì£º" + e.Mc4;
        TxtMc5.text = "Ê£ÓàBoss£º" + e.Mc5;
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


        //var msg = GameObject.Instantiate(this.msgPrefab);
        //msg.transform.SetParent(this.sr_BattleMsg.content);
        //msg.transform.localScale = Vector3.one;

        //msg.GetComponent<Text>().text =e.Message;
        //this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
        //GameProcessor.Inst.EventCenter.Raise(new UpdateTowerWindowEvent());
    }
    
    private void OnBattleLoseEvent(BattleLoseEvent msg)
    {
        this.OnClick_Exit();
    }

    private void OnClick_Exit()
    {
        GameProcessor.Inst.OnDestroy();
        this.gameObject.SetActive(false);
        GameProcessor.Inst.EventCenter.Raise(new EndCopyEvent());
        GameProcessor.Inst.SetGameOver(PlayerType.Hero);
        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            var map = GameObject.Find("Canvas").GetComponentInChildren<ViewBattleProcessor>(true).transform;
            GameProcessor.Inst.LoadMap(RuleType.Normal, 0, 0, map);
        });
    }
}
