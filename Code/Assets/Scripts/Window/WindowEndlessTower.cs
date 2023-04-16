using Game;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowEndlessTower : MonoBehaviour, IBattleLife
{
    [Title("�޾���")]
    [LabelText("�˳�")]
    public Button btn_Exit;

    [LabelText("����")]
    public ScrollRect sr_BattleMsg;

    private bool isViewMapShowing = false;

    private GameObject msgPrefab;
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
    }

    private void OnShowTowerWindowEvent(ShowTowerWindowEvent msg)
    {
        this.gameObject.SetActive(true);

    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
        var msg = GameObject.Instantiate(this.msgPrefab);
        msg.transform.SetParent(this.sr_BattleMsg.content);
        msg.transform.localScale = Vector3.one;

        if (e.MsgType == MsgType.SecondExp)
        {
            msg.GetComponent<TextMeshProUGUI>().text = $"�����ݵ㾭��{e.Exp}";
            this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
            GameProcessor.Inst.EventCenter.Raise(new UpdateTowerWindowEvent());
        }
        else if (e.BattleType == BattleType.Tower)
        {
            string drops = "";
            if (e.Drops != null && e.Drops.Count > 0)
            {
                drops = ",����";
                foreach (var drop in e.Drops)
                {
                    drops += $"<color=#D800FF>[{drop.Name}]";
                }
            }
            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            msg.GetComponent<TextMeshProUGUI>().text = $"<color=white>�ݵ㾭��������:{e.Exp},�����{hero.TowerFloor}��";
            msg.name = $"msg_{e.RoundNum}";

            this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
            GameProcessor.Inst.EventCenter.Raise(new UpdateTowerWindowEvent());
        }
    }
}
