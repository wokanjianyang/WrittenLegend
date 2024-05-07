using Game;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapAnDian : MonoBehaviour, IBattleLife
{

    public Text Txt_Count;
    public Button btn_Level;
    public Text txt_Btn_Level;

    public Button btn_Offline;
    public Text txt_Offline;

    public ScrollRect sr_BattleMsg;

    [LabelText("退出")]
    public Button btn_Exit;

    private GameObject msgPrefab;
    private int msgId = 0;
    private List<Text> msgPool = new List<Text>();

    private long MapTime = 0;
    private int Level = 1;

    private int MaxLevel = 10;

    public int Order => (int)ComponentOrder.BattleRule;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Exit.onClick.AddListener(this.OnClick_Exit);
        this.btn_Level.onClick.AddListener(this.OnClick_Level);
        this.btn_Offline.onClick.AddListener(this.OnClick_Offline);
    }

    public void OnBattleStart()
    {
        this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_DropMsg");

        GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
        GameProcessor.Inst.EventCenter.AddListener<ShowAnDianInfoEvent>(this.OnShowAnDianInfo);
        GameProcessor.Inst.EventCenter.AddListener<AnDianStartEvent>(this.OnAnDianStart);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);

        this.gameObject.SetActive(false);
    }


    public void OnAnDianStart(AnDianStartEvent e)
    {
        StartCopy();
    }

    private void StartCopy()
    {
        this.gameObject.SetActive(true);
        this.MapTime = TimeHelper.ClientNowSeconds();

        User user = GameProcessor.Inst.User;

        int MapId = Math.Max(user.MapId - 1, ConfigHelper.MapStartId);

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("MapTime", MapTime);
        param.Add("MapId", MapId);

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.AnDian, this.transform, param);
            ShowLevel();
        });

        if (user.OffLineMapId > 0)
        {
            txt_Offline.text = "离线暗殿";
        }
        else
        {
            txt_Offline.text = "离线闯关";
        }
    }

    public void OnShowAnDianInfo(ShowAnDianInfoEvent e)
    {
        Txt_Count.text = "刷新统计：" + e.Count;
    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
        if (e.Type != RuleType.AnDian)
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
        if (e.Time == MapTime && e.Type == RuleType.AnDian)
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

    private void OnClick_Level()
    {
        Level = (Level % MaxLevel) + 1;
        ShowLevel();
    }

    private void ShowLevel()
    {
        txt_Btn_Level.text = "难度：" + Level;
        GameProcessor.Inst.EventCenter.Raise(new AnDianChangeLevel() { Level = this.Level });
    }

    private void OnClick_Offline()
    {
        User user = GameProcessor.Inst.User;
        if (user.OffLineMapId > 0)
        {
            user.OffLineMapId = 0;
            txt_Offline.text = "离线闯关";
        }
        else
        {
            int offLineMapId = user.MapId - MaxLevel;
            int maxMapId = MapConfigCategory.Instance.GetMaxMapId();
            if (offLineMapId < ConfigHelper.MapStartId || offLineMapId > maxMapId)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先通关副本", ToastType = ToastTypeEnum.Failure });
                return;
            }
            else
            {
                user.OffLineMapId = offLineMapId;
                txt_Offline.text = "离线暗殿";
            }
        }
        //Debug.Log("MapId:" + user.MapId);
        //Debug.Log("OffLineMapId:" + user.OffLineMapId);
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
