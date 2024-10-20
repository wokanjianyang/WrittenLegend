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

        private int[] excludeSkillList = { 1004, 2007, 2010, 3004, 3007, 3008, 3009 };
        //private int[] excludeSuitList = { 6 };

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

            this.Name = Config.Name;
            this.Level = (Config.MapId - 999) * 100;
            this.Exp = Config.Exp;
            this.Gold = Config.Gold;


            this.SetAttr();  //设置属性值

            if (this.Config.Layer <= 2)
            {
                this.SetSkill(); //设置技能
            }
            else
            {
                this.SetSkillNew();
            }

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            double hpModelRate = ModelConfig == null ? 1 : ModelConfig.HpRate;
            double attrModelRate = ModelConfig == null ? 1 : ModelConfig.AttrRate;
            double defModelRate = ModelConfig == null ? 1 : ModelConfig.DefRate;

            double hp = Double.Parse(Config.HP);
            double attr = Double.Parse(Config.PhyAttr);
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

            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroBase, Config.Miss);
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroBase, Config.Accuracy);
            AttributeBonus.SetAttr(AttributeEnum.Protect, AttributeFrom.HeroBase, Config.Protect);
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
                List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.StartMapId == 0).ToList();
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

        private void SetSkillNew()
        {
            List<SkillData> list = new List<SkillData>();

            PlayerModel model = null;

            int position = this.MapId % 5 + 1;

            List<PlayerModel> models = PlayerModelCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Quality == 5
            && m.StartMapId <= MapId && MapId <= m.EndMapId).ToList();

            if (models.Count > 0)
            {
                int index = RandomHelper.RandomNumber(0, models.Count);
                model = models[index];
                if (model.SkillList != null)
                {
                    for (int i = 0; i < model.SkillList.Length; i++)
                    {
                        int skillId = model.SkillList[i];
                        if (this.RuleType == RuleType.BossFamily && this.excludeSkillList.Contains(skillId))
                        {
                            continue;
                        }

                        list.Add(new SkillData(skillId, i)); //增加默认技能
                    }
                }
                this.Title = model.Name;
            }

            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {
                List<SkillRune> runeList = new List<SkillRune>();
                List<SkillSuit> suitList = new List<SkillSuit>();

                if (model != null)
                {
                    if (model.Rune > 0)
                    {
                        runeList = SkillRuneConfigCategory.Instance.GetAllRune(skillData.SkillId, model.Rune);
                    }

                    if (model.Suit > 0)
                    {
                        suitList = SkillSuitHelper.GetAllSuit(skillData.SkillId, model.Suit);
                    }
                }

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
            double countModelRate = ModelConfig == null ? 1 : ModelConfig.CountRate;

            long exp = (long)(this.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100 * rewardModelRate);
            long gold = (long)(this.Gold * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100 * rewardModelRate);

            QualityConfig qualityConfig = QualityConfigCategory.Instance.Get(Quality);

            //user.AddStartRate(this.MapId, qualityConfig.CountRate * countModelRate);

            double dropRate = user.GetRealDropRate();
            double modelRate = dropModelRate * qualityConfig.DropRate;
            double countRate = countModelRate * qualityConfig.CountRate;
            int soulPercent = (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.SoulPercent);

            List<Item> items = new List<Item>();
            //生成道具奖励 ,爆率 = 人物爆率*怪物类型爆率*怪物品质爆率
            List<KeyValuePair<double, DropConfig>> dropList = DropConfigCategory.Instance.GetByMapLevel(Config.MapId, dropRate * modelRate);

            //限时奖励
            int limit = user.GetLimitId();
            items.AddRange(DropLimitHelper.Build((int)DropLimitType.Normal, this.MapId, dropRate, modelRate, limit, countRate));
            items.AddRange(DropLimitHelper.Build((int)DropLimitType.Map, this.MapId, dropRate, modelRate, limit, countRate));

            if (this.RuleType == RuleType.EquipCopy || this.RuleType == RuleType.BossFamily)
            {
                items.AddRange(DropLimitHelper.Build((int)DropLimitType.JieRi, this.MapId, dropRate, modelRate, limit, countRate));
            }

            int qualityRate = qualityConfig.QualityRate * (100 + (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea)) / 100;
            items.AddRange(DropHelper.BuildDropItem(dropList, qualityRate));

            int mapIndex = Config.MapId - ConfigHelper.MapStartId;
            int quantity = mapIndex / 10 + 1 + user.SoulRingNumber + user.GetArtifactValue(ArtifactType.SoulStone);

            items.Add(ItemHelper.BuildSoulRingShard(quantity * 2 * user.GetDzRate()));

            double rs = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
            int itemCount = MathHelper.RandomBurstMul(rs);

            int soulRise = 0;
            if (soulPercent > 0)
            {
                soulRise = user.SoulRingNumber + user.GetArtifactValue(ArtifactType.SoulStone);
                soulRise = (int)(soulRise * soulPercent * dropModelRate / 100);
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType,
                Message = BattleMsgHelper.BuildMonsterDeadMessage(this, exp, gold, items, itemCount, soulRise)
            });

            if (itemCount > 0)
            {
                exp += exp * itemCount;
                gold += gold * itemCount;
                items.AddRange(ItemHelper.BurstMul(items, itemCount, qualityRate));
            }

            if (soulRise > 0)
            {
                items.Add(ItemHelper.BuildSoulRingShard(soulRise));
            }


            //先回收
            List<Item> recoveryList = user.CheckRecovery(items, out long recoveryGold, out int recoveryCount);
            if (recoveryCount > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Type = RuleType,
                    Message = BattleMsgHelper.BuildAutoRecoveryMessage(recoveryCount, recoveryList, recoveryGold)
                });
            }

            //增加经验,金币
            user.AddExpAndGold(exp, gold);
            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
        }
    }
}
