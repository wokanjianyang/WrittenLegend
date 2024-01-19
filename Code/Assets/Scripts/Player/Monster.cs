using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace Game
{
    public class Monster : APlayer
    {
        public int MonsterId;
        MonsterBase Config { get; set; }
        QualityConfig QualityConfig { get; set; }

        MonsterModelConfig ModelConfig { get; set; }

        public int GoldRate;
        public long Gold;
        public int AttTyp;
        public float Att;
        public float Def;
        public long Exp;
        public int range;

        private int RewardRate;

        public Monster(int monsterId, int quality, int rewarRate, int modelId, RuleType ruleType) : base()
        {
            this.MonsterId = monsterId;
            this.GroupId = 2;
            this.Quality = quality;

            this.RewardRate = rewarRate;
            this.RuleType = ruleType;

            this.Config = MonsterBaseCategory.Instance.Get(MonsterId);
            this.QualityConfig = QualityConfigCategory.Instance.Get(Quality);
            if (modelId > 0)
            {
                ModelConfig = MonsterModelConfigCategory.Instance.Get(modelId);
            }

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;

            string modelName = ModelConfig?.Name + "��";

            this.Name = modelName + Config.Name + QualityConfig.MonsterTitle;

            this.Level = (Config.MapId - 999) * 100;
            this.Exp = Config.Exp * QualityConfig.ExpRate;
            this.Gold = Config.Gold * QualityConfig.GoldRate;


            this.SetAttr();  //��������ֵ
            this.SetSkill(); //���ü���

            base.Load();
            this.Logic.SetData(null); //����UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double hpModelRate = ModelConfig == null ? 1 : ModelConfig.HpRate;
            double attrModelRate = ModelConfig == null ? 1 : ModelConfig.AttrRate;
            double defModelRate = ModelConfig == null ? 1 : ModelConfig.DefRate;

            double hp = Double.Parse(Config.HP);
            double attr = Double.Parse(Config.Attr);
            double def = Double.Parse(Config.Def);

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, (hp * hpModelRate));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, (attr * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, (attr * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, (attr * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, (def * defModelRate));

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);
            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //���ؼ���
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //����Ĭ�ϼ���

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = new List<SkillRune>();
                List<SkillSuit> suitList = new List<SkillSuit>();

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, false);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);
            }
        }

        private void MakeReward(DeadRewarddEvent dead)
        {
            //Log.Info("Monster :" + this.ToString() + " dead");

            for (int i = 0; i < RewardRate; i++)
            {
                BuildReword();
            }

            //�浵
            //UserData.Save();
        }

        private void BuildReword()
        {
            User user = GameProcessor.Inst.User;

            double rewardModelRate = ModelConfig == null ? 1 : ModelConfig.RewardRate;
            double dropModelRate = ModelConfig == null ? 1 : ModelConfig.DropRate;
            double countModelRate = ModelConfig == null ? 1 : ModelConfig.CountRate;

            long exp = (long)(this.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100 * rewardModelRate);
            long gold = (long)(this.Gold * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100 * rewardModelRate);

            //���Ӿ���,���
            user.AddExpAndGold(exp, gold);

            QualityConfig qualityConfig = QualityConfigCategory.Instance.Get(Quality);

            user.AddStartRate(qualityConfig.CountRate * countModelRate);

            double dropRate = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstIncrea) / 350.0;
            dropRate = Math.Min(8, 1 + dropRate);
            double modelRate = dropModelRate * qualityConfig.DropRate;

            //Debug.Log("dropRate:" + dropRate);

            //���ɵ��߽���
            List<KeyValuePair<double, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(Config.MapId, dropRate * modelRate);

            //��ʱ����
            dropList.AddRange(DropLimitHelper.Build((int)DropLimitType.Normal, dropRate, modelRate, user.RateData));

            if (this.RuleType == RuleType.EquipCopy || this.RuleType == RuleType.BossFamily)
            {
                dropList.AddRange(DropLimitHelper.Build((int)DropLimitType.JieRi, dropRate, modelRate));
            }

            int qualityRate = qualityConfig.QualityRate * (100 + (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea)) / 100;
            List<Item> items = DropHelper.BuildDropItem(dropList, qualityRate, user.RateData);

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType,
                Message = BattleMsgHelper.BuildMonsterDeadMessage(this, exp, gold, items)
            });

            //�Զ�����
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { RuleType = this.RuleType });
            }
        }
    }
}
