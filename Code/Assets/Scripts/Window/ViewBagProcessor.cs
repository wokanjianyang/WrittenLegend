using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewBagProcessor : AViewPage
    {
        [Title("包裹导航")]
        [LabelText("所有物品")]
        public Toggle toggle_All;

        [LabelText("装备")]
        public Toggle toggle_Equip;

        [LabelText("道具")]
        public Toggle toggle_Prop;

        [LabelText("锻造")]
        public Toggle toggle_Forging;

        [Title("包裹格子")]
        [LabelText("包裹")]
        public ScrollRect sr_Bag;

        private List<Com_Item> items;

        private int bagMaxCount = 50;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Bag;
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<EquipOneEvent>(this.OnEquipOneEvent);
            var hero = GameProcessor.Inst.PlayerManager.hero;
            hero.EventCenter.AddListener<HeroBagUpdateEvent>(this.OnHeroBagUpdateEvent);

            this.items = new List<Com_Item>();

            var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Empty");

            for (var i = 0; i < bagMaxCount; i++)
            {
                var empty = GameObject.Instantiate(emptyPrefab, (this.sr_Bag.content));
                empty.name = "Box_" + i;
            }

            if(hero.Bags!=null)
            {
                foreach (var item in hero.Bags)
                {
                    var box = this.CreateBox(item as Equip, item.BoxId);
                    box.transform.SetParent(this.sr_Bag.content.GetChild(item.BoxId));
                    box.transform.localPosition = Vector3.zero;
                    box.transform.localScale = Vector3.one;
                    box.SetBoxId(item.BoxId);
                    this.items.Add(box);
                }
            }

            if(hero.EquipPanel!=null)
            {
                foreach(var kvp in hero.EquipPanel)
                {
                    this.EquipOne(kvp.Key, kvp.Value);
                }
            }
        }
        private Com_Item CreateBox(Equip equip,int boxId=-1)
        {

            var item = EquipConfigCategory.Instance.Get(equip.ConfigId);
            GameObject yellow = null;
            switch (equip.Quality)
            {
                case 0:
                case 1:
                    {
                        var greenPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Green");
                        yellow = GameObject.Instantiate(greenPrefab);
                    }
                    break;
                case 2:
                    {
                        var yellowPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Yellow");
                        yellow = GameObject.Instantiate(yellowPrefab);
                    }
                    break;
                case 3:
                    {
                         var bluePrefab = Resources.Load<GameObject>("Prefab/Window/Box_Blue");
                        yellow = GameObject.Instantiate(bluePrefab);
                    }
                    break;
                case 4:
                    {
                        var pinkPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Pink");
                        yellow = GameObject.Instantiate(pinkPrefab);
                    }
                    break;
            }
            var comItem = yellow.GetComponent<Com_Item>();
            comItem.SetBoxId(boxId);
            comItem.SetItem(equip);
            return comItem;
        }
        private void OnEquipOneEvent(EquipOneEvent e)
        {
            this.EquipOne(0, e.Item);
        }
        private void EquipOne(int position, Item item, int boxId = -1)
        {
            Equip equip = item as Equip;
            var slots = this.transform.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == equip.Position).ToList();
            int emptySlotIndex = -1;
            if (slots.Count == 1)
            {
                emptySlotIndex = 0;
            }
            else
            {
                for (var i = 0; i < slots.Count; i++)
                {
                    if (slots[i].GetEquip() == null)
                    {
                        emptySlotIndex = -1;
                        break;
                    }
                }
                if (emptySlotIndex == -1)
                {
                    emptySlotIndex = 0;
                }
            }
            var equiped = slots[emptySlotIndex].GetEquip();
            if (equiped != null)
            {
                for (var i = 0; i < bagMaxCount; i++)
                {
                    var com = this.items.FirstOrDefault(c => c.boxId == i);
                    if (com == null)
                    {
                        equiped.transform.SetParent(this.sr_Bag.content.GetChild(i));
                        equiped.transform.localPosition = Vector3.zero;
                        equiped.SetBoxId(i);
                        this.items.Add(equiped);
                        break;
                    }
                }
            }
            var comItem = this.items.FirstOrDefault(i => i.boxId == boxId);
            if (comItem == null)
            {
                comItem = this.CreateBox(equip);
            }
            else
            {
                this.items.RemoveAt(boxId);
            }
            slots[emptySlotIndex].Equip(comItem);
            comItem.transform.SetParent(slots[emptySlotIndex].transform);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;
            comItem.SetBoxId(-1);

            if (position == 0)
            {  //原本就装备了，只是显示
                var hero = GameProcessor.Inst.PlayerManager.hero;
                hero.EventCenter.Raise(new HeroUseEquipEvent
                {
                    Position = position,
                    Equip = equip
                });
            }
        }
        private void OnHeroBagUpdateEvent(HeroBagUpdateEvent e)
        {
            Hero hero = GameProcessor.Inst.PlayerManager.hero;
            if (hero.Bags != null)
            {
                var newItems = hero.Bags.Where(b => b.BoxId == -1).ToList();

                int lastBoxId = 0;
                foreach (var newItem in newItems)
                {
                    for (var i = lastBoxId; i < bagMaxCount; i++)
                    {
                        var com = this.items.FirstOrDefault(c => c.boxId == i);
                        if (com == null)
                        {
                            var item = this.CreateBox(newItem as Equip, i);
                            item.transform.SetParent(this.sr_Bag.content.GetChild(i));
                            item.transform.localPosition = Vector3.zero;
                            item.transform.localScale = Vector3.one;
                            item.SetBoxId(i);
                            this.items.Add(item);
                            lastBoxId = i;
                            newItem.BoxId = i;
                            break;
                        }
                    }
                }

            }
        }
    }
}