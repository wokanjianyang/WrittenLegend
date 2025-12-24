using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class HeroSpirit : Hero
    {
        private int Scale = 0;

        public HeroSpirit() : base()
        {
            this.GroupId = 1;
            this.RuleType = RuleType.Myth;

            this.Init();
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;
            this.Camp = PlayerType.Hero;
            this.Name = user.Name;
            this.Level = user.MagicLevel.Data;
            this.FashionId = user.FashionUpId;

            //double power = user.AttributeBonus.GetPower();
            //double scale = Math.Log10(power) - 9;
            double scale = user.AttributeBonus.GetPowerNew().GetMythScale();
            this.Scale = (int)scale;

            this.SetAttr(user);  //设置属性值
            this.SetSkill(user); //设置技能

            double maxHP = AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
            SetHP(maxHP);

            base.Load();
            this.Logic.SetData(null); //设置UI
        }

        private void SetAttr(User user)
        {
            this.AttributeBonus = new AttributeBonus();

            //把用户面板属性，当做战斗的基本属性
            double spiritRate = 1 + user.AttributeBonus.GetTotalAttr(AttributeEnum.SpiritAll) / 100.0;

            double attr = 10000;
            double attrRate = (1 + Scale * 0.05)* spiritRate;

            //Debug.Log("myth scale Rate:" + attrRate);
            double attrRise = 1 + user.AttributeBonus.GetTotalAttr(AttributeEnum.MythAttr) / 100.0;
            double defRise = 1 + user.AttributeBonus.GetTotalAttr(AttributeEnum.MythDef) / 100.0;
            double hpRise = 1 + user.AttributeBonus.GetTotalAttr(AttributeEnum.MythHp) / 100.0;

            //Debug.Log("myth base:" + attr * attrRate);

            //Debug.Log("myth attrRise:" + attrRise);

            this.SetAttackSpeed((int)user.AttributeBonus.GetTotalAttr(AttributeEnum.Speed));
            this.SetMoveSpeed((int)user.AttributeBonus.GetTotalAttr(AttributeEnum.MoveSpeed));

            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroPanel, attr * attrRate * 150 * hpRise);

            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroPanel, attr * attrRate * attrRise);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroPanel, attr * attrRate * attrRise);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroPanel, attr * attrRate * attrRise);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroPanel, attr * attrRate * 10 * defRise);

            AttributeBonus.SetAttr(AttributeEnum.Speed, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Speed) / 2);

            //Debug.Log("myth PhyAtt:" + AttributeBonus.GetTotalAttrDouble(AttributeEnum.Def));

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }

        private void SetSkill(User user)
        {
            SelectSkillList = new List<SkillState>();

            List<SkillData> list = new List<SkillData>();
            foreach (KeyValuePair<int, Data.MagicData> sp in user.RingData)
            {
                long ringLevel = sp.Value.Data;
                RingConfig ringConfig = RingConfigCategory.Instance.Get(sp.Key);

                if (ringLevel >= ringConfig.RequireLevel && !user.RingSelect.ContainsKey(sp.Key))
                {
                    if (ringConfig.SkillId > 0)
                    {
                        SkillData sd = list.Where(m => m.SkillId == ringConfig.SkillId).FirstOrDefault();
                        if (sd == null)
                        {
                            sd = user.SkillList.Where(m => m.SkillId == ringConfig.SkillId).FirstOrDefault();
                            if (sd == null)  //没有学习，则默认为1级
                            {
                                sd = new SkillData(ringConfig.SkillId, 0);
                                sd.MagicLevel.Data = 1;
                            }
                            list.Add(sd); //没有上阵，则自动上阵
                        }
                    }
                }
            }

            List<int> rids = list.Select(m => m.SkillId).ToList();
            List<SkillData> userList = user.GetCurrentSkill(rids);
            list.AddRange(userList);

            //for (int i = 0; i < userList.Count; i++)
            //{
            //    SkillData skillData = new SkillData(userList[i].SkillId, userList[i].Position);
            //    skillData.MagicLevel.Data = Math.Max(1, userList[i].MagicLevel.Data / 10000);
            //    list.Add(skillData);
            //}

            list.Add(new SkillData(9001, (int)SkillPosition.Default));

            //Debug.Log("skill list:" + list.Select(m => m.SkillId).ToList().ListToString());

            for (int i = 0; i < list.Count; i++)
            {
                SkillData skillData = list[i];

                List<SkillRune> runeList = user.GetRuneList(skillData.SkillId, null);

                List<SkillSuit> suitList = user.GetSuitList(skillData.SkillId);

                int petRate = user.GetPetSkillRate(skillData.SkillConfig.Role);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, true, RuleType, petRate);

                SkillPanel from = null;
                if (skillPanel.SkillData.SkillConfig.FromId > 0)
                {
                    SkillData fromData = user.SkillList.Where(m => m.SkillId == skillPanel.SkillData.SkillConfig.FromId).FirstOrDefault();

                    if (fromData == null)
                    {
                        continue;
                    }

                    from = new SkillPanel(fromData, user.GetRuneList(fromData.SkillId, null), user.GetSuitList(fromData.SkillId), true, RuleType, petRate);
                }

                SkillState skill = new SkillState(this, skillPanel, from, i, 0);
                SelectSkillList.Add(skill);

                //Debug.Log(skillData.SkillConfig.Name + " Percent  :" + skillPanel.Percent);
                //Debug.Log(skillData.SkillConfig.Name + " Damage  :" + skillPanel.Damage);

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
                else if (skillData.SkillId == 1011)
                {
                    AttributeBonus.SetAttr(AttributeEnum.MulHp, AttributeFrom.Skill, skillPanel.Percent);
                    AttributeBonus.SetAttr(AttributeEnum.MulAttrPhy, AttributeFrom.Skill, skillPanel.Damage);
                }
                else if (skillData.SkillId == 2011)
                {
                    AttributeBonus.SetAttr(AttributeEnum.MulHp, AttributeFrom.Skill, skillPanel.Percent);
                    AttributeBonus.SetAttr(AttributeEnum.MulAttrMagic, AttributeFrom.Skill, skillPanel.Damage);
                }
                else if (skillData.SkillId == 3011)
                {
                    AttributeBonus.SetAttr(AttributeEnum.MulHp, AttributeFrom.Skill, skillPanel.Percent);
                    AttributeBonus.SetAttr(AttributeEnum.MulAttrSpirit, AttributeFrom.Skill, skillPanel.Damage);
                }
            }

            //InitDoubleHitSkill(user);

            base.SetSkillAfter();
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

            //1. 控制前计算高优级技能
            SkillState skill;

            //3.寻找目标
            _enemy = this.CalcEnemy();

            //4 尝试攻击优先目标
            skill = this.GetSkill(0);
            if (skill != null)
            {  //使用技能
                //Debug.Log($"{(this.Name)}使用技能:{(skill.SkillPanel.SkillData.SkillConfig.Name)}");
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
                    skill.Do(SkillRunType.Double);
                    //Debug.Log(" Double Hit " + skill.SkillPanel.SkillData.SkillConfig.Name);
                    return;
                }
            }
        }

        public override void OnHit(DamageResult dr)
        {
            //if (dr.Damage > 10000)
            //{
            //    Debug.Log("heor hit by skill " + dr.SkillId + " damage:" + StringHelper.FormatNumber(dr.Damage) + " maxHP:" + StringHelper.FormatNumber(this.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP)));
            //}

            base.OnHit(dr);
        }
    }
}
