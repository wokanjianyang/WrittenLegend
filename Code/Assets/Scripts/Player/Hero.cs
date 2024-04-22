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
        private Dictionary<int, long> skillUseCache = new Dictionary<int, long>();

        public List<SkillState> DoubleHitSkillList { get; set; } = new List<SkillState>();

        public Hero(RuleType ruleType) : base()
        {
            this.GroupId = 1;
            this.RuleType = ruleType;

            this.Init();

            this.EventCenter.AddListener<HeroLevelUp>(LevelUp);
            this.EventCenter.AddListener<HeroAttrChangeEvent>(HeroAttrChange);
            this.EventCenter.AddListener<HeroBuffChangeEvent>(OnHeroBuffChange);

            User user = GameProcessor.Inst.User;
            user.EventCenter.AddListener<HeroUpdateSkillEvent>(OnHeroUpdateAllSkillEvent);

        }

        private void LevelUp(HeroLevelUp e)
        {
            User user = GameProcessor.Inst.User;
            this.Level = user.MagicLevel.Data;

            this.SetAttr(user);  //设置属性值
            this.Logic.SetData(null); //设置UI
        }

        public void HeroAttrChange(HeroAttrChangeEvent e)
        {
            User user = GameProcessor.Inst.User;
            this.SetAttr(user);  //设置属性值
        }

        private void OnHeroUpdateAllSkillEvent(HeroUpdateSkillEvent e)
        {
            this.UpdateSkills();
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;
            this.Camp = PlayerType.Hero;
            this.Name = user.Name;
            this.Level = user.MagicLevel.Data;

            this.SetAttr(user);  //设置属性值
            this.SetSkill(user); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr(User user)
        {
            this.AttributeBonus = new AttributeBonus();

            //计算Buff
            if (RuleType == RuleType.Defend)
            {
                List<DefendBuffConfig> buffList = user.DefendData.GetBuffList(DefendBuffType.Attr);

                this.AttributeBonus.SetBuffList(buffList);
            }

            //把用户面板属性，当做战斗的基本属性

            if (RuleType == RuleType.HeroPhantom)
            {
                AttributeBonus.SetAttr(AttributeEnum.MulHp, AttributeFrom.HeroPanel, ConfigHelper.PvpRate * 100);
            }

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyAtt));
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicAtt));
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritAtt));
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Def));
            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Speed));
            AttributeBonus.SetAttr(AttributeEnum.MoveSpeed, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MoveSpeed));
            AttributeBonus.SetAttr(AttributeEnum.Lucky, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Lucky));
            AttributeBonus.SetAttr(AttributeEnum.CritRate, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRate));
            AttributeBonus.SetAttr(AttributeEnum.CritDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritDamage));
            AttributeBonus.SetAttr(AttributeEnum.CritRateResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritRateResist));
            AttributeBonus.SetAttr(AttributeEnum.CritDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.CritDamageResist));
            AttributeBonus.SetAttr(AttributeEnum.DamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.DamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.DamageResist));
            AttributeBonus.SetAttr(AttributeEnum.InheritIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.InheritIncrea));
            AttributeBonus.SetAttr(AttributeEnum.RestoreHp, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RestoreHp));
            AttributeBonus.SetAttr(AttributeEnum.RestoreHpPercent, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.RestoreHpPercent));
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Miss));
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Accuracy));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.AurasDamageResist));

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyDamage));
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicDamage));
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritDamage));

            AttributeBonus.SetAttr(AttributeEnum.MulDamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.MulDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MulDamageResist));

            AttributeBonus.SetAttr(AttributeEnum.SkillValetCount, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SkillValetCount));
            AttributeBonus.SetAttr(AttributeEnum.SkillValetSpeed, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SkillValetSpeed));

            //this.AurasList = new List<AAuras>();
            //foreach (var ac in user.GetAurasList())
            //{
            //    AAuras auras = AurasFactory.BuildAuras(this, ac.Key, ac.Value);
            //    this.AurasList.Add(auras);
            //}

            if (user.SoulRingData.Count > 0)
            {
                this.RingType = 1;
            }

            //回满当前血量
            this.SetAttackSpeed((int)AttributeBonus.GetTotalAttrDouble(AttributeEnum.Speed));
            this.SetMoveSpeed((int)AttributeBonus.GetTotalAttrDouble(AttributeEnum.MoveSpeed));

            double maxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
            SetHP(maxHP);
            //Debug.Log("Hero Hp:" + StringHelper.FormatNumber(maxHP));
        }

        private void OnHeroBuffChange(HeroBuffChangeEvent e)
        {
            //计算Buff
            if (RuleType == RuleType.Defend)
            {
                List<DefendBuffConfig> buffList = GameProcessor.Inst.User.DefendData.GetBuffList(DefendBuffType.Attr);
                this.AttributeBonus.SetBuffList(buffList);

                double maxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
                SetHP(maxHP);
                //Debug.Log("Hero Hp:" + StringHelper.FormatNumber(maxHP));
            }
        }

        private void SetSkill(User user)
        {
            SelectSkillList = new List<SkillState>();

            List<SkillData> list = user.GetCurrentSkill();
            list.Add(new SkillData(9001, (int)SkillPosition.Default));

            if (RuleType == RuleType.Defend)
            {
                List<int> ids = user.GetCurrentSkillList();

                List<DefendBuffConfig> buffList = user.DefendData.GetBuffList(DefendBuffType.Skill);
                foreach (DefendBuffConfig config in buffList)
                {
                    if (!ids.Contains(config.SkillId))
                    {
                        var sd = new SkillData(config.SkillId, (int)SkillPosition.Default);
                        sd.MagicLevel.Data = 500;
                        list.Add(sd);
                    }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                SkillData skillData = list[i];

                List<SkillRuneConfig> buffRuneList = null;
                if (RuleType == RuleType.Defend)
                {
                    buffRuneList = user.DefendData.GetBuffRuneList(skillData.SkillId);
                }

                List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, buffRuneList);

                List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, true);

                SkillState skill = new SkillState(this, skillPanel, i, 0);
                SelectSkillList.Add(skill);

                //职业专精技能的属性
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
                else if (skillData.SkillId == 3010)
                {
                    AttributeBonus.SetAttr(AttributeEnum.InheritAdvance, AttributeFrom.Skill, skillPanel.Percent);
                    AttributeBonus.SetAttr(AttributeEnum.SkillValetHp, AttributeFrom.Skill, skillPanel.Damage);
                }
            }

            InitDoubleHitSkill(user);
        }

        private void InitDoubleHitSkill(User user)
        {
            DoubleHitSkillList.Clear();

            foreach (var kv in user.ExclusivePanelList[user.ExclusiveIndex])
            {
                ExclusiveItem exclusive = kv.Value;

                if (exclusive.DoubleHitId > 0)
                {
                    int skillId = exclusive.DoubleHitConfig.SkillId;

                    SkillData skillData = user.SkillList.Where(m => m.SkillId == skillId).FirstOrDefault();

                    if (skillData == null)
                    {
                        break;
                    }

                    List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, null);
                    List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                    SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, true);

                    SkillState skill = DoubleHitSkillList.Where(m => m.SkillPanel.SkillId == skillId).FirstOrDefault();

                    if (skill == null)
                    {
                        skill = new SkillState(this, skillPanel, skillData.Position, 0);
                        DoubleHitSkillList.Add(skill);
                    }
                    skill.AddRate(exclusive.DoubleHitConfig.Rate);
                }
            }
        }

        private void UpdateSkills()
        {
            foreach (var skillState in SelectSkillList)
            {
                skillUseCache[skillState.SkillPanel.SkillId] = skillState.LastUseTime;
            }

            var user = GameProcessor.Inst.User;

            this.SetSkill(user);

            foreach (var skillState in SelectSkillList)
            {
                skillUseCache.TryGetValue(skillState.SkillPanel.SkillId, out long LastUseTime);
                if (LastUseTime > 0)
                {
                    skillState.SetLastUseTime(LastUseTime);
                }
            }
        }

        public override float AttackLogic()
        {
            //Debug.Log("生命:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP));
            //Debug.Log("减伤:"+AttributeBonus.GetAttackAttr(AttributeEnum.DamageResist)+" 增伤:"+ AttributeBonus.GetAttackAttr(AttributeEnum.DamageIncrea));

            //Debug.Log("瞬移魔法伤害:" + AttributeBonus.GetAttackAttr(AttributeEnum.MagicDamage));

            //1. 控制前计算高优级技能
            SkillState skill;

            //3.寻找目标
            _enemy = this.CalcEnemy();

            //4 尝试攻击优先目标
            skill = this.GetSkill(0);
            if (skill != null)
            {  //使用技能
                //Debug.Log($"{(this.Name)}使用技能:{(skill.SkillPanel.SkillData.SkillConfig.Name)},攻击:" + targets.Count + "个");
                skill.Do();
                //this.EventCenter.Raise(new ShowAttackIcon ());

                if (skill.SkillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
                {
                    this.DoubleHit();
                }

                return AttckSpeed;
            }

            //5.朝目标移动
            if (_enemy != null)
            {
                var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                {
                    this.Move(endPos);
                    return MoveSpeed;
                }
            }

            //6 尝试更改攻击目标
            if (_enemy != null)
            {
                _enemy.EventCenter.Raise(new ShowAttackIcon { NeedShow = false });
            }
            _enemy = this.FindNearestEnemy();
            if (_enemy != null)
            {
                //如果有新目标
                skill = this.GetSkill(0);

                //6.1 先攻击新目标
                if (skill != null)
                {
                    skill.Do();
                    if (skill.SkillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
                    {
                        this.DoubleHit();
                    }
                    return AttckSpeed;
                }
                else
                {
                    //攻击不到，则移动过去
                    var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
                    if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                    {
                        this.Move(endPos);
                        return MoveSpeed;
                    }
                }
            }

            return AttckSpeed;
        }

        private void DoubleHit()
        {
            foreach (SkillState skill in this.DoubleHitSkillList)
            {
                if (RandomHelper.RandomRate(skill.Rate))
                {
                    skill.Do();
                    //Debug.Log(" Double Hit " + skill.SkillPanel.SkillData.SkillConfig.Name);
                    return;
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
        /// 复活
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
