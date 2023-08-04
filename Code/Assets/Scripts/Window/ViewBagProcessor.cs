using SA.Android.Utilities;
using SA.CrossPlatform.UI;
using Sirenix.OdinInspector;
using System;
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

        //[LabelText("整理")]
        //public Toggle toggle_Forging;

        [LabelText("整理")]
        public Button btn_Reset;

        [Title("包裹格子")]
        [LabelText("包裹")]
        public ScrollRect sr_Bag;

        [Title("个人信息")]
        [LabelText("属性")]
        public Button btn_PlayerAttribute;

        [LabelText("称号")]
        public Button btn_PlayerTitle;

        [LabelText("设置")]
        public Button btn_Setting;
        
        private List<Com_Box> items = new List<Com_Box>();


        // Start is called before the first frame update
        void Start()
        {
            this.btn_PlayerTitle.onClick.AddListener(this.OnClick_PlayerTitle);
            this.btn_Setting.onClick.AddListener(this.OnClick_Setting);
            this.btn_Reset.onClick.AddListener(OnRefreshBag);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<EquipOneEvent>(this.OnEquipOneEvent);
            GameProcessor.Inst.EventCenter.AddListener<SkillBookEvent>(this.OnSkillBookEvent);
            GameProcessor.Inst.EventCenter.AddListener<RecoveryEvent>(this.OnRecoveryEvent);
            GameProcessor.Inst.EventCenter.AddListener<AutoRecoveryEvent>(this.OnAutoRecoveryEvent);
            GameProcessor.Inst.EventCenter.AddListener<BagUseEvent>(this.OnBagUseEvent);
            GameProcessor.Inst.EventCenter.AddListener<CompositeEvent>(this.OnCompositeEvent);
            GameProcessor.Inst.EventCenter.AddListener<MaterialUseEvent>(this.OnMaterialUseEvent);
            GameProcessor.Inst.EventCenter.AddListener<EquipLockEvent>(this.OnEquipLockEvent);

            GameProcessor.Inst.StartCoroutine(LoadBox());
        }


        private IEnumerator LoadBox()
        {
            
            User user = GameProcessor.Inst.User;
            user.EventCenter.AddListener<HeroBagUpdateEvent>(this.OnHeroBagUpdateEvent);

            this.items = new List<Com_Box>();

            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");
            yield return null;

            foreach (var slotBox in this.transform.GetComponentsInChildren<SlotBox>())
            {
                slotBox.Init(prefab);
                yield return null;

            }

            if (user.EquipPanel != null)
            {
                //��ʼ��Ⱦ����װ��
                foreach (var kvp in user.EquipPanel)
                {
                    this.CreateEquipPanelItem(kvp.Key, kvp.Value);
                    yield return null;

                }
            }
            var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Empty");
            yield return null;
            for (var i = 0; i < user.BagNum; i++)
            {
                var empty = GameObject.Instantiate(emptyPrefab, (this.sr_Bag.content));
                empty.name = "Box_" + i;
                
                yield return null;

            }

            RefreshBag();
            yield return null;

        }

        private void OnRefreshBag()
        {
            RefreshBag();
        }

        private void RefreshBag()
        {
            foreach (Com_Box box in items)
            {
                GameObject.Destroy(box.gameObject);
            }
            items = new List<Com_Box>();

            User user = GameProcessor.Inst.User;

            if (user.Bags != null)
            {
                user.Bags = user.Bags.OrderBy(m => m.Item.GetQuality()).OrderBy(m => m.Item.Type).OrderBy(m => m.Item.ConfigId).ToList();
                for (int BoxId = 0; BoxId < user.Bags.Count; BoxId++)
                {
                    BoxItem item = user.Bags[BoxId];
                    item.BoxId = BoxId;

                    Com_Box box = this.CreateBox(item);
                    box.transform.SetParent(this.sr_Bag.content.GetChild(BoxId));
                    box.transform.localPosition = Vector3.zero;
                    box.transform.localScale = Vector3.one;
                    box.SetBoxId(BoxId);
                    this.items.Add(box);
                }
            }
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Bag;
        }

   
        private Com_Box CreateBox(BoxItem item)
        {
            GameObject prefab = null;
            if (item.Item.Type == ItemType.Material || item.Item.Type == ItemType.SkillBox)
            {
                prefab = Resources.Load<GameObject>("Prefab/Window/Box_SkillOrMat");
            }
            else
            {
                switch (item.Item.GetQuality())
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
            }
            
            var go = GameObject.Instantiate(prefab);
            var comItem = go.GetComponent<Com_Box>();
            comItem.SetBoxId(item.BoxId);
            comItem.SetItem(item);
            return comItem;
        }

        private void OnCompositeEvent(CompositeEvent e)
        {
            CompositeConfig config = e.Config;

            User user = GameProcessor.Inst.User;

            List<BoxItem> list = user.Bags.Where(m => (int)m.Item.Type == config.FromItemType && m.Item.ConfigId == config.FromId).ToList();

            int count = list.Select(m => m.Number).Sum();

            if (count < config.Quantity)
            {
                return;
            }

            int useCount = (int)config.Quantity;

            foreach (BoxItem boxItem in list)
            {
                int boxUseCount = Math.Min(boxItem.Number, useCount);

                Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId);
                boxItem.RemoveStack(boxUseCount);
                boxUI.RemoveStack(boxUseCount);

                if (boxItem.Number <= 0)
                {
                    user.Bags.Remove(boxItem);

                    this.items.Remove(boxUI);
                    GameObject.Destroy(boxUI.gameObject);

                }

                useCount = useCount - boxUseCount;

                if (useCount <= 0)
                {
                    break;
                }
            }

            //
            Item item = ItemHelper.BuildItem((ItemType)config.TargetType, config.TargetId, 1, 1);

            AddBoxItem(item);

            GameProcessor.Inst.EventCenter.Raise(new CompositeUIFreshEvent());
        }

        private void OnMaterialUseEvent(MaterialUseEvent e)
        {
            User user = GameProcessor.Inst.User;

            List<BoxItem> list = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == e.MaterialId).ToList();

            int count = list.Select(m => m.Number).Sum();

            int useCount = (int)e.Quantity;

            foreach (BoxItem boxItem in list)
            {
                int boxUseCount = Math.Min(boxItem.Number, useCount);

                Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId);
                boxItem.RemoveStack(boxUseCount);
                boxUI.RemoveStack(boxUseCount);

                if (boxItem.Number <= 0)
                {
                    user.Bags.Remove(boxItem);

                    this.items.Remove(boxUI);
                    GameObject.Destroy(boxUI.gameObject);

                }

                useCount = useCount - boxUseCount;

                if (useCount <= 0)
                {
                    break;
                }
            }
        }
        
        private void OnEquipOneEvent(EquipOneEvent e)
        {
            var equip = e.Item as Equip;

            if (e.IsWear)
            {
                this.WearEquipment(equip, e.BoxId);
            }
            else
            {
                this.RmoveEquipment(e.Position, equip);
            }
            //UserData.Save();

            TaskHelper.CheckTask(TaskType.Equip, 1);
        }
        private void OnSkillBookEvent(SkillBookEvent e)
        {
            User user = GameProcessor.Inst.User;

            UseBoxItem(e.BoxId,1);

            user.EventCenter.Raise(new HeroUseSkillBookEvent
            {
                IsLearn = e.IsLearn,
                Item = e.Item
            });
        }

        private void OnRecoveryEvent(RecoveryEvent e)
        {
            User user = GameProcessor.Inst.User;

            UseBoxItem(e.BoxId, 1);

            int refineStone = 0;
            if (e.Item.Type == ItemType.Equip)
            {
                Equip equip = e.Item as Equip;
                refineStone += equip.Level / 10 * equip.GetQuality();
            }

            long gold = e.Item.Gold;

            user.AddExpAndGold(0, gold);

            Item item = ItemHelper.BuildRefineStone(Math.Max(1, refineStone));
            AddBoxItem(item);

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildAutoRecoveryMessage(1, refineStone, gold)
            });

        }

        private void OnAutoRecoveryEvent(AutoRecoveryEvent e)
        {
            User user = GameProcessor.Inst.User;

            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoverySetting.CheckRecovery(m.Item)).ToList();

            int refineStone = 0;
            long gold = 0;

            foreach (BoxItem box in recoveryList)
            {
                gold += box.Item.Gold * box.Number;

                if (box.Item.Type == ItemType.Equip)
                {
                    Equip equip = box.Item as Equip;
                    refineStone += equip.Level / 10 * equip.GetQuality();
                }
                //Log.Debug("自动回收:" + box.Item.Name + " " + box.Number + "个");

                box.Number = 0;
                UseBoxItem(box.BoxId, 1);
            }

            if (gold > 0)
            {
                user.AddExpAndGold(0, gold);
            }

            if (refineStone > 0)
            {
                Item item = ItemHelper.BuildRefineStone(refineStone);
                AddBoxItem(item);

                if (recoveryList.Count > 0)
                {
                    GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                    {
                        Message = BattleMsgHelper.BuildAutoRecoveryMessage(recoveryList.Count, refineStone, gold)
                    });
                }
            }
        }

        private void OnBagUseEvent(BagUseEvent e)
        {
            User user = GameProcessor.Inst.User;

            BoxItem boxItem = user.Bags.Find(m => m.BoxId == e.BoxId);
            int quantity = e.Quantity == 1 ? 1 : boxItem.Number;

            UseBoxItem(e.BoxId, quantity);

            //use logic
            if (boxItem.Item.Type == ItemType.SkillBox)
            {
                user.EventCenter.Raise(new HeroUseSkillBookEvent
                {
                    IsLearn = false,
                    Item = boxItem.Item,
                    Quantity = quantity,
                });
            }
            else if (boxItem.Item.Type == ItemType.ExpPack)
            {
                long exp = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold);

                ItemConfig config = ItemConfigCategory.Instance.Get(boxItem.Item.ConfigId);

                exp = exp * config.UseParam * 720; //3600/5 = 720,配置的是小时

                user.AddExpAndGold(exp, 0);
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildGiftPackMessage("道具奖励", exp, 0)
                });
            }
            else if (boxItem.Item.Type == ItemType.GoldPack)
            {
                long gold = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp);

                ItemConfig config = ItemConfigCategory.Instance.Get(boxItem.Item.ConfigId);

                gold = gold * config.UseParam * 720; //3600/5 = 720,配置的是小时

                user.AddExpAndGold(0, gold);
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildGiftPackMessage("道具奖励", 0, gold)
                });
            }
            else if (boxItem.Item.Type == ItemType.GiftPack)
            {
                int gold = 0;
                List<Item> items = GiftPackHelper.BuildItems(boxItem.Item.ConfigId, ref gold);

                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildGiftPackMessage(items)
                });

                if (gold > 0)
                {
                    user.AddExpAndGold(0, gold);
                }
            }
        }
        
        private void OnEquipLockEvent(EquipLockEvent e)
        {
            e.Item.IsLock = e.IsLock;
            Com_Box boxUI = this.items.Find(m => m.boxId == e.BoxId);
            boxUI.SetLock(e.IsLock);
        }
            
        private void UseBoxItem(int boxId,int quantity)
        {
            User user = GameProcessor.Inst.User;

            //逻辑处理
            BoxItem boxItem = user.Bags.Find(m => m.BoxId == boxId);
            Com_Box boxUI = this.items.Find(m => m.boxId == boxId);

            if (boxItem == null)
            {
                Log.Debug("此物品已经被使用了");
                return;
            }
            boxItem.RemoveStack(quantity);
            boxUI.RemoveStack(quantity);

            //用光了，移除队列
            if (boxItem.Number <= 0)
            {
                user.Bags.Remove(boxItem);

                this.items.Remove(boxUI);
                GameObject.Destroy(boxUI.gameObject);
            }
        }

        private void AddBoxItem(Item newItem)
        {
            User user = GameProcessor.Inst.User;

            BoxItem boxItem = user.Bags.Find(m => !m.IsFull() && m.Item.ConfigId == newItem.ConfigId);  //ͬ����Ʒ������û�����ѵ��ĸ���

            if (boxItem != null)
            {  //堆叠UI
                var item = this.items.Find(b => b.boxId == boxItem.BoxId);
                item.AddStack(newItem.Quantity);

                boxItem.AddStack(newItem.Quantity);
            }
            else
            {
                int lastBoxId = GetNextBoxId();

                if (lastBoxId < 0) { //包裹已经满了
                    return;
                }

                boxItem = new BoxItem();
                boxItem.Item = newItem;
                boxItem.Number = 1;
                boxItem.BoxId = lastBoxId;

                var item = this.CreateBox(boxItem);
                item.transform.SetParent(this.sr_Bag.content.GetChild(lastBoxId));
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.SetBoxId(lastBoxId);
                this.items.Add(item);

                user.Bags.Add(boxItem);
            }
        }

        private void WearEquipment(Equip equip,int BoxId)
        {
            User user = GameProcessor.Inst.User;

            int Part = equip.Part;
            //增加一次穿戴记录，用做轮流穿戴左右
            if (!user.EquipRecord.ContainsKey(Part))
            {
                user.EquipRecord[Part] = 0;
            }
            user.EquipRecord[Part]++;
            int PartIndex = user.EquipRecord[Part] % equip.Position.Length;
            int Position = equip.Position[PartIndex];

            //从包袱移除
            UseBoxItem(BoxId,1);

            //如果存在旧装备，增加到包裹
            if (user.EquipPanel.ContainsKey(Position))
            {
                //装备栏卸载
                var slot = this.transform.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();
                Com_Box comItem = slot.GetEquip();
                slot.UnEquip();
                GameObject.Destroy(comItem.gameObject);

                AddBoxItem(user.EquipPanel[Position]);
            }

            //穿戴到格子上
            this.CreateEquipPanelItem(Position, equip);
            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUseEquipEvent
            {
                Position = Position,
                Equip = equip
            });
        }

        private void CreateEquipPanelItem(int Position, Equip equip)
        {
            User user = GameProcessor.Inst.User;

            var slot = this.transform.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();

            //生成格子
            BoxItem boxItem = new BoxItem();
            boxItem.Item = equip;
            boxItem.Number = 1;
            boxItem.BoxId = -1;

            Com_Box comItem = this.CreateBox(boxItem);
            comItem.transform.SetParent(slot.transform);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;
            comItem.SetBoxId(-1);
            comItem.SetEquipPosition(Position);

            //穿戴
            slot.Equip(comItem);
        }

        private void RmoveEquipment(int position, Equip equip)
        {
            //装备栏卸载
            var slot = this.transform.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            Com_Box comItem = slot.GetEquip();
            slot.UnEquip();
            GameObject.Destroy(comItem.gameObject);

            //装备移动到包裹里面
            AddBoxItem(equip);

            //通知英雄更新属性
            User user = GameProcessor.Inst.User;
            user.EventCenter.Raise(new HeroUnUseEquipEvent()
            {
                Position = position,
                Equip = equip
            });

            //UserData.Save();
        }
        private void OnHeroBagUpdateEvent(HeroBagUpdateEvent e)
        {
            User user = GameProcessor.Inst.User;
            if (user.Bags != null)
            {
                var newItems = e.ItemList;

                foreach (var newItem in newItems)
                {
                    if (newItem.Type == ItemType.Buff)
                    {
                        //TODO
                    }
                    else
                    {
                        AddBoxItem(newItem);
                    }
                }
            }
        }

        public int GetNextBoxId()
        {
            int maxNum = ConfigHelper.MaxBagCount;
            for (int boxId = 0; boxId < maxNum; boxId++)
            {
                if (this.items.Find(m => m.boxId == boxId) == null)
                {
                    return boxId;
                }
            }
            return -1;
        }

        public void OnClick_PlayerTitle()
        {
            

        }
        
        public void OnClick_Setting()
        {
            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent(){IsOpen = true});
        }

    }
}