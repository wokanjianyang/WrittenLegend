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
        [Title("包裹")]
        public List<Toggle> Toggle_Bag_List = new List<Toggle>();
        public List<ScrollRect> Bag_List = new List<ScrollRect>();
        public Button Btn_Reset;

        [Title("方案")]
        public List<Toggle> Toggle_Plan_List = new List<Toggle>();
        public List<RectTransform> Equip_Plan_List = new List<RectTransform>();
        public RectTransform EquipInfoSpecial;
        public Button Btn_ReName;
        public Transform Tran_Plan;
        public InputField If_Name;
        public Button Btn_Ok;
        public Button Btn_Cancle;

        public RectTransform Tf_Equip_Golden;


        [Title("功能按钮")]
        public Button Btn_Attr;
        public Button Btn_Achievement;
        public Button Btn_Cycle;

        public Button btn_SoulRing;
        public Button btn_Wing;
        public Button btn_Exclusive;
        public Button btn_Card;
        public Button btn_Fashion;
        public Button btn_Halidom;
        public Button btn_Artifact;
        public Button btn_Ring;
        public Button btn_Pill;

        public Button btn_Equip_Golden;

        [Title("功能框")]
        public Dialog_Exclusive ExclusiveDialog;
        public Dialog_Card DialogCard;
        public Dialog_Wing DialogWing;
        public Dialog_Halidom DialogHalidom;
        public Dialog_Artifact DialogArtifact;
        public Dialog_Ring DialogRing;
        public Dialog_Cycle DialogCycle;
        public Dialog_Pill DialogPill;

        private List<Com_Box> items = new List<Com_Box>();

        private void Awake()
        {
            for (int i = 0; i < Toggle_Bag_List.Count; i++)
            {
                int index = i;
                Toggle_Bag_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ShowBagPanel(index);
                    }
                });
            }
            this.Btn_Cycle.gameObject.SetActive(false);

            this.Btn_Attr.onClick.AddListener(this.OnClick_Attr);
            this.Btn_Achievement.onClick.AddListener(this.OnClick_Achievement);

            this.btn_SoulRing.onClick.AddListener(this.OnClick_RingSoul);
            this.btn_Wing.onClick.AddListener(OnOpenWing);
            this.btn_Exclusive.onClick.AddListener(OnExclusive);
            this.btn_Fashion.onClick.AddListener(OpenFashion);
            this.btn_Card.onClick.AddListener(OnOpenCard);
            this.btn_Halidom.onClick.AddListener(OnOpenHalidom);
            this.btn_Artifact.onClick.AddListener(OnOpenArtifact);
            this.btn_Ring.onClick.AddListener(OnOpenRing);
            this.btn_Pill.onClick.AddListener(OnOpenPill);
            this.btn_Equip_Golden.onClick.AddListener(OnOpenEquipGolden);

            this.Btn_Reset.onClick.AddListener(OnRefreshBag);
            this.Btn_ReName.onClick.AddListener(OnSetPlanName);
            this.Btn_Ok.onClick.AddListener(OnPlanNameOK);
            this.Btn_Cancle.onClick.AddListener(OnPlanNameClose);
        }

        // Start is called before the first frame update
        void Start()
        {
            ShowEquipPanel();

            User user = GameProcessor.Inst.User;

            string account = user.Account;
            if (account.Length > 0)
            {
                this.Btn_Cycle.gameObject.SetActive(true);
            }

            if (user.Cycle.Data <= 0 && user.MagicLevel.Data < ConfigHelper.Max_Level)
            {
                this.Btn_Cycle.gameObject.SetActive(false);
            }

            if (user.Cycle.Data > 0)
            {
                this.btn_Pill.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Pill.gameObject.SetActive(false);
            }

            if (user.MapId >= 1104)
            {
                this.btn_Equip_Golden.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Equip_Golden.gameObject.SetActive(false);
            }
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
            GameProcessor.Inst.EventCenter.AddListener<RestoreEvent>(this.OnRestoreEvent);
            GameProcessor.Inst.EventCenter.AddListener<LoseEvent>(this.OnLoseEvent);
            GameProcessor.Inst.EventCenter.AddListener<AutoRecoveryEvent>(this.OnAutoRecoveryEvent);
            GameProcessor.Inst.EventCenter.AddListener<BagUseEvent>(this.OnBagUseEvent);
            GameProcessor.Inst.EventCenter.AddListener<BagRemoveEvent>(this.OnBagRemove);

            GameProcessor.Inst.EventCenter.AddListener<CompositeEvent>(this.OnCompositeEvent);
            GameProcessor.Inst.EventCenter.AddListener<SystemUseEvent>(this.OnSystemUse);
            GameProcessor.Inst.EventCenter.AddListener<SelectGiftEvent>(this.OnSelectGift);
            GameProcessor.Inst.EventCenter.AddListener<EquipLockEvent>(this.OnEquipLockEvent);
            GameProcessor.Inst.EventCenter.AddListener<ExchangeEvent>(this.OnExchangeEvent);
            GameProcessor.Inst.EventCenter.AddListener<ChangeExclusiveEvent>(this.OnChangeExclusiveEvent);

            int EquipPanelIndex = GameProcessor.Inst.User.EquipPanelIndex;
            Toggle_Plan_List[EquipPanelIndex].isOn = true;
            this.InitPlanName();

            for (int i = 0; i < Equip_Plan_List.Count; i++)
            {
                int index = i;
                Toggle_Plan_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ChangePlan(index);
                    }
                });
            }

            string account = GameProcessor.Inst.User.Account;
            if (account.Length > 0)
            {
                this.Btn_Cycle.onClick.AddListener(this.OnClick_Cycle);
            }

            GameProcessor.Inst.StartCoroutine(LoadBox());
        }


        private IEnumerator LoadBox()
        {
            User user = GameProcessor.Inst.User;
            user.EventCenter.AddListener<HeroBagUpdateEvent>(this.OnHeroBagUpdateEvent);

            this.items = new List<Com_Box>();

            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");
            yield return null;

            for (int i = 0; i < Equip_Plan_List.Count; i++)
            {
                var EquipInfo = Equip_Plan_List[i];
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

            foreach (var slotBox in Tf_Equip_Golden.GetComponentsInChildren<SlotBox>())
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

            //穿戴金装
            foreach (var kvp in user.EquipPanelGolden)
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

            for (int k = 0; k < Bag_List.Count; k++)
            {
                for (var i = 0; i < ConfigHelper.BagCount[k]; i++)
                {
                    var empty = GameObject.Instantiate(emptyPrefab, this.Bag_List[k].content);
                    empty.name = "Box_" + i;
                    //yield return null;
                }
            }

            //先回收,再加载
            this.FirstRecovery();

            RefreshBag();

            //yield return null;
        }

        private void OnRefreshBag()
        {
            User user = GameProcessor.Inst.User;
            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoverySetting.CheckRecovery(m.Item)).ToList();
            this.RecoveryAll(recoveryList, RuleType.Normal);

            RefreshBag();
        }

        private void OnSetPlanName()
        {
            this.Tran_Plan.gameObject.SetActive(true);
        }

        private void OnPlanNameOK()
        {
            string name = this.If_Name.text.Trim();

            if (name.Length > 2)
            {
                name = name.Substring(0, 2);
            }

            User user = GameProcessor.Inst.User;

            user.PlanNameList[user.EquipPanelIndex] = name;

            this.InitPlanName();
            this.Tran_Plan.gameObject.SetActive(false);
        }

        private void InitPlanName()
        {
            User user = GameProcessor.Inst.User;

            for (int i = 0; i < Toggle_Plan_List.Count; i++)
            {
                user.PlanNameList.TryGetValue(i, out string name);
                if (name != null)
                {
                    Text tt = Toggle_Plan_List[i].GetComponentInChildren<Text>();
                    tt.text = name;
                }
            }
        }

        private void OnPlanNameClose()
        {
            this.Tran_Plan.gameObject.SetActive(false);
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
                for (int i = 0; i < this.Bag_List.Count; i++)
                {
                    BuildBag(i);
                }
            }
        }

        private void BuildBag(int index)
        {
            ScrollRect bagRect = this.Bag_List[index];
            List<BoxItem> list = GameProcessor.Inst.User.Bags.Where(m => m.GetBagType() == index).OrderBy(m => m.GetBagSort()).ToList();

            for (int BoxId = 0; BoxId < list.Count; BoxId++)
            {
                if (BoxId + 1 > ConfigHelper.BagCount[index])
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
            GameObject prefab = PrefabHelper.Instance().GetBoxPrefab(item.Item.GetQuality());
            ////if (item.Item.Type == ItemType.Material || item.Item.Type == ItemType.SkillBox)
            ////{
            ////    prefab = Resources.Load<GameObject>("Prefab/Window/Box_SkillOrMat");
            ////}
            ////else
            ////{
            //switch (item.Item.GetQuality())
            //{
            //    case 0:
            //    case 1:
            //        {
            //            prefab = Resources.Load<GameObject>("Prefab/Window/Box_White");
            //        }
            //        break;
            //    case 2:
            //        {
            //            prefab = Resources.Load<GameObject>("Prefab/Window/Box_Green");
            //        }
            //        break;
            //    case 3:
            //        {
            //            prefab = Resources.Load<GameObject>("Prefab/Window/Box_Blue");
            //        }
            //        break;
            //    case 4:
            //        {
            //            prefab = Resources.Load<GameObject>("Prefab/Window/Box_Pink");
            //        }
            //        break;
            //    case 5:
            //        {
            //            prefab = Resources.Load<GameObject>("Prefab/Window/Box_Orange");
            //        }
            //        break;
            //    case 6:
            //        {
            //            prefab = Resources.Load<GameObject>("Prefab/Window/Box6");
            //        }
            //        break;
            //        //}
            //}

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
            for (int i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    User user = GameProcessor.Inst.User;
                    BoxItem boxItem = user.Bags.Where(m => m.Item.Type == ItemType.Exclusive && m.Item.GetQuality() >= 5 && !m.Item.IsLock).FirstOrDefault();

                    GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent()
                    {
                        BoxItem = boxItem
                    });
                }
                else
                {
                    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                    {
                        Type = ItemType.Material,
                        ItemId = Config.ItemId,
                        Quantity = Config.ItemCount
                    });
                }
            }

            List<Item> list = new List<Item>();
            Item item = ItemHelper.BuildItem((ItemType)Config.TargetType, Config.TargetId, 5, 1);
            list.Add(item);
            GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = list });

            GameProcessor.Inst.EventCenter.Raise(new ExchangeUIFreshEvent());
        }

        private void OnSystemUse(SystemUseEvent e)
        {
            User user = GameProcessor.Inst.User;

            List<BoxItem> list = user.Bags.Where(m => m.Item.Type == e.Type && m.Item.ConfigId == e.ItemId).ToList();

            long count = list.Select(m => m.MagicNubmer.Data).Sum();

            long useCount = Math.Abs(e.Quantity);

            if (count < useCount)
            {
                throw new Exception();
                //GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
            }

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


            List<BoxItem> newList = user.Bags.Where(m => m.Item.Type == e.Type && m.Item.ConfigId == e.ItemId).ToList();

            long newCount = newList.Select(m => m.MagicNubmer.Data).Sum();
            if (newCount >= count)
            {
                throw new Exception();
                //GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
            }

        }

        private void OnSelectGift(SelectGiftEvent e)
        {
            if (UseBoxItem(e.BoxItem, 1))
            {
                List<Item> items = new List<Item>();
                items.Add(e.Item);
                GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
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
            GameProcessor.Inst.User.EventCenter.Raise(new SkillChangePlanEvent());

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

            Tf_Equip_Golden.gameObject.SetActive(false);

            for (int i = 0; i < this.Equip_Plan_List.Count; i++)
            {
                if (i == position)
                {
                    this.Equip_Plan_List[i].gameObject.SetActive(true);
                }
                else
                {
                    this.Equip_Plan_List[i].gameObject.SetActive(false);
                }
            }
            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        }

        private void ShowBagPanel(int index)
        {
            for (int i = 0; i < this.Bag_List.Count; i++)
            {
                if (i == index)
                {
                    this.Bag_List[i].gameObject.SetActive(true);
                }
                else
                {
                    this.Bag_List[i].gameObject.SetActive(false);
                }
            }
        }


        private void OnEquipOneEvent(EquipOneEvent e)
        {
            User user = GameProcessor.Inst.User;
            int type = e.BoxItem.GetBagType();
            int total = user.GetBagIdleCount(type);

            if (total < 5)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "包裹空额不足了，请先清理包裹", ToastType = ToastTypeEnum.Failure });
                return;
            }


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

            if (UseBoxItem(e.BoxItem, 1))
            {
                user.EventCenter.Raise(new HeroUseSkillBookEvent
                {
                    IsLearn = true,
                    BoxItem = e.BoxItem,
                    Quantity = 1,
                });
            }
        }

        private void FirstRecovery()
        {
            User user = GameProcessor.Inst.User;
            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoverySetting.CheckRecovery(m.Item)).ToList();
            this.RecoveryAll(recoveryList, RuleType.Normal);
        }

        private void OnRecoveryEvent(RecoveryEvent e)
        {
            //手动点击回收的
            if (e.Quantity > 0)
            {
                this.RecoverySingle(e.BoxItem, e.Quantity);
            }
            else
            {
                List<BoxItem> recoveryList = new List<BoxItem>();
                recoveryList.Add(e.BoxItem);
                this.RecoveryAll(recoveryList, RuleType.Normal);
            }

        }

        private void OnRestoreEvent(RestoreEvent e)
        {
            BoxItem boxItem = e.BoxItem;
            int bagType = boxItem.GetBagType();

            User user = GameProcessor.Inst.User;

            int haveCount = user.GetBagIdleCount(bagType);

            if (user.MagicGold.Data <= ConfigHelper.RestoreGold)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足5000兆", ToastType = ToastTypeEnum.Failure });
                return;
            }

            List<Item> newList = new List<Item>();

            if (boxItem.Item.Type == ItemType.Exclusive)
            {
                ExclusiveItem oldExclusive = boxItem.Item as ExclusiveItem;

                ExclusiveItem exclusive = new ExclusiveItem(oldExclusive.ConfigId, oldExclusive.RuneConfigId, oldExclusive.SuitConfigId, oldExclusive.Quality, oldExclusive.DoubleHitId);
                newList.Add(exclusive);
                for (int i = 0; i < oldExclusive.RuneConfigIdList.Count; i++)
                {
                    ExclusiveItem item = new ExclusiveItem(oldExclusive.ConfigId, oldExclusive.RuneConfigIdList[i], oldExclusive.SuitConfigIdList[i], oldExclusive.Quality, oldExclusive.DoubleHitId);
                    newList.Add(item);
                }

                Dictionary<int, int> useMeterial = ExclusiveDevourConfigCategory.Instance.GetUseList(oldExclusive.GetLayer());
                foreach (KeyValuePair<int, int> kv in useMeterial)
                {
                    //int mc = Math.Max(1, (int)(kv.Value * 0.8));
                    Item item = ItemHelper.BuildMaterial(kv.Key, kv.Value);
                    newList.Add(item);
                }

                foreach (var kv in oldExclusive.LevelDict)
                {
                    int runeId = kv.Key;
                    int runeLevel = kv.Value;
                    SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.Get(runeId);
                    int suitId = SkillSuitHelper.RandomSuit(0, runeConfig.SkillId, runeConfig.Type).Id;

                    for (int i = 0; i < runeLevel; i++)
                    {
                        ExclusiveItem item = new ExclusiveItem(oldExclusive.ConfigId, runeId, suitId, oldExclusive.Quality, oldExclusive.DoubleHitId);
                        newList.Add(item);
                    }

                    int[] ItemIdList = new int[] { ItemHelper.SpecialId_Exclusive_Stone, ItemHelper.SpecialId_Exclusive_Heart };
                    int[] ItemCountList = new int[] { 50, 5 };

                    for (int i = 0; i < ItemIdList.Length; i++)
                    {
                        Item item = ItemHelper.BuildMaterial(ItemIdList[i], ItemCountList[i] * runeLevel);
                        newList.Add(item);
                    }
                }

                if (haveCount < newList.Count)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留" + newList.Count + "个包裹空额", ToastType = ToastTypeEnum.Failure });
                    return;
                }
            }
            else if (boxItem.Item.Type == ItemType.Equip)
            {
                Equip equip = boxItem.Item as Equip;
                int layer = equip.Layer;

                for (int l = 1; l < layer; l++)
                {
                    EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Part == equip.Part && m.Layer == l).FirstOrDefault();

                    Item item = ItemHelper.BuildMaterial(config.MetailId, config.MetailCount);
                    newList.Add(item);

                    Item item1 = ItemHelper.BuildMaterial(config.MetailId1, config.MetailCount1);
                    newList.Add(item1);

                }

                int redNumber = 0;
                foreach (var kv in equip.HoneList)
                {
                    int honeLevel = kv.Value;
                    redNumber += EquipHoneConfigCategory.Instance.GetTotalNeedNumber(honeLevel);
                }

                if (redNumber > 0)
                {
                    Item redItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Red_Stone, redNumber);
                    newList.Add(redItem);
                }

                if (haveCount < newList.Count)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留" + newList.Count + "个包裹空额", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                equip.Layer = 1;
                equip.HoneList = new Dictionary<int, int>();
                newList.Add(equip);
            }

            //Fee
            user.SubGold(ConfigHelper.RestoreGold);

            //销毁旧的
            user.Bags.Remove(boxItem);
            Com_Box boxUI = this.items.Find(m => m.BoxItem == boxItem);
            if (boxUI != null) //上线自动回收，可能还没加载
            {
                this.items.Remove(boxUI);
                GameObject.Destroy(boxUI.gameObject);
            }

            //生成新的
            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = newList });
        }

        private void OnLoseEvent(LoseEvent e)
        {
            User user = GameProcessor.Inst.User;

            BoxItem boxItem = e.BoxItem;

            if (boxItem == null)
            {
                //Log.Debug("此物品已经被使用了");
                return;
            }

            user.Bags.Remove(boxItem);

            //Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType());
            Com_Box boxUI = this.items.Find(m => m.BoxItem == boxItem);
            if (boxUI != null) //上线自动回收，可能还没加载
            {
                this.items.Remove(boxUI);
                GameObject.Destroy(boxUI.gameObject);
            }
        }

        private void OnAutoRecoveryEvent(AutoRecoveryEvent e)
        {
            User user = GameProcessor.Inst.User;
            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoverySetting.CheckRecovery(m.Item)).ToList();
            this.RecoveryAll(recoveryList, e.RuleType);
        }

        private void RecoverySingle(BoxItem boxItem, int quantity)
        {
            User user = GameProcessor.Inst.User;

            long gold = 0;

            UseBoxItem(boxItem, quantity);

            List<Item> itemList = new List<Item>();
            if (boxItem.Item.ItemConfig.RecoveryItemId > 0)
            {
                Item item = ItemHelper.BuildMaterial(boxItem.Item.ItemConfig.RecoveryItemId, quantity * boxItem.Item.ItemConfig.RecoveryCount);
                AddBoxItem(item);
                itemList.Add(item);
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType.Normal,
                Message = BattleMsgHelper.BuildAutoRecoveryMessage(quantity, itemList, gold)
            });
        }

        private void RecoveryAll(List<BoxItem> recoveryList, RuleType ruleType)
        {
            User user = GameProcessor.Inst.User;

            List<Item> itemList = new List<Item>();

            int refineStone = 0;
            int speicalStone = 0;
            int exclusiveStone = 0;
            int cardStone = 0;

            Dictionary<int, int> recoveryDict = new Dictionary<int, int>();

            long gold = 0;

            foreach (BoxItem box in recoveryList)
            {
                //gold += box.Item.Gold * box.MagicNubmer.Data;

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

                    int RecoveryItemId = equip.EquipConfig.RecoveryItemId;
                    if (equip.GetQuality() >= 5 && RecoveryItemId > 0)
                    {
                        if (!recoveryDict.ContainsKey(RecoveryItemId))
                        {
                            recoveryDict[RecoveryItemId] = 0;
                        }
                        recoveryDict[RecoveryItemId] += 1;
                    }

                    gold += equip.EquipConfig.Price;
                }
                else if (box.Item.Type == ItemType.Exclusive)
                {
                    exclusiveStone += box.Item.GetQuality() * 1;
                }
                else if (box.Item.Type == ItemType.Card)
                {
                    cardStone += box.Item.GetQuality() * ((int)box.MagicNubmer.Data);
                }
                else if (box.Item.ItemConfig.RecoveryItemId > 0)
                {
                    Item item = ItemHelper.BuildMaterial(box.Item.ItemConfig.RecoveryItemId, box.MagicNubmer.Data * box.Item.ItemConfig.RecoveryCount);
                    AddBoxItem(item);
                    itemList.Add(item);
                }
                else
                {
                    gold += box.Item.ItemConfig.Price * ((int)box.MagicNubmer.Data);
                }

                UseBoxItem(box, box.MagicNubmer.Data);
            }

            if (gold > 0)
            {
                user.AddExpAndGold(0, gold);
            }

            if (refineStone > 0)
            {
                Item item = ItemHelper.BuildRefineStone(refineStone);
                AddBoxItem(item);
                itemList.Add(item);
            }
            if (exclusiveStone > 0)
            {
                Item exStoneItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Stone, exclusiveStone);
                AddBoxItem(exStoneItem);
                itemList.Add(exStoneItem);
            }
            if (speicalStone > 0)
            {
                Item speicalStoneItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Equip_Speical_Stone, speicalStone);
                AddBoxItem(speicalStoneItem);
                itemList.Add(speicalStoneItem);
            }
            if (cardStone > 0)
            {
                Item cardItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Card_Stone, cardStone);
                AddBoxItem(cardItem);
                itemList.Add(cardItem);
            }
            foreach (var kvp in recoveryDict)
            {
                Item recoveryItem = ItemHelper.BuildMaterial(kvp.Key, kvp.Value);
                AddBoxItem(recoveryItem);
                itemList.Add(recoveryItem);
            }

            if (recoveryList.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Type = ruleType,
                    Message = BattleMsgHelper.BuildAutoRecoveryMessage(recoveryList.Count, itemList, gold)
                });
            }
        }

        private void OnBagUseEvent(BagUseEvent e)
        {
            User user = GameProcessor.Inst.User;

            BoxItem boxItem = e.BoxItem;
            long quantity = e.Quantity <= 0 ? boxItem.MagicNubmer.Data : e.Quantity;

            if (boxItem.Item.Type == ItemType.Ticket && boxItem.Item.ConfigId == ItemHelper.SpecialId_Copy_Ticket && e.Quantity == -1)
            {
                if (user.MagicCopyTikerCount.Data >= ConfigHelper.CopyTicketMax)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "次数已经满了", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                quantity = Math.Min(quantity, ConfigHelper.CopyTicketMax - user.MagicCopyTikerCount.Data);
            }
            else if (boxItem.Item.Type == ItemType.Material_Usable && boxItem.Item.ConfigId == ItemHelper.SpecialId_Level_Stone)
            {
                quantity = Math.Min(quantity, user.GetMaxLevel() - user.MagicLevel.Data);

                if (quantity <= 0)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满级了", ToastType = ToastTypeEnum.Failure });
                    return;
                }
            }

            if (quantity <= 0)
            {
                throw new Exception();
                //GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
            }

            if (!UseBoxItem(boxItem, quantity))
            {
                return;
            }

            //use logic
            if (boxItem.Item.Type == ItemType.Material_Usable && boxItem.Item.ConfigId == ItemHelper.SpecialId_Level_Stone)
            {
                quantity = Math.Min(quantity, user.GetMaxLevel() - user.MagicLevel.Data);

                if (quantity <= 0)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满级了", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                user.MagicLevel.Data += quantity;
                user.EventCenter.Raise(new SetPlayerLevelEvent { Cycle = user.Cycle.Data, Level = user.MagicLevel.Data });
            }
            else if (boxItem.Item.Type == ItemType.SkillBox)
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
                GiftPack giftPack = boxItem.Item as GiftPack;
                GiftPackConfig pc = giftPack.Config;

                List<Item> items = new List<Item>();
                for (int i = 0; i < pc.ItemIdList.Length; i++)
                {
                    Item item = ItemHelper.BuildItem((ItemType)pc.ItemTypeList[i], pc.ItemIdList[i], 1, (int)(quantity * pc.ItemCountList[i]));
                    //this.AddBoxItem(item);
                    items.Add(item);
                }
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildGiftPackMessage("礼包奖励:", 0, 0, items)
                });
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
            else if (boxItem.Item.Type == ItemType.Ticket)
            {
                if (boxItem.Item.ConfigId == ItemHelper.SpecialId_Copy_Ticket)
                {
                    user.MagicCopyTikerCount.Data += quantity;
                }
                else if (boxItem.Item.ConfigId == ItemHelper.SpecialId_Legacy_Ticket)
                {
                    user.LegacyTikerCount.Data += quantity;
                }
                else if (boxItem.Item.ConfigId == ItemHelper.SpecialId_Pill_Ticket)
                {
                    user.PillTime.Time.Data += quantity * ConfigHelper.PillDefaultTime;
                }

            }

        }

        private void OnBagRemove(BagRemoveEvent e)
        {
            if (GameProcessor.Inst.User.RemoveBagItem(e.BoxItem))
            {
                Com_Box boxUI = this.items.Find(m => m.BoxItem == e.BoxItem);
                if (boxUI != null) //上线自动回收，可能还没加载
                {
                    this.items.Remove(boxUI);
                    GameObject.Destroy(boxUI.gameObject);
                    boxUI = null;
                }
            }
            else
            {
                throw new Exception("道具不存在");
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

        private bool UseBoxItem(BoxItem boxItem, long quantity)
        {
            User user = GameProcessor.Inst.User;

            //逻辑处理

            if (boxItem == null)
            {
                //Log.Debug("此物品已经被使用了");
                return false;
            }

            boxItem.RemoveStack(quantity);

            //Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType());
            Com_Box boxUI = this.items.Find(m => m.BoxItem == boxItem);
            if (boxUI != null) //上线自动回收，可能还没加载
            {
                boxUI.RemoveStack(quantity);
                if (boxItem.MagicNubmer.Data <= 0)
                {
                    this.items.Remove(boxUI);
                    GameObject.Destroy(boxUI.gameObject);
                }
            }

            //用光了，移除队列
            if (boxItem.MagicNubmer.Data <= 0)
            {
                user.Bags.Remove(boxItem);
                boxItem = null;
            }

            return true;
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

                int bagType = boxItem.GetBagType();
                int idleCount = user.GetBagIdleCount(bagType);

                if (idleCount < 1)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "包裹" + (bagType + 1) + "已经满了,请清理包裹", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                user.Bags.Add(boxItem);

                int lastBoxId = GetNextBoxId(bagType);
                if (lastBoxId < 0)
                {  //包裹已经满了,不显示，但是实际保留
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "包裹" + (bagType + 1) + "已经满了,请清理包裹", ToastType = ToastTypeEnum.Failure });
                    return;
                }
                boxItem.BoxId = lastBoxId;

                var item = this.CreateBox(boxItem);
                item.transform.SetParent(this.Bag_List[bagType].content.GetChild(lastBoxId));
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

            if (Part <= 10)
            {
                ep = user.EquipPanelList[user.EquipPanelIndex]; ;
            }
            else if (Part >= 11 && Part <= 14)
            {
                ep = user.EquipPanelSpecial;
            }
            else if (Part >= 21 && Part <= 30)
            {
                ep = user.EquipPanelGolden;
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
                    slot = Equip_Plan_List[user.EquipPanelIndex].GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();
                }
                else if (Position >= 11 && Position <= 14)
                {
                    slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();
                }
                else if (Position >= 21 && Position <= 30)
                {
                    slot = Tf_Equip_Golden.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();
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

                var EquipInfo = Equip_Plan_List[pi];
                slot = EquipInfo.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            return slot;
        }

        private void CreateEquipPanelItem(int pi, int position, Item equip)
        {

            SlotBox slot = null;

            if (position <= 10)
            {
                var EquipInfo = Equip_Plan_List[pi];
                slot = EquipInfo.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 11 && position <= 14)
            {
                slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 15 && position <= 20)
            {
                slot = ExclusiveDialog.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 21 && position <= 30)
            {
                slot = Tf_Equip_Golden.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
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
                slot = Equip_Plan_List[user.EquipPanelIndex].GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 11 && position <= 14)
            {
                slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 15 && position <= 20)
            {
                slot = ExclusiveDialog.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 21 && position <= 30)
            {
                slot = Tf_Equip_Golden.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }

            Com_Box comItem = slot.GetEquip();
            slot.UnEquip();
            GameObject.Destroy(comItem.gameObject);

            //装备移动到包裹里面
            AddBoxItem(equip);

            //
            if (position <= 10)
            {
                user.EquipPanelList[user.EquipPanelIndex].Remove(position);

            }
            else if (position >= 11 && position <= 14)
            {
                user.EquipPanelSpecial.Remove(position);
            }
            else if (position >= 15 && position <= 20)
            {

            }
            else if (position >= 21 && position <= 30)
            {
                user.EquipPanelGolden.Remove(position);
            }

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

                foreach (Item newItem in newItems)
                {
                    if (newItem.Type == ItemType.Buff)
                    {
                        //TODO
                    }
                    else if (newItem.Type == ItemType.Artifact)
                    {
                        //user.MetalData
                        user.SaveArtifactLevel(newItem.ConfigId, (int)newItem.Count);
                    }
                    else if (newItem.Type == ItemType.Card || newItem.Type == ItemType.Fashion || (newItem.Type == ItemType.Material && newItem.ConfigId == ItemHelper.SpecialId_Card_Stone))
                    {
                        user.SaveItemMeterialCount(newItem.ConfigId, newItem.Count);
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
            int maxNum = ConfigHelper.BagCount[bagType];
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

        public void OnClick_Attr()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowDialogUserAttrEvent());
        }

        public void OnClick_Achievement()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowAchievementEvent());
        }
        public void OnClick_Cycle()
        {
            this.DialogCycle.gameObject.SetActive(true);

        }
        public void OnExclusive()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowExclusiveEvent());
        }

        public void OnOpenCard()
        {
            this.DialogCard.gameObject.SetActive(true);
        }

        public void OnOpenHalidom()
        {
            this.DialogHalidom.gameObject.SetActive(true);
        }

        public void OnOpenArtifact()
        {
            this.DialogArtifact.Show();
        }

        public void OnOpenRing()
        {
            this.DialogRing.Show();
        }

        public void OnOpenPill()
        {
            this.DialogPill.gameObject.SetActive(true);
        }

        public void OnOpenWing()
        {
            this.DialogWing.gameObject.SetActive(true);
        }

        public void OnOpenEquipGolden()
        {
            this.Tf_Equip_Golden.gameObject.SetActive(true);

            for (int i = 0; i < this.Equip_Plan_List.Count; i++)
            {
                this.Equip_Plan_List[i].gameObject.SetActive(false);
            }
        }

        public void OpenFashion()
        {
            GameProcessor.Inst.EventCenter.Raise(new OpenFashionDialogEvent());
        }
    }
}