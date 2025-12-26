using Newtonsoft.Json;
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

        //public RectTransform Tf_Equip_Golden;


        [Title("功能按钮")]
        public Button Btn_Attr;
        public Button Btn_Achievement;
        public Button Btn_Cycle;

        public Button btn_SoulRing;
        public Button btn_Wing;
        public Button btn_Exclusive;
        public Button btn_Card;
        public Button btn_Shengxiao;

        public Button btn_Fashion;
        public Button btn_Halidom;
        public Button btn_Artifact;
        public Button btn_Ring;
        public Button btn_Pill;
        public Button btn_Talent;
        public Button btn_Pet;

        public Button btn_Equip_Golden;
        public Button btn_Equip_Dark_Gold;
        public Button btn_Equip_Hundun;
        public Button btn_Relic;


        [Title("功能框")]
        public Dialog_Exclusive ExclusiveDialog;
        public Dialog_Card DialogCard;
        public Dialog_Wing DialogWing;
        public Dialog_Halidom DialogHalidom;
        public Dialog_Artifact DialogArtifact;
        public Dialog_Ring DialogRing;
        public Dialog_Cycle DialogCycle;
        public Dialog_Pill DialogPill;
        public Dialog_EquipGolden DialogEquipGolden;
        public Dialog_Equip_Dark_Gold DialogEquipDarkGold;
        public Dialog_Equip_Hundun DialogEquipHundun;
        public Dialog_Shengxiao_Panel DialogShengxiaoPanel;

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
            this.btn_Equip_Dark_Gold.onClick.AddListener(OnOpenEquipDarkGold);
            this.btn_Equip_Hundun.onClick.AddListener(OnOpenEquipHundun);
            this.btn_Talent.onClick.AddListener(OnOpenTalent);
            this.btn_Relic.onClick.AddListener(OnOpenRelic);
            this.btn_Shengxiao.onClick.AddListener(OnOpenShengxiao);

            this.Btn_Reset.onClick.AddListener(OnRefreshBag);
            this.Btn_ReName.onClick.AddListener(OnSetPlanName);
            this.Btn_Ok.onClick.AddListener(OnPlanNameOK);
            this.Btn_Cancle.onClick.AddListener(OnPlanNameClose);

            this.btn_Pet.onClick.AddListener(OnOpenPet);
        }

        // Start is called before the first frame update
        void Start()
        {
            ShowEquipPanel();

            User user = GameProcessor.Inst.User;
            bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

            string account = user.Account;

            if (account.Length > 0 || user.GetLimitId() >= 1030)
            {
                this.Btn_Cycle.gameObject.SetActive(true);
                this.Btn_Cycle.onClick.AddListener(this.OnClick_Cycle);
            }

            if (user.Cycle.Data <= 0 && user.MagicLevel.Data < ConfigHelper.Max_Level - 10000)
            {
                this.Btn_Cycle.gameObject.SetActive(false);
            }

            if (user.Cycle.Data > 0)
            {
                this.btn_Pill.gameObject.SetActive(true);
                this.btn_Talent.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Pill.gameObject.SetActive(false);
                this.btn_Talent.gameObject.SetActive(false);
            }

            if (user.Cycle.Data >= 4)
            {
                this.btn_Relic.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Relic.gameObject.SetActive(false);
            }

            if (user.MapId >= 1051)
            {
                this.btn_Halidom.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Halidom.gameObject.SetActive(false);
            }

            if (user.MapId >= 1104)
            {
                this.btn_Equip_Golden.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Equip_Golden.gameObject.SetActive(false);
            }

            if (user.MapId >= 1139 && !ac)
            {
                this.btn_Equip_Dark_Gold.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Equip_Dark_Gold.gameObject.SetActive(false);
            }

            if (user.MapId >= 1174 && !ac)
            {
                this.btn_Equip_Hundun.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Equip_Hundun.gameObject.SetActive(false);
            }

            if (user.Cycle.Data > 0 || user.MagicLevel.Data >= 50000 || user.PetList.Count > 0)
            {
                this.btn_Pet.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Pet.gameObject.SetActive(false);
            }

            if (user.Cycle.Data >= 10)
            {
                this.btn_Shengxiao.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Shengxiao.gameObject.SetActive(false);
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
            //GameProcessor.Inst.EventCenter.AddListener<ChangeExclusiveEvent>(this.OnChangeExclusiveEvent);
            GameProcessor.Inst.EventCenter.AddListener<ChangeEquipPlanEvent>(this.OnChangeEquipPlanEvent);

            GameProcessor.Inst.EventCenter.AddListener<PetBattleUpEvent>(this.PetBattleUp);

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

            List<SlotBox> gds = DialogEquipGolden.GetComponentsInChildren<SlotBox>().ToList();
            for (int i = 0; i < gds.Count; i++)
            {
                gds[i].Init(prefab, 21 + i);
                yield return null;
            }

            List<SlotBox> dgds = DialogEquipDarkGold.GetComponentsInChildren<SlotBox>().ToList();
            for (int i = 0; i < dgds.Count; i++)
            {
                dgds[i].Init(prefab, 31 + i);
                yield return null;
            }

            List<SlotBox> hunduns = DialogEquipHundun.GetComponentsInChildren<SlotBox>().ToList();
            for (int i = 0; i < dgds.Count; i++)
            {
                hunduns[i].Init(prefab, 41 + i);
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
            foreach (var kvp in user.EquipPanelGoldenList[user.EquipGoldenIndex])
            {
                this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
                //yield return null;
            }

            //穿戴暗金装
            foreach (var kvp in user.EquipPanelDarkGoldList[user.EquipDarkGoldIndex])
            {
                this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
                //yield return null;
            }

            //穿戴混沌装
            foreach (var kvp in user.EquipPanelHundunList[user.EquipHundunIndex])
            {
                this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
                //yield return null;
            }

            ////穿戴专属
            //foreach (var kvp in user.ExclusivePanelList[user.ExclusiveIndex])
            //{
            //    this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
            //}

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
            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoveryNew.CheckRecovery(m.Item, RecoveryType.Other)).ToList();
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

                Com_Box box = PrefabHelper.Instance().CreateComBox(item);
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

        private void OnCompositeEvent(CompositeEvent e)
        {
            CompositeConfig Config = e.Config;
            long number = e.Number;

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

                    number = 1; //防止其他地方数量配置不对
                }
                else
                {
                    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                    {
                        Type = type,
                        ItemId = configId,
                        Quantity = Config.ItemCountList[i] * number
                    });
                }
            }

            Item item = ItemHelper.BuildItem((ItemType)Config.TargetType, Config.TargetId, 1, number);
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
                    BoxItem boxItem = user.Bags.Where(m => m.Item.Type == ItemType.Exclusive && m.Item.GetQuality() == 5 && !m.Item.IsLock && (m.Item as ExclusiveItem).GetLevel() < 1 && (m.Item as ExclusiveItem).GetLayer() <= 1).FirstOrDefault();

                    if (boxItem == null)
                    {
                        return;
                    }

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
            if (UseBoxItem(e.BoxItem, e.Nubmer))
            {
                List<Item> items = new List<Item>();

                e.Item.Count *= e.Nubmer;

                items.Add(e.Item);
                GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
        }

        //private void OnChangeExclusiveEvent(ChangeExclusiveEvent e)
        //{
        //    User user = GameProcessor.Inst.User;
        //    user.ExclusiveIndex = e.Index;

        //    for (int i = 15; i <= 20; i++)
        //    {
        //        this.ClearEquipPanelItem(i);
        //    }

        //    foreach (var kvp in user.ExclusivePanelList[e.Index])
        //    {
        //        this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
        //    }

        //    //Debug.Log("OnChangeExclusiveEvent");
        //}

        private void OnChangeEquipPlanEvent(ChangeEquipPlanEvent e)
        {
            User user = GameProcessor.Inst.User;

            if (e.Type == 3)
            {
                user.EquipGoldenIndex = e.Index;

                for (int i = 21; i <= 30; i++)
                {
                    this.ClearEquipPanelItem(i);
                }

                foreach (var kvp in user.EquipPanelGoldenList[e.Index])
                {
                    this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
                }
            }
            else if (e.Type == 4)
            {
                user.EquipDarkGoldIndex = e.Index;

                for (int i = 31; i <= 40; i++)
                {
                    this.ClearEquipPanelItem(i);
                }

                foreach (var kvp in user.EquipPanelDarkGoldList[e.Index])
                {
                    this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
                }

            }
            else if (e.Type == 5)
            {
                user.EquipHundunIndex = e.Index;

                for (int i = 41; i <= 50; i++)
                {
                    this.ClearEquipPanelItem(i);
                }

                foreach (var kvp in user.EquipPanelHundunList[e.Index])
                {
                    this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
                }
            }
        }


        private void ChangePlan(int index)
        {
            User user = GameProcessor.Inst.User;
            user.EquipPanelIndex = index;

            if (user.ExclusiveSetting)
            {
                user.ExclusiveIndex = index;
                //GameProcessor.Inst.EventCenter.Raise(new ChangeExclusiveEvent() { Index = index });
            }

            if (user.EquipGoldenSetting)
            {
                user.EquipGoldenIndex = index;
                GameProcessor.Inst.EventCenter.Raise(new ChangeEquipPlanEvent() { Type = 3, Index = index });
            }

            if (user.EquipDarkGoldSetting)
            {
                user.EquipDarkGoldIndex = index;
                GameProcessor.Inst.EventCenter.Raise(new ChangeEquipPlanEvent() { Type = 4, Index = index });
            }

            if (user.EquipHundunSetting)
            {
                user.EquipHundunIndex = index;
                GameProcessor.Inst.EventCenter.Raise(new ChangeEquipPlanEvent() { Type = 5, Index = index });
            }

            user.SkillPanelIndex = index;

            ShowEquipPanel();

            GameProcessor.Inst.User.EventCenter.Raise(new SkillChangePlanEvent());
            GameProcessor.Inst.User.EventCenter.Raise(new UserAttrChangeEvent());
        }

        private void ShowEquipPanel()
        {
            int position = GameProcessor.Inst.User.EquipPanelIndex;

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
                else if (e.BoxItem.Item.Type == ItemType.Shengxiao)
                {
                    this.WearShengxiao(e.BoxItem);
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
                else if (e.BoxItem.Item.Type == ItemType.Shengxiao)
                {
                    this.RemoveShengxiao(e.BoxItem);
                }
                else
                {
                    this.RmoveEquipment(e.Part, e.BoxItem);
                }
            }
            //UserData.Save();

            TaskHelper.CheckTask(TaskType.Equip, 1);
        }

        private void PetBattleUp(PetBattleUpEvent e)
        {
            User user = GameProcessor.Inst.User;

            int pg = Math.Min(3, user.GetPetSpeicalGroupLevel() / 3);

            if (user.PetList.Count >= ConfigHelper.PetMax + pg)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "宠物上阵位置已经满了", ToastType = ToastTypeEnum.Failure });
                return;
            }

            Pet pet = e.BoxItem.Item as Pet;

            int roleCount = user.PetList.Where(m => m.Role == pet.Role).Count();
            if (roleCount >= 3)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "同种宠物上阵数量已满", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //从包袱移除
            UseBoxItem(e.BoxItem, 1);

            user.PetList.Add(pet);

            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUseEquipEvent { });
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
            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoveryNew.CheckRecovery(m.Item, RecoveryType.Other)).ToList();
            this.RecoveryAll(recoveryList, RuleType.Normal);
        }

        private void OnRecoveryEvent(RecoveryEvent e)
        {
            //回收部分
            if (e.Quantity > 0)
            {
                this.RecoverySingle(e.BoxItem, e.Quantity);
            }
            //回收全部
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
                exclusive.Count = 1;
                newList.Add(exclusive);
                for (int i = 0; i < oldExclusive.RuneConfigIdList.Count; i++)
                {
                    ExclusiveItem item = new ExclusiveItem(oldExclusive.ConfigId, oldExclusive.RuneConfigIdList[i], oldExclusive.SuitConfigIdList[i], oldExclusive.Quality, oldExclusive.DoubleHitId);
                    item.Count = 1;
                    newList.Add(item);
                }

                Dictionary<int, int> useMeterial = ExclusiveDevourConfigCategory.Instance.GetUseList(oldExclusive.ExclusiveConfig.Cycle, oldExclusive.GetLayer(), oldExclusive.ExclusiveLevel);
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
                        item.Count = 1;
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
            else if (boxItem.Item.Type == ItemType.Pet)
            {
                Pet pet = boxItem.Item as Pet;
                long level = pet.PetLevel.Data;
                long layer = pet.PetLayer.Data;
                int quality = pet.GetQuality();

                long expCount = PetConfigCategory.Instance.GetFeeTotal(level) + pet.LevelExp.Data;
                long layerCount = PetConfigCategory.Instance.GetPetLayerFeeTotal(layer);

                pet.PetLayer.Data = 1;
                pet.PetLevel.Data = 1;
                pet.LevelExp.Data = 0;


                //Debug.Log("pet exp count:" + expCount);
                if (expCount > 0)
                {
                    Item levelItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Pet_Exp, expCount);
                    newList.Add(levelItem);
                }

                if (layerCount > 0)
                {
                    Item layerItem = ItemHelper.BuildMaterial(ItemHelper.Specail_Pet_Layer[quality - 5], layerCount);
                    newList.Add(layerItem);
                }

                newList.Add(pet);
            }
            else if (boxItem.Item.Type == ItemType.Shengxiao)
            {
                Shengxiao pet = boxItem.Item as Shengxiao;
                long level = pet.LevelData.Data;
                long layer = pet.LayerData.Data;
                int quality = pet.GetQuality();

                pet.LevelData.Data = 0;
                pet.LayerData.Data = 0;

                //Debug.Log("pet exp count:" + expCount);
                if (level > 0)
                {
                    Item levelItem = ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao, 5000000 * level);
                    newList.Add(levelItem);

                    Item layerItem1 = ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao1, 30 * level);
                    newList.Add(layerItem1);
                }

                if (layer > 0)
                {
                    Item layerItem = ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao1, 20 * layer);
                    newList.Add(layerItem);

                    Item layerItem1 = ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao2, layer);
                    newList.Add(layerItem1);
                }

                newList.Add(pet);
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

                //生成新的
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = newList });
            }
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
            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoveryNew.CheckRecovery(m.Item, RecoveryType.Other)).ToList();
            this.RecoveryAll(recoveryList, e.RuleType);
        }

        private void RecoverySingle(BoxItem boxItem, int quantity)
        {
            User user = GameProcessor.Inst.User;

            long gold = 0;

            UseBoxItem(boxItem, quantity);

            List<Item> itemList = new List<Item>();

            Dictionary<int, long> recoveryDict = user.Recovery(boxItem.Item, out long recoveryGold);

            gold += recoveryGold * boxItem.MagicNubmer.Data;

            foreach (var kvp in recoveryDict)
            {
                if (kvp.Value > 0)
                {
                    Item recoveryItem = ItemHelper.BuildMaterial(kvp.Key, kvp.Value * quantity);
                    AddBoxItem(recoveryItem);
                    itemList.Add(recoveryItem);
                }
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

            Dictionary<int, long> recoveryDict = new Dictionary<int, long>();

            long gold = 0;

            foreach (BoxItem box in recoveryList)
            {
                Dictionary<int, long> dict = user.Recovery(box.Item, out long recoveryGold);

                gold += recoveryGold;

                foreach (var sp in dict)
                {
                    if (!recoveryDict.ContainsKey(sp.Key))
                    {
                        recoveryDict[sp.Key] = 0;
                    }

                    long count = Math.Max(1, box.MagicNubmer.Data);
                    recoveryDict[sp.Key] += (sp.Value * count);
                }


                UseBoxItem(box, box.MagicNubmer.Data);
            }

            if (gold > 0)
            {
                user.AddExpAndGold(0, gold);
            }

            foreach (var kvp in recoveryDict)
            {
                if (kvp.Value > 0)
                {
                    Item recoveryItem = ItemHelper.BuildMaterial(kvp.Key, kvp.Value);
                    AddBoxItem(recoveryItem);
                    itemList.Add(recoveryItem);
                }
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
            else if (boxItem.Item.Type == ItemType.Material_Usable && boxItem.Item.ConfigId == ItemHelper.SpecialId_Talent_Book)
            {
                ItemConfig config = ItemConfigCategory.Instance.Get(boxItem.Item.ConfigId);

                user.TalentExp.Data += quantity * config.UseParam;
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
                    Item item = ItemHelper.BuildItem((ItemType)pc.ItemTypeList[i], pc.ItemIdList[i], 1, (quantity * pc.ItemCountList[i]));
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
            else if (boxItem.Item.Type == ItemType.Pet)
            {
                //build pet
                //string str_json = JsonConvert.SerializeObject(e.Flairs);
                //Debug.Log("build pet flairs:" + str_json);
                List<Item> items = new List<Item>();

                Item item = PetConfigCategory.Instance.BuildPet(boxItem.Item.ConfigId);
                items.Add(item);

                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
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

                var item = PrefabHelper.Instance().CreateComBox(boxItem);
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
                ep = user.EquipPanelGoldenList[user.EquipGoldenIndex];
            }
            else if (Part >= 31 && Part <= 40)
            {
                ep = user.EquipPanelDarkGoldList[user.EquipDarkGoldIndex];
            }
            else if (Part >= 41 && Part <= 50)
            {
                ep = user.EquipPanelHundunList[user.EquipHundunIndex];
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
                    slot = DialogEquipGolden.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();
                }
                else if (Position >= 31 && Position <= 40)
                {
                    slot = DialogEquipDarkGold.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();
                }
                else if (Position >= 41 && Position <= 50)
                {
                    slot = DialogEquipHundun.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();
                }

                slot.UnEquip();

                AddBoxItem(ep[Position]);
            }

            //穿戴到格子上
            this.CreateEquipPanelItem(user.EquipPanelIndex, Position, equip);

            //记录
            ep[Position] = equip;

            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUseEquipEvent { });
        }

        private IDictionary<int, ExclusiveItem> GetExclusivePanel(ExclusiveItem exclusive)
        {
            User user = GameProcessor.Inst.User;

            return user.ExclusivePanelList[user.ExclusiveIndex];

            //if (exclusive.ExclusiveConfig.Cycle == 1)
            //{
            //    return user.ExclusivePanelList[user.ExclusiveIndex];
            //}
            //else if (exclusive.ExclusiveConfig.Cycle == 2)
            //{
            //    return user.ExclusivePanelList[user.ExclusiveIndex];

            //}
            //else if (exclusive.ExclusiveConfig.Cycle == 3)
            //{
            //    return user.ExclusivePanelList[user.ExclusiveIndex];
            //}

            //return null;
        }

        public void WearExclusive(BoxItem boxItem)
        {
            User user = GameProcessor.Inst.User;

            var exclusive = boxItem.Item as ExclusiveItem;

            IDictionary<int, ExclusiveItem> ep = GetExclusivePanel(exclusive);
            int Position = exclusive.Part;

            //从包袱移除
            UseBoxItem(boxItem, 1);

            //如果存在旧装备，增加到包裹
            if (ep.ContainsKey(Position))
            {
                //装备栏卸载
                SlotBox slot = ExclusiveDialog.ItemList.Where(s => s.Part == Position).FirstOrDefault();

                if (slot != null)
                {
                    slot.UnEquip();
                }

                AddBoxItem(ep[Position]);
            }

            //穿戴到格子上
            ExclusiveDialog.Wear(exclusive);
            //this.CreateEquipPanelItem(-1, Position, exclusive);

            ep[Position] = exclusive;

            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUseEquipEvent { });
        }

        public void WearShengxiao(BoxItem boxItem)
        {
            User user = GameProcessor.Inst.User;

            var exclusive = boxItem.Item as Shengxiao;

            IDictionary<int, Shengxiao> ep = user.ShengxiaoList;
            int Position = exclusive.ShengxiaoConfig.Part;

            //从包袱移除
            UseBoxItem(boxItem, 1);

            //如果存在旧装备，增加到包裹
            if (ep.ContainsKey(Position))
            {
                //装备栏卸载
                SlotBox slot = DialogShengxiaoPanel.ItemList.Where(s => s.Part == Position).FirstOrDefault();

                if (slot != null)
                {
                    slot.UnEquip();
                }

                AddBoxItem(ep[Position]);
            }

            //穿戴到格子上
            DialogShengxiaoPanel.Wear(exclusive);
            //this.CreateEquipPanelItem(-1, Position, exclusive);

            ep[Position] = exclusive;

            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUseEquipEvent { });
        }

        private void ClearEquipPanelItem(int position)
        {
            SlotBox slot = GetCurrentPanelEquipSolt(position);
            slot.UnEquip();
        }

        private SlotBox GetCurrentPanelEquipSolt(int position)
        {
            SlotBox slot = null;

            if (position <= 10)
            {
                int pi = GameProcessor.Inst.User.EquipPanelIndex;

                var EquipInfo = Equip_Plan_List[pi];
                slot = EquipInfo.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 11 && position <= 14)
            {
                slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 15 && position <= 20)
            {
                //slot = ExclusiveDialog.ItemList.Where(s => s.Part == position).First();
            }
            else if (position >= 21 && position <= 30)
            {
                slot = DialogEquipGolden.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 31 && position <= 40)
            {
                slot = DialogEquipDarkGold.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 41 && position <= 50)
            {
                slot = DialogEquipHundun.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
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
                slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).FirstOrDefault();
            }
            else if ((position >= 15 && position <= 20) || position > 1000)
            {
                //slot = ExclusiveDialog.ItemList.Where(s => s.Part == position).First();

                //if (slot == null)
                //{
                //    return; //没有切换到这个，就跳过
                //}
                return;
            }
            else if (position >= 21 && position <= 30)
            {
                slot = DialogEquipGolden.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 31 && position <= 40)
            {
                slot = DialogEquipDarkGold.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 41 && position <= 50)
            {
                slot = DialogEquipHundun.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }

            if (slot.GetEquip() != null) //防止叠加，无限刷道具
            {
                return;
            }

            //生成格子
            BoxItem boxItem = new BoxItem();
            boxItem.Item = equip;
            boxItem.MagicNubmer.Data = 1;
            boxItem.BoxId = -1;

            Com_Box comItem = PrefabHelper.Instance().CreateComBox(boxItem);
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
                slot = ExclusiveDialog.ItemList.Where(s => s.Part == position).First();
            }
            else if (position >= 21 && position <= 30)
            {
                slot = DialogEquipGolden.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 31 && position <= 40)
            {
                slot = DialogEquipDarkGold.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }
            else if (position >= 41 && position <= 50)
            {
                slot = DialogEquipHundun.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            }

            slot.UnEquip();

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
                user.EquipPanelGoldenList[user.EquipGoldenIndex].Remove(position);
            }
            else if (position >= 31 && position <= 40)
            {
                user.EquipPanelDarkGoldList[user.EquipDarkGoldIndex].Remove(position);
            }
            else if (position >= 41 && position <= 50)
            {
                user.EquipPanelHundunList[user.EquipHundunIndex].Remove(position);
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
            SlotBox slot = ExclusiveDialog.ItemList.Where(s => s.Part == position).First();
            if (slot != null)
            {
                slot.UnEquip();
            }

            //装备移动到包裹里面
            AddBoxItem(exclusive);

            var ep = GetExclusivePanel(exclusive);

            ep.Remove(position);

            //通知英雄更新属性
            user.EventCenter.Raise(new HeroUnUseEquipEvent() { });
        }

        private void RemoveShengxiao(BoxItem boxItem)
        {
            User user = GameProcessor.Inst.User;

            var exclusive = boxItem.Item as Shengxiao;
            int position = exclusive.ShengxiaoConfig.Part;

            //装备栏卸载
            SlotBox slot = DialogShengxiaoPanel.ItemList.Where(s => s.Part == position).First();
            if (slot != null)
            {
                slot.UnEquip();
            }

            //装备移动到包裹里面
            AddBoxItem(exclusive);

            var ep = user.ShengxiaoList;

            ep.Remove(position);

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
                    else if (newItem.Type == ItemType.Card || newItem.Type == ItemType.Fashion || newItem.Type == ItemType.Spirit || (newItem.Type == ItemType.Material && newItem.ConfigId == ItemHelper.SpecialId_Card_Stone))
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
            DialogEquipGolden.Show();
        }

        public void OnOpenEquipDarkGold()
        {
            DialogEquipDarkGold.Show();
        }

        public void OnOpenEquipHundun()
        {
            DialogEquipHundun.Show();
        }

        public void OnOpenTalent()
        {
            GameProcessor.Inst.EventCenter.Raise(new TalentShowEvent());
        }

        public void OnOpenRelic()
        {
            GameProcessor.Inst.EventCenter.Raise(new RelicShowEvent());
        }

        public void OnOpenShengxiao()
        {
            DialogShengxiaoPanel.gameObject.SetActive(true);
        }


        public void OnOpenPet()
        {
            GameProcessor.Inst.EventCenter.Raise(new PetShowEvent());
        }

        public void OpenFashion()
        {
            GameProcessor.Inst.EventCenter.Raise(new OpenFashionDialogEvent());
        }
    }
}