using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Game
{
    public class Monster_Defend : APlayer
    {
        public int Progeress;
        MonsterDefendConfig Config { get; set; }
        QualityConfig QualityConfig { get; set; }

        public Monster_Defend(long progress, int quality) : base()
        {
            this.Progeress = (int)progress;
            this.GroupId = 2;
            this.Quality = quality;

            this.Config = MonsterDefendConfigCategory.Instance.Get(Progeress);
            this.QualityConfig = QualityConfigCategory.Instance.Get(this.Quality);

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.Name = Config.Name + QualityConfig.MonsterTitle;
            this.Level = Progeress * 100;

            this.SetAttr();  //设置属性值
            this.SetSkill(); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, (long)(Config.HP * QualityConfig.HpRate));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, (long)(Config.PhyAttr * QualityConfig.AttrRate));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, (long)(Config.PhyAttr * QualityConfig.AttrRate));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, (long)(Config.PhyAttr * QualityConfig.AttrRate));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, (long)(Config.Def * QualityConfig.DefRate));

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill()
        {
            //加载技能
            List<SkillData> list = new List<SkillData>();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            if (Quality >= 5) {
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
            BuildReword();
        }

        private void BuildReword()
        {
            User user = GameProcessor.Inst.User;

            long exp = (long)(Config.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100);
            long gold = (long)(Config.Gold * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100);

            //增加经验,金币
            user.AddExpAndGold(exp, gold);

            List<KeyValuePair<int, DropConfig>> dropList = new List<KeyValuePair<int, DropConfig>>();

            //掉落道具
            for (int i = 0; i < Config.DropIdList.Length; i++)
            {
                DropConfig dropConfig = DropConfigCategory.Instance.Get(Config.DropIdList[i]);
                dropList.Add(new KeyValuePair<int, DropConfig>(Config.DropRateList[i], dropConfig));
            }

            List<Item> items = DropHelper.BuildDropItem(dropList, 1);

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildMonsterDeadMessage(this, exp, gold, items)
            });
        }
    }
}
