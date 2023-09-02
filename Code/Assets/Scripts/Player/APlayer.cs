using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using SDD.Events;
using Newtonsoft.Json;
using System.Linq;

namespace Game
{
    abstract public class APlayer
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public long Level { get; set; }
        public long HP { get; set; }
        public int Quality { get; set; }

        public List<SkillData> SkillList { get; set; } = new List<SkillData>();

        [JsonIgnore]
        public PlayerType Camp { get; set; }

        public int RingType { get; set; } = 0;

        [JsonIgnore]
        public MondelType ModelType { get; set; } = MondelType.Nomal;

        [JsonIgnore]
        public Vector3Int Cell { get; set; }

        [JsonIgnore]
        public AttributeBonus AttributeBonus { get; set; }

        [JsonIgnore]
        public Transform Transform { get; private set; }

        [JsonIgnore]
        public Logic Logic { get; private set; }

        [JsonIgnore]
        public int RoundCounter { get; set; }

        [JsonIgnore]
        public EventManager EventCenter { get; private set; }

        [JsonIgnore]
        public bool IsSurvice
        {
            get
            {
                return this.Logic.IsSurvice && this.HP > 0;
            }
        }
        [JsonIgnore]
        public List<SkillState> SelectSkillList { get; set; }

        [JsonIgnore]
        protected Dictionary<int, List<Effect>> EffectMap = new Dictionary<int, List<Effect>>();

        [JsonIgnore]
        private Dictionary<int, int> SkillUseRoundCache = new Dictionary<int, int>();
        public void ChangeMaxHp(int fromId, long total)
        {
            long MaxHp = this.AttributeBonus.GetAttackAttr(AttributeEnum.HP);

            double rate = this.HP * 1f / MaxHp;

            long currentHp = (long)(total * rate);

            this.AttributeBonus.SetAttr(AttributeEnum.PanelHp, fromId, total);
            this.HP += currentHp;

            this.EventCenter.Raise(new SetPlayerHPEvent { });
        }

        [JsonIgnore]
        public int GroupId { get; set; }

        [JsonIgnore]
        public APlayer Enemy
        {
            get
            {
                if (_enemy != null && _enemy.IsSurvice)
                {
                    return _enemy;
                }

                _enemy = null;

                return _enemy;
            }
        }
        protected APlayer _enemy { get; set; }

        [JsonIgnore]
        public string UUID { get; set; }

        public List<AAuras> AurasList = null;

        public APlayer()
        {
            this.UUID = System.Guid.NewGuid().ToString("N");
            this.EventCenter = new EventManager();
            this.AttributeBonus = new AttributeBonus();
            this.SkillUseRoundCache = new Dictionary<int, int>();
            this.SelectSkillList = new List<SkillState>();

            //this.Load();
        }

        [JsonIgnore]
        public int UseSkillPosition { get; set; } = 0;

        virtual public void Load()
        {
            var prefab = Resources.Load<GameObject>("Prefab/Char/Model");
            this.Transform = GameObject.Instantiate(prefab).transform;
            this.Transform.SetParent(GameProcessor.Inst.PlayerRoot);
            var rect = this.Transform.GetComponent<RectTransform>();
            rect.sizeDelta = GameProcessor.Inst.MapData.CellSize;
            rect.localScale = Vector3.one;
            this.Logic = this.Transform.GetComponent<Logic>();
            var coms = this.Transform.GetComponents<MonoBehaviour>();
            foreach (var com in coms)
            {
                if (com is IPlayer _com)
                {
                    _com.SetParent(this);
                }
            }

            //加载技能
            //LoadSkill();
        }

        internal SkillPanel GetOfflineSkill()
        {
            List<SkillPanel> list = SelectSkillList.Where(m => m.SkillPanel.CD == 0).Select(m => m.SkillPanel).OrderBy(m => m.Percent).ToList();
            return list.Count > 0 ? list[list.Count - 1] : new SkillPanel(new SkillData(9001, (int)SkillPosition.Default), new List<SkillRune>(), new List<SkillSuit>());
        }


        public virtual long GetRoleAttack(int role)
        {
            return DamageHelper.GetRoleAttack(this.AttributeBonus, role);
        }

        public long GetRolePercent(int role)
        {
            return DamageHelper.GetRolePercent(this.AttributeBonus, role);
        }

        public long GetRoleDamage(int role)
        {
            return DamageHelper.GetRoleDamage(this.AttributeBonus, role);
        }

        virtual public SkillState GetSkill()
        {
            List<SkillState> list = SelectSkillList.OrderBy(m => m.UserCount * 1000 + m.Priority).ToList();

            foreach (SkillState state in list)
            {
                if (state.IsCanUse())
                {
                    state.UserCount = state.UserCount + 1;
                    return state;
                }
            }

            var flag = this.IsEnemyClosest(Enemy);
            //当敌人在邻格时，强制使用普攻
            if (flag)
            {
                return SelectSkillList.FirstOrDefault(s => s.SkillPanel.SkillData.SkillId == 9001);
            }
            return null;
        }

        public virtual void DoEvent()
        {
            this.RoundCounter++;

            if (!this.IsSurvice) return;

            //光环
            if (this.Camp == PlayerType.Hero)
            {
                //
                //Debug.Log("Hero Def:" + this.AttributeBonus.GetTotalAttr(AttributeEnum.Def));
                Debug.Log("Hero Hp:" + this.AttributeBonus.GetAttackAttr(AttributeEnum.HP));

                if (this.AurasList != null)
                {
                    foreach (AAuras auras in this.AurasList)
                    {
                        auras.Do();
                    }
                }
            }

            //行动前计算buff
            bool pause = false;
            foreach (List<Effect> list in EffectMap.Values)
            {
                foreach (Effect effect in list)
                {
                    effect.Do();
                    if (effect.Data.Config.Type == 2)
                    {
                        pause = true;

                        this.EventCenter.Raise(new ShowMsgEvent
                        {
                            Type = MsgType.Other,
                            Content = effect.Data.Config.Name
                        });
                    }
                }
                list.RemoveAll(m => m.Duration <= m.DoCount);//移除已结束的
            }

            //回血
            long restoreHp = AttributeBonus.GetAttackAttr(AttributeEnum.RestoreHp) +
                AttributeBonus.GetAttackAttr(AttributeEnum.RestoreHpPercent) * AttributeBonus.GetAttackAttr(AttributeEnum.HP) / 100;
            if (restoreHp > 0)
            {
                this.OnRestore(this.ID, restoreHp);
            }

            if (pause)
            {
                return; //如果有控制技能，不继续后续行动
            }

            //1.尝试攻击手选目标或上回合目标
            _enemy = this.CalcEnemy();
            SkillState skill = this.GetSkill();

            if (skill != null)
            {  //使用技能
                //Debug.Log($"{(this.Name)}使用技能:{(skill.SkillPanel.SkillData.SkillConfig.Name)},攻击:" + targets.Count + "个");
                skill.Do();
                //this.EventCenter.Raise(new ShowAttackIcon ());
                return;
            }

            //2.尝试攻击周围目标
            if (_enemy != null)
            {

                //如果有锁定目标，需要在攻击后恢复
                var oldEnemy = Enemy;

                _enemy = this.FindNearestEnemy();
                if (Enemy.ID != oldEnemy.ID)
                {

                    skill = this.GetSkill();

                    if (skill != null)
                    {
                        skill.Do();
                        _enemy = oldEnemy;
                        //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "无法攻击指定目标，尝试攻击其它目标",ToastType = ToastTypeEnum.Normal});
                        return;
                    }
                }
            }
            else
            {
                _enemy = this.FindNearestEnemy();
                skill = this.GetSkill();

                if (skill != null)
                {
                    skill.Do();
                    return;
                }
            }

            
            //this.EventCenter.Raise(new ShowAttackIcon { NeedShow = false });
            //3.没有目标时什么都不做
            //移动
            if (_enemy == null)
            {
                return;
            }

            //4.如果周围有目标，但没有可攻击的技能，也什么都不做
            if (IsEnemyClosest(_enemy))
            {
                return;
            }
            //5.朝目标移动
            var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, _enemy.Cell);
            if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
            {
                this.Move(endPos);
            }
        }

        protected bool IsEnemyClosest(APlayer enemy)
        {
            if (enemy == null)
            {
                return false;
            }
            var up = this.Cell + Vector3Int.up;
            if (up == enemy.Cell)
            {
                return true;
            }
            var left = this.Cell + Vector3Int.left;
            if (left == enemy.Cell)
            {
                return true;

            }
            var right = this.Cell + Vector3Int.right;
            if (right == enemy.Cell)
            {
                return true;

            }
            var down = this.Cell + Vector3Int.down;
            if (down == enemy.Cell)
            {
                return true;

            }

            return false;
        }
        public void RunEffect(APlayer attchPlayer, EffectData effectData, long total)
        {
            Effect effect = new Effect(this, effectData, total);
            effect.Do();
        }
        public void AddEffect(APlayer attchPlayer, EffectData effectData, long total)
        {
            if (!EffectMap.TryGetValue(effectData.FromId, out List<Effect> list))
            {
                list = new List<Effect>();
                EffectMap[effectData.FromId] = list;
            }

            if (list.Count > 0 && list.Count >= effectData.Max)
            {
                //移除旧的
                list.RemoveRange(0, list.Count - Math.Max(0, effectData.Max - 1));
            }

            Effect effect = new Effect(this, effectData, total);
            list.Add(effect);
        }

        public void Move(Vector3Int cell)
        {
            //this.EventCenter.Raise(new ShowMsgEvent
            //{
            //    Content = "移动"
            //});
            this.SetPosition(cell);
            var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
            this.Transform.DOKill(true);
            this.Transform.DOLocalMove(targetPos, 1f);
        }

        public void SetPosition(Vector3 pos, bool isGraphic = false)
        {
            this.Cell = new Vector3Int((int)pos.x, (int)pos.y, 0);
            if (isGraphic)
            {
                this.Transform.localPosition = GameProcessor.Inst.MapData.GetWorldPosition(this.Cell);
            }
        }

        virtual public APlayer CalcEnemy()
        {
            return Enemy;
        }

        public APlayer FindNearestEnemy()
        {

            APlayer ret = null;

            //查找和自己不同类的,并且不是自己的主人/仆人
            var enemys = GameProcessor.Inst.PlayerManager.GetAllPlayers().FindAll(p => p.Camp != this.Camp && p.IsSurvice && p.GroupId != this.GroupId);

            if (enemys.Count > 0)
            {
                enemys.Sort((a, b) =>
                {
                    var distance = a.Cell - this.Cell;
                    var l0 = Math.Abs(distance.x) + Math.Abs(distance.y);

                    distance = b.Cell - this.Cell;
                    var l1 = Math.Abs(distance.x) + Math.Abs(distance.y);

                    if (l0 < l1)
                    {
                        return -1;
                    }
                    else if (l0 > l1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                });

                ret = enemys[0];
            }

            return ret;
        }

        public virtual void OnHit(DamageResult dr)
        {
            this.Logic.OnDamage(dr);
        }

        public void OnRestore(int fromId, long hp)
        {
            this.Logic.OnRestore(hp);
        }

        public void SetHP(long hp)
        {
            this.HP = hp;

        }

        public T GetComponent<T>()
        {
            return this.Transform.GetComponent<T>();
        }

        public void OnDestroy()
        {
            //foreach (var skill in this.SelectSkillList)
            //{
            //    skill.Destory();
            //}
            //SelectSkillList.Clear();

            this.EventCenter.RemoveAllListeners();
            if (this.Transform != null)
            {
                GameObject.Destroy(this.Transform.gameObject);
            }
        }
    }
}
