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

            this.Scale = scale;

            this.Init();
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;
            this.Camp = PlayerType.Enemy;

            int configId = GameProcessor.Inst.User.HeroPhatomData.Current.ConfigId;

            if (configId > 0)
            {
                HeroPhatomConfig heroPhatomConfig = HeroPhatomConfigCategory.Instance.Get(configId);
                this.Name = heroPhatomConfig.Name;
            }
            else
            {
                this.Name = "����-" + user.Name;
            }

            this.Level = user.MagicLevel.Data;

            this.SetAttr(user);  //��������ֵ
            this.SetSkill(user); //���ü���

            base.Load();
            this.Logic.SetData(null); //����UI
        }

        private void SetAttr(User user)
        {
            this.AttributeBonus = new AttributeBonus();

            //���û�������ԣ�����ս���Ļ�������

            double phRate = 0.4 + 0.1 * Scale;

            this.SetAttackSpeed((int)user.AttributeBonus.GetTotalAttr(AttributeEnum.Speed));
            this.SetMoveSpeed((int)user.AttributeBonus.GetTotalAttr(AttributeEnum.MoveSpeed));

            double attr = Math.Max(user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.PhyAtt), user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.MagicAtt));
            attr = Math.Max(attr, user.AttributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritAtt));

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

            AttributeBonus.SetAttr(AttributeEnum.AurasDamageIncrea, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageIncrea));
            AttributeBonus.SetAttr(AttributeEnum.AurasDamageResist, AttributeFrom.HeroPanel, user.AttributeBonus.GetTotalAttr(AttributeEnum.AurasDamageResist));

            this.AurasList = new List<AAuras>();
            foreach (var ac in user.GetAurasList())
            {
                AAuras auras = AurasFactory.BuildAuras(this, ac);
                this.AurasList.Add(auras);
            }

            if (user.SoulRingData.Count > 0)
            {
                this.RingType = 1;
            }

            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP));
        }



        private List<SkillRune> RandomSkillRuneList(int skillId)
        {
            List<SkillRune> runeList = new List<SkillRune>();

            List<SkillRuneConfig> runeConfigs = SkillRuneConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SkillId == skillId).ToList();

            if (runeConfigs.Count > 0)
            {
                SkillRune skillRune = new SkillRune(runeConfigs[0].Id, 4);
                runeList.Add(skillRune);
            }

            return runeList;
        }

        private List<SkillSuit> RandomSkillSuitList(int skillId)
        {
            List<SkillSuit> suitList = new List<SkillSuit>();

            List<SkillSuitConfig> suitConfigs = SkillSuitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SkillId == skillId).ToList();
            if (suitConfigs.Count > 0)
            {
                SkillSuit suit = new SkillSuit(suitConfigs[0].Id);
                suitList.Add(suit);
            }

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
                skillData.MagicLevel.Data = skillData.SkillConfig.MaxLevel / 10 * Scale;
                list.Add(skillData);
            }
            list.Add(new SkillData(9001, (int)SkillPosition.Default)); //����Ĭ�ϼ���

            foreach (SkillData skillData in list)
            {

                List<SkillRune> runeList = RandomSkillRuneList(skillData.SkillId);
                List<SkillSuit> suitList = RandomSkillSuitList(skillData.SkillId);

                SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList, true);

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

            InitDoubleHitSkill(user);
        }

        private void InitDoubleHitSkill(User user)
        {
            DoubleHitSkillList.Clear();

            foreach (var kv in user.ExclusiveList)
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

                    List<SkillRune> runeList = user.GetRuneList(skillData.SkillId);
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
            //Debug.Log("����:" + AttributeBonus.GetAttackAttr(AttributeEnum.HP));
            //Debug.Log("����:"+AttributeBonus.GetAttackAttr(AttributeEnum.DamageResist)+" ����:"+ AttributeBonus.GetAttackAttr(AttributeEnum.DamageIncrea));

            //Debug.Log("˲��ħ���˺�:" + AttributeBonus.GetAttackAttr(AttributeEnum.MagicDamage));

            //0 ���ȹ���Ӣ��
            if (_enemy == null)
            {
                _enemy = GameProcessor.Inst.PlayerManager.GetHero();
            }

            //1. ����ǰ������ż�����
            SkillState skill;

            //3.Ѱ��Ŀ��
            _enemy = this.CalcEnemy();

            //4 ���Թ�������Ŀ��
            skill = this.GetSkill(0);
            if (skill != null)
            {  //ʹ�ü���
                //Debug.Log($"{(this.Name)}ʹ�ü���:{(skill.SkillPanel.SkillData.SkillConfig.Name)},����:" + targets.Count + "��");
                skill.Do();
                //this.EventCenter.Raise(new ShowAttackIcon ());

                if (skill.SkillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
                {
                    this.DoubleHit();
                }

                return AttckSpeed;
            }

            //5.��Ŀ���ƶ�
            if (_enemy != null)
            {
                var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                {
                    this.Move(endPos);
                    return MoveSpeed;
                }
            }

            //6 ���Ը��Ĺ���Ŀ��
            if (_enemy != null)
            {
                _enemy.EventCenter.Raise(new ShowAttackIcon { NeedShow = false });
            }
            _enemy = this.FindNearestEnemy();
            if (_enemy != null)
            {
                //�������Ŀ��
                skill = this.GetSkill(0);

                //6.1 �ȹ�����Ŀ��
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
                    //�������������ƶ���ȥ
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