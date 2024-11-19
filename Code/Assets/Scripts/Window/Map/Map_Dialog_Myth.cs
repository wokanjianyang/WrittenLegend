using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Myth : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    public Button Btn_Close;

    private GameObject ItemPrefab;
    List<Map_Myth_Item> items = new List<Map_Myth_Item>();

    // Start is called before the first frame update
    void Start()
    {

        Btn_Close.onClick.AddListener(OnClick_Close);
        this.Init();
    }

    void OnEnable()
    {

    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;
        user.PillTime.Check(user.Cycle.Data);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Pill/Map_Pill_Item");

        List<MythConfig> list = MythConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        long num = Math.Min(user.Cycle.Data, list.Count);

        for (int i = 0; i < num; i++)
        {
            BuildItem(list[i]);
        }
    }

    private void BuildItem(MythConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Map_Myth_Item>();

        com.SetContent(config);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }


    public void OnClick_Start()
    {
        User user = GameProcessor.Inst.User;

        if (user.BabelCount.Data < 5)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "挑战次数不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.gameObject.SetActive(false);

        var vm = this.GetComponentInParent<ViewMore>();
        vm.StartBabel();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
