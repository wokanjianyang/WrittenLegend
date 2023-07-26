using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;
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

        public int Level { get; set; }

        public int BagNum { get; set; } = 150;

        public IDictionary<int, Equip> EquipPanel { get; set; } = new Dictionary<int, Equip>();

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

        public IDictionary<int, bool> GiftList { get; set; } = new Dictionary<int, bool>();

        public long LastOut { get; set; }

        private bool isInLevelUp;

        /// <summary>
        /// 无尽塔层数
        /// </summary>
        public int TowerFloor { get; set; } = 1;

        public int MapId { get; set; } = 1000;

        public int TaskId { get; set; } = 1;
        public Dictionary<int ,bool> TaskLog = new Dictionary<int, bool>();

        //主线boss记录
        public Dictionary<int, long> MapBossTime { get; } = new Dictionary<int, long>();

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
            this.AttributeBonus = new AttributeBonus();

            SetAttr();

            //设置Boss刷新时间
            Dictionary<int, MapConfig> mapList = MapConfigCategory.Instance.GetAll();
            foreach (MapConfig mapConfig in mapList.Values)
            {
                if (!MapBossTime.ContainsKey(mapConfig.Id))
                {
                    MapBossTime[mapConfig.Id] = 0;
                }
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
            int PanelPosition = e.Position;
            Equip equip = e.Equip;

            EquipPanel[PanelPosition] = equip;

            //更新属性面板
            GameProcessor.Inst.UpdateInfo();
        }

        private void HeroUnUseEquip(HeroUnUseEquipEvent e)
        {
            EquipPanel.Remove(e.Position);

            //更新属性面板
            GameProcessor.Inst.UpdateInfo();
        }

        private void HeroUseSkillBook(HeroUseSkillBookEvent e)
        {
            SkillBook Book = e.Item as SkillBook;

            SkillData skillData;

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

            //更新技能面板
            SkillPanel skillPanel = new SkillPanel(skillData, GetRuneList(skillData.SkillId), GetSuitList(skillData.SkillId));
            this.EventCenter.Raise(new HeroUpdateSkillEvent() { SkillPanel = skillPanel });
        }

        private void UserAttrChange(UserAttrChangeEvent e)
        {
            this.SetAttr();
        }

        public void BuildReword()
        {
            Dictionary<int, RewardConfig> rewordList = RewardConfigCategory.Instance.GetAll();
            foreach (int rewordId in rewordList.Keys)
            {
                if (!GiftList.Keys.Contains(rewordId))
                {
                    RewardConfig config = rewordList[rewordId];

                    BoxItem boxItem = new BoxItem();

                    if (config.type == 4) //礼包
                    {
                        GiftPack gift = new GiftPack(config.ItemId);
                        boxItem.Item = gift;
                        boxItem.Number = 1;
                        boxItem.BoxId = -1;
                    }

                    this.Bags.Add(boxItem);

                    GiftList.Add(rewordId, true);
                }
            }
        }

        public List<SkillRune> GetRuneList(int skillId)
        {
            List<SkillRune> list = new List<SkillRune>();

            //计算装备的词条加成
            List<Equip> skillList = this.EquipPanel.Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value).ToList();

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
            List<Equip> skillList = this.EquipPanel.Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value).ToList();
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
            int count = this.EquipPanel.Where(m => m.Value.SkillSuitConfig != null && m.Value.SuitConfigId == suitId).Count();
            return Math.Min(count, SkillSuitHelper.SuitMax);
        }

        private void SetAttr()
        {
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


            //测试属性
            //AttributeBonus.SetAttr(AttributeEnum.ExpIncrea, AttributeFrom.Test, 1000);
            //AttributeBonus.SetAttr(AttributeEnum.GoldIncrea, AttributeFrom.Test, 1000);

            //装备属性
            foreach (var kvp in EquipPanel)
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

            //UpExp = config.Exp;
            UpExp = upExp;

            //更新面板
            if (GameProcessor.Inst.PlayerInfo != null)
            {
                GameProcessor.Inst.PlayerInfo.UpdateAttrInfo(this);
            }
        }

        public void AddExpAndGold(long exp, long gold)
        {
            this.Exp += exp;
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
                Exp -= UpExp;
                Level++;

                //Add Base Attr
                EventCenter.Raise(new UserAttrChangeEvent());
                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });
                EventCenter.Raise(new UserInfoUpdateEvent());

                if (GameProcessor.Inst.PlayerManager != null && GameProcessor.Inst.PlayerManager.GetHero()!=null)
                {
                    //GameProcessor.Inst.PlayerManager.GetHero().EventCenter.Raise(new HeroLevelUp());
                }

                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;
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
