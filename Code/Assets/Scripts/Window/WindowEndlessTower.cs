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

    private bool isViewMapShowing = false;

    private GameObject msgPrefab;
    private List<Text> msgPool = new List<Text>();
    private int msgId = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.btn_Exit.onClick.AddListener(this.OnClick_Exit);
    }


    private void OnClick_Exit()
    {
        this.gameObject.SetActive(false);
        GameProcessor.Inst.OnDestroy();
        var map = GameObject.Find("Canvas").GetComponentInChildren<ViewBattleProcessor>(true).transform;
        GameProcessor.Inst.LoadMap(RuleType.Normal, 0, map);
    }
    public int Order => (int)ComponentOrder.Window;

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<ShowTowerWindowEvent>(this.OnShowTowerWindowEvent);

        this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item_DropMsg");

        GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
        GameProcessor.Inst.EventCenter.AddListener<ChangeFloorEvent>(this.OnChangeFloorEvent);

        ShowName();
    }

    private void OnShowTowerWindowEvent(ShowTowerWindowEvent msg)
    {
        this.gameObject.SetActive(true);

    }

    private void OnChangeFloorEvent(ChangeFloorEvent e) {
        ShowName();
    }

    private void ShowName()
    {
        User user = GameProcessor.Inst.User;

        if (user != null)
        {
            int mapName = user.TowerFloor;
            txt_FloorName.text = mapName + "²ã";
        }

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
}
