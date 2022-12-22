using SA.Android.Utilities;
using SA.CrossPlatform.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Game.PocketAD;

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

        [Title("个人信息")]
        [LabelText("属性")]
        public Button btn_PlayerAttribute;

        [LabelText("称号")]
        public Button btn_PlayerTitle;

        private List<Com_Box> items;


        // Start is called before the first frame update
        void Start()
        {
            this.btn_PlayerTitle.onClick.AddListener(this.OnClick_PlayerTitle);
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
            GameProcessor.Inst.EventCenter.AddListener<SkillBookEvent>(this.OnSkillBookEvent);
            var hero = GameProcessor.Inst.PlayerManager.hero;
            hero.EventCenter.AddListener<HeroBagUpdateEvent>(this.OnHeroBagUpdateEvent);

            this.items = new List<Com_Box>();

            var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Empty");

            for (var i = 0; i < PlayerHelper.bagMaxCount; i++)
            {
                var empty = GameObject.Instantiate(emptyPrefab, (this.sr_Bag.content));
                empty.name = "Box_" + i;
            }

            if(hero.Bags!=null)
            {
                foreach (var item in hero.Bags)
                {
                    Com_Box box = this.items.Find(b => b.boxId == item.BoxId); 
                    if(box==null)
                    {
                        box = this.CreateBox(item, item.BoxId);
                        box.transform.SetParent(this.sr_Bag.content.GetChild(item.BoxId));
                        box.transform.localPosition = Vector3.zero;
                        box.transform.localScale = Vector3.one;
                        box.SetBoxId(item.BoxId);
                        this.items.Add(box);
                    }
                    else
                    {
                        box.AddStack();
                    }
                }
            }

            if(hero.EquipPanel!=null)
            {
                foreach(var kvp in hero.EquipPanel)
                {
                    this.WearEquipment(kvp.Key,kvp.Value);
                }
            }
        }
        private Com_Box CreateBox(Item item,int boxId=-1)
        {

            GameObject prefab = null;
            GameObject go = null;
            switch (item.Quality)
            {
                case 0:
                case 1:
                    {
                        prefab = Resources.Load<GameObject>("Prefab/Window/Box_White");
                    }
                    break;
                case 2:
                    {
                        prefab = Resources.Load<GameObject>("Prefab/Window/Box_Green");
                    }
                    break;
                case 3:
                    {
                        prefab = Resources.Load<GameObject>("Prefab/Window/Box_Blue");
                    }
                    break;
                case 4:
                    {
                        prefab = Resources.Load<GameObject>("Prefab/Window/Box_Pink");
                    }
                    break;
            }
            go = GameObject.Instantiate(prefab);
            var comItem = go.GetComponent<Com_Box>();
            comItem.SetBoxId(boxId);
            comItem.SetItem(item);
            return comItem;
        }
        private void OnEquipOneEvent(EquipOneEvent e)
        {
            var equip = e.Item as Equip;

            if (e.IsWear)
            {
                this.WearEquipment(0,equip, e.BoxId);
            }
            else
            {
                this.RmoveEquipment(equip);
            }
            UserData.Save();
        }
        private void OnSkillBookEvent(SkillBookEvent e)
        {
            var box = this.items[e.BoxId];

            var hero = GameProcessor.Inst.PlayerManager.hero;
            hero.EventCenter.Raise(new HeroUseSkillBookEvent
            {
                IsLearn = e.IsLearn,
                Book = box.Item as SkillBook
            });

            box.RemoveStack();
            if(box.Count==0)
            {
                this.items.RemoveAt(e.BoxId);
                GameObject.Destroy(box.gameObject);
            }
            UserData.Save();
        }
        private void WearEquipment(int position,Equip equip, int boxId = -1)
        {
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
                        emptySlotIndex = i;
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
                for (var i = 0; i < PlayerHelper.bagMaxCount; i++)
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
                this.items.Remove(comItem);
            }
            slots[emptySlotIndex].Equip(comItem);
            comItem.transform.SetParent(slots[emptySlotIndex].transform);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;
            comItem.SetBoxId(-1);

            if (position == 0)
            {  
                var hero = GameProcessor.Inst.PlayerManager.hero;
                equip.Position = equip.Position + emptySlotIndex * PlayerHelper.MAX_EQUIP_COUNT;
                hero.EventCenter.Raise(new HeroUseEquipEvent
                {
                    Position = equip.Position + emptySlotIndex * PlayerHelper.MAX_EQUIP_COUNT,
                    Equip = equip
                });
            }
            else
            {
                //原本就装备了，只是显示
            }
        }

        private void RmoveEquipment(Equip equip)
        {
            var slots = this.transform.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == equip.Position % PlayerHelper.MAX_EQUIP_COUNT).ToList();
            var slotInex = 0;
            if (slots.Count==2)
            {
                slotInex = equip.Position / PlayerHelper.MAX_EQUIP_COUNT;
            }
            var comItem = slots[slotInex].GetEquip();
            slots[slotInex].UnEquip();

            for(var i=0;i< PlayerHelper.bagMaxCount;i++)
            {
                var box = this.items.FirstOrDefault(item => item.boxId == i);
                if (box == null)
                {
                    comItem.transform.SetParent(this.sr_Bag.content.GetChild(i));
                    comItem.transform.localPosition = Vector3.zero;
                    comItem.transform.localScale = Vector3.one;
                    comItem.SetBoxId(i);
                    this.items.Add(comItem);
                    break;
                }
            }
            var hero = UserData.Load();
            hero.EventCenter.Raise(new HeroUnUseEquipEvent() { 
                Equip = equip
            });

            UserData.Save();
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
                    if(newItem.Type == ItemType.SkillBox)
                    {
                        var box = this.items.Find(b => b.Item.ConfigId == newItem.ConfigId);
                        if(box!=null)
                        {
                            box.AddStack();
                            newItem.BoxId = box.boxId;
                            continue;
                        }
                    }
                    for (var i = lastBoxId; i < PlayerHelper.bagMaxCount; i++)
                    {
                        var com = this.items.FirstOrDefault(c => c.boxId == i);
                        if (com == null)
                        {
                            var item = this.CreateBox(newItem, i);
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


        private AdStateCallBack adStateCallBack;
        public void OnClick_PlayerTitle()
        {
            adStateCallBack += OnAdStateCallBack;
            string title = "显示广告";
            string message = "激励视频广告测试";
            var builder = new UM_NativeDialogBuilder(title, message);
            builder.SetPositiveButton("打开", () => {
                AN_Logger.Log("Okay button pressed");

                PocketAD.Inst.ShowAD("称号", adStateCallBack);
            });

            var dialog = builder.Build();
            dialog.Show();


        }

        public void OnAdStateCallBack(int rv, AdStateEnum state, AdTypeEnum adType)
        {

        }
    }
}