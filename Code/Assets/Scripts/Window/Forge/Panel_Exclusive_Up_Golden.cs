using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Exclusive_Up_Golden : MonoBehaviour
{
    public ScrollRect ds_Panel;
    public Transform Tf_Attr;

    private List<Item_Forge_Main> items = new List<Item_Forge_Main>();

    private List<StrenthAttrItem> AttrList;

    public List<Text> TxtCommissionNameList;
    public List<Text> TxtCommissionCountList;

    public Button Btn_OK;

    private const int MaxMain = 6; //10件装备
    private const int StartPosition = 1007;

    private bool check = false;
    private int[] ItemIdList = null;
    private int[] ItemCountList = null;
    private const int MaxLevel = 20;

    private Item_Forge_Main SelectMain;

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        AttrList = Tf_Attr.GetComponentsInChildren<StrenthAttrItem>().ToList();

        this.Btn_OK.onClick.AddListener(OnClickOK);
    }

    // Update is called once per frame
    void Start()
    {
        //GameProcessor.Inst.EventCenter.AddListener<BoxSelectEvent>(this.OnBoxSelect);
        ExclusiveDevourConfig config = ExclusiveDevourConfigCategory.Instance.GetByCycleAndLevel(2, 1);

        ItemIdList = config.UpItemIdList;
        ItemCountList = config.UpItemCountList;
    }

    void OnEnable()
    {
        this.Load();
    }

    public void Init()
    {
        var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Empty");

        for (var i = 0; i < MaxMain; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.ds_Panel.content);
            empty.name = "Des_" + i;
        }
    }

    private void Load()
    {
        //把之前的卸载
        this.SelectMain = null;

        foreach (Item_Forge_Main cb in items)
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

        for (int BoxId = 0; BoxId < MaxMain; BoxId++)
        {
            int postion = BoxId + StartPosition;

            var bagBox = this.ds_Panel.content.GetChild(BoxId);
            if (bagBox == null || !dict.ContainsKey(postion))
            {
                continue;
            }

            ExclusiveItem exclusive = dict[postion];

            if (exclusive.GetQuality() < 7 || exclusive.GetLayer() <= 2)
            {
                continue;
            }

            Item_Forge_Main box = this.CreateItem(exclusive, bagBox, BoxId);
            this.items.Add(box);
        }


        this.Btn_OK.gameObject.SetActive(false);
    }

    private Item_Forge_Main CreateItem(ExclusiveItem exclusive, Transform parent, int index)
    {
        ToggleGroup toggleGroup = ds_Panel.GetComponent<ToggleGroup>();

        GameObject prefab = Resources.Load<GameObject>("Prefab/Window/Forge/Item_Forge_Main");

        var go = GameObject.Instantiate(prefab);
        Item_Forge_Main comItem = go.GetComponent<Item_Forge_Main>();
        comItem.Init(exclusive, toggleGroup);
        comItem.AddListener(OnSelectMain);

        comItem.transform.SetParent(parent);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void OnSelectMain(Item_Forge_Main item)
    {
        this.SelectMain = item;

        this.ShowMain();
    }

    private void ShowMain()
    {
        this.Btn_OK.gameObject.SetActive(false);

        if (SelectMain == null)
        {
            return;
        }

        SelectMain.Refresh();

        for (int i = 0; i < ItemIdList.Length; i++)
        {
            string color = "#FF0000";
            TxtCommissionCountList[i].text = string.Format("<color={0}>({1}/{2})</color>", color, 0, 0);
        }

        ExclusiveItem exclusiveMain = SelectMain.GameItem as ExclusiveItem;

        ExclusiveAttrConfig attrConfig = ExclusiveAttrConfigCategory.Instance.GetAttr(exclusiveMain.ExclusiveConfig.Cycle, exclusiveMain.GetLayer());

        IDictionary<int, long> ExclusiveAttrList = exclusiveMain.GetBaseAttrList();

        for (int i = 0; i < AttrList.Count; i++)
        {
            if (i >= attrConfig.AttrIdList.Length)
            {
                AttrList[i].gameObject.SetActive(false);
            }
            else
            {
                AttrList[i].gameObject.SetActive(true);

                int attrId = attrConfig.AttrIdList[i];
                AttrList[i].SetContent(attrId, ExclusiveAttrList[attrId], attrConfig.AttchValueList[i]);
            }
        }

        if (exclusiveMain.GetLevel() >= MaxLevel) //最高99级
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满级了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.Check();
        if (this.check)
        {
            this.Btn_OK.gameObject.SetActive(true);
        }
    }

    private void Check()
    {
        User user = GameProcessor.Inst.User;

        this.check = true;

        for (int i = 0; i < ItemIdList.Length; i++)
        {
            int MaxCount = ItemCountList[i];

            long count = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemIdList[i]).Select(m => m.MagicNubmer.Data).Sum();

            string color = "#00FF00";

            if (count < MaxCount)
            {
                color = "#FF0000";
                this.check = false;
            }

            TxtCommissionCountList[i].text = string.Format("<color={0}>({1}/{2})</color>", color, count, MaxCount);
        }
    }
    public void OnClickOK()
    {
        this.Btn_OK.gameObject.SetActive(false);

        this.Check();

        if (!check)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        ExclusiveItem exclusiveMain = SelectMain.GameItem as ExclusiveItem;

        //材料
        for (int i = 0; i < ItemIdList.Length; i++)
        {
            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = ItemIdList[i],
                Quantity = ItemCountList[i]
            });
        }

        exclusiveMain.UpNew();

        this.ShowMain();

        GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        GameProcessor.Inst.SaveData();
    }
}

