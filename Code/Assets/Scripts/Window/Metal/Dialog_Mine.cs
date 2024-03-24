using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Mine : MonoBehaviour, IBattleLife
{

    public Button Btn_Full;
    public Button Btn_Close;
    public Button Btn_Add;
    public Button Btn_Info;
    public Text Txt_Info;

    public Transform Tf_Miner;
    private GameObject Pab_Miner;
    List<MinerUI> miners = new List<MinerUI>();

    public ScrollRect sr_BattleMsg;
    private GameObject msgPrefab;
    private int msgId = 0;
    private List<Text> msgPool = new List<Text>();

    public Dialog_Metal DialogMetal;

    public int Order => (int)ComponentOrder.BattleRule;

    void Awake()
    {
        this.Pab_Miner = Resources.Load<GameObject>("Prefab/Window/More/MinerUI");
    }

    // Start is called before the first frame update
    void Start()
    {
        Btn_Full.onClick.AddListener(OnClick_Close);
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Add.onClick.AddListener(OnAdd);
        Btn_Info.onClick.AddListener(OnShowInfo);

        Init();
    }

    public void OnBattleStart()
    {
        this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_DropMsg");
        GameProcessor.Inst.EventCenter.AddListener<MineMsgEvent>(this.ShowMsg);
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        Debug.Log("MinerList Count" + user.MinerList.Count);

        long maxCount = user.GetLimitMineCount();

        if (user.MinerList.Count < maxCount)
        {
            this.Btn_Add.gameObject.SetActive(true);
            this.Txt_Info.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_Add.gameObject.SetActive(false);
            this.Txt_Info.gameObject.SetActive(true);
        }

        for (int i = 0; i < user.MinerList.Count; i++)
        {
            var item = GameObject.Instantiate(Pab_Miner);

            MinerUI com = item.GetComponentInChildren<MinerUI>();

            item.transform.SetParent(this.Tf_Miner);
            item.transform.localScale = Vector3.one;

            miners.Add(com);
        }
    }

    private void OnAdd()
    {
        this.Btn_Add.gameObject.SetActive(false);

        User user = GameProcessor.Inst.User;

        long currentCount = user.MinerList.Count;
        long maxCount = user.GetLimitMineCount();

        for (long i = currentCount + 1; i <= maxCount; i++)
        {
            Miner miner = new Miner();
            miner.Init("矿工");

            user.MinerList.Add(miner);

            var item = GameObject.Instantiate(Pab_Miner);
            MinerUI com = item.GetComponentInChildren<MinerUI>();
            item.transform.SetParent(this.Tf_Miner);
            item.transform.localScale = Vector3.one;
            miners.Add(com);
        }


        string message = "一共领取了" + (maxCount - currentCount) + "个矿工";
        GameProcessor.Inst.EventCenter.Raise(new MineMsgEvent() { Message = message });
    }

    public void OnShowInfo()
    {
        DialogMetal.Show();
    }

    private void ShowMsg(MineMsgEvent e)
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

    public void OnClick_Close()
    {
        Debug.Log("Dialog_Mine Close");
        this.gameObject.SetActive(false);
    }
}
