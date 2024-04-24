using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Panel_Devour : MonoBehaviour
{
    public ScrollRect sr_Panel;

    public List<SlotBox> slots;

    public Button Btn_OK;
    public List<Text> TxtCommissionNameList;
    public List<Text> TxtCommissionCountList;

    private List<Com_Box> items = new List<Com_Box>();

    private const int MaxCount = 36;

    private bool check = false;

    private ExclusiveDevourConfig config = null;
    // Start is called before the first frame update
    void Awake()
    {
        this.Init();

        this.Btn_OK.onClick.AddListener(OnClickOK);
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<ComBoxSelectEvent>(this.OnComBoxSelect);
        GameProcessor.Inst.EventCenter.AddListener<ComBoxDeselectEvent>(this.OnComBoxDeselect);
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

        var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].Init(prefab);
        }
    }

    public void Load()
    {
        foreach (SlotBox slot in slots)
        {
            Com_Box comItem = slot.GetEquip();
            if (comItem != null)
            {
                slot.UnEquip();
                GameObject.Destroy(comItem.gameObject);
            }
        }

        foreach (var cb in items)
        {
            GameObject.Destroy(cb.gameObject);
        }
        items.Clear();

        User user = GameProcessor.Inst.User;

        List<BoxItem> list = user.Bags.Where(m => m.Item.Type == ItemType.Exclusive && m.Item.GetQuality() == 5 && !m.Item.IsLock).OrderBy(m => m.Item.ConfigId).ToList();
        //Debug.Log("es:" + list.Count);
        for (int BoxId = 0; BoxId < list.Count; BoxId++)
        {
            if (BoxId >= MaxCount)
            {
                return;
            }

            var bagBox = this.sr_Panel.content.GetChild(BoxId);
            if (bagBox == null)
            {
                return;
            }

            BoxItem item = list[BoxId];
            item.BoxId = BoxId;

            Com_Box box = this.CreateBox(item, bagBox);
            box.SetBoxId(BoxId);
            this.items.Add(box);
        }

        this.config = null;
    }

    private Com_Box CreateBox(BoxItem item, Transform parent)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefab/Window/Box_Orange");

        var go = GameObject.Instantiate(prefab);
        var comItem = go.GetComponent<Com_Box>();
        comItem.SetBoxId(item.BoxId);
        comItem.SetItem(item);
        comItem.SetType(ComBoxType.Exclusive_Devour);

        comItem.transform.SetParent(parent);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void OnComBoxSelect(ComBoxSelectEvent e)
    {
        SlotBox slot = null;
        int type = 0;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetEquip() == null)
            {
                slot = slots[i];
                type = i;
                break;
            }
        }

        if (slot == null)
        {
            return;
        }

        BoxItem boxItem = e.BoxItem;
        ExclusiveItem exclusive = boxItem.Item as ExclusiveItem;

        if (type == 1 && exclusive.GetLevel() > 1)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "副专属不能为升阶过的", ToastType = ToastTypeEnum.Failure });
            return;
        }

        int nextLevel = exclusive.GetLevel();
        int maxLevel = GameProcessor.Inst.User.GetExclusiveLimit();

        if (nextLevel >= maxLevel)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "专属已经满吞噬了", ToastType = ToastTypeEnum.Failure });
            return;
        }

        Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId);
        this.items.Remove(boxUI);
        GameObject.Destroy(boxUI.gameObject);

        boxItem.BoxId = -1;

        Com_Box comItem = this.CreateBox(boxItem, slot.transform);
        comItem.SetBoxId(-1);
        comItem.SetEquipPosition((int)slot.SlotType);

        slot.Equip(comItem);

        int index = slots.IndexOf(slot);
        if (index == 0)
        {

            this.config = ExclusiveDevourConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Level == nextLevel).FirstOrDefault();
            this.Check();
        }
    }

    private void OnComBoxDeselect(ComBoxDeselectEvent e)
    {
        SlotBox slot = slots.Where(m => m.SlotType == (SlotType)e.Position).FirstOrDefault();

        BoxItem boxItem = e.BoxItem;

        Com_Box comItem = slot.GetEquip();
        slot.UnEquip();
        GameObject.Destroy(comItem.gameObject);

        //装备移动到包裹里面
        int BoxId = GetNextBoxId();
        boxItem.BoxId = BoxId;

        var bagBox = this.sr_Panel.content.GetChild(BoxId);
        Com_Box box = this.CreateBox(boxItem, bagBox);
        box.SetBoxId(BoxId);
        this.items.Add(box);
    }

    public int GetNextBoxId()
    {
        for (int boxId = 0; boxId < MaxCount; boxId++)
        {
            if (this.items.Find(m => m.boxId == boxId) == null)
            {
                return boxId;
            }
        }
        return -1;
    }

    public void OnClickOK()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetEquip() == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先选择专属", ToastType = ToastTypeEnum.Failure });
                return;
            }
        }

        //if (slots[0].GetEquip().BoxItem.Item.ConfigId != slots[1].GetEquip().BoxItem.Item.ConfigId)
        //{
        //    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "主副专属必须是同位置", ToastType = ToastTypeEnum.Failure });
        //    return;
        //}

        if (!check)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.Check();

        if (!check)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        //材料
        for (int i = 0; i < config.ItemIdList.Length; i++)
        {
            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = config.ItemIdList[i],
                Quantity = config.ItemCountList[i]
            });
        }

        for (int i = 0; i < slots.Count; i++)
        {
            GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
            {
                Quantity = 1,
                BoxItem = slots[i].GetEquip().BoxItem
            });
        }

        ExclusiveItem main = slots[0].GetEquip().BoxItem.Item as ExclusiveItem;
        ExclusiveItem second = slots[1].GetEquip().BoxItem.Item as ExclusiveItem;

        main.Devour(second);

        List<Item> list = new List<Item>();
        list.Add(main);
        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = list });

        this.Load();
        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "吞噬成功", ToastType = ToastTypeEnum.Success });
    }

    private void Check()
    {

        int[] ItemIdList = config.ItemIdList;
        int[] ItemCountList = config.ItemCountList;

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
}

