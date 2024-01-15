using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Game
{
    public class Boss : APlayer
    {
        public int BossId;
        public int MapId;
        BossConfig Config { get; set; }
        MonsterModelConfig ModelConfig { get; set; }

        public int GoldRate;
        public long Gold;
        public float Att;
        public float Def;
        public long Exp;

        private int RewarCount = 1;

        public Boss(int bossId, int mapId, RuleType ruleType, int rewarCount, int modelId) : base()
        {
            this.BossId = bossId;
            this.MapId = mapId;
            this.GroupId = 2;
            this.Quality = 5;

            this.RuleType = ruleType;
            this.RewarCount = rewarCount;

            this.Config = BossConfigCategory.Instance.Get(BossId);
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
            this.ModelType = MondelType.Boss;

            string modelName = ModelConfig?.Name + "·";

            this.Name = modelName + Config.Name;
            this.Level = (Config.MapId - 999) * 100;
            this.Exp = Config.Exp;
            this.Gold = Config.Gold;


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

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, (Config.HP * hpModelRate));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, (Config.PhyAttr * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, (Config.PhyAttr * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, (Config.PhyAttr * attrModelRate));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, (Config.Def * defModelRate));

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

            if (Config.ModelType == 0)
            {
                //random model
                List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).ToList();
                int index = RandomHelper.RandomNumber(0, models.Count);
                PlayerModel model = models[index];

                if (model.SkillList != null)
                {
                    for (int i = 0; i < model.SkillList.Length; i++)
                    {
                        list.Add(new SkillData(model.SkillList[i], i)); //增加默认技能
                    }
                }

                this.Name = model.Name + "·" + Config.Name;
            }

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
            //Log.Info("Boss :" + this.ToString() + " dead");

            for (int i = 0; i < RewarCount; i++)
            {
                BuildReword();
            }
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

            double dropRate = 1 + user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstIncrea) / 350.0;
            dropRate = Math.Min(8, 1 + dropRate);
            double modelRate = dropModelRate * qualityConfig.DropRate;


            //生成道具奖励 ,爆率 = 人物爆率*怪物类型爆率*怪物品质爆率
            List<KeyValuePair<double, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(Config.MapId, dropRate * modelRate);

            //限时奖励
            dropList.AddRange(DropLimitHelper.Build((int)DropLimitType.Normal, dropRate, modelRate, user.RateData));

            if (this.RuleType == RuleType.EquipCopy || this.RuleType == RuleType.BossFamily)
            {
                dropList.AddRange(DropLimitHelper.Build((int)DropLimitType.JieRi, dropRate, modelRate));
            }

            int qualityRate = qualityConfig.QualityRate * (100 + (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea)) / 100;
            List<Item> items = DropHelper.BuildDropItem(dropList, qualityRate, user.RateData);

            int mapIndex = Config.MapId - ConfigHelper.MapStartId;
            int quantity = mapIndex / 10 + 1 + user.SoulRingNumber;

            items.Add(ItemHelper.BuildSoulRingShard(quantity * 2));

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }


            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType,
                Message = BattleMsgHelper.BuildBossDeadMessage(this, exp, gold, items)
            });


            //自动回收
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { RuleType = this.RuleType });
            }
        }
    }
}
