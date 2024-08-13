using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Devour : MonoBehaviour
{
    public ScrollRect ds_Panel;

    public ScrollRect sr_Panel;

    private List<Box_Select> mainList = new List<Box_Select>();

    private List<Box_Select> sourceList = new List<Box_Select>();

    public Box_Ready Box_Ready_Main;
    public Box_Ready Box_Ready_Material;

    public List<Text> TxtCommissionNameList;
    public List<Text> TxtCommissionCountList;

    public List<Item_Rune_Suit> ItemList;
    public Item_Rune_Suit AddItem;

    public Button Btn_OK;

    private const int MaxMain = 6; //10件装备
    private const int MaxMaterial = 24;

    private bool check = false;
    private ExclusiveDevourConfig config = null;

    Box_Select SelectMain;
    Box_Select SelectMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        this.Btn_OK.onClick.AddListener(OnClickOK);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<BoxSelectEvent>(this.OnBoxSelect);
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


        for (var i = 0; i < MaxMaterial; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.sr_Panel.content);
            empty.name = "Src_" + i;
        }

        Box_Ready_Main.Init("主专属");
        Box_Ready_Material.Init("材料专属");
    }

    private void Load()
    {
        //把之前的卸载
        this.SelectMain = null;
        this.SelectMaterial = null;

        foreach (Box_Select cb in mainList)
        {
            GameObject.Destroy(cb.gameObject);
        }
        mainList.Clear();

        foreach (Box_Select sb in sourceList)
        {
            GameObject.Destroy(sb.gameObject);
        }
        sourceList.Clear();

        AddItem.gameObject.SetActive(false);
        foreach (Item_Rune_Suit item in ItemList)
        {
            item.gameObject.SetActive(false);
        }

        Box_Ready_Main.Down();
        Box_Ready_Material.Down();

        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

        int maxLevel = user.GetExclusiveLimit();

        IDictionary<int, ExclusiveItem> dict = user.ExclusivePanelList[user.ExclusiveIndex];

        for (int BoxId = 0; BoxId < MaxMain; BoxId++)
        {
            int postion = BoxId + 15;

            var bagBox = this.ds_Panel.content.GetChild(BoxId);
            if (bagBox == null || !dict.ContainsKey(postion))
            {
                continue;
            }

            ExclusiveItem exclusive = dict[postion];

            if (exclusive.GetQuality() < 5) //|| exclusive.GetLayer() >= maxLevel
            {
                continue;
            }

            BoxItem boxItem = new BoxItem();
            boxItem.Item = exclusive;
            boxItem.MagicNubmer.Data = 1;

            Box_Select box = PrefabHelper.Instance().CreateBoxSelect(bagBox, boxItem, ComBoxType.Exclusive_Devour_Main);
            this.mainList.Add(box);
        }

        for (int i = 0; i < TxtCommissionCountList.Count; i++)
        {
            TxtCommissionCountList[i].text = string.Format("<color={0}>({1}/{2})</color>", "#FF0000", 0, 0);
        }

        this.Btn_OK.gameObject.SetActive(false);
    }

    private void OnBoxSelect(BoxSelectEvent e)
    {
        if (e.Type == ComBoxType.Exclusive_Devour_Main)
        {
            ExclusiveItem exclusiveMain = e.Box.BoxItem.Item as ExclusiveItem;

            int maxLevel = GameProcessor.Inst.User.GetExclusiveLimit();

            if (exclusiveMain.GetLayer() >= maxLevel)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满阶了", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.SelectMain = e.Box;
            Box_Ready_Main.Up(e.Box.BoxItem);
            this.ShowMain();
        }
        else if (e.Type == ComBoxType.Exclusive_Devour_Material)
        {
            this.SelectMaterial = e.Box;
            Box_Ready_Material.Up(e.Box.BoxItem);

            this.ShowMaterial();
        }
    }

    private void ShowMain()
    {
        this.Btn_OK.gameObject.SetActive(false);

        foreach (Box_Select sb in sourceList)
        {
            GameObject.Destroy(sb.gameObject);
        }
        sourceList.Clear();

        ExclusiveItem exclusiveMain = SelectMain.BoxItem.Item as ExclusiveItem;

        int nextLayer = exclusiveMain.GetLayer();
        this.config = ExclusiveDevourConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Level == nextLayer).FirstOrDefault();

        this.Check();
        int maxLevel = GameProcessor.Inst.User.GetExclusiveLimit();

        if (nextLayer >= maxLevel) //不能超过上限
        {
            return;
        }

        ItemList[0].gameObject.SetActive(true);
        ItemList[0].SetItem(exclusiveMain.RuneConfigId, exclusiveMain.SuitConfigId);
        for (int i = 1; i < ItemList.Count; i++)
        {
            if (i > exclusiveMain.SuitConfigIdList.Count)
            {
                ItemList[i].gameObject.SetActive(false);
            }
            else
            {
                ItemList[i].gameObject.SetActive(true);
                ItemList[i].SetItem(exclusiveMain.RuneConfigIdList[i - 1], exclusiveMain.SuitConfigIdList[i - 1]);
            }
        }

        //选择符合条件的exclusive
        User user = GameProcessor.Inst.User;

        List<BoxItem> list = user.Bags.Where(m => m.Item.Type == ItemType.Exclusive && m.Item.GetQuality() == 5 && !m.Item.IsLock).ToList();
        //Debug.Log("es:" + list.Count);
        int BoxId = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (BoxId >= MaxMaterial)
            {
                return;
            }

            var bagBox = this.sr_Panel.content.GetChild(BoxId);

            BoxItem item = list[i];
            ExclusiveItem exclusive = item.Item as ExclusiveItem;
            if (exclusive.GetLayer() > 1 || exclusive.GetLevel() > 0)
            {
                continue;
            }

            Box_Select box = PrefabHelper.Instance().CreateBoxSelect(bagBox, item, ComBoxType.Exclusive_Devour_Material);
            this.sourceList.Add(box);

            BoxId++;
        }
    }

    private void ShowMaterial()
    {
        if (this.check)
        {
            this.Btn_OK.gameObject.SetActive(true);
        }

        ExclusiveItem exclusiveMaterial = SelectMaterial.BoxItem.Item as ExclusiveItem;

        AddItem.gameObject.SetActive(true);
        AddItem.SetItem(exclusiveMaterial.RuneConfigId, exclusiveMaterial.SuitConfigId);
    }
    private void Check()
    {
        User user = GameProcessor.Inst.User;

        this.check = true;

        int[] ItemIdList = config.ItemIdList;
        int[] ItemCountList = config.ItemCountList;
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

        ExclusiveItem exclusiveMain = SelectMain.BoxItem.Item as ExclusiveItem;
        ExclusiveItem exclusiveMaterial = SelectMaterial.BoxItem.Item as ExclusiveItem;

        //材料
        int[] ItemIdList = config.ItemIdList;
        int[] ItemCountList = config.ItemCountList;
        for (int i = 0; i < ItemIdList.Length; i++)
        {
            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = ItemIdList[i],
                Quantity = ItemCountList[i]
            });
        }

        //销毁
        GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent()
        {
            BoxItem = SelectMaterial.BoxItem
        });

        Box_Ready_Material.Down(); //销毁已选
        sourceList.Remove(SelectMaterial);//移除包裹
        GameObject.Destroy(SelectMaterial.gameObject); //销毁包裹
        SelectMaterial = null;

        exclusiveMain.Devour(exclusiveMaterial);

        this.Load();

        GameProcessor.Inst.SaveData();
    }
}

