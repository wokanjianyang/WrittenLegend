using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace Game
{
    public class Monster_Defend : APlayer
    {
        public int Progeress;
        public int Type;
        MonsterDefendConfig Config { get; set; }

        public Monster_Defend(long progress, int type) : base()
        {
            this.Progeress = (int)progress;
            this.Type = type;
            this.GroupId = 2;

            this.Config = MonsterDefendConfigCategory.Instance.Get(Progeress);

            this.Init();
            this.EventCenter.AddListener<DeadRewarddEvent>(MakeReward);
        }

        private void Init()
        {
            this.Camp = PlayerType.Enemy;
            this.Name = Config.Name;
            this.Level = Progeress * 100;

            this.SetAttr();  //��������ֵ
            this.SetSkill(); //���ü���

            base.Load();
            this.Logic.SetData(null); //����UI
        }

        private void SetAttr()
        {
            this.AttributeBonus = new AttributeBonus();



            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, (long)(Config.HP));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, (long)(Config.PhyAttr));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, (long)(Config.PhyAttr));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, (long)(Config.PhyAttr));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, (long)(Config.Def));

            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroBase, Config.DamageIncrea);
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroBase, Config.DamageResist);
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroBase, Config.CritRate);
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroBase, Config.CritDamage);
            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
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
            BuildReword();
        }

        private void BuildReword()
        {
            User user = GameProcessor.Inst.User;

            long exp = (long)(Config.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100);
            long gold = (long)(Config.Gold * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100);

            //���Ӿ���,���
            user.AddExpAndGold(exp, gold);
        }
    }
}
