using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Spirit : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    public Button Btn_Close;

    public Button Btn_Attr;
    public Dialog_Spirit DialogSpirit;

    public Button Btn_Offline;
    public Dialog_Spirit_Offline DialogSpiritOffline;

    public Text Txt_Require;
    public Toggle toggle_Auto;

    private GameObject ItemPrefab;
    List<Map_Spirit_Item> items = new List<Map_Spirit_Item>();

    // Start is called before the first frame update
    void Start()
    {

        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Attr.onClick.AddListener(OnClick_Attr);
        Btn_Offline.onClick.AddListener(OnClick_Offline);
        this.Init();

        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            AppHelper.Spirit_Auto = isOn;
        });
    }

    private void OnEnable()
    {
        this.ShowItemMax();
    }


    private void ShowItemMax()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }

        long total = user.AttributeBonus.GetTotalAttr(AttributeEnum.SpiritAll);

        Debug.Log("total:" + total);

        int nextRequire = SpiritCopyConfigCategory.Instance.GetAll().Select(m => m.Value.Require).Where(m => m > total).FirstOrDefault();

        Txt_Require.text = "当前英灵加成：" + total + "%，" + (nextRequire > 0 ? "下一个副本解锁需要：" + nextRequire + "%" : "已全部解锁副本");

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetMax(total);
        }
    }


    private void Init()
    {
        User user = GameProcessor.Inst.User;

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Spirit/Map_Spirit_Item");

        List<SpiritCopyConfig> list = SpiritCopyConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            BuildItem(list[i]);
        }

        this.ShowItemMax();
    }

    private void BuildItem(SpiritCopyConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Map_Spirit_Item>();

        com.SetContent(config);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    public void OnClick_Attr()
    {
        DialogSpirit.gameObject.SetActive(true);
    }

    public void OnClick_Offline()
    {
        DialogSpiritOffline.gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
