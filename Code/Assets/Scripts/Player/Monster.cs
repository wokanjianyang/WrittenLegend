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

            string modelName = ModelConfig?.Name + "·";

            this.Name = modelName + Config.Name + QualityConfig.MonsterTitle;

            this.Level = (Config.MapId - 999) * 100;
            this.Exp = Config.Exp * QualityConfig.ExpRate;
            this.Gold = Config.Gold * QualityConfig.GoldRate;


            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double hpModelRate = ModelConfig == null ? 1 : ModelConfig.HpRate;
            double attrModelRate = ModelConfig == null ? 1 : ModelConfig.AttrRate;
            double defModelRate = ModelConfig == null ? 1 : ModelConfig.DefRate;

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, (Config.HP * QualityConfig.HpRate * hpModelRate));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, (Config.Attr * QualityConfig.AttrRate * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, (Config.Attr * QualityConfig.AttrRate * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, (Config.Attr * QualityConfig.AttrRate * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, (Config.Def * QualityConfig.DefRate * defModelRate));

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);
            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //加载技能
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

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

            //存档
            //UserData.Save();
        }

        private void BuildReword()
        {
            User user = GameProcessor.Inst.User;

            double rewardModelRate = ModelConfig == null ? 1 : ModelConfig.RewardRate;
            double dropModelRate = ModelConfig == null ? 1 : ModelConfig.DropRate;

            long exp = (long)(this.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100 * rewardModelRate);
            long gold = (long)(this.Gold * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100 * rewardModelRate);

            //增加经验,金币
            user.AddExpAndGold(exp, gold);

            QualityConfig qualityConfig = QualityConfigCategory.Instance.Get(Quality);

            double dropRate = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstIncrea) / 350.0;
            dropRate = Math.Min(8, 1 + dropRate);
            dropRate = dropRate * dropModelRate * qualityConfig.DropRate; //爆率 = 人物爆率*怪物类型爆率*怪物品质爆率

            //Debug.Log("dropRate:" + dropRate);

            //生成道具奖励
            List<KeyValuePair<double, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(Config.MapId, dropRate);
            //限时奖励
            dropList.AddRange(DropLimitHelper.Build((int)DropLimitType.Normal, dropRate));
            if (ModelConfig == null)
            {
                dropList.AddRange(DropLimitHelper.Build((int)DropLimitType.EquipCopy, dropRate));
            }
            else
            {
                //dropList.AddRange(DropLimitHelper.Build((int)DropLimitType.AnDian, dropRate));
            }

            int qualityRate = qualityConfig.QualityRate * (100 + (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea)) / 100;
            List<Item> items = DropHelper.BuildDropItem(dropList, qualityRate);

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType,
                Message = BattleMsgHelper.BuildMonsterDeadMessage(this, exp, gold, items)
            });

            //自动回收
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { RuleType = this.RuleType });
            }
        }
    }
}
