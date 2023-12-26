using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;
using Game.Data;
using SDD.Events;

namespace Game
{
    public class User
    {
        public long Essence { get; set; }
        public int LastCityId { get; set; }

        public long Power { get; set; }

        public long SecondExpTick { get; set; }

        public int ID { get; set; }

        public string Name { get; set; }

        public long DataDate { get; set; } = 0;
        public int DataProgeress { get; set; } = 0;
        public int SaveLimit { get; set; } = 0;
        public int LoadLimit { get; set; } = 0;

        public MagicData MagicLevel { get; } = new MagicData();

        public MagicData MagicGold { get; } = new MagicData();

        public MagicData MagicExp { get; } = new MagicData();

        public MagicData MagicUpExp { get; } = new MagicData();
        public MagicData MagicTowerFloor { get; } = new MagicData();

        public IDictionary<int, Equip> EquipPanel { get; set; } = new Dictionary<int, Equip>();

        public IDictionary<int, IDictionary<int, Equip>> EquipPanelList { get; set; } = new Dictionary<int, IDictionary<int, Equip>>();

        public IDictionary<int, Equip> EquipPanelSpecial { get; set; } = new Dictionary<int, Equip>();

        public IDictionary<int, IDictionary<int, ExclusiveItem>> ExclusivePanelList { get; set; } = new Dictionary<int, IDictionary<int, ExclusiveItem>>();

        public IDictionary<int, ExclusiveItem> ExclusiveList { get; set; } = new Dictionary<int, ExclusiveItem>();

        public int EquipPanelIndex { get; set; } = 0;

        public bool ExclusiveSetting { get; set; } = false;
        public int ExclusiveIndex { get; set; } = 0;

        public int SkillPanelIndex { get; set; } = 0;

        public IDictionary<int, MagicData> MagicEquipStrength { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, int> EquipRefine { get; set; } = new Dictionary<int, int>();
        public IDictionary<int, MagicData> MagicEquipRefine { get; set; } = new Dictionary<int, MagicData>();

        public RecoverySetting RecoverySetting { get; set; } = new RecoverySetting();

        public bool ShowMonsterSkill { get; set; } = true;

        public List<SkillData> SkillList { get; set; } = new List<SkillData>();

        public IDictionary<int, List<int>> SkillPanelList { get; set; } = new Dictionary<int, List<int>>();

        public IDictionary<AchievementSourceType, MagicData> MagicRecord { get; set; } = new Dictionary<AchievementSourceType, MagicData>();

        public DefendData DefendData { get; set; }

        public HeroPhatomData HeroPhatomData { get; set; }

        public bool isError = false;

        /// <summary>
        /// 包裹
        /// </summary>
        public List<BoxItem> Bags { get; set; } = new List<BoxItem>();

        public IDictionary<string, bool> GiftList { get; set; } = new Dictionary<string, bool>();

        public Dictionary<int, long> VersionLog { get; } = new Dictionary<int, long>();

        public long LastOut { get; set; }

        private bool isInLevelUp;

        public int MapId { get; set; } = 1000;

        public int TaskId { get; set; } = 1;
        public Dictionary<int, bool> TaskLog = new Dictionary<int, bool>();

        //副本次数记录
        public long CopyTicketTime { get; set; } = 0;

        public MagicData MagicCopyTikerCount { get; } = new MagicData();

        public Dictionary<int, long> MapBossTime { get; } = new Dictionary<int, long>();

        //幻神记录
        public Dictionary<int, int> PhantomRecord { get; } = new Dictionary<int, int>();

        public ADShowData ADShowData { get; set; } = new ADShowData();

        public RecordData Record { get; set; } = new RecordData();

        public long AdLastTime { get; set; } = 0;

        public Dictionary<int, MagicData> SoulRingData { get; } = new Dictionary<int, MagicData>();

        public MagicData WingData { get; set; } = new MagicData();

        public Dictionary<int, int> AchievementData { get; } = new Dictionary<int, int>();

        public Dictionary<int, MagicData> CardData { get; } = new Dictionary<int, MagicData>();

        public bool GameCheat { get; set; } = false;

        [JsonIgnore]
        public IDictionary<int, int> EquipRecord { get; set; } = new Dictionary<int, int>();

        [JsonIgnore]
        public EventManager EventCenter { get; private set; }

        [JsonIgnore]
        public AttributeBonus AttributeBonus { get; set; }

        [JsonIgnore]
        public int SuitMax = 0;
        [JsonIgnore]
        public int StoneNumber = 0;
        [JsonIgnore]
        public int SoulRingNumber = 0;
        [JsonIgnore]
        public int TowerNumber = 0;
        [JsonIgnore]
        public int SkillNumber = 0;

        public User()
        {
            this.EventCenter = new EventManager();

            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);
            this.EventCenter.AddListener<HeroUseEquipEvent>(HeroUseEquip);
            this.EventCenter.AddListener<HeroUnUseEquipEvent>(HeroUnUseEquip);
            this.EventCenter.AddListener<HeroUseSkillBookEvent>(HeroUseSkillBook);
            this.EventCenter.AddListener<UserAttrChangeEvent>(UserAttrChange);
        }

        public void Init()
        {
            //设置各种属性值
            SetAttr();

            //设置Boss刷新时间
            //Dictionary<int, MapConfig> mapList = MapConfigCategory.Instance.GetAll();
            //foreach (MapConfig mapConfig in mapList.Values)
            //{
            //    if (!MapBossTime.ContainsKey(mapConfig.Id))
            //    {
            //        MapBossTime[mapConfig.Id] = 0;
            //    }
            //}
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            long Level = MagicLevel.Data;
            long levelAttr = LevelConfigCategory.GetLevelAttr(Level);

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, levelAttr * 10 + 40);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, levelAttr + 10);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, levelAttr + 10);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, levelAttr + 10);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, levelAttr / 5 + 1);

            //设置升级属性
            SetUpExp();

            //装备属性
            foreach (var kvp in EquipPanelList[EquipPanelIndex])
            {
                EquipRefineConfig refineConfig = null;
                if (MagicEquipRefine.TryGetValue(kvp.Key, out MagicData refineData))
                {
                    refineConfig = EquipRefineConfigCategory.Instance.GetByLevel(Math.Min(refineData.Data, ConfigHelper.Max_Level_Refine));
                }

                foreach (var a in kvp.Value.GetTotalAttrList(refineConfig))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }

            foreach (var kvp in EquipPanelSpecial)
            {
                foreach (var a in kvp.Value.GetTotalAttrList(null))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }

            //套装属性
            List<EquipGroupConfig> suitList = GetEquipGroups();
            foreach (EquipGroupConfig item in suitList)
            {
                for (int i = 0; i < item.AttrIdList.Length; i++)
                {
                    AttributeBonus.SetAttr((AttributeEnum)item.AttrIdList[i], AttributeFrom.EquipSuit, item.Position, item.AttrValueList[i]);
                }
            }

            //强化属性
            foreach (var sp in this.MagicEquipStrength)
            {
                EquipStrengthConfig strengthConfig = EquipStrengthConfigCategory.Instance.GetByPositioin(sp.Key);
                for (int i = 0; i < strengthConfig.AttrList.Length; i++)
                {
                    long strenthAttr = LevelConfigCategory.GetLevelAttr(sp.Value.Data);
                    AttributeBonus.SetAttr((AttributeEnum)strengthConfig.AttrList[i], AttributeFrom.EquiStrong, sp.Key, strenthAttr * strengthConfig.AttrValueList[i]);
                }
            }

            //专属属性
            foreach (var sp in this.ExclusivePanelList[ExclusiveIndex])
            {
                foreach (var a in sp.Value.GetTotalAttrList())
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.Exclusive, sp.Key, a.Value);
                }
            }

            //图鉴属性
            foreach (var sp in this.CardData)
            {
                if (sp.Value.Data > 0)
                {
                    CardConfig cardConfig = CardConfigCategory.Instance.Get(sp.Key);
                    long cardAttr = cardConfig.AttrValue + (sp.Value.Data - 1) * cardConfig.LevelIncrea;
                    AttributeBonus.SetAttr((AttributeEnum)cardConfig.AttrId, AttributeFrom.Card, sp.Key, cardAttr);
                }
            }

            //无尽塔属性
            if (this.MagicTowerFloor.Data > 1)
            {
                long secondExp = 0;
                long secondGold = 0;
                MonsterTowerHelper.GetTowerSecond(this.MagicTowerFloor.Data - 1, out secondExp, out secondGold);

                AttributeBonus.SetAttr(AttributeEnum.SecondExp, AttributeFrom.Tower, secondExp);
                AttributeBonus.SetAttr(AttributeEnum.SecondGold, AttributeFrom.Tower, secondGold);
            }

            //幻神属性
            foreach (var sp in PhantomRecord)
            {
                PhantomAttrConfig phantomAttrConfig = PhantomConfigCategory.Instance.GetAttrConfig(sp.Key, sp.Value - 1);
                if (phantomAttrConfig != null)
                {
                    AttributeBonus.SetAttr((AttributeEnum)phantomAttrConfig.RewardId, AttributeFrom.Phantom, phantomAttrConfig.RewardBase);
                }
            }

            //魂环
            foreach (var sl in SoulRingData)
            {
                if (sl.Value.Data > 0)
                {
                    SoulRingAttrConfig ringConfig = SoulRingConfigCategory.Instance.GetAttrConfig(sl.Key, sl.Value.Data);
                    for (int i = 0; i < ringConfig.AttrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)ringConfig.AttrIdList[i], AttributeFrom.SoulRing, sl.Key, ringConfig.AttrValueList[i]);
                    }
                }
            }

            //翅膀
            if (WingData.Data > 0)
            {
                WingConfig wingConfig = WingConfigCategory.Instance.GetByLevel(WingData.Data);
                for (int i = 0; i < wingConfig.AttrIdList.Length; i++)
                {
                    AttributeBonus.SetAttr((AttributeEnum)wingConfig.AttrIdList[i], AttributeFrom.Wing, wingConfig.AttrValueList[i]);
                }
            }

            //光环
            foreach (var ar in GetAurasList())
            {
                AurasAttrConfig config = AurasAttrConfigCategory.Instance.Get(ar.Key);
                int arValue = config.AttrValue + (ar.Value - 1) * config.Rise;
                Debug.Log("auras value :" + arValue);
                AttributeBonus.SetAttr((AttributeEnum)config.AttrId, AttributeFrom.Auras, arValue);
            }

            this.SuitMax = ConfigHelper.SkillSuitMax;
            this.StoneNumber = 0;
            this.SoulRingNumber = 0;
            this.TowerNumber = 0;
            this.SkillNumber = ConfigHelper.SkillNumber;

            //专属
            if (this.ExclusivePanelList[ExclusiveIndex].Count >= 6)
            {
                this.SkillNumber += 1;
            }

            //成就
            foreach (int aid in AchievementData.Keys)
            {
                AchievementConfig achievementConfig = AchievementConfigCategory.Instance.Get(aid);
                if (achievementConfig.RewardType == (int)AchievementRewardType.Attr)
                {
                    AttributeBonus.SetAttr((AttributeEnum)achievementConfig.AttrId, AttributeFrom.Achivement, achievementConfig.Id, achievementConfig.AttrValue);
                }
                else if (achievementConfig.RewardType == (int)AchievementRewardType.Suit)
                {
                    this.SuitMax--;
                }
                else if (achievementConfig.RewardType == (int)AchievementRewardType.Stone)
                {
                    this.StoneNumber += achievementConfig.AttrValue;
                }
                else if (achievementConfig.RewardType == (int)AchievementRewardType.SoulRing)
                {
                    this.SoulRingNumber += achievementConfig.AttrValue;
                }
                else if (achievementConfig.RewardType == (int)AchievementRewardType.Tower)
                {
                    this.TowerNumber += achievementConfig.AttrValue;
                }
            }

            this.SuitMax = Math.Max(this.SuitMax, ConfigHelper.SkillSuitMin);

            //更新面板
            if (GameProcessor.Inst.PlayerInfo != null)
            {
                GameProcessor.Inst.PlayerInfo.UpdateAttrInfo(this);
            }
        }

        public int CalStone(Equip equip)
        {
            int count = (equip.Level * 3 / 20 + this.StoneNumber) * equip.GetQuality();

            return count;
        }

        public int CalSpecailStone(Equip equip)
        {
            int count = 1;
            for (int i = 0; i < equip.Level; i++)
            {
                count *= 2;
            }
            return count;
        }

        private void HeroChange(HeroChangeEvent e)
        {
            switch (e.Type)
            {
                case UserChangeType.LevelUp:
                    if (!this.isInLevelUp)
                    {
                        this.isInLevelUp = true;
                        GameProcessor.Inst.StartCoroutine(LevelUp());
                    }
                    break;
            }
        }

        private void HeroUseEquip(HeroUseEquipEvent e)
        {
            //更新属性面板
            GameProcessor.Inst.UpdateInfo();

            //更新技能描述
            this.EventCenter.Raise(new SkillShowEvent());
        }

        private void HeroUnUseEquip(HeroUnUseEquipEvent e)
        {
            //更新属性面板
            GameProcessor.Inst.UpdateInfo();

            //更新技能描述
            this.EventCenter.Raise(new SkillShowEvent());
        }

        private void HeroUseSkillBook(HeroUseSkillBookEvent e)
        {
            SkillBook Book = e.BoxItem.Item as SkillBook;

            SkillData skillData;

            TaskHelper.CheckTask(TaskType.SkillBook, 1);

            if (e.IsLearn)
            {
                //第一次学习，创建技能数据
                skillData = new SkillData(Book.ConfigId, 0);
                skillData.Status = SkillStatus.Learn;
                skillData.MagicLevel.Data = 1;
                skillData.MagicExp.Data = 0;

                this.SkillList.Add(skillData);
            }
            else
            {
                skillData = this.SkillList.Find(b => b.SkillId == Book.ConfigId);
                skillData.AddExp(Book.ItemConfig.UseParam * e.Quantity);
            }

            this.EventCenter.Raise(new SkillShowEvent());
        }

        private void UserAttrChange(UserAttrChangeEvent e)
        {
            this.SetAttr();
        }

        public List<SkillRune> GetRuneList(int skillId)
        {
            List<SkillRune> list = new List<SkillRune>();

            //计算装备的词条加成
            List<SkillRuneConfig> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value.SkillRuneConfig).ToList();

            skillList.AddRange(this.ExclusivePanelList[ExclusiveIndex].Select(m => m.Value).Where(m => m.SkillRuneConfig != null && m.SkillRuneConfig.SkillId == skillId).Select(m => m.SkillRuneConfig).ToList());

            //按单件分组,词条有堆叠上限
            var runeGroup = skillList.GroupBy(m => m.Id);

            foreach (IGrouping<int, SkillRuneConfig> runeItem in runeGroup)
            {
                SkillRune skillRune = new SkillRune(runeItem.Key, runeItem.Count());
                list.Add(skillRune);
            }

            return list;
        }

        public List<SkillSuit> GetSuitList(int skillId)
        {
            List<SkillSuit> list = new List<SkillSuit>();

            //计算装备的套装加成
            List<SkillSuitConfig> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value.SkillSuitConfig).ToList();
            var el = this.ExclusivePanelList[ExclusiveIndex].Select(m => m.Value).Where(m => m.SkillSuitConfig != null && m.SkillSuitConfig.SkillId == skillId).Select(m => m.SkillSuitConfig).ToList();
            if (el.Count > 0)
            {
                skillList.AddRange(el);
            }

            var suitGroup = skillList.GroupBy(m => m.Id);

            foreach (var suitItem in suitGroup)
            {
                if (suitItem.Count() >= this.SuitMax)
                {  //SkillSuitHelper.SuitMax 件才成套,并且只能有一套能生效
                    SkillSuit suit = new SkillSuit(suitItem.Key);
                    list.Add(suit);
                }
            }

            return list;
        }

        public int GetSuitCount(int suitId)
        {
            int count = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SuitConfigId == suitId).Count();
            count += this.ExclusivePanelList[ExclusiveIndex].Where(m => m.Value.SuitConfigId == suitId).Count();
            return Math.Min(count, this.SuitMax);
        }

        public List<EquipGroupConfig> GetEquipGroups()
        {
            var currentPanel = this.EquipPanelList[EquipPanelIndex];

            List<EquipGroupConfig> list = new List<EquipGroupConfig>();

            for (int i = 1; i < 10; i = i + 2)
            {  //1,3,5,7,9
                if (currentPanel.TryGetValue(i, out Equip equip))
                {
                    EquipSuit es = GetEquipSuit(equip.EquipConfig);
                    if (es.Active)
                    {
                        list.Add(es.Config);
                    }
                }
            }

            return list;
        }

        public EquipSuit GetEquipSuit(EquipConfig config)
        {
            EquipSuit suit = new EquipSuit();

            suit.Self = new EquipSuitItem(config.Id, config.Name, true);

            int gid = 0; //关联套装Id
            if (config.Part == 5 || config.Part == 7)
            {
                gid = config.Id;
            }
            else
            {
                gid = config.Part % 2 == 1 ? config.Id + 1 : config.Id - 1;
            }

            EquipConfig gc = EquipConfigCategory.Instance.Get(gid);
            EquipSuitItem target = new EquipSuitItem(gc.Id, gc.Name, false);

            int count = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.EquipConfig.Id == gid).Count();
            if ((gid != config.Id && count >= 1) || count >= 2) //非手镯戒指只要一个，手镯戒指要2个
            {
                target.Active = true;
                suit.Active = true;
            }

            suit.ItemList.Add(suit.Self);
            suit.ItemList.Add(target);

            EquipGroupConfig groupConfig = EquipGroupConfigCategory.Instance.GetByLevelAndPart(config.LevelRequired, Math.Min(config.Part, gc.Part));

            suit.Config = groupConfig;

            return suit;
        }


        public ExclusiveSuit GetExclusiveSuit(ExclusiveConfig config)
        {
            ExclusiveSuit suit = new ExclusiveSuit();
            suit.ActiveCount = 0;
            suit.Active = true;

            //suit.Self = new ExclusiveSuitItem(config.Id, config.Name, true);

            List<ExclusiveConfig> configs = ExclusiveConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Type == config.Type && m.Quality < 0).ToList();

            foreach (ExclusiveConfig item in configs)
            {
                ExclusiveSuitItem target = new ExclusiveSuitItem(item.Id, item.Name, false);

                if (this.ExclusivePanelList[ExclusiveIndex].ContainsKey(item.Part))
                {
                    target.Active = true;
                    suit.ActiveCount++;
                }
                else
                {
                    suit.Active = false;
                }

                suit.ItemList.Add(target);
            }

            return suit;
        }

        public Dictionary<int, int> GetAurasList()
        {
            Dictionary<int, int> list = new Dictionary<int, int>();

            foreach (var sl in SoulRingData)
            {
                if (sl.Value.Data > 0)
                {
                    SoulRingAttrConfig ringConfig = SoulRingConfigCategory.Instance.GetAttrConfig(sl.Key, sl.Value.Data);

                    if (ringConfig.AurasId > 0)
                    {
                        list.Add(ringConfig.AurasId, ringConfig.AurasLevel);
                    }
                }
            }
            return list;
        }

        public long GetAchievementProgeress(AchievementSourceType type)
        {
            long progress = 0;

            switch (type)
            {
                case AchievementSourceType.Advert:
                    if (!this.MagicRecord.ContainsKey(AchievementSourceType.Advert))
                    {
                        this.MagicRecord[AchievementSourceType.Advert] = new MagicData();
                    }
                    progress = this.Record.GetRecord((int)RecordType.AdVirtual) + this.Record.GetRecord((int)RecordType.AdReal) * 2;
                    break;
                case AchievementSourceType.RealAdvert:
                    if (!this.MagicRecord.ContainsKey(AchievementSourceType.RealAdvert))
                    {
                        this.MagicRecord[AchievementSourceType.RealAdvert] = new MagicData();
                    }
                    progress = this.Record.GetRecord((int)RecordType.AdReal);
                    break;
                case AchievementSourceType.Strong:
                    progress = this.MagicEquipStrength.Select(m => m.Value.Data).Sum();
                    break;
                case AchievementSourceType.Refine:
                    progress = this.MagicEquipRefine.Select(m => m.Value.Data).Sum();
                    break;
                case AchievementSourceType.Level:
                    progress = this.MagicLevel.Data;
                    break;
                case AchievementSourceType.BossFamily:
                    if (!this.MagicRecord.ContainsKey(AchievementSourceType.BossFamily))
                    {
                        this.MagicRecord[AchievementSourceType.BossFamily] = new MagicData();
                    }

                    progress = this.MagicRecord[AchievementSourceType.BossFamily].Data;
                    break;
                case AchievementSourceType.Defend:
                    {
                        if (!this.MagicRecord.ContainsKey(AchievementSourceType.Defend))
                        {
                            this.MagicRecord[AchievementSourceType.Defend] = new MagicData();
                        }
                        progress = this.MagicRecord[AchievementSourceType.Defend].Data;
                    }
                    break;
            }

            return progress;
        }
        public void AddExpAndGold(long exp, long gold)
        {
            if (this.MagicGold.Data < 0)
            {
                return;
            }

            if (this.MagicLevel.Data < ConfigHelper.Max_Level)
            {
                this.MagicExp.Data += exp;
            }
            else
            {
                this.MagicExp.Data = 0;
            }

            this.MagicGold.Data += gold;

            EventCenter.Raise(new UserInfoUpdateEvent()); //更新UI

            if (MagicExp.Data >= MagicUpExp.Data)
            {
                GameProcessor.Inst.StartCoroutine(LevelUp()); //升级
            }
        }

        IEnumerator LevelUp()
        {

            while (this.MagicExp.Data >= this.MagicUpExp.Data && this.MagicLevel.Data < ConfigHelper.Max_Level)
            {
                MagicExp.Data -= MagicUpExp.Data;
                this.MagicLevel.Data++;

                SetUpExp();

                EventCenter.Raise(new UserInfoUpdateEvent());
                EventCenter.Raise(new SetPlayerLevelEvent { Level = this.MagicLevel.Data });
                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;

            EventCenter.Raise(new UserAttrChangeEvent());
        }

        private void SetUpExp()
        {
            long levelAttr = LevelConfigCategory.GetLevelAttr(MagicLevel.Data);
            LevelConfig config = LevelConfigCategory.Instance.GetAll().Where(m => m.Value.StartLevel <= MagicLevel.Data && m.Value.EndLevel >= MagicLevel.Data).First().Value;
            MagicUpExp.Data = levelAttr * config.Exp;
        }

        public long GetMaterialCount(int id)
        {
            long count = this.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == id).Select(m => m.MagicNubmer.Data).Sum();
            return count;
        }

        public List<int> GetCurrentSkillList()
        {
            if (!SkillPanelList.ContainsKey(SkillPanelIndex))
            {
                SkillPanelList[SkillPanelIndex] = new List<int>();
            }
            return SkillPanelList[SkillPanelIndex];
        }

        public List<SkillData> GetCurrentSkill()
        {
            List<int> ids = GetCurrentSkillList();

            //Debug.Log(JsonConvert.SerializeObject(ids));

            List<SkillData> list = new List<SkillData>();

            for (int i = 0; i < ids.Count; i++)
            {
                SkillData skill = SkillList.Where(m => m.SkillId == ids[i]).FirstOrDefault();
                list.Add(skill);
            }

            return list;
        }
    }

    public enum UserChangeType
    {
        LevelUp = 0,
        AttrChange = 1
    }
}
