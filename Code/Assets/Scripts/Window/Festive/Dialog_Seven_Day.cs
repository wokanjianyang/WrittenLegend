using Game;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Dialog_Seven_Day : MonoBehaviour
{
    public Text Txt_Total;
    public Text Txt_Des;

    public ScrollRect sr_Panel;
    private GameObject ItemPrefab;

    public Button Btn_Close;

    private List<Item_Seven_Day> itemList = new List<Item_Seven_Day>();
    public Toggle Toggle_Auto;

    // Start is called before the first frame update
    void Start()
    {
        this.Init();
        this.Btn_Close.onClick.AddListener(OnClose);
        Toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            this.ChangeAuto(isOn);
        });

        GameProcessor.Inst.EventCenter.AddListener<FestiveUIFreshEvent>(this.OnFestiveUIFresh);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Seven_Day");

        List<SevenDayConfig> list = SevenDayConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (var config in list)
        {
            var Item = GameObject.Instantiate(ItemPrefab);
            Item.transform.SetParent(sr_Panel.content);
            Item.transform.localScale = Vector3.one;
            Item.gameObject.SetActive(true);

            Item_Seven_Day com = Item.GetComponent<Item_Seven_Day>();
            com.SetData(config);

            itemList.Add(com);
        }

        User user = GameProcessor.Inst.User;
        long day = (TimeHelper.ClientNowSeconds() - user.First_Create_Time) / 86400 + 1;

        day = 30 - day;

        this.Txt_Des.text = "开局七天，每天解锁一批奖励，" + day + "天之后关闭新手活动入口，请及时兑换完毕";
    }

    private void ChangeAuto(bool isOn)
    {

        foreach (Item_Seven_Day item in itemList)
        {
            item.ChangeAuto(isOn);
        }
    }

    private void OnFestiveUIFresh(FestiveUIFreshEvent e)
    {
        long count = GameProcessor.Inst.User.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();
        this.Txt_Total.text = count + " 个";

        foreach (Item_Seven_Day item in itemList)
        {
            item.ChangeAuto(Toggle_Auto.isOn);
        }
    }

    public void Open()
    {
        long count = GameProcessor.Inst.User.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();
        this.Txt_Total.text = count + " 个";

        this.gameObject.SetActive(true);
    }

    private void OnClose()
    {
        this.gameObject.SetActive(false);
    }
}

