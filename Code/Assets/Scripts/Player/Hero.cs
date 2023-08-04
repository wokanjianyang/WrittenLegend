using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Game
{
    public class Hero : APlayer
    {
        private Dictionary<int, int> skillUseCache = new Dictionary<int, int>();
        public Hero() : base()
        {
            this.GroupId = 1;

            this.Init();

            this.EventCenter.AddListener<HeroLevelUp>(LevelUp);
            this.EventCenter.AddListener<HeroAttrChangeEvent>(HeroAttrChange);
            User user = GameProcessor.Inst.User;
            user.EventCenter.AddListener<HeroUpdateAllSkillEvent>(OnHeroUpdateAllSkillEvent);

        }

        private void LevelUp(HeroLevelUp e)
        {
            User user = GameProcessor.Inst.User;
            this.Level = user.Level;

            this.SetAttr(user);  //��������ֵ
            this.Logic.SetData(null); //����UI
        }

        public void HeroAttrChange(HeroAttrChangeEvent e)
        {
            User user = GameProcessor.Inst.User;
            this.SetAttr(user);  //��������ֵ
        }

        private void OnHeroUpdateAllSkillEvent(HeroUpdateAllSkillEvent e)
        {
            this.UpdateSkills();
        }
        private void Init()
        {
            User user = GameProcessor.Inst.User;
            this.Camp = PlayerType.Hero;
            this.Name = user.Name;
            this.Level = user.Level;

            this.SetAttr(user);  //��������ֵ
            this.SetSkill(user); //���ü���

            base.Load();
            this.Logic.SetData(null); //����UI
        }

        private void SetAttr(User user)
        {
            this.AttributeBonus = new AttributeBonus();

            //���û�������ԣ�����ս���Ļ�������
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.HP));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.MagicAtt));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.SpiritAtt));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.Def));
            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.Speed));
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky));
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate));
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage));
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist));
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist));
            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist));
            AttributeBonus.SetAttr(AttributeEnum.InheritIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.InheritIncrea));
            AttributeBonus.SetAttr(AttributeEnum.RestoreHp, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.RestoreHp));
            AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.RestoreHpPercent));


            //TEST
            //AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, 100);

            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        private void SetSkill(User user)
        {
            SelectSkillList = new List<SkillState>();

            //���ؼ���
            List<SkillData> list = user.SkillList.FindAll(m => m.Status == SkillStatus.Equip).OrderBy(m => m.Position).ToList();
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //����Ĭ�ϼ���

            //TEST skill
            //list.Clear();
            //list.Add(new SkillData(2004, (int)SkillPosition.Default)); //����Ĭ�ϼ���

            foreach (SkillData skillData in list)
            {

                List<SkillRune> runeList = user.GetRuneList(skillData.SkillId);
                List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
                SelectSkillList.Add(skill);

                //ְҵר�����ܵ�����
                if (skillData.SkillConfig.Type == (int)SkillType.Expert)
                {
                    int attrKey = (int)AttributeFrom.Skill * 10000 + skillData.SkillId;

                    if (skillData.SkillConfig.Role == (int)RoleType.Warrior)
                    {
                        AttributeBonus.SetAttr(AttributeEnum.WarriorSkillPercent, attrKey, skillPanel.Percent);
                        AttributeBonus.SetAttr(AttributeEnum.WarriorSkillDamage, attrKey, skillPanel.Damage);
                    }
                    else if (skillData.SkillConfig.Role == (int)RoleType.Mage)
                    {
                        AttributeBonus.SetAttr(AttributeEnum.MageSkillPercent, attrKey, skillPanel.Percent);
                        AttributeBonus.SetAttr(AttributeEnum.MageSkillDamage, attrKey, skillPanel.Damage);
                    }
                    else if (skillData.SkillConfig.Role == (int)RoleType.Warlock)
                    {
                        AttributeBonus.SetAttr(AttributeEnum.WarlockSkillPercent, attrKey, skillPanel.Percent);
                        AttributeBonus.SetAttr(AttributeEnum.WarlockSkillDamage, attrKey, skillPanel.Damage);
                    }
                }
            }
        }

        private void UpdateSkills()
        {
            foreach (var skillState in SelectSkillList)
            {
                skillUseCache[skillState.SkillPanel.SkillId] = skillState.lastUseRound;
            }

            var user = GameProcessor.Inst.User;

            this.SetSkill(user);

            foreach (var skillState in SelectSkillList)
            {
                skillUseCache.TryGetValue(skillState.SkillPanel.SkillId, out int lastUseRound);
                if (lastUseRound > 0)
                {
                    skillState.SetLastUseRound(lastUseRound);
                }
            }
        }

        public override APlayer CalcEnemy()
        {
            var ret = base.CalcEnemy();

            ret?.EventCenter.Raise(new ShowAttackIcon { NeedShow = true });

            return ret;
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Resurrection()
        {
            this.Logic.ResetData();
            this._enemy = null;
        }

        public void UpdateEnemy(APlayer player)
        {
            this._enemy = player;
        }
    }
}
