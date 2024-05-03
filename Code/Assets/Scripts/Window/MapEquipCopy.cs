using Game;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEquipCopy : MonoBehaviour, IBattleLife
{
    [LabelText("退出")]
    public Button btn_Exit;

    [LabelText("掉落")]
    public ScrollRect sr_BattleMsg;

    [LabelText("地图名称")]
    public Text txt_FloorName;

    public Button btn_Stop;
    public Text txt_Stop;

    public Text TxtMc1;

    public Text TxtMc2;

    public Text TxtMc3;

    public Text TxtMc4;

    public Text TxtMc5;

    private GameObject msgPrefab;
    private List<Text> msgPool = new List<Text>();
    private int msgId = 0;

    private int CopyMapId = 0;
    private long MapTime = 0;

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
        GameProcessor.Inst.EventCenter.AddListener<StartCopyEvent>(this.OnStartCopy);
        GameProcessor.Inst.EventCenter.AddListener<ShowCopyInfoEvent>(this.OnShowCopyInfoEvent);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);
        GameProcessor.Inst.EventCenter.AddListener<AutoStartCopyEvent>(this.OnAutoStartCopyEvent);

        //ShowMapInfo();
        this.gameObject.SetActive(false);
    }
    private void ShowMapInfo(int rate)
    {
        User user = GameProcessor.Inst.User;

        long oldData = user.MagicCopyTikerCount.Data;

        user.MagicCopyTikerCount.Data -= Math.Abs(rate);

        user.SetAchievementProgeress(AchievementSourceType.EquipCopy, rate);

        long newData = user.MagicCopyTikerCount.Data;
        if (newData >= oldData)
        {
            GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
        }

        MapConfig config = MapConfigCategory.Instance.Get(this.CopyMapId);
        txt_FloorName.text = config.Name;
    }

    public void OnStartCopy(StartCopyEvent e)
    {
        this.gameObject.SetActive(true);
        if (GameProcessor.Inst.EquipBossFamily_Auto)
        {
            txt_Stop.text = "自动中...";
        }
        else
        {
            txt_Stop.text = "不自动";
        }

        this.CopyMapId = e.MapId;
        this.MapTime = TimeHelper.ClientNowSeconds();

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("MapId", e.MapId);
        param.Add("MapTime", MapTime);
        param.Add("MapRate", e.Rate);

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.EquipCopy, this.transform, param);
        });

        ShowMapInfo(e.Rate);
    }

    public void OnAutoStartCopyEvent(AutoStartCopyEvent e)
    {
        if (GameProcessor.Inst.User.MagicCopyTikerCount.Data <= 0)
        {
            return;
        }

        int rate = GameProcessor.Inst.EquipCopySetting_Rate ? 5 : 1;

        this.gameObject.SetActive(true);
        this.MapTime = TimeHelper.ClientNowSeconds();

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("MapId", this.CopyMapId);
        param.Add("MapTime", MapTime);
        param.Add("MapRate", rate); //自动是1倍

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.EquipCopy, this.transform, param);
        });

        ShowMapInfo(rate);
    }

    public void OnShowCopyInfoEvent(ShowCopyInfoEvent e)
    {
        TxtMc1.text = "剩余小怪：" + e.Mc1;
        TxtMc2.text = "剩余精英：" + e.Mc2;
        TxtMc3.text = "剩余头目：" + e.Mc3;
        TxtMc4.text = "剩余首领：" + e.Mc4;
        TxtMc5.text = "剩余Boss：" + e.Mc5;
    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
        if (e.Type != RuleType.EquipCopy)
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


        //var msg = GameObject.Instantiate(this.msgPrefab);
        //msg.transform.SetParent(this.sr_BattleMsg.content);
        //msg.transform.localScale = Vector3.one;

        //msg.GetComponent<Text>().text =e.Message;
        //this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
        //GameProcessor.Inst.EventCenter.Raise(new UpdateTowerWindowEvent());
    }

    private void OnBattleLoseEvent(BattleLoseEvent e)
    {
        if (MapTime == e.Time && e.Type == RuleType.EquipCopy)
        {
            this.Exit();
        }
    }

    private void OnClick_Stop()
    {
        if (GameProcessor.Inst.EquipCopySetting_Auto)
        {
            GameProcessor.Inst.EquipCopySetting_Auto = false;
            txt_Stop.text = "不自动";
        }
        else
        {
            GameProcessor.Inst.EquipCopySetting_Auto = true;
            txt_Stop.text = "自动中...";
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
        GameProcessor.Inst.EventCenter.Raise(new EndCopyEvent());
        GameProcessor.Inst.SetGameOver(PlayerType.Hero);
        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            var map = GameObject.Find("Canvas").GetComponentInChildren<ViewBattleProcessor>(true).transform;
            GameProcessor.Inst.LoadMap(RuleType.Normal, map, null);
        });
    }
}
