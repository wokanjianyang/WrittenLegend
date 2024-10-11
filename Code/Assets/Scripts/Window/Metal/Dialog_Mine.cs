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
    public Button Btn_Info;

    public ScrollRect sr_BattleMsg;
    private GameObject msgPrefab;
    private int msgId = 0;
    private List<Text> msgPool = new List<Text>();

    public Dialog_Metal DialogMetal;

    public Text Txt_Level1;
    public Text Txt_LEvel2;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Full.onClick.AddListener(OnClick_Close);
        Btn_Info.onClick.AddListener(OnShowInfo);
    }

    void OnEnable()
    {
        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

        int levelN = user.GetLimitMineCount();

        Txt_Level1.text = "Level:" + levelN;

        int levelS = user.GetLimitMineCount2();

        Txt_LEvel2.text = "Level:" + levelS;
    }

    public void OnBattleStart()
    {
        this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_DropMsg");
        GameProcessor.Inst.EventCenter.AddListener<MineMsgEvent>(this.ShowMsg);

        GameProcessor.Inst.EventCenter.AddListener<OpenMineEvent>(this.OpenMineEvent);
    }

    private void OpenMineEvent(OpenMineEvent e)
    {
        this.gameObject.SetActive(true);
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
