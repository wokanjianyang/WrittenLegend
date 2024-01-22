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

    private List<Com_Box> items = new List<Com_Box>();

    private const int MaxCount = 36;

    // Start is called before the first frame update
    void Awake()
    {
        this.Init();
    }

    // Update is called once per frame
    void Start()
    {
        GameProcessor.Inst.EventCenter.AddListener<ComBoxSelectEvent>(this.OnComBoxSelect);
        GameProcessor.Inst.EventCenter.AddListener<ComBoxDeselectEvent>(this.OnComBoxDeselect);
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
        User user = GameProcessor.Inst.User;

        List<BoxItem> list = user.Bags.Where(m => m.Item.Type == ItemType.Exclusive && m.Item.GetQuality() == 5).ToList();

        Debug.Log("Count:" + list.Count);

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

            Com_Box box = this.CreateBox(item);
            box.transform.SetParent(bagBox);
            box.transform.localPosition = Vector3.zero;
            box.transform.localScale = Vector3.one;
            box.SetBoxId(BoxId);
            this.items.Add(box);
        }

    }

    private Com_Box CreateBox(BoxItem item)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefab/Window/Box_Orange");

        var go = GameObject.Instantiate(prefab);
        var comItem = go.GetComponent<Com_Box>();
        comItem.SetBoxId(item.BoxId);
        comItem.SetItem(item);
        comItem.SetType(ComBoxType.Exclusive_Devour);
        return comItem;
    }

    private void OnComBoxSelect(ComBoxSelectEvent e)
    {
        SlotBox slot = slots.Where(m => m.GetEquip() == null).FirstOrDefault();

        if (slot == null)
        {
            return;
        }

        BoxItem boxItem = e.BoxItem;

        Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId);
        GameObject.Destroy(boxUI.gameObject);

        boxItem.BoxId = -1;

        Com_Box comItem = this.CreateBox(boxItem);
        comItem.transform.SetParent(slot.transform);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;
        comItem.SetBoxId(-1);
        comItem.SetEquipPosition((int)slot.SlotType);

        slot.Equip(comItem);
    }

    private void OnComBoxDeselect(ComBoxDeselectEvent e)
    {
        SlotBox slot = slots.Where(m => m.SlotType == (SlotType)e.Position).FirstOrDefault();

        BoxItem boxItem = e.BoxItem;

        Com_Box comItem = slot.GetEquip();
        slot.UnEquip();
        GameObject.Destroy(comItem.gameObject);

        //装备移动到包裹里面
        CreateBox(boxItem);
    }
}

