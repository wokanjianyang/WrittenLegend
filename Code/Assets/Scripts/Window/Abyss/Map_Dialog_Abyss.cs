using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Map_Dialog_Abyss : MonoBehaviour
{
    public int Order => (int)ComponentOrder.Dialog;

    public ScrollRect sr_Boss;
    public Button Btn_Close;

    public Button Btn_Attr;
    public Dialog_Abyss Dlg_Abyss;

    public Button Btn_Offline;
    public Dialog_Abyss_Offline Dlg_Offline;

    public Text Txt_Require;
    public Toggle toggle_Auto;

    public Transform tf_tgs;
    private List<Toggle> toggles;
    private int Type = 1;

    private GameObject ItemPrefab;
    List<Map_Abyss_Item> items = new List<Map_Abyss_Item>();


    // Start is called before the first frame update
    void Start()
    {
        toggles = tf_tgs.GetComponentsInChildren<Toggle>().ToList();

        Btn_Close.onClick.AddListener(OnClick_Close);
        //Btn_Attr.onClick.AddListener(OnClick_Attr);
        //Btn_Offline.onClick.AddListener(OnClick_Offline);

        this.Init();

        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            AppHelper.Abyss_Auto = isOn;
        });

        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i + 1;


            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                this.ShowPanel(index);
            });

        }

        this.ShowItemMax();
    }

    private void OnEnable()
    {
        this.ShowItemMax();
    }

    private void ShowPanel(int type)
    {
        this.Type = type;
        ShowItemMax();
    }

    private void ShowItemMax()
    {
        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Show(Type);
        }
    }


    private void Init()
    {
        User user = GameProcessor.Inst.User;

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Abyss/Map_Abyss_Item");

        List<AbyssCopyConfig> list = AbyssCopyConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            BuildItem(list[i]);
        }

        this.ShowItemMax();
    }

    private void BuildItem(AbyssCopyConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Map_Abyss_Item>();

        com.SetContent(config);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    public void OnClick_Attr()
    {
        Dlg_Abyss.gameObject.SetActive(true);
    }

    public void OnClick_Offline()
    {
        Dlg_Offline.gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
