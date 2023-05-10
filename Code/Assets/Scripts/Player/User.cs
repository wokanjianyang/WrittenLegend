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

        public int MapId { get; set; }

        public int LastCityId { get; set; }

        public long Power { get; set; }

        public long SecondExpTick { get; set; }

        public int ID { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public IDictionary<int, Equip> EquipPanel { get; set; } = new Dictionary<int, Equip>();

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

        public IDictionary<int,bool> GiftList { get; set; } = new Dictionary<int, bool>();

        public long LastOut { get; set; }

        private bool isInLevelUp;

        /// <summary>
        /// 无尽塔层数
        /// </summary>
        public int TowerFloor = 1;

        public User()  {
            this.EventCenter = new EventManager();

            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);
            this.EventCenter.AddListener<HeroUseEquipEvent>(HeroUseEquip);
            this.EventCenter.AddListener<HeroUnUseEquipEvent>(HeroUnUseEquip);
            this.EventCenter.AddListener<HeroUseSkillBookEvent>(HeroUseSkillBook);
        }

        public void Init() {
            //设置各种属性值
            this.AttributeBonus = new AttributeBonus();

            SetLevelConfigAttr();
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.Test, 10000);
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.Test, 10000);
            AttributeBonus.SetAttr(AttributeEnum.ExpIncrea, AttributeFrom.Test, 1000);

            //设置回收选项
            RecoverySetting.SetType(2, true);
            RecoverySetting.SetQuanlity(1, true);
            RecoverySetting.SetQuanlity(2, true);
            RecoverySetting.SetQuanlity(3, true);
            //RecoverySetting.SetQuanlity(4, true);
            RecoverySetting.SetLevel(100);
        }

        public void UpdatePlayerInfo() {
            GameProcessor.Inst.PlayerInfo.UpdateAttrInfo(this);
        }

        private void HeroChange(HeroChangeEvent e)
        {
            switch (e.Type)
            {
                case UserChangeType.LevelUp:
                    if(!this.isInLevelUp)
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

            //替换属性
            foreach (var a in equip.GetTotalAttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase,PanelPosition, a.Value);
            }

            //更新属性面板
            UpdatePlayerInfo();
        }

        private void HeroUnUseEquip(HeroUnUseEquipEvent e)
        {
            EquipPanel.Remove(e.Position);

            //替换属性
            foreach (var a in e.Equip.GetTotalAttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, e.Position, 0);
            }

            //更新属性面板
            UpdatePlayerInfo();
        }

        private void HeroUseSkillBook(HeroUseSkillBookEvent e)
        {
            SkillBook Book = e.Item as SkillBook;

            if (e.IsLearn)
            {
                //第一次学习，创建技能数据
                SkillData skillData = new SkillData(Book.ConfigId,0);
                skillData.Status = SkillStatus.Learn;
                skillData.Level = 1;
                skillData.Exp = 0;

                this.SkillList.Add(skillData);

                SkillPanel skillPanel = new SkillPanel(skillData, GetRuneList(skillData.SkillId), GetSuitList(skillData.SkillId));
                this.EventCenter.Raise(new HeroUpdateSkillEvent()
                {
                    SkillPanel = skillPanel
                });
            }
            else
            {
                SkillData skillData = this.SkillList.Find(b => b.SkillId == Book.ConfigId);
                skillData.AddExp(Book.ItemConfig.UseParam);

                SkillPanel skillPanel = new SkillPanel(skillData, GetRuneList(skillData.SkillId), GetSuitList(skillData.SkillId));
                this.EventCenter.Raise(new HeroUpdateSkillEvent()
                {
                    SkillPanel = skillPanel
                });
            }
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
            List<Equip> skillList = this.EquipPanel.Where(m =>m.Value.SkillRuneConfig!=null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value).ToList();

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

        public int GetSuitCount(int suitId) {
            int count = this.EquipPanel.Where(m => m.Value.SkillSuitConfig != null && m.Value.SuitConfigId == suitId).Count();
            return Math.Min(count, SkillSuitHelper.SuitMax);
        }

        private void SetLevelConfigAttr()
        {
            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);

            if (this.TowerFloor > 1)
            {
                TowerConfig towerConfig = TowerConfigCategory.Instance.Get(this.TowerFloor - 1);
                AttributeBonus.SetAttr(AttributeEnum.SecondExp, AttributeFrom.Tower, towerConfig.TotalExp);
            }

            UpExp = config.Exp;
        }

        IEnumerator LevelUp()
        {
            while (this.Exp >= this.UpExp)
            {
                Exp -= UpExp;
                Level++;

                //Add Base Attr
                SetLevelConfigAttr();

                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });
                EventCenter.Raise(new HeroInfoUpdateEvent());

                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;
        }

        public enum UserChangeType
        {
            LevelUp = 0,
            AttrChange = 1
        }
    }

}
