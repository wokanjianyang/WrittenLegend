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
        public long Exp { get; set; }

        public long UpExp { get; set; }

        public long Gold { get; set; }

        public long Essence { get; set; }



        public int LastCityId { get; set; }

        public long Power { get; set; }

        public long SecondExpTick { get; set; }

        public int ID { get; set; }

        public string Name { get; set; }

        public long Level { get; set; }

        public IDictionary<int, Equip> EquipPanel { get; set; } = new Dictionary<int, Equip>();

        public IDictionary<int, IDictionary<int, Equip>> EquipPanelList { get; set; } = new Dictionary<int, IDictionary<int, Equip>>();

        public IDictionary<int, Equip> EquipPanelSpecial { get; set; } = new Dictionary<int, Equip>();

        public int EquipPanelIndex { get; set; } = 0;

        public IDictionary<int, long> EquipStrength { get; set; } = new Dictionary<int, long>();

        public IDictionary<int, int> EquipRefine { get; set; } = new Dictionary<int, int>();

        [JsonIgnore]
        public IDictionary<int, int> EquipRecord { get; set; } = new Dictionary<int, int>();

        public RecoverySetting RecoverySetting { get; set; } = new RecoverySetting();

        public List<SkillData> SkillList { get; set; } = new List<SkillData>();

        [JsonIgnore]
        public EventManager EventCenter { get; private set; }

        [JsonIgnore]
        public AttributeBonus AttributeBonus { get; set; }

        /// <summary>
        /// 包裹
        /// </summary>
        public List<BoxItem> Bags { get; set; } = new List<BoxItem>();

        public IDictionary<string, bool> GiftList { get; set; } = new Dictionary<string, bool>();

        public Dictionary<int, long> VersionLog { get; } = new Dictionary<int, long>();

        public long LastOut { get; set; }

        private bool isInLevelUp;

        /// <summary>
        /// 无尽塔层数
        /// </summary>
        public long TowerFloor { get; set; } = 1;

        public int MapId { get; set; } = 1000;

        public int TaskId { get; set; } = 1;
        public Dictionary<int, bool> TaskLog = new Dictionary<int, bool>();

        //副本次数记录
        public long CopyTicketTime { get; set; } = 0;
        public int CopyTikerCount { get; set; } = 0;

        public Dictionary<int, long> MapBossTime { get; } = new Dictionary<int, long>();

        //幻神记录
        public Dictionary<int, int> PhantomRecord { get; } = new Dictionary<int, int>();

        public ADShowData ADShowData { get; set; } = new ADShowData();

        public RecordData Record { get; set; } = new RecordData();

        public long AdLastTime { get; set; } = 0;

        public Dictionary<int, MagicData> SoulRingData { get; } = new Dictionary<int, MagicData>();

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

            LevelConfig config = LevelConfigCategory.Instance.GetAll().Where(m => m.Value.StartLevel <= Level && m.Value.EndLevel >= Level).First().Value;

            //等级属性
            long rise = Level - config.StartLevel;
            long attr = config.StartAttr + (long)(rise * config.RiseAttr);
            long hp = config.StartHp + (long)(rise * config.RiseHp);
            long def = config.StartDef + (long)(rise * config.RiseDef);
            long upExp = config.StartExp + rise * config.RiseExp;

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);


            //Test属性
            //AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.Test, 1000000);
            //AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.Test, 1000);

            //装备属性
            foreach (var kvp in EquipPanelList[EquipPanelIndex])
            {
                EquipRefineConfig refineConfig = null;
                if (EquipRefine.TryGetValue(kvp.Key, out int refineLevel))
                {
                    refineConfig = EquipRefineConfigCategory.Instance.GetByLevel(refineLevel);
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
            foreach (var sp in EquipStrength)
            {
                EquipStrengthConfig strengthConfig = EquipStrengthConfigCategory.Instance.GetByPositioin(sp.Key);
                for (int i = 0; i < strengthConfig.AttrList.Length; i++)
                {
                    AttributeBonus.SetAttr((AttributeEnum)strengthConfig.AttrList[i], AttributeFrom.EquiStrong, sp.Key, strengthConfig.AttrValueList[i] * sp.Value);
                }
            }

            //无尽塔属性
            if (this.TowerFloor > 1)
            {
                long secondExp = 0;
                long secondGold = 0;
                MonsterTowerHelper.GetTowerSecond(TowerFloor - 1, out secondExp, out secondGold);

                AttributeBonus.SetAttr(AttributeEnum.SecondExp, AttributeFrom.Tower, secondExp);
                AttributeBonus.SetAttr(AttributeEnum.SecondGold, AttributeFrom.Tower, secondGold);
            }

            //强化属性
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

            //UpExp = config.Exp;
            UpExp = upExp;

            //更新面板
            if (GameProcessor.Inst.PlayerInfo != null)
            {
                GameProcessor.Inst.PlayerInfo.UpdateAttrInfo(this);
            }
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
            User user = GameProcessor.Inst.User;

            int PanelPosition = e.Position;
            Equip equip = e.Equip;

            IDictionary<int, Equip> ep = null; ;
            if (equip.Part > 10)
            {
                ep = user.EquipPanelSpecial;
            }
            else
            {
                ep = user.EquipPanelList[user.EquipPanelIndex]; ;
            }

            ep[PanelPosition] = equip;

            //更新属性面板
            GameProcessor.Inst.UpdateInfo();

            //更新技能描述
            this.EventCenter.Raise(new HeroUpdateAllSkillEvent());
        }

        private void HeroUnUseEquip(HeroUnUseEquipEvent e)
        {
            User user = GameProcessor.Inst.User;

            IDictionary<int, Equip> ep = null; ;
            if (e.Position > 10)
            {
                ep = user.EquipPanelSpecial;
            }
            else
            {
                ep = user.EquipPanelList[user.EquipPanelIndex]; ;
            }

            ep.Remove(e.Position);

            //更新属性面板
            GameProcessor.Inst.UpdateInfo();

            //更新技能描述
            this.EventCenter.Raise(new HeroUpdateAllSkillEvent());
        }

        private void HeroUseSkillBook(HeroUseSkillBookEvent e)
        {
            SkillBook Book = e.Item as SkillBook;

            SkillData skillData;

            TaskHelper.CheckTask(TaskType.SkillBook, 1);

            if (e.IsLearn)
            {
                //第一次学习，创建技能数据
                skillData = new SkillData(Book.ConfigId, 0);
                skillData.Status = SkillStatus.Learn;
                skillData.Level = 1;
                skillData.Exp = 0;

                this.SkillList.Add(skillData);
            }
            else
            {
                skillData = this.SkillList.Find(b => b.SkillId == Book.ConfigId);
                skillData.AddExp(Book.ItemConfig.UseParam * e.Quantity);
            }

            this.EventCenter.Raise(new HeroUpdateAllSkillEvent());
        }

        private void UserAttrChange(UserAttrChangeEvent e)
        {
            this.SetAttr();

            this.EventCenter.Raise(new HeroUpdateAllSkillEvent());
        }

        public List<SkillRune> GetRuneList(int skillId)
        {
            List<SkillRune> list = new List<SkillRune>();

            //计算装备的词条加成
            List<Equip> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value).ToList();

            //按单件分组,词条有堆叠上限
            var runeGroup = skillList.GroupBy(m => m.RuneConfigId);
            foreach (IGrouping<int, Equip> runeItem in runeGroup)
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
            List<Equip> skillList = this.EquipPanelList[EquipPanelIndex].Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value).ToList();
            var suitGroup = skillList.GroupBy(m => m.SuitConfigId);

            foreach (var suitItem in suitGroup)
            {
                if (suitItem.Count() >= SkillSuitHelper.SuitMax)
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
            return Math.Min(count, SkillSuitHelper.SuitMax);
        }

        public List<EquipGroupConfig> GetEquipGroups()
        {
            var currentPanel = this.EquipPanelList[EquipPanelIndex];

            List<EquipGroupConfig> list = new List<EquipGroupConfig>();

            for (int i = 1; i < 10; i = i + 2)
            {  //1,3,5,7,9
                if (currentPanel.TryGetValue(i,out Equip equip))
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


        public void AddExpAndGold(long exp, long gold)
        {
            if (Level < ConfigHelper.Max_Level || exp < 0)
            {
                this.Exp += exp;
            }

            this.Gold += gold;

            EventCenter.Raise(new UserInfoUpdateEvent()); //更新UI

            if (Exp >= UpExp)
            {
                GameProcessor.Inst.StartCoroutine(LevelUp()); //升级
            }
        }

        IEnumerator LevelUp()
        {
            while (this.Exp >= this.UpExp && this.Level < ConfigHelper.Max_Level)
            {
                LevelConfig config = LevelConfigCategory.Instance.GetAll().Where(m => m.Value.StartLevel <= Level && m.Value.EndLevel >= Level).First().Value;

                long upLevel = this.Exp / UpExp;

                upLevel = Math.Min(upLevel, config.EndLevel - Level + 1);
                if (upLevel > 1000)
                {
                    upLevel = Math.Max(1, upLevel / 10);
                }

                Exp -= UpExp * upLevel;
                Level += upLevel;

                SetUpExp();

                EventCenter.Raise(new UserInfoUpdateEvent());
                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });
                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;

            EventCenter.Raise(new UserAttrChangeEvent());
        }

        private void SetUpExp()
        {
            LevelConfig config = LevelConfigCategory.Instance.GetAll().Where(m => m.Value.StartLevel <= Level && m.Value.EndLevel >= Level).First().Value;
            long rise = Level - config.StartLevel;
            UpExp = config.StartExp + rise * config.RiseExp;
        }

        public long GetMaterialCount(int id)
        {
            long count = this.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == id).Select(m => m.Number).Sum();
            return count;
        }
    }

    public enum UserChangeType
    {
        LevelUp = 0,
        AttrChange = 1
    }
}
