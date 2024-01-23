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
        public List<Toggle> BagToggleList = new List<Toggle>();

        public RectTransform EquipInfoSpecial;
        public List<RectTransform> EquipInfoList = new List<RectTransform>();
        public List<Button> Equip_Plan_List = new List<Button>();

        [LabelText("整理")]
        public Button btn_Reset;

        public List<ScrollRect> BagList = new List<ScrollRect>();

        [Title("个人信息")]
        [LabelText("魂环")]
        public Button btn_SoulRing;

        [LabelText("称号")]
        public Button btn_PlayerTitle;
        [LabelText("图鉴")]
        public Button btn_Card;
        [LabelText("设置")]
        public Button btn_Setting;
        public Button btn_Wing;
        public Button btn_Fashion;

        public Dialog_Exclusive ExclusiveDialog;
        public Dialog_Card Dialog_Card;
        public Dialog_Wing Dialog_Wing;

        public Button Btn_Exclusive;

        private List<Com_Box> items = new List<Com_Box>();

        private void Awake()
        {

            for (int i = 0; i < Equip_Plan_List.Count; i++)
            {
                int index = i;
                Equip_Plan_List[i].onClick.AddListener(() =>
                {
                    ChangePlan(index);
                });
            }

            for (int i = 0; i < BagToggleList.Count; i++)
            {
                int index = i;
                BagToggleList[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ShowBagPanel(index);
                    }
                });
            }

            this.btn_SoulRing.onClick.AddListener(this.OnClick_RingSoul);
            this.btn_PlayerTitle.onClick.AddListener(this.OnClick_PlayerTitle);
            this.btn_Setting.onClick.AddListener(this.OnClick_Setting);
            this.btn_Reset.onClick.AddListener(OnRefreshBag);
            this.Btn_Exclusive.onClick.AddListener(OnExclusive);
            this.btn_Card.onClick.AddListener(OnOpenCard);
            this.btn_Wing.onClick.AddListener(OnOpenWing);
            this.btn_Fashion.onClick.AddListener(OpenFashion);
        }

        // Start is called before the first frame update
        void Start()
        {
            ShowEquipPanel();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<EquipOneEvent>(this.OnEquipOneEvent);
            GameProcessor.Inst.EventCenter.AddListener<SkillBookLearnEvent>(this.OnSkillBookLearn);
            GameProcessor.Inst.EventCenter.AddListener<RecoveryEvent>(this.OnRecoveryEvent);
            GameProcessor.Inst.EventCenter.AddListener<AutoRecoveryEvent>(this.OnAutoRecoveryEvent);
            GameProcessor.Inst.EventCenter.AddListener<BagUseEvent>(this.OnBagUseEvent);
            GameProcessor.Inst.EventCenter.AddListener<CompositeEvent>(this.OnCompositeEvent);
            GameProcessor.Inst.EventCenter.AddListener<SystemUseEvent>(this.OnSystemUse);
            GameProcessor.Inst.EventCenter.AddListener<SelectGiftEvent>(this.OnSelectGift);
            GameProcessor.Inst.EventCenter.AddListener<EquipLockEvent>(this.OnEquipLockEvent);
            GameProcessor.Inst.EventCenter.AddListener<ExchangeEvent>(this.OnExchangeEvent);
            GameProcessor.Inst.EventCenter.AddListener<ChangeExclusiveEvent>(this.OnChangeExclusiveEvent);

            GameProcessor.Inst.StartCoroutine(LoadBox());
        }


        private IEnumerator LoadBox()
        {
            //先回收,再加载
            this.FirstRecovery();

            User user = GameProcessor.Inst.User;
            user.EventCenter.AddListener<HeroBagUpdateEvent>(this.OnHeroBagUpdateEvent);

            this.items = new List<Com_Box>();

            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");
            yield return null;

            for (int i = 0; i < EquipInfoList.Count; i++)
            {
                var EquipInfo = EquipInfoList[i];
                foreach (var slotBox in EquipInfo.GetComponentsInChildren<SlotBox>())
                {
                    slotBox.Init(prefab);
                    //yield return null;
                }
            }

            foreach (var slotBox in EquipInfoSpecial.GetComponentsInChildren<SlotBox>())
            {
                slotBox.Init(prefab);
                yield return null;
            }

            foreach (var kvEp in user.EquipPanelList)
            {
                foreach (var kvp in kvEp.Value)
                {
                    this.CreateEquipPanelItem(kvEp.Key, kvp.Key, kvp.Value);
                    //yield return null;
                }
            }

            //穿戴四格
            foreach (var kvp in user.EquipPanelSpecial)
            {
                this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
                //yield return null;
            }

            //穿戴专属
            foreach (var kvp in user.ExclusivePanelList[user.ExclusiveIndex])
            {
                this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
            }

            var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Empty");
            yield return null;

            for (int k = 0; k < BagList.Count; k++)
            {
                for (var i = 0; i < ConfigHelper.MaxBagCount; i++)
                {
                    var empty = GameObject.Instantiate(emptyPrefab, this.BagList[k].content);
                    empty.name = "Box_" + i;
                    //yield return null;
                }
            }

            RefreshBag();

            //yield return null;
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
            items.Clear();

            User user = GameProcessor.Inst.User;

            if (user.Bags != null)
            {
                for (int i = 0; i < this.BagList.Count; i++)
                {
                    List<BoxItem> list = user.Bags.Where(m => m.GetBagType() == i).OrderBy(m => m.GetBagSort()).ToList();

                    BuildBag(this.BagList[i], list);
                }
            }
        }

        private void BuildBag(ScrollRect bagRect, List<BoxItem> list)
        {
            for (int BoxId = 0; BoxId < list.Count; BoxId++)
            {
                if (BoxId + 1 > ConfigHelper.MaxBagCount)
                {
                    return;
                }

                var bagBox = bagRect.content.GetChild(BoxId);
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

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Bag;
        }


        private Com_Box CreateBox(BoxItem item)
        {
            GameObject prefab = null;
            //if (item.Item.Type == ItemType.Material || item.Item.Type == ItemType.SkillBox)
            //{
            //    prefab = Resources.Load<GameObject>("Prefab/Window/Box_SkillOrMat");
            //}
            //else
            //{
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
                case 5:
                    {
                        prefab = Resources.Load<GameObject>("Prefab/Window/Box_Orange");
                    }
                    break;
                    //}
            }

            var go = GameObject.Instantiate(prefab);
            var comItem = go.GetComponent<Com_Box>();
            comItem.SetBoxId(item.BoxId);
            comItem.SetItem(item);
            return comItem;
        }

        private void OnCompositeEvent(CompositeEvent e)
        {
            CompositeConfig Config = e.Config;

            User user = GameProcessor.Inst.User;

            for (int i = 0; i < Config.ItemIdList.Length; i++)
            {
                ItemType type = (ItemType)(Config.ItemTypeList[i]);
                int configId = Config.ItemIdList[i];
                int quality = Config.ItemQualityList[i];

                if (type == ItemType.Equip)
                {
                    BoxItem boxItem = user.Bags.Where(m => m.Item.Type == type && m.Item.ConfigId == configId).FirstOrDefault();

                    GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
                    {
                        Quantity = 1,
                        BoxItem = boxItem
                    });
                }
                else
                {
                    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                    {
                        Type = type,
                        ItemId = configId,
                        Quantity = Config.ItemCountList[i]
                    });
                }
            }

            Item item = ItemHelper.BuildItem((ItemType)Config.TargetType, Config.TargetId, 1, 1);

            AddBoxItem(item);

            GameProcessor.Inst.EventCenter.Raise(new CompositeUIFreshEvent());
        }

        private void OnExchangeEvent(ExchangeEvent e)
        {
            ExchangeConfig Config = e.Config;
            for (int i = 0; i < Config.ItemIdList.Length; i++)
            {
                ItemType type = (ItemType)(Config.ItemTypeList[i]);
                int configId = Config.ItemIdList[i];
                int quality = Config.ItemQualityList[i];

                if (type == ItemType.Exclusive)
                {
                    User user = GameProcessor.Inst.User;
                    BoxItem boxItem = user.Bags.Where(m => m.Item.Type == type && m.Item.ConfigId == configId
                    && m.Item.GetQuality() >= quality && !m.Item.IsLock).FirstOrDefault();


                    GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
                    {
                        Quantity = 1,
                        BoxItem = boxItem
                    });
                }
                else
                {
                    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                    {
                        Type = type,
                        ItemId = configId,
                        Quantity = Config.ItemCountList[i]
                    });
                }
            }

            Item item = ItemHelper.BuildItem((ItemType)Config.TargetType, Config.TargetId, 5, 1);

            AddBoxItem(item);

            GameProcessor.Inst.EventCenter.Raise(new ExchangeUIFreshEvent());
        }

        private void OnSystemUse(SystemUseEvent e)
        {
            User user = GameProcessor.Inst.User;

            List<BoxItem> list = user.Bags.Where(m => m.Item.Type == e.Type && m.Item.ConfigId == e.ItemId).ToList();

            long count = list.Select(m => m.MagicNubmer.Data).Sum();

            long useCount = e.Quantity;

            foreach (BoxItem boxItem in list)
            {
                long boxUseCount = Math.Min(boxItem.MagicNubmer.Data, useCount);

                Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType());
                boxItem.RemoveStack(boxUseCount);
                boxUI.RemoveStack(boxUseCount);

                if (boxItem.MagicNubmer.Data <= 0)
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

        private void OnSelectGift(SelectGiftEvent e)
        {
            UseBoxItem(e.BoxItem, 1);
            AddBoxItem(e.Item);
        }

        private void OnChangeExclusiveEvent(ChangeExclusiveEvent e)
        {
            User user = GameProcessor.Inst.User;
            user.ExclusiveIndex = e.Index;

            for (int i = 15; i <= 20; i++)
            {
                this.ClearEquipPanelItem(i);
            }

            foreach (var kvp in user.ExclusivePanelList[e.Index])
            {
                this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
            }

            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());

            //Debug.Log("OnChangeExclusiveEvent");
        }

        private void ChangePlan(int index)
        {
            User user = GameProcessor.Inst.User;
            user.EquipPanelIndex = index;

            if (user.ExclusiveSetting)
            {
                user.ExclusiveIndex = index;
                GameProcessor.Inst.EventCenter.Raise(new ChangeExclusiveEvent() { Index = index });
            }

            user.SkillPanelIndex = index;
            GameProcessor.Inst.User.EventCenter.Raise(new SkillChangePlanEvent());

            ShowEquipPanel();
        }

        private void ShowEquipPanel()
        {
            int position = GameProcessor.Inst.User.EquipPanelIndex;

            for (int i = 0; i < this.EquipInfoList.Count; i++)
            {
                if (i == position)
                {
                    this.EquipInfoList[i].gameObject.SetActive(true);
                }
                else
                {
                    this.EquipInfoList[i].gameObject.SetActive(false);
                }
            }
            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        }

        private void ShowBagPanel(int index)
        {
            for (int i = 0; i < this.BagList.Count; i++)
            {
                if (i == index)
                {
                    this.BagList[i].gameObject.SetActive(true);
                }
                else
                {
                    this.BagList[i].gameObject.SetActive(false);
                }
            }
        }


        private void OnEquipOneEvent(EquipOneEvent e)
        {
            if (e.IsWear)
            {
                if (e.BoxItem.Item.Type == ItemType.Exclusive)
                {
                    this.WearExclusive(e.BoxItem);
                }
                else
                {
                    this.WearEquipment(e.BoxItem);
                }
            }
            else
            {
                if (e.BoxItem.Item.Type == ItemType.Exclusive)
                {
                    this.RmoveExclusive(e.BoxItem);
                }
                else
                {
                    this.RmoveEquipment(e.Part, e.BoxItem);
                }
            }
            //UserData.Save();

            TaskHelper.CheckTask(TaskType.Equip, 1);
        }

        private void OnSkillBookLearn(SkillBookLearnEvent e)
        {
            User user = GameProcessor.Inst.User;

            UseBoxItem(e.BoxItem, 1);

            user.EventCenter.Raise(new HeroUseSkillBookEvent
            {
                IsLearn = true,
                BoxItem = e.BoxItem,
                Quantity = 1,
            });
        }

        private void OnRecoveryEvent(RecoveryEvent e)
        {
            User user = GameProcessor.Inst.User;

            Item item = e.BoxItem.Item;
            int refineStone = 0;
            int speicalStone = 0;
            int exclusiveStone = 0;
            int cardStone = 0;
            int number = (int)e.BoxItem.MagicNubmer.Data;

            if (item.Type == ItemType.Equip)
            {
                Equip equip = item as Equip;

                if (equip.Part <= 10)
                {
                    refineStone += user.CalStone(equip);
                }
                else
                {
                    speicalStone += user.CalSpecailStone(equip);
                }
            }
            else if (item.Type == ItemType.Exclusive)
            {
                exclusiveStone = item.GetQuality();
            }
            else if (item.Type == ItemType.Card)
            {
                cardStone = item.GetQuality() * number;
            }

            long gold = item.Gold;

            UseBoxItem(e.BoxItem, number);

            user.AddExpAndGold(0, gold);

            if (refineStone > 0)
            {
                Item stoneItem = ItemHelper.BuildRefineStone(refineStone);
                AddBoxItem(stoneItem);
            }
            if (exclusiveStone > 0)
            {
                Item exStoneItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Stone, exclusiveStone);
                AddBoxItem(exStoneItem);
            }
            if (cardStone > 0)
            {
                Item cardItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Card_Stone, cardStone);
                AddBoxItem(cardItem);
            }
            if (speicalStone > 0)
            {
                Item speicalStoneItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Equip_Speical_Stone, speicalStone);
                AddBoxItem(speicalStoneItem);
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildAutoRecoveryMessage(1, refineStone, speicalStone, exclusiveStone, cardStone, gold)
            });
        }

        private void OnAutoRecoveryEvent(AutoRecoveryEvent e)
        {
            User user = GameProcessor.Inst.User;

            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoverySetting.CheckRecovery(m.Item)).ToList();

            int refineStone = 0;
            int speicalStone = 0;
            int exclusiveStone = 0;
            long gold = 0;

            foreach (BoxItem box in recoveryList)
            {
                gold += box.Item.Gold * box.MagicNubmer.Data;

                if (box.Item.Type == ItemType.Equip)
                {
                    Equip equip = box.Item as Equip;

                    if (equip.Part <= 10)
                    {
                        refineStone += user.CalStone(equip);
                    }
                    else
                    {
                        speicalStone += user.CalSpecailStone(equip);
                    }
                }
                else if (box.Item.Type == ItemType.Exclusive)
                {
                    exclusiveStone += box.Item.GetQuality() * 1;
                }
                //Log.Debug("自动回收:" + box.Item.Name + " " + box.Number + "个");
                //box.MagicNubmer.Data = 0;
                UseBoxItem(box, 1);
            }

            if (gold > 0)
            {
                user.AddExpAndGold(0, gold);
            }

            if (refineStone > 0)
            {
                Item item = ItemHelper.BuildRefineStone(refineStone);
                AddBoxItem(item);
            }
            if (exclusiveStone > 0)
            {
                Item exStoneItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Stone, exclusiveStone);
                AddBoxItem(exStoneItem);
            }
            if (speicalStone > 0)
            {
                Item speicalStoneItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Equip_Speical_Stone, speicalStone);
                AddBoxItem(speicalStoneItem);
            }

            if (recoveryList.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Type = e.RuleType,
                    Message = BattleMsgHelper.BuildAutoRecoveryMessage(recoveryList.Count, refineStone, speicalStone, exclusiveStone, 0, gold)
                });
            }
        }

        private void FirstRecovery()
        {
            User user = GameProcessor.Inst.User;

            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoverySetting.CheckRecovery(m.Item)).ToList();

            int refineStone = 0;
            int speicalStone = 0;
            int exclusiveStone = 0;
            long gold = 0;

            foreach (BoxItem box in recoveryList)
            {
                gold += box.Item.Gold * box.MagicNubmer.Data;

                if (box.Item.Type == ItemType.Equip)
                {
                    Equip equip = box.Item as Equip;

                    if (equip.Part <= 10)
                    {
                        refineStone += user.CalStone(equip);
                    }
                    else
                    {
                        speicalStone += user.CalSpecailStone(equip);
                    }
                }
                else if (box.Item.Type == ItemType.Exclusive)
                {
                    exclusiveStone = box.Item.GetQuality() * 1;
                }
            }

            if (gold > 0)
            {
                user.AddExpAndGold(0, gold);
            }

            user.Bags.RemoveAll(m => !m.Item.IsLock && user.RecoverySetting.CheckRecovery(m.Item)); //移除

            if (refineStone > 0)
            {
                Item item = ItemHelper.BuildRefineStone(refineStone);
                AddBoxItem(item);
            }
            if (speicalStone > 0)
            {
                Item speicalStoneItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Equip_Speical_Stone, speicalStone);
                AddBoxItem(speicalStoneItem);
            }

            if (recoveryList.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildAutoRecoveryMessage(recoveryList.Count, refineStone, speicalStone, exclusiveStone, 0, gold)
                });
            }
        }

        private void OnBagUseEvent(BagUseEvent e)
        {
            User user = GameProcessor.Inst.User;

            BoxItem boxItem = e.BoxItem;
            long quantity = e.Quantity == -1 ? boxItem.MagicNubmer.Data : e.Quantity;

            if (boxItem.Item.Type == ItemType.Ticket && boxItem.Item.ConfigId == ItemHelper.SpecialId_Copy_Ticket)
            {
                if (user.MagicCopyTikerCount.Data >= ConfigHelper.CopyTicketMax)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "次数已经满了", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                quantity = Math.Min(quantity, ConfigHelper.CopyTicketMax - user.MagicCopyTikerCount.Data);
            }

            UseBoxItem(boxItem, quantity);

            //use logic
            if (boxItem.Item.Type == ItemType.SkillBox)
            {
                user.EventCenter.Raise(new HeroUseSkillBookEvent
                {
                    IsLearn = false,
                    BoxItem = boxItem,
                    Quantity = quantity,
                });
            }
            else if (boxItem.Item.Type == ItemType.ExpPack)
            {
                long exp = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp);

                ItemConfig config = ItemConfigCategory.Instance.Get(boxItem.Item.ConfigId);

                exp = exp * quantity * config.UseParam * 720; //3600/5 = 720,配置的是小时

                user.AddExpAndGold(exp, 0);
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildGiftPackMessage("道具奖励", exp, 0, null)
                });
            }
            else if (boxItem.Item.Type == ItemType.GoldPack)
            {
                long gold = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold);

                ItemConfig config = ItemConfigCategory.Instance.Get(boxItem.Item.ConfigId);

                gold = gold * quantity * config.UseParam * 720; //3600/5 = 720,配置的是小时

                user.AddExpAndGold(0, gold);
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildGiftPackMessage("道具奖励", 0, gold, null)
                });
            }
            else if (boxItem.Item.Type == ItemType.GiftPack)
            {
                //GameProcessor.Inst.EventCenter.Raise(new ShowSelect());
            }
            else if (boxItem.Item.Type == ItemType.Ticket)
            {
                user.MagicCopyTikerCount.Data += quantity;
            }
        }

        private void OnEquipLockEvent(EquipLockEvent e)
        {
            var boxItem = e.BoxItem;

            boxItem.Item.IsLock = e.IsLock;

            Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType()); //穿戴的找不到这个UI
            if (boxUI != null)
            {
                boxUI.SetLock(e.IsLock);
            }
        }

        private void UseBoxItem(BoxItem boxItem, long quantity)
        {
            User user = GameProcessor.Inst.User;

            //逻辑处理

            if (boxItem == null)
            {
                //Log.Debug("此物品已经被使用了");
                return;
            }

            boxItem.RemoveStack(quantity);

            //用光了，移除队列
            if (boxItem.MagicNubmer.Data <= 0)
            {
                user.Bags.Remove(boxItem);
            }

            //Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType());
            Com_Box boxUI = this.items.Find(m => m.BoxItem == boxItem);
            if (boxUI != null) //上线自动回收，可能还没加载
            {
                boxUI.RemoveStack(quantity);
                if (boxUI.Count <= 0)
                {
                    this.items.Remove(boxUI);
                    GameObject.Destroy(boxUI.gameObject);
                }
            }
        }

        private void AddBoxItem(Item newItem)
        {
            User user = GameProcessor.Inst.User;

            BoxItem boxItem = user.Bags.Find(m => !m.IsFull() && m.Item.Type == newItem.Type && m.Item.ConfigId == newItem.ConfigId);  //ͬ

            if (boxItem != null)
            {
                boxItem.AddStack(newItem.Count);

                //堆叠UI
                var boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType());
                if (boxUI != null)
                {
                    boxUI.AddStack(newItem.Count);
                }
            }
            else
            {
                boxItem = new BoxItem();
                boxItem.Item = newItem;
                boxItem.MagicNubmer.Data = newItem.Count;
                boxItem.BoxId = -1;
                user.Bags.Add(boxItem);

                int bagType = boxItem.GetBagType();

                int lastBoxId = GetNextBoxId(bagType);
                if (lastBoxId < 0)
                {  //包裹已经满了,不显示，但是实际保留
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "包裹" + (bagType + 1) + "已经满了,请清理包裹", ToastType = ToastTypeEnum.Failure });
                    return;
                }
                boxItem.BoxId = lastBoxId;

                var item = this.CreateBox(boxItem);
                item.transform.SetParent(this.BagList[bagType].content.GetChild(lastBoxId));
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.SetBoxId(lastBoxId);
                this.items.Add(item);
            }
        }

        private void WearEquipment(BoxItem boxItem)
        {
            User user = GameProcessor.Inst.User;

            var equip = boxItem.Item as Equip;
            int Part = equip.Part;

            IDictionary<int, Equip> ep = null; ;
            if (Part > 10)
            {
                ep = user.EquipPanelSpecial;
            }
            else
            {
                ep = user.EquipPanelList[user.EquipPanelIndex]; ;
            }

            //增加一次穿戴记录，用做轮流穿戴左右
            if (!user.EquipRecord.ContainsKey(Part))
            {
                user.EquipRecord[Part] = 0;
            }
            user.EquipRecord[Part]++;
            int PartIndex = user.EquipRecord[Part] % equip.Position.Length;
            int Position = equip.Position[PartIndex];

            //从包袱移除
            UseBoxItem(boxItem, 1);

            //如果存在旧装备，增加到包裹
            if (ep.ContainsKey(Position))
            {
                //装备栏卸载
                SlotBox slot = null;

                if (Position <= 10)
                {
                    slot = EquipInfoList[user.EquipPanelIndex].GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();
                }
                else
                {
                    slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();
                }

                Com_Box comItem = slot.GetEquip();
                slot.UnEquip();
                GameObject.Destroy(comItem.gameObject);

                AddBoxItem(ep[Position]);
            }

            //穿戴到格子上
            this.CreateEquipPanelItem(user.EquipPanelIndex, Position, equip);

            //记录
            ep[Position] = equip;

            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUseEquipEvent { });
        }

        public void WearExclusive(BoxItem boxItem)
        {
            User user = GameProcessor.Inst.User;

            var exclusive = boxItem.Item as ExclusiveItem;

            var ep = user.ExclusivePanelList[user.ExclusiveIndex];
            int Position = exclusive.Part;

            //从包袱移除
            UseBoxItem(boxItem, 1);

            //如果存在旧装备，增加到包裹
            if (ep.ContainsKey(Position))
            {
                //装备栏卸载
                SlotBox slot = ExclusiveDialog.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();

                Com_Box comItem = slot.GetEquip();
                slot.UnEquip();
                GameObject.Destroy(comItem.gameObject);

                AddBoxItem(ep[Position]);
            }

            //穿戴到格子上
            this.CreateEquipPanelItem(-1, Position, exclusive);

            user.ExclusivePanelList[user.ExclusiveIndex][Position] = exclusive;

            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUseEquipEvent { });
        }

        private void ClearEquipPanelItem(int position)
        {
            SlotBox slot = GetEquipSolt(position);

            Com_Box comItem = slot.GetEquip();
            if (comItem != null)
            {
                slot.UnEquip();
                GameObject.Destroy(comItem.gameObject);
            }
        }

        private SlotBox GetEquipSolt(int position)
        {
            SlotBox slot = null;

            if (position > 14)
            {
                slot = ExclusiveDialog.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position > 10)
            {
                slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else
            {
                int pi = GameProcessor.Inst.User.EquipPanelIndex;

                var EquipInfo = EquipInfoList[pi];
                slot = EquipInfo.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            return slot;
        }

        private void CreateEquipPanelItem(int pi, int position, Item equip)
        {

            SlotBox slot = null;

            if (position > 14)
            {
                slot = ExclusiveDialog.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position > 10)
            {
                slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else
            {
                var EquipInfo = EquipInfoList[pi];
                slot = EquipInfo.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }

            //生成格子
            BoxItem boxItem = new BoxItem();
            boxItem.Item = equip;
            boxItem.MagicNubmer.Data = 1;
            boxItem.BoxId = -1;

            Com_Box comItem = this.CreateBox(boxItem);
            comItem.transform.SetParent(slot.transform);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;
            comItem.SetBoxId(-1);
            comItem.SetEquipPosition(position);

            //穿戴
            slot.Equip(comItem);
        }

        private void RmoveEquipment(int position, BoxItem boxItem)
        {
            User user = GameProcessor.Inst.User;

            var equip = boxItem.Item as Equip;
            //装备栏卸载
            SlotBox slot = null;

            if (position <= 10)
            {
                slot = EquipInfoList[user.EquipPanelIndex].GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else
            {
                slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }

            Com_Box comItem = slot.GetEquip();
            slot.UnEquip();
            GameObject.Destroy(comItem.gameObject);

            //装备移动到包裹里面
            AddBoxItem(equip);

            //
            IDictionary<int, Equip> ep = null; ;
            if (position > 10)
            {
                ep = user.EquipPanelSpecial;
            }
            else
            {
                ep = user.EquipPanelList[user.EquipPanelIndex]; ;
            }
            ep.Remove(position);

            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUnUseEquipEvent() { });

            //UserData.Save();
        }

        private void RmoveExclusive(BoxItem boxItem)
        {
            User user = GameProcessor.Inst.User;

            var exclusive = boxItem.Item as ExclusiveItem;
            int position = exclusive.Part;

            //装备栏卸载
            SlotBox slot = ExclusiveDialog.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();

            Com_Box comItem = slot.GetEquip();
            slot.UnEquip();
            GameObject.Destroy(comItem.gameObject);

            //装备移动到包裹里面
            AddBoxItem(exclusive);

            user.ExclusivePanelList[user.ExclusiveIndex].Remove(position);

            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUnUseEquipEvent() { });
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

        public int GetNextBoxId(int bagType)
        {
            int maxNum = ConfigHelper.MaxBagCount;
            for (int boxId = 0; boxId < maxNum; boxId++)
            {
                if (this.items.Find(m => m.boxId == boxId && m.BagType == bagType) == null)
                {
                    return boxId;
                }
            }
            return -1;
        }

        public void OnClick_RingSoul()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowSoulRingEvent());
        }
        public void OnClick_PlayerTitle()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowAchievementEvent());
        }

        public void OnClick_Setting()
        {
            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent() { IsOpen = true });
        }

        public void OnExclusive()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowExclusiveEvent());
        }

        public void OnOpenCard()
        {
            this.Dialog_Card.gameObject.SetActive(true);
        }

        public void OnOpenWing()
        {
            this.Dialog_Wing.gameObject.SetActive(true);
        }

        public void OpenFashion()
        {
            GameProcessor.Inst.EventCenter.Raise(new OpenFashionDialogEvent());
        }
    }
}