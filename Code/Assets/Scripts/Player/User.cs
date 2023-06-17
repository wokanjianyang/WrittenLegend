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

        public int BagNum { get; set; } = 150;

        public IDictionary<int, Equip> EquipPanel { get; set; } = new Dictionary<int, Equip>();

        public IDictionary<int, int> EquipStrength { get; set; } = new Dictionary<int, int>();

        [JsonIgnore]
        public IDictionary<int, int> EquipRecord { get; set; } = new Dictionary<int, int>();

        public RecoverySetting RecoverySetting { get; set; } = new RecoverySetting();

        public List<SkillData> SkillList { get; set; } = new List<SkillData>();

        [JsonIgnore]
        public EventManager EventCenter { get; private set; }

        [JsonIgnore]
        public AttributeBonus AttributeBonus { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public List<BoxItem> Bags { get; set; } = new List<BoxItem>();

        public IDictionary<int, bool> GiftList { get; set; } = new Dictionary<int, bool>();

        public long LastOut { get; set; }

        private bool isInLevelUp;

        /// <summary>
        /// �޾�������
        /// </summary>
        public int TowerFloor = 1;

        //����boss��¼
        public Dictionary<int, long> MapBossTime { get; } = new Dictionary<int, long>();

        public User()
        {
            this.EventCenter = new EventManager();

            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);
            this.EventCenter.AddListener<HeroUseEquipEvent>(HeroUseEquip);
            this.EventCenter.AddListener<HeroUnUseEquipEvent>(HeroUnUseEquip);
            this.EventCenter.AddListener<HeroUseSkillBookEvent>(HeroUseSkillBook);
        }

        public void Init()
        {
            //���ø�������ֵ
            this.AttributeBonus = new AttributeBonus();

            SetAttr();

            //���û���ѡ��
            RecoverySetting.SetType(2, true);
            RecoverySetting.SetQuanlity(1, true);
            RecoverySetting.SetQuanlity(2, true);
            RecoverySetting.SetQuanlity(3, true);
            //RecoverySetting.SetQuanlity(4, true);
            RecoverySetting.SetLevel(100);

            //����Bossˢ��ʱ��
            Dictionary<int, MapConfig> mapList = MapConfigCategory.Instance.GetAll();
            foreach (MapConfig mapConfig in mapList.Values)
            {
                if (!MapBossTime.ContainsKey(mapConfig.Id))
                {
                    MapBossTime[mapConfig.Id] = 0;
                }
            }
        }

        public void UpdatePlayerInfo()
        {
            GameProcessor.Inst.PlayerInfo.UpdateAttrInfo(this);
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

            //�滻����
            foreach (var a in equip.GetTotalAttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, PanelPosition, a.Value);
            }

            //�����������
            UpdatePlayerInfo();
        }

        private void HeroUnUseEquip(HeroUnUseEquipEvent e)
        {
            EquipPanel.Remove(e.Position);

            //�滻����
            foreach (var a in e.Equip.GetTotalAttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, e.Position, 0);
            }

            //�����������
            UpdatePlayerInfo();
        }

        private void HeroUseSkillBook(HeroUseSkillBookEvent e)
        {
            SkillBook Book = e.Item as SkillBook;

            if (e.IsLearn)
            {
                //��һ��ѧϰ��������������
                SkillData skillData = new SkillData(Book.ConfigId, 0);
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

                    if (config.type == 4) //���
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

            //����װ���Ĵ����ӳ�
            List<Equip> skillList = this.EquipPanel.Where(m => m.Value.SkillRuneConfig != null && m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value).ToList();

            //����������,�����жѵ�����
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

            //����װ������װ�ӳ�
            List<Equip> skillList = this.EquipPanel.Where(m => m.Value.SkillSuitConfig != null && m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value).ToList();
            var suitGroup = skillList.GroupBy(m => m.SuitConfigId);

            foreach (var suitItem in suitGroup)
            {
                if (suitItem.Count() >= SkillSuitHelper.SuitMax)
                {  //SkillSuitHelper.SuitMax ���ų���,����ֻ����һ������Ч
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
            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            //�ȼ�����
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);

            //��������
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.Test, 10000);
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.Test, 10000);
            AttributeBonus.SetAttr(AttributeEnum.ExpIncrea, AttributeFrom.Test, 1000);

            //װ������
            foreach (var kvp in EquipPanel)
            {
                foreach (var a in kvp.Value.GetTotalAttrList)
                {
                    AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, kvp.Key, a.Value);
                }
            }

            //�޾�������
            if (this.TowerFloor > 1)
            {
                TowerConfig towerConfig = TowerConfigCategory.Instance.Get(this.TowerFloor - 1);
                AttributeBonus.SetAttr(AttributeEnum.SecondExp, AttributeFrom.Tower, towerConfig.TotalExp);
            }

            UpExp = config.Exp;

            //�������
            UpdatePlayerInfo();
        }

        public void AddExpAndGold(long exp, long gold)
        {
            this.Exp += exp;
            this.Gold += gold;
            EventCenter.Raise(new UserInfoUpdateEvent()); //����UI

            if (Exp >= UpExp)
            {
                GameProcessor.Inst.StartCoroutine(LevelUp()); //����
            }
        }

        IEnumerator LevelUp()
        {
            while (this.Exp >= this.UpExp && this.Level < PlayerHelper.Max_Level)
            {
                Exp -= UpExp;
                Level++;

                //Add Base Attr
                SetAttr();

                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });
                EventCenter.Raise(new UserInfoUpdateEvent());

                var hero = GameProcessor.Inst.PlayerManager.GetHero();
                if (hero != null)
                {
                    hero.EventCenter.Raise(new HeroLevelUp());
                }

                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;
        }
    }

    public enum UserChangeType
    {
        LevelUp = 0,
        AttrChange = 1
    }
}
