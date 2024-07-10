using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Exclusive_Up : MonoBehaviour
{
    public ScrollRect ds_Panel;

    public ScrollRect sr_Panel;

    private List<Box_Select> items = new List<Box_Select>();

    private List<Box_Select> sourceList = new List<Box_Select>();

    public List<Attr_Exclusive_Up> levelList = new List<Attr_Exclusive_Up>();

    public Box_Ready Box_Ready_Main;
    public Box_Ready Box_Ready_Material;

    public List<Text> TxtCommissionNameList;
    public List<Text> TxtCommissionCountList;

    public Button Btn_OK;

    private const int MaxMain = 6; //10件装备
    private const int MaxMaterial = 24;

    private bool check = false;
    private int[] ItemIdList = new int[] { ItemHelper.SpecialId_Exclusive_Stone, ItemHelper.SpecialId_Exclusive_Heart };
    private int[] ItemCountList = new int[] { 50, 5 };

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

        foreach (Box_Select cb in items)
        {
            GameObject.Destroy(cb.gameObject);
        }
        items.Clear();

        foreach (Box_Select sb in sourceList)
        {
            GameObject.Destroy(sb.gameObject);
        }
        sourceList.Clear();

        foreach (Attr_Exclusive_Up item in levelList)
        {
            item.Clear();
        }

        Box_Ready_Main.Down();
        Box_Ready_Material.Down();

        User user = GameProcessor.Inst.User;
        if (user == null)
        {
            return;
        }

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

            if (exclusive.GetQuality() < 5)
            {
                continue;
            }

            BoxItem boxItem = new BoxItem();
            boxItem.Item = exclusive;
            boxItem.MagicNubmer.Data = 1;

            Box_Select box = PrefabHelper.Instance().CreateBoxSelect(bagBox, boxItem, ComBoxType.Exclusive_Up_Main);
            this.items.Add(box);
        }


        this.Btn_OK.gameObject.SetActive(false);
    }

    private void OnBoxSelect(BoxSelectEvent e)
    {
        if (e.Type == ComBoxType.Exclusive_Up_Main)
        {
            this.SelectMain = e.Box;
            Box_Ready_Main.Up(e.Box.BoxItem);

            this.ShowMain();
        }
        else if (e.Type == ComBoxType.Exclusive_Up_Material)
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

        for (int i = 0; i < ItemIdList.Length; i++)
        {
            string color = "#FF0000";
            TxtCommissionCountList[i].text = string.Format("<color={0}>({1}/{2})</color>", color, 0, 0);
        }

        ExclusiveItem exclusiveMain = SelectMain.BoxItem.Item as ExclusiveItem;
        List<int> lvs = exclusiveMain.LevelDict.Select(m => m.Key).ToList();
        for (int i = 0; i < levelList.Count; i++)
        {
            if (i < lvs.Count)
            {
                int runeId = lvs[i];
                levelList[i].SetContent(runeId, exclusiveMain.LevelDict[runeId]);
            }
            else
            {
                levelList[i].SetContent(0, 0);
            }
        }

        if (exclusiveMain.GetLevel() >= 99) //最高99级
        {
            return;
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
            if (lvs.Count > 0 && !lvs.Contains(exclusive.RuneConfigId))
            {
                continue;
            }

            Box_Select box = PrefabHelper.Instance().CreateBoxSelect(bagBox, item, ComBoxType.Exclusive_Up_Material);
            this.sourceList.Add(box);

            BoxId++;
        }
    }

    private void ShowMaterial()
    {
        this.Check();
        if (this.check)
        {
            this.Btn_OK.gameObject.SetActive(true);
        }

        ExclusiveItem exclusiveMaterial = SelectMaterial.BoxItem.Item as ExclusiveItem;

        for (int i = 0; i < levelList.Count; i++)
        {
            Attr_Exclusive_Up upItem = levelList[i];

            upItem.SetUp(exclusiveMaterial.RuneConfigId);

            return;
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

        ExclusiveItem exclusiveMain = SelectMain.BoxItem.Item as ExclusiveItem;
        ExclusiveItem exclusiveMaterial = SelectMaterial.BoxItem.Item as ExclusiveItem;

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

        //销毁
        GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent()
        {
            BoxItem = SelectMaterial.BoxItem
        });

        Box_Ready_Material.Down(); //销毁已选
        sourceList.Remove(SelectMaterial);//移除包裹
        GameObject.Destroy(SelectMaterial.gameObject); //销毁包裹
        SelectMaterial = null;

        exclusiveMain.Up(exclusiveMaterial);

        this.ShowMain();
    }
}

