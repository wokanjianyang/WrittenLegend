using Game;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Dialog_Festive : MonoBehaviour
{
    public Text Txt_Title;
    public Text Txt_Total;
    public Text Txt_Time;

    public ScrollRect sr_Panel;
    private GameObject ItemPrefab;

    public Button Btn_Close;

    private List<Item_Festive> itemList = new List<Item_Festive>();
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
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Festive");

        List<FestiveConfig> list = FestiveConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (var config in list)
        {
            var Item = GameObject.Instantiate(ItemPrefab);
            Item.transform.SetParent(sr_Panel.content);
            Item.transform.localScale = Vector3.one;
            Item.gameObject.SetActive(true);

            Item_Festive com = Item.GetComponent<Item_Festive>();
            com.SetData(config);

            itemList.Add(com);
        }

        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(ItemHelper.SpecialId_Chunjie);
        this.Txt_Title.text = "��ǰӵ��" + itemConfig.Name + "������";

        DropLimitConfig dropLimit = DropLimitConfigCategory.Instance.Get(1);
        string startTime = DateTime.Parse(dropLimit.StartDate).ToString("M��d��");
        string endTime = DateTime.Parse(dropLimit.EndDate).AddDays(-1).ToString("M��d��");

        this.Txt_Time.text = string.Format("�����ʱ�� {0}00:00  �� {1}23:59  ", startTime, endTime);
    }

    private void ChangeAuto(bool isOn) {

        foreach (Item_Festive item in itemList) {
            item.ChangeAuto(isOn);
        }
    }

    private void OnFestiveUIFresh(FestiveUIFreshEvent e)
    {
        long count = GameProcessor.Inst.User.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();
        this.Txt_Total.text = count + " ��";
    }

    public void Open()
    {
        long count = GameProcessor.Inst.User.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie).Select(m => m.MagicNubmer.Data).Sum();
        this.Txt_Total.text = count + " ��";

        this.gameObject.SetActive(true);
    }

    private void OnClose()
    {
        this.gameObject.SetActive(false);
    }
}

