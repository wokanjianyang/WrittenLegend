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

        public string DeviceId { get; set; } = "";

        public string Account { get; set; } = "";
        public string Name { get; set; }

        public long DataDate { get; set; } = 0;
        public int DataProgeress { get; set; } = 0;

        public int OffLineMapId { get; set; }

        public RandomRecord RandomRecord { get; set; } = new RandomRecord();

        public MagicData Cycle { get; set; } = new MagicData();

        public MagicData MagicLevel { get; } = new MagicData();

        public MagicDouble MagicGold { get; } = new MagicDouble();

        public MagicDouble MagicExp { get; } = new MagicDouble();

        public MagicData MagicUpExp { get; } = new MagicData();

        public MagicData MagicTowerFloor { get; } = new MagicData();

        public MagicData BabelData { get; } = new MagicData();
        public MagicData BabelCount { get; } = new MagicData();

        public MagicData RedRefreshCount { get; } = new MagicData();

        public IDictionary<int, double> KillRecord { get; } = new Dictionary<int, double>();

        public Dictionary<int, MagicData> RingData { get; } = new Dictionary<int, MagicData>();
        public Dictionary<int, int> RingSelect { get; set; } = new Dictionary<int, int>();

        public IDictionary<int, IDictionary<int, Equip>> EquipPanelList { get; set; } = new Dictionary<int, IDictionary<int, Equip>>();

        public IDictionary<int, Equip> EquipPanelSpecial { get; set; } = new Dictionary<int, Equip>();

        public IDictionary<int, Equip> EquipPanelGolden { get; set; } = new Dictionary<int, Equip>();

        public IDictionary<int, IDictionary<int, ExclusiveItem>> ExclusivePanelList { get; set; } = new Dictionary<int, IDictionary<int, ExclusiveItem>>();

        public IDictionary<int, ExclusiveItem> ExclusiveList { get; set; } = new Dictionary<int, ExclusiveItem>();

        public int EquipPanelIndex { get; set; } = 0;
        public IDictionary<int, string> PlanNameList { get; set; } = new Dictionary<int, string>();

        public bool ExclusiveSetting { get; set; } = false;
        public int ExclusiveIndex { get; set; } = 0;

        public int SkillPanelIndex { get; set; } = 0;

        public IDictionary<int, MagicData> MagicEquipStrength { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> MagicEquipRefine { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> LegacyLevel { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> LegacyLayer { get; set; } = new Dictionary<int, MagicData>();

        public MagicData LegacyPoint { get; } = new MagicData();

        public RecoverySetting RecoverySetting { get; set; } = new RecoverySetting();

        public bool ShowMonsterSkill { get; set; } = true;

        public bool ShowMonsterDamage { get; set; } = true;

        public bool ShowPlayerEffect { get; set; } = true;

        public List<SkillData> SkillList { get; set; } = new List<SkillData>();

        public IDictionary<int, List<int>> SkillPanelList { get; set; } = new Dictionary<int, List<int>>();

        public IDictionary<AchievementSourceType, MagicData> MagicRecord { get; set; } = new Dictionary<AchievementSourceType, MagicData>();

        public DefendData DefendData { get; set; }

        public InfiniteData InfiniteData { get; set; }

        public LegacyData LegacyData { get; set; }

        public HeroPhatomData HeroPhatomData { get; set; }

        /// <summary>
        /// 包裹
        /// </summary>
        public List<BoxItem> Bags { get; set; } = new List<BoxItem>();

        public IDictionary<string, bool> GiftList { get; set; } = new Dictionary<string, bool>();

        public Dictionary<int, long> VersionLog { get; } = new Dictionary<int, long>();

        public bool OldCJCheck = false;
        public bool OldRingCheck = false;

        public int GetArtifactValue(ArtifactType type)
        {
            List<ArtifactConfig> list = ArtifactConfigCategory.Instance.GetListByType(type);

            int total = 0;
            foreach (ArtifactConfig config in list)
            {
                int artifactLevel = Math.Min(config.MaxCount, this.GetArtifactLevel(config.Id));
                total += artifactLevel * config.AttrValue;
            }

            return total;
        }

        public long GetLimitLevel()
        {
            long level = this.MagicLevel.Data;

            if (this.Cycle.Data > 0)
            {
                level = Math.Max(level, ConfigHelper.Max_Level + (this.Cycle.Data - 1) * ConfigHelper.Cycle_Level);
            }

            return (level) / 5000 + 1;
        }

        public int GetSkillLimit(SkillConfig skillConfig)
        {
            long limit = skillConfig.MaxLevel + skillConfig.RiseMaxLevel * GetLimitLevel();
            limit = limit * (100 + GetArtifactValue(ArtifactType.SkillLimit)) / 100;
            return (int)limit;
        }

        public int GetSoulRingLimit()
        {
            long limit = GetLimitLevel() * 2 + 25;
            limit = limit + GetArtifactValue(ArtifactType.SoulRingLimit);
            return (int)limit;
        }

        public int GetWingLimit()
        {
            long limit = GetLimitLevel() * 2 + 30;
            limit = limit + GetArtifactValue(ArtifactType.WingLimit);
            return (int)limit;
        }

        public int GetStrengthLimit()
        {
            long limit = GetLimitLevel() * 5000 + 10000;
            limit = limit + GetArtifactValue(ArtifactType.StrengthLimit);
            return (int)limit;
        }

        public int GetRefineLimit()
        {
            long limit = GetLimitLevel() * 25 + 50;
            limit = limit + GetArtifactValue(ArtifactType.RefintLimit);
            return (int)limit;
        }

        public int GetExclusiveLimit()
        {
            long limit = 2 + GetArtifactValue(ArtifactType.ExclusiveLimit);
            return (int)limit;
        }

        public int GetHolidomLimit()
        {
            long limit = 4 + GetArtifactValue(ArtifactType.HolidomLimit);
            return (int)limit;
        }

        public int GetCardLimit(CardConfig cardConfig)
        {
            long limit = cardConfig.RiseLevel * GetLimitLevel();
            limit = limit * (100 + GetArtifactValue(ArtifactType.CardLimit)) / 100;
            return (int)limit;
        }

        public int GetLimitMineCount()
        {
            int limit = GetArtifactValue(ArtifactType.MineCount);
            return (int)(GetLimitLevel() - 4 + limit);
        }

        public int GetLimitMineCount2()
        {
            int limit = GetArtifactValue(ArtifactType.MineCount2);
            return (int)limit;
        }

        public long LastUploadTime { get; set; }

        public long LastSaveTime { get; set; }

        private bool isInLevelUp;

        public int MapId { get; set; } = 1000;

        public int TaskId { get; set; } = 1;
        public Dictionary<int, bool> TaskLog = new Dictionary<int, bool>();

        //副本次数记录
        public long CopyTicketTime { get; set; } = 0;

        public long LegacyTicketTime { get; set; } = 0;

        public MagicData LegacyTikerCount { get; } = new MagicData();

        public long SaveTicketTime { get; set; } = 0;

        public long SaveTickeTimeHand { get; set; } = 0;

        public long LoadTicketTime { get; set; } = 0;

        public long First_Create_Time { get; set; } = 0;

        public MagicData MagicCopyTikerCount { get; } = new MagicData();

        public Dictionary<int, long> MapBossTime { get; } = new Dictionary<int, long>();

        //幻神记录
        public Dictionary<int, int> PhantomRecord { get; } = new Dictionary<int, int>();

        public ADShowData ADShowData { get; set; } = new ADShowData();

        public RecordData Record { get; set; } = new RecordData();

        public AdData AdData { get; } = new AdData();

        public long AdLastTime { get; set; } = 0;

        public Dictionary<int, MagicData> SoulRingData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> SoulBoneData { get; } = new Dictionary<int, MagicData>();

        public MagicData WingData { get; set; } = new MagicData();

        public MagicData PillData { get; set; } = new MagicData();

        public PillTime PillTime { get; set; } = new PillTime();

        public Dictionary<int, Dictionary<int, MagicData>> FashionData { get; set; } = new Dictionary<int, Dictionary<int, MagicData>>();
        public Dictionary<int, MagicData> ItemMeterialData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, int> AchievementData { get; } = new Dictionary<int, int>();

        public Dictionary<int, MagicData> CardData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> HalidomData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> ArtifactData { get; } = new Dictionary<int, MagicData>();

        public List<DropData> DropDataList { get; } = new List<DropData>();

        public IDictionary<int, int> FestiveData_101 { get; set; } = new Dictionary<int, int>();

        public int MinerSeed = 1;
        public long MinerTime { get; set; } = 0;

        public Dictionary<int, MagicData> MetalData { get; } = new Dictionary<int, MagicData>();

        public bool GameDoCheat211 { get; set; } = false;

        public bool isClear { get; set; } = false;


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

        private bool isDingzhi = false;

        //private string[] DingzhiUserId = new string[] { "7B97AC4A45", "0AF588B5A9", "A99597B885", "495FD8195B" }; //
        //private string[] DingzhiAccount = new string[] { "lucky1500", "154940963" };

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
        }

        //public bool IsDz()
        //{
        //    return false;
        //}
        public int GetDzRate()
        {
            return 1;  //isDingzhi ? 2 : 1;
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

            if (isDingzhi)
            {
                AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.Dingzhi, 100);
                AttributeBonus.SetAttr(AttributeEnum.MoveSpeed, AttributeFrom.Dingzhi, 100);
            }

            //AttributeBonus.SetAttr(AttributeEnum.QualityIncrea, AttributeFrom.Test + 1, 1000000000);
            //AttributeBonus.SetAttr(AttributeEnum.MulAttr, AttributeFrom.Test + 1, 100000);

            //设置升级属性
            SetUpExp();

            //转生属性
            if (Cycle.Data > 0)
            {
                CycleConfig cycleConfig = CycleConfigCategory.Instance.GetByCycle(Cycle.Data);
                for (int i = 0; i < cycleConfig.AttrIdList.Length; i++)
                {
                    AttributeBonus.SetAttr((AttributeEnum)cycleConfig.AttrIdList[i], AttributeFrom.Cycle, i, cycleConfig.AttrValueList[i]);
                }
            }

            //装备属性-普通装备
            foreach (KeyValuePair<int, Equip> kvp in EquipPanelList[EquipPanelIndex])
            {
                long refineLevel = GetRefineLevel(kvp.Key);

                foreach (KeyValuePair<int, long> a in kvp.Value.GetTotalAttrList(refineLevel))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }
            //装备属性-四格装备
            foreach (KeyValuePair<int, Equip> kvp in EquipPanelSpecial)
            {
                foreach (KeyValuePair<int, long> a in kvp.Value.GetTotalAttrList(0))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }
            //装备属性-金色装备
            foreach (KeyValuePair<int, Equip> kvp in EquipPanelGolden)
            {
                foreach (KeyValuePair<int, long> a in kvp.Value.GetTotalAttrList(0))
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

            //装备红色属性
            for (int role = 1; role <= 3; role++)
            {
                EquipRedSuit red6 = GetEquipRedConfig(role, 6);
                foreach (EquipRedItem redItem in red6.List)
                {
                    if (redItem.Level > 0)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(redItem.Config.AttrId), AttributeFrom.EquipRed, 60 + role, redItem.Config.AttrValue + redItem.Config.AttrRise * (redItem.Level - 1));
                    }
                }

                EquipRedSuit red7 = GetEquipRedConfig(role, 7);
                foreach (EquipRedItem redItem in red7.List)
                {
                    if (redItem.Level > 0)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(redItem.Config.AttrId), AttributeFrom.EquipRed, 70 + role, redItem.Config.AttrValue + redItem.Config.AttrRise * (redItem.Level - 1));
                    }
                }
            }

            //强化属性
            foreach (var sp in this.MagicEquipStrength)
            {
                int position = sp.Key;
                EquipStrengthConfig strengthConfig = EquipStrengthConfigCategory.Instance.GetByPositioin(position);
                for (int i = 0; i < strengthConfig.AttrList.Length; i++)
                {
                    long strenthAttr = LevelConfigCategory.GetLevelAttr(sp.Value.Data);
                    double strenthPercetn = this.GetRefineStrenthPercetn(position) / 100.0 + 1;
                    strenthAttr = (long)(strenthAttr * strenthPercetn);
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
                int phLevel = sp.Value - 1;
                if (phLevel > 0)
                {
                    PhantomAttrConfig phantomAttrConfig = PhantomConfigCategory.Instance.GetAttrConfig(sp.Key, phLevel);
                    int phAttr = phantomAttrConfig.GetRewardAttr(phLevel);
                    AttributeBonus.SetAttr((AttributeEnum)phantomAttrConfig.RewardId, AttributeFrom.Phantom, phAttr);
                }
            }

            //魂环
            foreach (var sl in SoulRingData)
            {
                if (sl.Value.Data > 0)
                {
                    int sid = sl.Key;
                    long srLevel = sl.Value.Data;

                    SoulRingAttrConfig ringConfig = SoulRingConfigCategory.Instance.GetAttrConfig(sid, srLevel);
                    for (int i = 0; i < ringConfig.AttrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)ringConfig.AttrIdList[i], AttributeFrom.SoulRing, sid, ringConfig.GetAttr(i, srLevel));
                    }

                    long sbLevel = GetSoulBoneLevel(sid);
                    if (sbLevel > 0)
                    {
                        SoulBoneConfig boneConfig = SoulBoneConfigCategory.Instance.Get(sid);
                        for (int i = 0; i < boneConfig.AttrIdList.Length; i++)
                        {
                            AttributeBonus.SetAttr((AttributeEnum)boneConfig.AttrIdList[i], AttributeFrom.SoulBone, sid, boneConfig.AttrValueList[i] * sbLevel * srLevel);
                        }
                    }
                }
            }

            //翅膀
            long wingLevel = WingData.Data;
            if (wingLevel > 0)
            {
                WingConfig wingConfig = WingConfigCategory.Instance.GetByLevel(wingLevel);
                for (int i = 0; i < wingConfig.AttrIdList.Length; i++)
                {
                    long wingValue = wingConfig.GetAttr(i, wingLevel);
                    AttributeBonus.SetAttr((AttributeEnum)wingConfig.AttrIdList[i], AttributeFrom.Wing, wingValue);
                }
            }

            //矿石
            foreach (var kv in MetalData)
            {
                long level = kv.Value.Data;

                if (level > 0 && kv.Key > 0)
                {
                    MetalConfig metalConfig = MetalConfigCategory.Instance.Get(kv.Key);
                    long percent = GetMetalQualityLevel(metalConfig.Quality);
                    long riseLevel = level * percent / 100;
                    riseLevel = Math.Max(riseLevel, percent);

                    AttributeBonus.SetAttr((AttributeEnum)metalConfig.AttrId, AttributeFrom.Metal, kv.Key, metalConfig.GetAttr(level + riseLevel));
                }
            }

            //fashion
            foreach (var kv in FashionData)
            {
                int suitId = kv.Key;

                foreach (var fashionItem in kv.Value)
                {
                    long itemLevel = fashionItem.Value.Data;
                    if (itemLevel > 0)
                    {
                        int part = fashionItem.Key;
                        FashionConfig fashionConfig = FashionConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SuitId == suitId && m.Part == part).FirstOrDefault();

                        for (int i = 0; i < fashionConfig.AttrIdList.Length; i++)
                        {
                            long itemValue = fashionConfig.AttrValueList[i] + (itemLevel - 1) * fashionConfig.AttrRiseList[i];

                            AttributeBonus.SetAttr((AttributeEnum)fashionConfig.AttrIdList[i], AttributeFrom.Fashion, suitId * 100 + part, itemValue);
                        }
                    }
                }

                long suitLevel = kv.Value.Select(m => m.Value.Data).Min();
                if (suitLevel > 0)
                {
                    FashionSuitConfig suitConfig = FashionSuitConfigCategory.Instance.Get(suitId);

                    long suitValue = suitConfig.AttrValue + (suitLevel - 1) * suitConfig.AttrRise;
                    AttributeBonus.SetAttr((AttributeEnum)suitConfig.AttrId, AttributeFrom.Fashion, suitId, suitValue);
                }
            }

            //Halidom
            foreach (var sp in this.HalidomData)
            {
                if (sp.Value.Data > 0)
                {
                    HalidomConfig halidomConfig = HalidomConfigCategory.Instance.Get(sp.Key);
                    long halidomAttr = halidomConfig.AttrValue + (sp.Value.Data - 1) * halidomConfig.RiseAttr;
                    AttributeBonus.SetAttr((AttributeEnum)halidomConfig.AttrId, AttributeFrom.Halidom, sp.Key, halidomAttr);
                }
            }

            //Legacy
            List<LegacyConfig> legacyConfigs = LegacyConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
            foreach (LegacyConfig config in legacyConfigs)
            {
                long layer = GetLegacyLayer(config.Id);
                if (layer > 0)
                {
                    for (int i = 0; i < config.LayerIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)config.LayerIdList[i], AttributeFrom.Legacy, 100 + config.Id, config.GetLayerAttr(i, layer));
                    }
                }

                long level = GetLegacyLevel(config.Id);
                if (level > 0)
                {
                    for (int i = 0; i < config.AttrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)config.AttrIdList[i], AttributeFrom.Legacy, config.Id, config.GetLevelAttr(i, level));
                    }
                }
            }

            //Ring
            foreach (var sp in this.RingData)
            {
                if (sp.Value.Data > 0)
                {
                    RingConfig ringConfig = RingConfigCategory.Instance.Get(sp.Key);
                    for (int i = 0; i < ringConfig.AttrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)ringConfig.AttrIdList[i], AttributeFrom.Ring, sp.Key, ringConfig.GetAttr(i, sp.Value.Data));
                    }
                }
            }

            //修炼
            Dictionary<int, long> attrDict = PillConfigCategory.Instance.ParseLevel(PillData.Data);
            foreach (var kv in attrDict)
            {
                if (kv.Value > 0)
                {
                    AttributeBonus.SetAttr((AttributeEnum)kv.Key, AttributeFrom.Auras, kv.Value);
                }
            }


            //光环
            foreach (var ar in GetAurasList())
            {
                AurasAttrConfig aurasAttrConfig = AurasAttrConfigCategory.Instance.GetConfig(ar.Key);
                AurasAttrConfig config = AurasAttrConfigCategory.Instance.Get(ar.Key);
                AttributeBonus.SetAttr((AttributeEnum)config.AttrId, AttributeFrom.Auras, aurasAttrConfig.GetAttr(ar.Value));
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
                else if (achievementConfig.RewardType == (int)AchievementRewardType.Skill)
                {
                    this.SkillNumber += achievementConfig.AttrValue;
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
            int count = MathHelper.CalRefineStone(equip.Level, this.StoneNumber + this.GetArtifactValue(ArtifactType.RefineStone)) * equip.GetQuality();
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

        public List<SkillRune> GetRuneList(int skillId, List<SkillRuneConfig> buffList)
        {
            List<SkillRune> list = new List<SkillRune>();


            //专属词条
            Dictionary<int, int> skillDict = new Dictionary<int, int>();

            foreach (var ex in this.ExclusivePanelList[ExclusiveIndex].Values)
            {
                ex.GetRuneList(skillId, skillDict);
            }

            //计算装备的词条加成
            List<int> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value.SkillRuneConfig.Id).ToList();

            //金装词条
            skillList.AddRange(this.EquipPanelGolden.Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value.SkillRuneConfig.Id).ToList());

            //buff 词条
            if (buffList != null)
            {
                skillList.AddRange(buffList.Select(m => m.Id));
            }

            foreach (int runeId in skillList)
            {
                if (!skillDict.ContainsKey(runeId))
                {
                    skillDict[runeId] = 0;
                }

                skillDict[runeId] += 1;
            }

            foreach (var kv in skillDict)
            {
                SkillRune skillRune = new SkillRune(kv.Key, kv.Value);
                list.Add(skillRune);
            }

            //if (skillId == 1002)
            //{
            //    Debug.Log(JsonConvert.SerializeObject(list));
            //}

            return list;
        }

        public List<SkillSuit> GetSuitList(int skillId)
        {
            List<SkillSuit> list = new List<SkillSuit>();

            //计算装备的套装加成
            List<SkillSuitConfig> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value.SkillSuitConfig).ToList();

            //金装套装
            skillList.AddRange(this.EquipPanelGolden.Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value.SkillSuitConfig).ToList());

            foreach (var ex in this.ExclusivePanelList[ExclusiveIndex].Values)
            {
                skillList.AddRange(ex.GetSuitList(skillId));
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
            count += this.ExclusivePanelList[ExclusiveIndex].Select(m => m.Value.GetSuitCount(suitId)).Sum();
            count += this.EquipPanelGolden.Where(m => m.Value.SkillSuitConfig != null && m.Value.SuitConfigId == suitId).Count();

            return count;
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

        public EquipRedSuit GetEquipRedConfig(int role, int quality)
        {
            List<Equip> equips = null;
            if (quality == 6)
            {
                equips = this.EquipPanelList[EquipPanelIndex].Select(m => m.Value).Where(m => m.GetQuality() == quality && m.EquipConfig.Role == role).ToList();
            }
            else if (quality == 7)
            {
                equips = this.EquipPanelGolden.Select(m => m.Value).Where(m => m.GetQuality() == quality && m.EquipConfig.Role == role).ToList();
            }

            List<int> layers = equips.Select(m => m.Layer).OrderByDescending(m => m).ToList();

            //Debug.Log("red layers:" + layers.ListToString());

            List<EquipRedConfig> list = EquipRedConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Role == role && m.Quality == quality).ToList();

            List<EquipRedItem> redList = new List<EquipRedItem>();

            for (int i = 0; i < list.Count; i++)
            {
                EquipRedConfig config = list[i];

                int redLevel = layers.Count >= config.Count ? layers[config.Count - 1] : 0;

                EquipRedItem redItem = new EquipRedItem();
                redItem.Level = redLevel;
                redItem.Count = layers.Where(m => m >= redLevel).Count();
                redItem.Config = config;

                redList.Add(redItem);
            }

            EquipRedSuit red = new EquipRedSuit();
            red.List = redList;

            return red;
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

        public Dictionary<int, long> GetAurasList()
        {
            Dictionary<int, long> list = new Dictionary<int, long>();

            foreach (var sl in SoulRingData)
            {
                if (sl.Value.Data > 0)
                {
                    long soulLevel = sl.Value.Data;
                    SoulRingAttrConfig ringConfig = SoulRingConfigCategory.Instance.GetAttrConfig(sl.Key, soulLevel);

                    if (ringConfig.AurasId > 0)
                    {
                        list.Add(ringConfig.AurasId, ringConfig.GetAurasLevel(soulLevel));
                    }
                }
            }
            return list;
        }

        public void SetAchievementProgeress(AchievementSourceType type, long count)
        {
            if (!this.MagicRecord.ContainsKey(type))
            {
                this.MagicRecord[type] = new MagicData();
            }

            this.MagicRecord[type].Data += count;
        }

        public long GetAchievementProgeress(AchievementSourceType type)
        {
            long progress = 0;

            switch (type)
            {
                case AchievementSourceType.Advert:
                    progress = this.Record.GetRecord((int)RecordType.AdVirtual) + this.Record.GetRecord((int)RecordType.AdReal) * 2;
                    break;
                case AchievementSourceType.RealAdvert:
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
                case AchievementSourceType.EquipCopy:
                case AchievementSourceType.Defend:
                case AchievementSourceType.Infinite:
                case AchievementSourceType.Legacy:
                default:
                    {
                        if (!this.MagicRecord.ContainsKey(type))
                        {
                            this.MagicRecord[type] = new MagicData();
                        }
                        progress = this.MagicRecord[type].Data;
                    }
                    break;
            }

            return progress;
        }


        public void AddExpAndGold(double exp, double gold)
        {
            if (this.MagicGold.Data < 0 || gold >= 8223372036854775807)
            {
                GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                return;
            }

            if (exp > 0)
            {
                if (this.MagicLevel.Data < GetMaxLevel())
                {
                    this.MagicExp.Data += exp;
                }
                else
                {
                    this.MagicExp.Data = 0;
                }
            }

            if (gold > 0)
            {
                this.MagicGold.Data += gold;
            }

            EventCenter.Raise(new UserInfoUpdateEvent()); //更新UI

            if (MagicExp.Data >= MagicUpExp.Data)
            {
                GameProcessor.Inst.StartCoroutine(LevelUp()); //升级
            }
        }

        public void SubExp(long exp)
        {
            if (exp <= 0 || this.MagicExp.Data < 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                return;
            }
            this.MagicExp.Data -= exp;

            EventCenter.Raise(new UserInfoUpdateEvent()); //更新UI
        }

        public void SubGold(double gold)
        {
            if (gold <= 0 || this.MagicGold.Data < 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                return;
            }

            this.MagicGold.Data -= gold;

            EventCenter.Raise(new UserInfoUpdateEvent()); //更新UI
        }

        IEnumerator LevelUp()
        {

            while (this.MagicExp.Data >= this.MagicUpExp.Data && this.MagicLevel.Data < GetMaxLevel())
            {
                MagicExp.Data -= MagicUpExp.Data;
                this.MagicLevel.Data++;

                SetUpExp();

                EventCenter.Raise(new UserInfoUpdateEvent());
                EventCenter.Raise(new SetPlayerLevelEvent { Cycle = this.Cycle.Data, Level = this.MagicLevel.Data });
                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;

            if (this.MagicLevel.Data < 10000 && this.Cycle.Data <= 0)
            {
                EventCenter.Raise(new UserAttrChangeEvent());
            }
        }

        private void SetUpExp()
        {
            long levelAttr = LevelConfigCategory.GetLevelAttr(MagicLevel.Data);
            LevelConfig config = LevelConfigCategory.Instance.GetAll().Where(m => m.Value.StartLevel <= MagicLevel.Data && m.Value.EndLevel >= MagicLevel.Data).First().Value;
            MagicUpExp.Data = levelAttr * config.Exp;
        }

        public long GetBagItemCount(int id)
        {
            long count = this.Bags.Where(m => m.Item.Type != ItemType.Equip && m.Item.ConfigId == id).Select(m => m.MagicNubmer.Data).Sum();
            return count;
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

        public List<SkillData> GetCurrentSkill(List<int> existsList)
        {
            List<int> ids = GetCurrentSkillList();

            //Debug.Log(JsonConvert.SerializeObject(ids));

            List<SkillData> list = new List<SkillData>();

            for (int i = 0; i < ids.Count; i++)
            {
                SkillData skill = SkillList.Where(m => m.SkillId == ids[i] && !existsList.Contains(m.SkillId)).FirstOrDefault();
                if (skill != null)
                {
                    list.Add(skill);
                }
            }

            return list;
        }

        public int GetArtifactLevel(int artifactId)
        {
            if (!this.ArtifactData.ContainsKey(artifactId))
            {
                ArtifactData[artifactId] = new MagicData();
            }

            return (int)ArtifactData[artifactId].Data;
        }

        public void SaveArtifactLevel(int itemId, int level)
        {
            int artifactId = ArtifactConfigCategory.Instance.GetByItemId(itemId).Id;

            if (!this.ArtifactData.ContainsKey(artifactId))
            {
                ArtifactData[artifactId] = new MagicData();
            }
            ArtifactData[artifactId].Data += level;
        }

        public int GetFestiveStep()
        {
            int currentStep = 99;

            List<FestiveConfig> list = FestiveConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
            foreach (FestiveConfig config in list)
            {
                int max = this.GetFestiveCount(config.Id);
                if (max < config.Max && config.Step > 0 && config.Step < currentStep)
                {
                    currentStep = config.Step;
                }
            }

            return currentStep;
        }

        public int GetFestiveCount(int id)
        {
            if (!this.FestiveData_101.ContainsKey(id))
            {
                this.FestiveData_101[id] = 0;
            }

            return this.FestiveData_101[id];
        }

        public void SaveFestiveCount(int configId, int count)
        {
            if (this.FestiveData_101.ContainsKey(configId))
            {
                this.FestiveData_101[configId] += count;
            }
            else
            {
                this.FestiveData_101[configId] = count;
            }
        }

        public double GetRealDropRate()
        {
            long dropRate = this.AttributeBonus.GetTotalAttr(AttributeEnum.BurstIncrea);

            double realRate = MathHelper.ConvertionDropRate(dropRate);
            //Debug.Log("realRate:" + realRate);

            return 1 + realRate;
        }

        public double GetKillRecord(int dropId)
        {
            if (!KillRecord.ContainsKey(dropId))
            {
                KillRecord[dropId] = 0;
            }

            return KillRecord[dropId];
        }

        public void SaveKillRecord(int dropId, double kc)
        {
            if (!KillRecord.ContainsKey(dropId))
            {
                KillRecord[dropId] = 0;
            }

            KillRecord[dropId] += kc;
        }

        public long GetItemMeterialCount(int configId)
        {
            if (!ItemMeterialData.ContainsKey(configId))
            {
                ItemMeterialData[configId] = new MagicData();
            }

            return ItemMeterialData[configId].Data;
        }

        public void SaveItemMeterialCount(int configId, long count)
        {
            if (!ItemMeterialData.ContainsKey(configId))
            {
                ItemMeterialData[configId] = new MagicData();
            }

            ItemMeterialData[configId].Data += count;
        }

        public void UseItemMeterialCount(int configId, long count)
        {
            if (ItemMeterialData[configId].Data < count || count <= 0)
            {
                throw new Exception("数值错误");
            }

            ItemMeterialData[configId].Data -= count;
        }

        public long GetCardLevel(int cardId)
        {
            if (!CardData.ContainsKey(cardId))
            {
                CardData[cardId] = new MagicData();
            }

            return CardData[cardId].Data;
        }

        public void SaveCardLevel(int cardId, long level)
        {
            CardData[cardId].Data += level;
        }

        public long GetStrengthLevel(int position)
        {
            if (!MagicEquipStrength.ContainsKey(position))
            {
                MagicEquipStrength[position] = new MagicData();
            }

            return MagicEquipStrength[position].Data;
        }

        public long GetRefineLevel(int position)
        {
            if (!MagicEquipRefine.ContainsKey(position))
            {
                MagicEquipRefine[position] = new MagicData();
            }

            return MagicEquipRefine[position].Data;
        }

        public long GetLegacyLevel(int id)
        {
            if (!LegacyLevel.ContainsKey(id))
            {
                LegacyLevel[id] = new MagicData();
            }

            return LegacyLevel[id].Data;
        }

        public void SaveLegacyLevel(int id)
        {
            if (!LegacyLevel.ContainsKey(id))
            {
                LegacyLevel[id] = new MagicData();
            }

            LegacyLevel[id].Data++;
        }

        public long GetLegacyLayer(int id)
        {
            if (!LegacyLayer.ContainsKey(id))
            {
                LegacyLayer[id] = new MagicData();
            }

            return LegacyLayer[id].Data;
        }

        public void SaveLegacyLayer(int id, int layer)
        {
            if (!LegacyLayer.ContainsKey(id))
            {
                LegacyLayer[id] = new MagicData();
            }

            LegacyLayer[id].Data = layer;
        }

        public long GetRefineStrenthPercetn(int position)
        {
            long refineLevel = GetRefineLevel(position);

            if (refineLevel <= 0)
            {
                return 0;
            }

            long percent = EquipRefineConfigCategory.Instance.GetByLevel(refineLevel).GetStengthPercent(refineLevel);

            return percent;
        }

        public long GetRingLevel(int ringId)
        {
            if (!RingData.ContainsKey(ringId))
            {
                RingData[ringId] = new MagicData();
            }

            return RingData[ringId].Data;
        }

        public void AddRingLevel(int ringId)
        {
            if (!RingData.ContainsKey(ringId))
            {
                RingData[ringId] = new MagicData();
            }

            RingData[ringId].Data++;
        }

        public long GetSoulRingLevel(int sid)
        {
            if (!SoulRingData.ContainsKey(sid))
            {
                SoulRingData[sid] = new MagicData();
            }
            return SoulRingData[sid].Data;
        }

        public long GetSoulBoneLevel(int sid)
        {
            if (!SoulBoneData.ContainsKey(sid))
            {
                SoulBoneData[sid] = new MagicData();
            }
            return SoulBoneData[sid].Data;
        }

        public void AddSoulBoneLevel(int sid)
        {
            if (!SoulBoneData.ContainsKey(sid))
            {
                SoulBoneData[sid] = new MagicData();
            }
            SoulBoneData[sid].Data++;
        }

        public long GetHalidomLevel(int id)
        {
            if (!HalidomData.ContainsKey(id))
            {
                HalidomData[id] = new MagicData();
            }

            return HalidomData[id].Data;
        }


        public void SaveHalidom(int id)
        {
            if (!HalidomData.ContainsKey(id))
            {
                HalidomData[id] = new MagicData();
            }

            HalidomData[id].Data++;
        }

        public long GetMetalLevel(int id)
        {
            if (!MetalData.ContainsKey(id))
            {
                MetalData[id] = new MagicData();
            }

            return MetalData[id].Data;
        }

        public long GetMetalQualityLevel(int quality)
        {
            MetalConfig config = MetalConfigCategory.Instance.GetQualityRiseConfig(quality);

            if (config == null)
            {
                return 0;
            }

            if (!MetalData.ContainsKey(config.Id))
            {
                return 0;
            }

            return MetalData[config.Id].Data;
        }

        internal long GetMaxLevel()
        {
            return Cycle.Data * ConfigHelper.Cycle_Level + ConfigHelper.Max_Level;
        }

        public bool RemoveBagItem(BoxItem boxItem)
        {
            if (Bags.Contains(boxItem))
            {
                Bags.Remove(boxItem);
                return true;
            }
            return false;
        }

        public int GetBagIdleCount(int index)
        {
            return ConfigHelper.BagCount[index] - this.Bags.Where(m => m.GetBagType() == index).Count();
        }

        public int GetLimitId()
        {
            int limitId = 0;

            if (this.First_Create_Time > 0)
            {
                limitId += (int)((TimeHelper.ClientNowSeconds() - this.First_Create_Time) / 86400);
            }
            else
            {
                limitId += 2000;
            }

            limitId += this.Account.Length * 1000;

            return limitId + 1000;
        }

        public List<Item> CheckRecovery(List<Item> items, out long recoveryGold, out int recoveryCount)
        {
            List<Item> newList = new List<Item>();
            recoveryGold = 0;

            List<Item> recoveryList = items.Where(m => RecoverySetting.CheckRecovery(m)).ToList();
            recoveryCount = recoveryList.Count;
            if (recoveryList.Count > 0)
            {
                Dictionary<int, int> recoveryDict = new Dictionary<int, int>();
                recoveryDict[ItemHelper.SpecialId_EquipRefineStone] = 0;
                recoveryDict[ItemHelper.SpecialId_Equip_Speical_Stone] = 0;
                recoveryDict[ItemHelper.SpecialId_Exclusive_Stone] = 0;

                foreach (Item item in recoveryList)
                {
                    if (item.Type == ItemType.Equip)
                    {
                        Equip equip = item as Equip;

                        if (equip.Part <= 10)
                        {
                            recoveryDict[ItemHelper.SpecialId_EquipRefineStone] += CalStone(equip);
                        }
                        else
                        {
                            recoveryDict[ItemHelper.SpecialId_Equip_Speical_Stone] += CalSpecailStone(equip);
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

                        recoveryGold += equip.EquipConfig.Price;
                    }
                    else if (item.Type == ItemType.Exclusive)
                    {
                        recoveryDict[ItemHelper.SpecialId_Exclusive_Stone] += item.GetQuality() * 1;
                    }
                }

                foreach (var kvp in recoveryDict)
                {
                    if (kvp.Value > 0)
                    {
                        Item recoveryItem = ItemHelper.BuildMaterial(kvp.Key, kvp.Value);
                        newList.Add(recoveryItem);
                    }
                }

                items.RemoveAll(m => RecoverySetting.CheckRecovery(m));
                items.AddRange(newList);
            }

            return newList;
        }
    }

    public enum UserChangeType
    {
        LevelUp = 0,
        AttrChange = 1
    }
}
