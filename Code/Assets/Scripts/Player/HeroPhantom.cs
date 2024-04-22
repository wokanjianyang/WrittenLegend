using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Game
{
    public class HeroPhantom : APlayer
    {
        private Dictionary<int, long> skillUseCache = new Dictionary<int, long>();

        public List<SkillState> DoubleHitSkillList { get; set; } = new List<SkillState>();

        public int Scale = 1;

        public HeroPhantom(int scale) : base()
        {
            this.GroupId = 2;
            this.RuleType = RuleType.HeroPhantom;

            this.Scale = scale;

            this.Init();
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;
            this.Camp = PlayerType.HeroPhatom;

            int configId = GameProcessor.Inst.User.HeroPhatomData.Current.ConfigId;

            if (configId > 0)
            {
                HeroPhatomConfig heroPhatomConfig = HeroPhatomConfigCategory.Instance.Get(configId);
                this.Name = heroPhatomConfig.Name;
            }
            else
            {
                this.Name = "镜像-" + user.Name;
            }

            this.Level = user.MagicLevel.Data;

            this.SetAttr(user);  //设置属性值
            this.SetSkill(user); //设置技能

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr(User user)
        {
            this.AttributeBonus = new AttributeBonus();

            //把用户面板属性，当做战斗的基本属性

            double phRate = 0;
            if (Scale <= 5)
            {
                phRate = 0.6 + 0.1 * (Scale - 1);
            }
            else
            {
                phRate = 1.0 + 0.1 * (Scale - 5);
            }

            //this.SetAttackSpeed((int)user.AttributeBonus.GetTotalAttr(AttributeEnum.Speed));
            //this.SetMoveSpeed((int)user.AttributeBonus.GetTotalAttr(AttributeEnum.MoveSpeed));

            double attr = Math.Max(user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyAtt), user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicAtt));
            attr = Math.Max(attr, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritAtt));

            double hp = user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP) * ConfigHelper.PvpRate * phRate;
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP) * ConfigHelper.PvpRate * phRate);

            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, attr * phRate);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, attr * phRate);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, attr * phRate);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.Def) * phRate);
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
            AttributeBonus.SetAttr(AttributeEnum.Miss, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.Miss));
            AttributeBonus.SetAttr(AttributeEnum.Accuracy, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.Accuracy));

            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageResist));

            AttributeBonus.SetAttr(AttributeEnum.PhyDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyDamage));
            AttributeBonus.SetAttr(AttributeEnum.MagicDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicDamage));
            AttributeBonus.SetAttr(AttributeEnum.SpiritDamage, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritDamage));

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
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }

        private List<SkillRune> RandomSkillRuneList(int skillId)
        {
            List<SkillRune> runeList = new List<SkillRune>();

            if (this.Scale <= 5)
            {
                return runeList;
            }

            runeList = SkillRuneHelper.GetAllRune(skillId, 4);

            return runeList;
        }

        private List<SkillSuit> RandomSkillSuitList(int skillId)
        {
            List<SkillSuit> suitList = new List<SkillSuit>();

            if (this.Scale <= 3)
            {
                return suitList;
            }

            suitList = SkillSuitHelper.GetAllSuit(skillId, 4);

            return suitList;
        }

        private void SetSkill(User user)
        {
            SelectSkillList = new List<SkillState>();

            List<int> skillIdList = GameProcessor.Inst.User.HeroPhatomData.Current.SkillIdList;

            //Debug.Log("Hero Phantom  " + JsonConvert.SerializeObject(skillIdList));

            List<SkillData> list = new List<SkillData>();
            for (int i = 0; i < skillIdList.Count; i++)
            {
                SkillData skillData = new SkillData(skillIdList[i], i);
                skillData.MagicLevel.Data = user.GetSkillLimit(skillData.SkillConfig) * Scale / 10;
                list.Add(skillData);
            }
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

            foreach (SkillData skillData in list)
            {

                List<SkillRune> runeList = RandomSkillRuneList(skillData.SkillId);
                List<SkillSuit> suitList = RandomSkillSuitList(skillData.SkillId);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, true);

                SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
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


        public override float AttackLogic()
        {
            //Debug.Log("生命:" + AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP));
            //Debug.Log("减伤:"+AttributeBonus.GetAttackAttr(AttributeEnum.DamageResist)+" 增伤:"+ AttributeBonus.GetAttackAttr(AttributeEnum.DamageIncrea));

            //Debug.Log("瞬移魔法伤害:" + AttributeBonus.GetAttackAttr(AttributeEnum.MagicDamage));

            //0 优先攻击英雄
            _enemy = GameProcessor.Inst.PlayerManager.GetHero();

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
    }
}
