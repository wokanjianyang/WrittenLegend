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
        public bool OldFile { get; set; } = false;
        public int Serial { get; set; } = 0;

        public long Power { get; set; }

        public long SecondExpTick { get; set; }

        public int ID { get; set; }

        public string DeviceId { get; set; } = "";

        public string Account { get; set; } = "";
        public string Name { get; set; }

        public long DataDate { get; set; } = 0;
        public int DataProgeress { get; set; } = 0;

        public int OffLineMapId { get; set; }

        public bool SpiritOfflineFlag { get; set; } = false;
        public Dictionary<int, int> SpiritOfflineLog { get; set; } = new Dictionary<int, int>();

        public RandomRecord RandomRecord { get; set; } = new RandomRecord();

        public MagicData Cycle { get; set; } = new MagicData();

        public MagicData MagicLevel { get; } = new MagicData();

        public MagicDouble MagicGold { get; } = new MagicDouble();

        public MagicDouble MagicExp { get; } = new MagicDouble();

        public MagicDouble MagicUpExp { get; } = new MagicDouble();

        public MagicData MagicTowerFloor { get; } = new MagicData();

        public MagicData BabelData { get; } = new MagicData();
        public MagicData BabelCount { get; } = new MagicData();

        public MagicData RedRefreshCount { get; } = new MagicData();

        public IDictionary<int, double> KillRecord { get; } = new Dictionary<int, double>();

        public Dictionary<int, MagicData> RingData { get; } = new Dictionary<int, MagicData>();
        public Dictionary<int, int> RingSelect { get; set; } = new Dictionary<int, int>();

        public IDictionary<int, IDictionary<int, Equip>> EquipPanelList { get; set; } = new Dictionary<int, IDictionary<int, Equip>>();

        public IDictionary<int, IDictionary<int, Equip>> EquipPanelGoldenList { get; set; } = new Dictionary<int, IDictionary<int, Equip>>();

        public IDictionary<int, IDictionary<int, Equip>> EquipPanelDarkGoldList { get; set; } = new Dictionary<int, IDictionary<int, Equip>>();

        public IDictionary<int, IDictionary<int, Equip>> EquipPanelHundunList { get; set; } = new Dictionary<int, IDictionary<int, Equip>>();

        public IDictionary<int, Equip> EquipPanelSpecial { get; set; } = new Dictionary<int, Equip>();

        public Dictionary<int, long> RecordMax = new Dictionary<int, long>();

        public IDictionary<int, IDictionary<int, ExclusiveItem>> ExclusivePanelList { get; set; } = new Dictionary<int, IDictionary<int, ExclusiveItem>>();

        public IDictionary<int, IDictionary<int, ExclusiveItem>> ExclusivePanelGoldenList { get; set; } = new Dictionary<int, IDictionary<int, ExclusiveItem>>();

        public IDictionary<int, IDictionary<int, ExclusiveItem>> ExclusivePanelDarkList { get; set; } = new Dictionary<int, IDictionary<int, ExclusiveItem>>();

        public IDictionary<int, ExclusiveItem> ExclusiveList { get; set; } = new Dictionary<int, ExclusiveItem>();

        public IDictionary<int, Shengxiao> ShengxiaoList { get; set; } = new Dictionary<int, Shengxiao>();

        public int EquipPanelIndex { get; set; } = 0;
        public IDictionary<int, string> PlanNameList { get; set; } = new Dictionary<int, string>();

        public bool ExclusiveSetting { get; set; } = false;
        public int ExclusiveIndex { get; set; } = 0;

        public bool EquipGoldenSetting { get; set; } = false;
        public bool EquipDarkGoldSetting { get; set; } = false;

        public bool EquipHundunSetting { get; set; } = false;

        public int EquipGoldenIndex { get; set; } = 0;

        public int EquipDarkGoldIndex { get; set; } = 0;

        public int EquipHundunIndex { get; set; } = 0;

        public int SkillPanelIndex { get; set; } = 0;

        public IDictionary<int, MagicData> MagicEquipStrength { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> MagicEquipRefine { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> MagicEquipReform { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> LegacyLevel { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> LegacyLayer { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, StoneRecord> StoneData { get; set; } = new Dictionary<int, StoneRecord>();

        public IDictionary<int, MagicData> PetSpeicalLayerData { get; set; } = new Dictionary<int, MagicData>();

        public IDictionary<int, MagicData> PetSpeicalLevelData { get; set; } = new Dictionary<int, MagicData>();

        public MagicData LegacyPoint { get; } = new MagicData();

        //public RecoverySetting RecoverySetting { get; set; } = new RecoverySetting();

        public RecoverySettingNew RecoveryNew { get; set; } = new RecoverySettingNew();

        public bool ShowMonsterSkill { get; set; } = true;

        public bool ShowMonsterDamage { get; set; } = true;

        public bool ShowPlayerEffect { get; set; } = true;

        public int InfoColor { get; set; } = 1;

        public List<SkillData> SkillList { get; set; } = new List<SkillData>();

        public IDictionary<int, List<int>> SkillPanelList { get; set; } = new Dictionary<int, List<int>>();

        public IDictionary<AchievementSourceType, MagicData> MagicRecord { get; set; } = new Dictionary<AchievementSourceType, MagicData>();

        public IDictionary<int, int> RecordData { get; set; } = new Dictionary<int, int>();

        public DefendData DefendData { get; set; }

        public InfiniteData InfiniteData { get; set; }

        public LegacyData LegacyData { get; set; }

        public HeroPhatomData HeroPhatomData { get; set; }

        public List<Pet> PetList { get; set; } = new List<Pet>();

        /// <summary>
        /// 包裹
        /// </summary>
        public List<BoxItem> Bags { get; set; } = new List<BoxItem>();

        public IDictionary<string, bool> GiftListNew { get; set; } = new Dictionary<string, bool>();

        public Dictionary<int, long> VersionLog { get; } = new Dictionary<int, long>();

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
            double limit = (int)(skillConfig.MaxLevel + skillConfig.RiseMaxLevel * GetLimitLevel());
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

        public int GetReformLimit(int position)
        {
            long limit = (GetStrengthLevel(position) - 300000) / 1000;
            return (int)limit;
        }

        public int GetExclusiveLimit()
        {
            long limit = GetArtifactValue(ArtifactType.ExclusiveLimit);
            return (int)limit;
        }

        public int GetFashionLimit()
        {
            int atLevel = GetArtifactValue(ArtifactType.FashionLimit);

            int percent = GetArtifactValue(ArtifactType.FashinPercentLimit);
            if (percent > 0)
            {
                atLevel = atLevel * (100 + percent) / 100;
            }

            return atLevel;
        }

        public int GetHolidomLimit()
        {
            long limit = 4 + GetArtifactValue(ArtifactType.HolidomLimit);
            return (int)limit;
        }

        public int GetCardLimit(CardConfig cardConfig)
        {
            long limit = cardConfig.RiseLevel * (GetLimitLevel() + GetArtifactValue(ArtifactType.CardBaseLimit));

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

        public Dictionary<int, MagicData> RelicData { get; } = new Dictionary<int, MagicData>();

        public MagicData TalentExp { get; set; } = new MagicData();
        public Dictionary<int, MagicData> TalentData { get; } = new Dictionary<int, MagicData>();

        public int TalentPoint { get; set; } = 0;

        public MythData MythData { get; set; } = new MythData();

        public FestiveMapData FestiveMapData01 { get; set; } = new FestiveMapData();

        public Dictionary<int, MagicData> FestiveAttrData { get; } = new Dictionary<int, MagicData>();

        public WorldData WorldData { get; set; } = new WorldData();
        public MagicData WingData { get; set; } = new MagicData();

        public MagicData PillData { get; set; } = new MagicData();

        public MagicData PillData2 { get; set; } = new MagicData();

        public MagicData PillData3 { get; set; } = new MagicData();

        public PillTime PillTime { get; set; } = new PillTime();

        public Dictionary<int, Dictionary<int, MagicData>> FashionData { get; set; } = new Dictionary<int, Dictionary<int, MagicData>>();

        public int FashionUpId { get; set; } = 0;
        public Dictionary<int, MagicData> ItemMeterialData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, int> AchievementData { get; } = new Dictionary<int, int>();

        public Dictionary<int, MagicData> CardData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> CardSpecialData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> HalidomData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> FashionSpecialData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, MagicData> ArtifactData { get; } = new Dictionary<int, MagicData>();

        public Dictionary<int, int> PetCountData { get; } = new Dictionary<int, int>();

        public Dictionary<int, SpiritData> SpiritRecord { get; } = new Dictionary<int, SpiritData>();

        public List<DropData> DropDataList { get; } = new List<DropData>();

        public FestiveWeekData WeekData = new FestiveWeekData();

        public IDictionary<int, int> FestiveData_0202 { get; set; } = new Dictionary<int, int>();
        public IDictionary<int, int> FestiveData_0302 { get; set; } = new Dictionary<int, int>();

        public IDictionary<int, int> FestiveData_1202 { get; set; } = new Dictionary<int, int>();

        public IDictionary<int, int> FestiveData_0102 { get; set; } = new Dictionary<int, int>();

        public IDictionary<int, int> SevenDayData { get; set; } = new Dictionary<int, int>();

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

            //AttributeBonus.SetAttr(AttributeEnum.QualityIncrea, AttributeFrom.Test + 1, 1000000000);
            //AttributeBonus.SetAttr(AttributeEnum.MulAttr, AttributeFrom.Test + 1, 100000);

            //设置升级属性
            SetUpExp();

            //转生属性
            if (Cycle.Data > 0)
            {
                int maxType = (int)((Cycle.Data - 1) / 10);
                for (int cc = 0; cc < maxType; cc++)
                {
                    CycleConfig ccConfig = CycleConfigCategory.Instance.GetByCycle(cc, (cc + 1) * 10);
                    for (int i = 0; i < ccConfig.AttrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)ccConfig.AttrIdList[i], AttributeFrom.Cycle, cc * 100 + i, ccConfig.AttrValueList[i]);
                    }
                }

                CycleConfig cycleConfig = CycleConfigCategory.Instance.GetByCycle(maxType, Cycle.Data);
                for (int i = 0; i < cycleConfig.AttrIdList.Length; i++)
                {
                    AttributeBonus.SetAttr((AttributeEnum)cycleConfig.AttrIdList[i], AttributeFrom.Cycle, maxType * 100 + i, cycleConfig.AttrValueList[i]);
                }
            }

            //装备属性-普通装备
            foreach (KeyValuePair<int, Equip> kvp in EquipPanelList[EquipPanelIndex])
            {
                long refineLevel = GetRefineLevel(kvp.Key);

                foreach (KeyValuePair<int, double> a in kvp.Value.GetTotalAttrList(refineLevel))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }
            //装备属性-四格装备
            foreach (KeyValuePair<int, Equip> kvp in EquipPanelSpecial)
            {
                foreach (KeyValuePair<int, double> a in kvp.Value.GetTotalAttrList(0))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }
            //装备属性-金色装备
            foreach (KeyValuePair<int, Equip> kvp in EquipPanelGoldenList[EquipGoldenIndex])
            {
                foreach (KeyValuePair<int, double> a in kvp.Value.GetTotalAttrList(0))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }

            //装备属性-暗金色装备
            foreach (KeyValuePair<int, Equip> kvp in EquipPanelDarkGoldList[EquipDarkGoldIndex])
            {
                foreach (KeyValuePair<int, double> a in kvp.Value.GetTotalAttrList(0))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }

            //装备属性-混沌装备
            foreach (KeyValuePair<int, Equip> kvp in EquipPanelHundunList[EquipHundunIndex])
            {
                foreach (KeyValuePair<int, double> a in kvp.Value.GetTotalAttrList(0))
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }

            //装备属性-生肖
            foreach (KeyValuePair<int, Shengxiao> kvp in ShengxiaoList)
            {
                foreach (KeyValuePair<int, long> a in kvp.Value.GetTotalAttrList())
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.Shengxiao, kvp.Key, a.Value);
                }
            }

            //生肖-套装
            ShengxiaoGroup shengxiaoGroup = this.GetShengxiaoGroup();
            foreach (ShengxiaoGroupItem sp in shengxiaoGroup.List)
            {
                if (sp.Count >= sp.Config.Count)
                {
                    AttributeBonus.SetAttr((AttributeEnum)sp.Config.AttrId, AttributeFrom.Shengxiao, 100 + sp.Config.Id, sp.Config.AttrValue);
                }
            }

            //英灵
            foreach (var sp in SpiritRecord)
            {
                SpiritConfig spiritConfig = SpiritConfigCategory.Instance.Get(sp.Key);
                long splevel = sp.Value.Level.Data;
                if (splevel > 0)
                {
                    for (int i = 0; i < spiritConfig.AttrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)spiritConfig.AttrIdList[i], AttributeFrom.Spirit, spiritConfig.Id, spiritConfig.AttrValueList[i] * splevel);
                    }
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


                EquipRedSuit red8 = GetEquipRedConfig(role, 8);
                foreach (EquipRedItem redItem in red8.List)
                {
                    if (redItem.Level > 0)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)(redItem.Config.AttrId), AttributeFrom.EquipRed, 80 + role, redItem.Config.AttrValue + redItem.Config.AttrRise * (redItem.Level - 1));
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

            //改造属性
            foreach (var sp in this.MagicEquipReform)
            {
                int position = sp.Key;
                EquipReformConfig reformConfig = EquipReformConfigCategory.Instance.Get(position);

                long reformLevel = sp.Value.Data;
                if (reformLevel > 0)
                {
                    for (int i = 0; i < reformConfig.AttrList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)reformConfig.AttrList[i], AttributeFrom.EquipReform, position, reformConfig.GetAttr(reformLevel, i));
                    }
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
            int cardGroupLevel = GetCardSpecialGroupLevel();
            foreach (var sp in this.CardData)
            {
                if (sp.Value.Data > 0)
                {
                    CardConfig cardConfig = CardConfigCategory.Instance.Get(sp.Key);

                    long cardLevel = sp.Value.Data;
                    long riseLevel = GetCardRiseLevel(cardConfig.Quality, cardLevel, cardGroupLevel);

                    long totalLevel = cardLevel + riseLevel;

                    long val = cardConfig.AttrValue * totalLevel;

                    long riseValue = cardConfig.GetCardRiseValue(totalLevel, cardGroupLevel);

                    AttributeBonus.SetAttr((AttributeEnum)cardConfig.AttrId, AttributeFrom.Card, sp.Key, val + riseValue);
                }
            }

            foreach (var sp in this.CardSpecialData)
            {
                if (sp.Value.Data > 0)
                {
                    CardSpecialConfig cardSpecialConfig = CardSpecialConfigCategory.Instance.Get(sp.Key);

                    int cardSpecialLevel = (int)sp.Value.Data;

                    for (int i = 0; i < cardSpecialConfig.AttrIdList.Length; i++)
                    {
                        int attrCardSpeicalId = cardSpecialConfig.AttrIdList[i];
                        double attrCardSpeicalValue = cardSpecialConfig.GetAttrValue(i, cardSpecialLevel);

                        AttributeBonus.SetAttr((AttributeEnum)attrCardSpeicalId, AttributeFrom.CardSpeical, sp.Key, attrCardSpeicalValue);
                    }
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
                        SoulBoneConfig boneConfig = SoulBoneConfigCategory.Instance.GetConfig(sid, sbLevel);
                        //SoulBoneConfig boneConfig = SoulBoneConfigCategory.Instance.Get(sid);
                        for (int i = 0; i < boneConfig.AttrIdList.Length; i++)
                        {
                            AttributeBonus.SetAttr((AttributeEnum)boneConfig.AttrIdList[i], AttributeFrom.SoulBone, sid, boneConfig.AttrValueList[i] * sbLevel * srLevel);
                        }
                    }
                }
            }

            //神器
            long relicRecord = GetRecordMax((int)AbcType.Relic);
            long relicMax = AbcHelper.GetRecord((int)AbcType.Relic);
            int relicRise = GetRelicRise();
            foreach (var rl in RelicData)
            {
                int rid = rl.Key;
                int level = (int)rl.Value.Data;
                if (level > 0 && relicRecord < relicMax)
                {
                    RelicConfig relicConfig = RelicConfigCategory.Instance.Get(rid);
                    for (int i = 0; i < relicConfig.AttrIdList.Length; i++)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)relicConfig.AttrIdList[i], AttributeFrom.Relic, rid, relicConfig.GetAttrValue(i, level + relicRise));
                    }
                }
            }

            //神器套装
            List<RelicGroupConfig> relicGroups = RelicGroupConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
            foreach (var relicGroupConfig in relicGroups)
            {
                int groupLevel = GetRelicGroupLevel(relicGroupConfig.Id);
                if (groupLevel > 0 && relicRecord < relicMax)
                {
                    double groupValue = relicGroupConfig.GetAttrValue(groupLevel);
                    AttributeBonus.SetAttr((AttributeEnum)relicGroupConfig.AttrId, AttributeFrom.Relic, 999, groupValue);
                }
            }

            //宝石
            long stoneRecord = GetRecordMax((int)AbcType.Stone);
            long stoneMax = AbcHelper.GetRecord((int)AbcType.Stone);

            foreach (var sp in StoneData)
            {
                int ps = sp.Key;

                foreach (var ss in sp.Value.List)
                {
                    StoneSet sd = ss.Value;

                    int stoneId = sd.StoneId;
                    int stoneLevel = (int)sd.StoneLevel.Data;

                    StoneConfig stoneConfig = StoneConfigCategory.Instance.Get(stoneId);
                    int attrValue = stoneConfig.GetAttr(stoneLevel);

                    if (stoneLevel > 0 && stoneRecord < stoneMax)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)stoneConfig.AttrId, AttributeFrom.Stone, ps * 10 + ss.Key, attrValue);
                    }
                }
            }

            //神宠
            for (int id = 1; id <= 3; id++)
            {
                int layer = GetPetSpeicalLayer(id);
                int level = GetPetSpeicalLevel(id);

                List<PetSpeicalAttrConfig> configs = PetSpeicalAttrConfigCategory.Instance.GetList(id, layer);


                for (int i = 0; i < configs.Count; i++)
                {
                    PetSpeicalAttrConfig config = configs[i];
                    double attrValue = config.AttrValue * level;

                    AttributeBonus.SetAttr((AttributeEnum)config.AttrId, AttributeFrom.PetSpeical, id, attrValue);
                }

            }

            for (int type = 1; type <= 1; type++)
            {
                long level = GetFestiveAttrLevel(type);
                List<FestiveAttrConfig> configs = FestiveAttrConfigCategory.Instance.GetList(type, level);

                for (int i = 0; i < configs.Count; i++)
                {
                    FestiveAttrConfig config = configs[i];
                    double attrValue = config.AttrValue * level;

                    AttributeBonus.SetAttr((AttributeEnum)config.AttrId, AttributeFrom.Festive, type, attrValue);
                }
            }

            //宠物
            for (int i = 0; i < PetList.Count; i++)
            {
                Pet pet = PetList[i];
                var attrList = pet.GetBaseAttr();

                foreach (var sp in attrList)
                {
                    AttributeBonus.SetAttr((AttributeEnum)sp.Key, AttributeFrom.Pet, i, sp.Value);
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

                    for (int i = 0; i < suitConfig.AttrIdList.Length; i++)
                    {
                        long suitValue = suitConfig.GetAttrValue(i, suitLevel);
                        AttributeBonus.SetAttr((AttributeEnum)suitConfig.AttrIdList[i], AttributeFrom.Fashion, suitId * 10 + i, suitValue);
                    }
                }
            }

            //fashion-special
            foreach (var sp in FashionSpecialData)
            {
                int fsId = sp.Key;
                long fsLevel = sp.Value.Data;
                if (fsLevel > 0)
                {
                    FashionSpecialConfig fashionSpecialConfig = FashionSpecialConfigCategory.Instance.Get(fsId);

                    for (int i = 0; i < fashionSpecialConfig.AttrIdList.Length; i++)
                    {
                        long fsValue = fashionSpecialConfig.AttrValueList[i];
                        AttributeBonus.SetAttr((AttributeEnum)fashionSpecialConfig.AttrIdList[i], AttributeFrom.FashionSpeical, fsId, fsValue);
                    }

                    if (fsId == FashionUpId)
                    {
                        AttributeBonus.SetAttr((AttributeEnum)fashionSpecialConfig.UpAttrId, AttributeFrom.FashionSpeical, 0, fashionSpecialConfig.UpAttrValue);
                    }

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
                    AttributeBonus.SetAttr((AttributeEnum)kv.Key, AttributeFrom.Pill, kv.Value);
                }
            }

            Dictionary<int, double> pillDict2 = PillConfig2Category.Instance.ParseLevel(PillData2.Data);
            foreach (var kv in pillDict2)
            {
                if (kv.Value > 0)
                {
                    AttributeBonus.SetAttr((AttributeEnum)kv.Key, AttributeFrom.Pill2, kv.Value);
                }
            }

            Dictionary<int, double> pillDict3 = PillConfig3Category.Instance.ParseLevel(PillData3.Data);
            foreach (var kv in pillDict3)
            {
                if (kv.Value > 0)
                {
                    AttributeBonus.SetAttr((AttributeEnum)kv.Key, AttributeFrom.Pill3, kv.Value);
                }
            }

            //天赋
            //神器
            long talentRecord = GetRecordMax((int)AbcType.Talent);
            long talentMax = AbcHelper.GetRecord((int)AbcType.Talent);
            foreach (var sp in this.TalentData)
            {
                if (sp.Value.Data > 0 && talentRecord < talentMax)
                {
                    TalentConfig talentConfig = TalentConfigCategory.Instance.Get(sp.Key);
                    AttributeBonus.SetAttr((AttributeEnum)talentConfig.AttrId, AttributeFrom.Talent, sp.Key, talentConfig.GetAttrValue(sp.Value.Data));
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
            if (this.ExclusivePanelList[ExclusiveIndex].Select(m => m.Key <= 6).Count() >= 6)
            {
                this.SkillNumber += 1;
            }

            List<ExclusiveSuitConfig> exclusiveSuits = ExclusiveSuitConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
            for (int i = 0; i < exclusiveSuits.Count; i++)
            {
                ExclusiveSuitConfig exclusiveSuit = exclusiveSuits[i];
                int esc = this.ExclusivePanelList[ExclusiveIndex].Where(m => exclusiveSuit.StartPart <= m.Key && m.Key <= exclusiveSuit.EndPart).Count();
                //Debug.Log("exclusive suit " + i + " " + esc);
                if (esc >= 6)
                {
                    AttributeBonus.SetAttr((AttributeEnum)exclusiveSuit.AttrId, AttributeFrom.Exclusive, 100, exclusiveSuit.AttrValue);
                }
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

        public long CalSpecailStone(Equip equip)
        {
            int level = equip.Level;

            if (level <= 10)
            {
                return (long)Math.Pow(2, level);
            }

            return CompositeConfigCategory.Instance.GetTotalFee(level);
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

            int skillLayer = SkillConfigCategory.Instance.Get(skillId).SkillLayer;

            //专属词条
            Dictionary<int, int> skillDict = new Dictionary<int, int>();

            foreach (var ex in this.ExclusivePanelList[ExclusiveIndex].Values)
            {
                ex.GetRuneList(skillId, skillDict);

                if (skillLayer > 0)
                {
                    ex.GetRuneListByLayer(skillLayer, skillDict);
                }
            }
            //计算装备的词条加成
            List<int> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value.SkillRuneConfig.Id).ToList();

            //金装词条
            skillList.AddRange(this.EquipPanelGoldenList[EquipGoldenIndex].Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value.SkillRuneConfig.Id).ToList());

            //混沌词条
            skillList.AddRange(this.EquipPanelHundunList[EquipHundunIndex].Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value.SkillRuneConfig.Id).ToList());

            //暗金词条
            if (skillLayer > 0)
            {
                skillList.AddRange(this.EquipPanelDarkGoldList[EquipDarkGoldIndex].Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillLayer == skillLayer).Select(m => m.Value.SkillRuneConfig.Id).ToList());
            }

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

            int skillLayer = SkillConfigCategory.Instance.Get(skillId).SkillLayer;

            //计算装备的套装加成
            List<SkillSuitConfig> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value.SkillSuitConfig).ToList();

            //金装套装
            skillList.AddRange(this.EquipPanelGoldenList[EquipGoldenIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value.SkillSuitConfig).ToList());

            //混沌套装
            skillList.AddRange(this.EquipPanelHundunList[EquipHundunIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value.SkillSuitConfig).ToList());

            //暗金词条
            if (skillLayer > 0)
            {
                skillList.AddRange(this.EquipPanelDarkGoldList[EquipDarkGoldIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillLayer == skillLayer).Select(m => m.Value.SkillSuitConfig).ToList());
            }

            foreach (var ex in this.ExclusivePanelList[ExclusiveIndex].Values)
            {
                skillList.AddRange(ex.GetSuitList(skillId));

                if (skillLayer > 0)
                {
                    skillList.AddRange(ex.GetSuitListByLayer(skillLayer));
                }
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
            count += this.EquipPanelGoldenList[EquipGoldenIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SuitConfigId == suitId).Count();
            count += this.EquipPanelDarkGoldList[EquipDarkGoldIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SuitConfigId == suitId).Count();
            count += this.EquipPanelHundunList[EquipHundunIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SuitConfigId == suitId).Count();

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
                    if (es.Active && es.Config != null)
                    {
                        list.Add(es.Config);
                    }
                }
            }

            return list;
        }

        public ShengxiaoGroup GetShengxiaoGroup()
        {
            List<Shengxiao> equips = this.ShengxiaoList.Select(m => m.Value).ToList();

            List<ShengxiaoGroupItem> redList = new List<ShengxiaoGroupItem>();

            for (int i = 3; i <= 12; i += 3)
            {
                List<ShengxiaoGroupConfig> list = ShengxiaoGroupConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Count == i).OrderByDescending(m => m.Quality).ToList();

                for (int j = 0; j < list.Count; j++)
                {
                    ShengxiaoGroupConfig config = list[j];

                    int count = equips.Where(m => m.GetQuality() >= config.Quality).Count();

                    if (count >= config.Count || config.Quality == 6) //如果激活了，则显示激活的颜色，如果没激活，则显示最低紫色的
                    {
                        ShengxiaoGroupItem redItem = new ShengxiaoGroupItem();
                        redItem.Count = count;
                        redItem.Config = config;
                        redList.Add(redItem);

                        break;
                    }
                }
            }

            ShengxiaoGroup red = new ShengxiaoGroup();
            red.List = redList;

            return red;
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
                equips = this.EquipPanelGoldenList[EquipGoldenIndex].Select(m => m.Value).Where(m => m.GetQuality() == quality && m.EquipConfig.Role == role).ToList();
            }
            else if (quality == 8)
            {
                equips = this.EquipPanelDarkGoldList[EquipDarkGoldIndex].Select(m => m.Value).Where(m => m.GetQuality() == quality && m.EquipConfig.Role == role).ToList();
            }
            else if (quality == 9)
            {
                equips = this.EquipPanelHundunList[EquipHundunIndex].Select(m => m.Value).Where(m => m.GetQuality() == quality && m.EquipConfig.Role == role).ToList();
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
            ExclusiveSuit suit = new ExclusiveSuit(config.Cycle);
            suit.ActiveCount = 0;
            suit.Active = true;

            //suit.Self = new ExclusiveSuitItem(config.Id, config.Name, true);

            List<ExclusiveConfig> configs = ExclusiveConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Cycle == config.Cycle).ToList();

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

        public int GetRecordData(int type)
        {
            if (!RecordData.ContainsKey(type))
            {
                RecordData[type] = 0;
            }

            return RecordData[type];
        }

        public void SaveRecordData(int type, int data)
        {
            RecordData[type] = data;
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
            if (this.MagicGold.Data < 0)
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

        public void SubExp(double exp)
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

            TaskHelper.CheckTask(TaskType.Cycle, this.Cycle.Data);
        }

        private void SetUpExp()
        {
            double levelAttr = LevelConfigCategory.GetLevelAttr(MagicLevel.Data);
            LevelConfig config = LevelConfigCategory.Instance.GetAll().Where(m => m.Value.StartLevel <= MagicLevel.Data && m.Value.EndLevel >= MagicLevel.Data).First().Value;

            double exp = StringHelper.StringToNumber(config.Exp);
            MagicUpExp.Data = levelAttr * exp;
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

        public long GetTicketCount(int id)
        {
            long count = this.Bags.Where(m => m.Item.Type == ItemType.Ticket && m.Item.ConfigId == id).Select(m => m.MagicNubmer.Data).Sum();
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

        public int GetPetCount(int configId)
        {
            if (!this.PetCountData.ContainsKey(configId))
            {
                PetCountData[configId] = 1;
            }

            return PetCountData[configId];
        }

        public int GetPetSkillRate(int role)
        {
            long rate = this.PetList.Where(m => m.Role == role).Select(m => m.GetSkillPercent()).Sum();

            return (int)rate;
        }

        public void SetPetCount(int configId)
        {
            if (!this.PetCountData.ContainsKey(configId))
            {
                PetCountData[configId] = 1;
            }

            PetCountData[configId]++;
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

            List<FestiveConfig> list = FestiveConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Step > 0 && m.RequireCycle <= Cycle.Data).ToList();
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
            if (!this.FestiveData_0102.ContainsKey(id))
            {
                this.FestiveData_0102[id] = 0;
            }

            return this.FestiveData_0102[id];
        }

        public void SaveFestiveCount(int configId, int count)
        {
            if (this.FestiveData_0102.ContainsKey(configId))
            {
                this.FestiveData_0102[configId] += count;
            }
            else
            {
                this.FestiveData_0102[configId] = count;
            }
        }

        public int GetSevenDayCount(int id)
        {
            if (!this.SevenDayData.ContainsKey(id))
            {
                this.SevenDayData[id] = 0;
            }

            return this.SevenDayData[id];
        }

        public void SaveSevenDayCount(int configId, int count)
        {
            if (this.SevenDayData.ContainsKey(configId))
            {
                this.SevenDayData[configId] += count;
            }
            else
            {
                this.SevenDayData[configId] = count;
            }
        }

        public double GetRealDropRate()
        {
            long dropRate = this.AttributeBonus.GetTotalAttr(AttributeEnum.BurstIncrea);

            double realRate = MathHelper.ConvertionDropRate(dropRate, 100);
            //Debug.Log("realRate:" + realRate);

            return 1 + realRate;
        }

        public int GetRealQualityRate()
        {
            long baseRate = this.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea);

            double realRate = MathHelper.ConvertionDropRate(baseRate, 50);
            //Debug.Log("realRate:" + realRate);

            return 1 + (int)realRate;
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

        public long GetCardRiseLevel(int quality, long cardLevel, int groupLevel)
        {
            CardConfig config = CardConfigCategory.Instance.GetQualityRiseConfig(quality);

            if (config == null)
            {
                return 0;
            }

            if (!CardData.ContainsKey(config.Id))
            {
                return 0;
            }

            long goldenLevel = CardData[config.Id].Data;

            long goldenRiseLevel = config.GetCardRiseValue(goldenLevel, groupLevel);

            long riseLevel = cardLevel * (goldenLevel + goldenRiseLevel) / 100;

            return riseLevel;
        }

        public void SaveCardLevel(int cardId, long level)
        {
            CardData[cardId].Data += level;
        }

        public int GetCardSpecialLevel(int cardId)
        {
            if (!CardSpecialData.ContainsKey(cardId))
            {
                CardSpecialData[cardId] = new MagicData();
            }

            return (int)CardSpecialData[cardId].Data;
        }

        public void SaveCardSpecialLevel(int cardId, int level)
        {
            CardSpecialData[cardId].Data += level;
        }

        public int GetCardSpecialGroupLevel()
        {
            int startId = 1;
            int endId = 5;

            long groupLevel = 999999;

            for (int i = startId; i <= endId; i++)
            {
                groupLevel = Math.Min(groupLevel, GetCardSpecialLevel(i));

                //Debug.Log("GetCardSpecialGroupLevel item id:" + i + ": " + groupLevel);
            }

            return (int)groupLevel;
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

        public long GetReformLevel(int position)
        {
            if (!MagicEquipReform.ContainsKey(position))
            {
                MagicEquipReform[position] = new MagicData();
            }

            return MagicEquipReform[position].Data;
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

        public int GetRelicGroupLevel(int gid)
        {
            int startId = 1 + (gid - 1) * 8;
            int endId = gid * 8;

            long groupLevel = RelicData.Where(m => m.Key >= startId && m.Key <= endId).Select(m => m.Value.Data).DefaultIfEmpty(0).Min();

            return (int)Math.Min(groupLevel, Cycle.Data) + this.GetRelicRise();
        }

        public int GetRelicRise()
        {
            if (this.Cycle.Data <= 30)
            {
                return 0;
            }

            return (int)Math.Min(this.Cycle.Data - 30, 10);
        }

        public int GetRelicLevel(int rid)
        {
            if (!RelicData.ContainsKey(rid))
            {
                RelicData[rid] = new MagicData();
            }
            return (int)RelicData[rid].Data;
        }

        public void AddRelicLevel(int rid)
        {
            if (!RelicData.ContainsKey(rid))
            {
                RelicData[rid] = new MagicData();
            }
            RelicData[rid].Data++;
        }

        public long GetTalentLevel(int tid)
        {
            if (!TalentData.ContainsKey(tid))
            {
                TalentData[tid] = new MagicData();
            }
            return TalentData[tid].Data;
        }

        public void AddTalentLevel(int tid, int fee)
        {
            if (!TalentData.ContainsKey(tid))
            {
                TalentData[tid] = new MagicData();
            }
            TalentData[tid].Data++;
            TalentPoint += fee;
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

        public long GetFashionSpecialLevel(int id)
        {
            if (!FashionSpecialData.ContainsKey(id))
            {
                FashionSpecialData[id] = new MagicData();
            }

            return FashionSpecialData[id].Data;
        }

        public void SaveFashionSpecialLevel(int id)
        {
            if (!FashionSpecialData.ContainsKey(id))
            {
                FashionSpecialData[id] = new MagicData();
            }

            FashionSpecialData[id].Data++;
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

        public StoneRecord GetStoneRecord(int id)
        {
            if (!StoneData.ContainsKey(id))
            {
                StoneData[id] = new StoneRecord();
            }

            return StoneData[id];
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

            return limitId + 1020;
        }

        public List<Item> CheckRecovery(List<Item> items, out long gold, out int recoveryCount)
        {
            List<Item> newList = new List<Item>();
            gold = 0;

            List<Item> recoveryList = items.Where(m => RecoveryNew.CheckRecovery(m, RecoveryType.Drop)).ToList();
            recoveryCount = recoveryList.Count;
            if (recoveryList.Count > 0)
            {
                Dictionary<int, long> recoveryDict = new Dictionary<int, long>();

                foreach (Item item in recoveryList)
                {
                    Dictionary<int, long> dict = Recovery(item, out long recoveryGold);

                    gold += recoveryGold;

                    foreach (var sp in dict)
                    {
                        if (!recoveryDict.ContainsKey(sp.Key))
                        {
                            recoveryDict[sp.Key] = 0;
                        }

                        recoveryDict[sp.Key] += sp.Value;
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

                items.RemoveAll(m => RecoveryNew.CheckRecovery(m, RecoveryType.Drop));
                items.AddRange(newList);
            }

            return newList;
        }

        public Dictionary<int, long> Recovery(Item item, out long recoveryGold)
        {
            recoveryGold = 0;

            Dictionary<int, long> dict = new Dictionary<int, long>();

            if (item.Type == ItemType.Equip)
            {
                Equip equip = item as Equip;

                if (equip.EquipConfig.Cycle == 0)
                {
                    dict[ItemHelper.SpecialId_Equip_Speical_Stone] = CalSpecailStone(equip);
                }
                else if (equip.EquipConfig.Cycle == 1)
                {
                    dict[ItemHelper.SpecialId_EquipRefineStone] = CalStone(equip);

                    if (equip.GetQuality() >= 5)
                    {
                        dict[ItemHelper.SpecailEquipRefreshId] = 1;
                    }
                }
                else if (equip.EquipConfig.Cycle >= 2 && equip.EquipConfig.Cycle <= 4)
                {
                    int RecoveryItemId = equip.EquipConfig.RecoveryItemId;
                    if (RecoveryItemId > 0)
                    {
                        dict[RecoveryItemId] = 1;
                    }
                }
                else if (equip.EquipConfig.Cycle == 5)
                {
                    if (equip.GetQuality() == 9)
                    {
                        dict[ItemHelper.SpecialId_Equip_Hundun] = 1;
                    }
                    else
                    {
                        dict[ItemHelper.SpecialId_EquipRefineStone] = CalStone(equip);
                    }
                }

                recoveryGold += equip.EquipConfig.Price;
            }
            else if (item.Type == ItemType.Exclusive)
            {
                ExclusiveItem exclusive = item as ExclusiveItem;

                if (exclusive.ExclusiveConfig.Cycle >= 2 && exclusive.GetQuality() == 7)
                {
                    dict[ItemHelper.SpecialId_Exclusive_Golden] = 1;
                }
                else if (exclusive.ExclusiveConfig.Cycle >= 3 && exclusive.GetQuality() == 8)
                {
                    dict[ItemHelper.SpecialId_Exclusive_Dark] = 1;
                }
                else
                {
                    dict[ItemHelper.SpecialId_Exclusive_Stone] = item.GetQuality() * 1;
                }
            }
            else if (item.Type == ItemType.Pet)
            {
                Pet pet = item as Pet;
                int quality = item.GetQuality();
                dict[ItemHelper.SpecialId_Pet_Exp] = quality * 100;

                if (quality >= 5)
                {
                    dict[ItemHelper.Specail_Pet_Layer[quality - 5]] = 1;
                }
                if (pet.DevourFlairs.Count > 0)
                {
                    dict[ItemHelper.Specail_Pet_Layer[quality - 5]] += pet.DevourFlairs.Count;
                }
            }
            else if (item.Type == ItemType.Shengxiao)
            {
                int quality = item.GetQuality();
                if (quality <= 5)
                {
                    dict[ItemHelper.Specail_Shengxiao] = quality * 500;
                }
                else if (quality == 9)
                {
                    dict[ItemHelper.Specail_Shengxiao2] = 1;
                }
                else
                {
                    dict[ItemHelper.Specail_Shengxiao1] = (int)(Math.Pow(3, quality - 6));
                }
            }
            else if (item.ItemConfig.RecoveryItemId > 0)
            {
                int RecoveryItemId = item.ItemConfig.RecoveryItemId;

                dict[RecoveryItemId] = item.ItemConfig.RecoveryCount;
            }
            else
            {
                recoveryGold += item.ItemConfig.Price * item.Count;
            }

            return dict;
        }

        public bool CheckKeepSkill(int skillId, int skillLayer)
        {
            int c = GameProcessor.Inst.User.SkillList.Where(m => (m.SkillId == skillId || m.SkillConfig.SkillLayer == skillLayer) && m.Recovery).Count();

            return c > 0;
        }

        private long GetRecordMax(int key)
        {
            if (RecordMax.ContainsKey(key))
            {
                return RecordMax[key];
            }
            return 0;
        }

        public void SaveRecordMax(int key, long v)
        {
            if (!RecordMax.ContainsKey(key))
            {
                RecordMax[key] = v + 3;
            }

            if (RecordMax[key] < v)
            {
                RecordMax[key] = v + 5;
            }
        }

        public int GetPetSpeicalLevel(int rid)
        {
            if (!PetSpeicalLevelData.ContainsKey(rid))
            {
                PetSpeicalLevelData[rid] = new MagicData();
            }
            return (int)PetSpeicalLevelData[rid].Data;
        }

        public void AddPetSpeicalLevel(int rid)
        {
            if (!PetSpeicalLevelData.ContainsKey(rid))
            {
                PetSpeicalLevelData[rid] = new MagicData();
            }
            PetSpeicalLevelData[rid].Data++;
        }
        public int GetPetSpeicalLayer(int rid)
        {
            if (!PetSpeicalLayerData.ContainsKey(rid))
            {
                PetSpeicalLayerData[rid] = new MagicData();
            }
            return (int)PetSpeicalLayerData[rid].Data;
        }

        public void AddPetSpeicalLayer(int rid)
        {
            if (!PetSpeicalLayerData.ContainsKey(rid))
            {
                PetSpeicalLayerData[rid] = new MagicData();
            }
            PetSpeicalLayerData[rid].Data++;
        }

        public int GetPetSpeicalGroupLevel()
        {
            if (PetSpeicalLayerData.Count < 3)
            {
                return 0;
            }

            return (int)PetSpeicalLayerData.Select(m => m.Value.Data).Min();
        }


        public long GetFestiveAttrLevel(int key)
        {
            if (!FestiveAttrData.ContainsKey(key))
            {
                FestiveAttrData[key] = new MagicData();
            }
            return FestiveAttrData[key].Data;
        }

        public void SaveFestiveAttrLevel(int key)
        {
            if (!FestiveAttrData.ContainsKey(key))
            {
                FestiveAttrData[key] = new MagicData();
            }
            FestiveAttrData[key].Data++;
        }


        public void SaveSpiritLevel(int cardId, long level)
        {
            if (!SpiritRecord.ContainsKey(cardId))
            {
                SpiritRecord[cardId] = new SpiritData();
            }

            SpiritRecord[cardId].Level.Data += level;
        }

        public int GetSpiritLevel(int cardId)
        {
            if (!SpiritRecord.ContainsKey(cardId))
            {
                SpiritRecord[cardId] = new SpiritData();
            }

            return (int)SpiritRecord[cardId].Level.Data;
        }
    }

    public enum UserChangeType
    {
        LevelUp = 0,
        AttrChange = 1
    }
}
