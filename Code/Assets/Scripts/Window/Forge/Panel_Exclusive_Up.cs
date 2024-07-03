using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Exclusive_Up : MonoBehaviour
{
    public ScrollRect sr_Panel;

    private List<Item_Exclusive> items = new List<Item_Exclusive>();

    public Button Btn_OK;

    private const int MaxCount = 6; //10件装备

    ExclusiveItem SelectExclusive;

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        this.Btn_OK.onClick.AddListener(OnClickOK);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<ExclusiveUpEvent>(this.OnSelect);
    }

    void OnEnable()
    {
        this.Load();
    }

    public void Init()
    {
        var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Empty");

        for (var i = 0; i < MaxCount; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.sr_Panel.content);
            empty.name = "Box_" + i;
        }
    }

    private void Load()
    {
        //把之前的卸载
        this.SelectExclusive = null;

        foreach (Item_Exclusive cb in items)
        {
            GameObject.Destroy(cb.gameObject);
        }
        items.Clear();

        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

        IDictionary<int, ExclusiveItem> dict = user.ExclusivePanelList[user.ExclusiveIndex];

        for (int BoxId = 0; BoxId < MaxCount; BoxId++)
        {
            int postion = BoxId + 15;

            var bagBox = this.sr_Panel.content.GetChild(BoxId);
            if (bagBox == null || !dict.ContainsKey(postion))
            {
                continue;
            }

            ExclusiveItem exclusive = dict[postion];

            if (exclusive.GetQuality() < 5)
            {
                continue;
            }

            Item_Exclusive box = this.CreateBox(exclusive, bagBox);
            this.items.Add(box);
        }


        this.Btn_OK.gameObject.SetActive(false);
    }

    private Item_Exclusive CreateBox(ExclusiveItem exclusive, Transform parent)
    {
        ToggleGroup toggleGroup = sr_Panel.GetComponent<ToggleGroup>();

        GameObject prefab = Resources.Load<GameObject>("Prefab/Window/Forge/Item_Exclusive");

        var go = GameObject.Instantiate(prefab);
        Item_Exclusive comItem = go.GetComponent<Item_Exclusive>();
        comItem.Init(exclusive, toggleGroup);

        comItem.transform.SetParent(parent);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void OnSelect(ExclusiveUpEvent e)
    {
        this.SelectExclusive = e.Exclusive;

        this.Show();
    }

    private void Show()
    {
        this.Btn_OK.gameObject.SetActive(false);



    }

    public void OnClickOK()
    {
        //this.SelectExclusive.Refesh(true);

        this.Show();
    }
}

