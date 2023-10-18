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

        public bool IsHide { get; set; } = false;

        public float Speed { get; private set; } = 1;

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
            long PreMaxHp = this.AttributeBonus.GetAttackAttr(AttributeEnum.HP);
            //Debug.Log("PreMaxHp:" + PreMaxHp);
            double rate = this.HP * 1f / PreMaxHp;
            //Debug.Log("rate:" + rate);

            this.AttributeBonus.SetAttr(AttributeEnum.PanelHp, fromId, total);
            long CurrentMaxHp = this.AttributeBonus.GetAttackAttr(AttributeEnum.HP);
            //Debug.Log("CurrentMaxHp:" + CurrentMaxHp);
            long currentHp = (long)(CurrentMaxHp * rate);
            //Debug.Log("currentHp:" + currentHp);
            this.HP = currentHp;

            this.EventCenter.Raise(new SetPlayerHPEvent { });
        }

        [JsonIgnore]
        public int GroupId { get; set; }

        protected APlayer _enemy;
        public APlayer Enemy
        {
            get
            {
                //if (_enemy != null && _enemy.IsSurvice)
                //{
                //    return _enemy;
                //}

                //_enemy = null;

                return _enemy;
            }
        }

        virtual public APlayer CalcEnemy()
        {
            if (_enemy != null && (_enemy.IsHide || !_enemy.IsSurvice))
            {
                _enemy = null;
            }

            return _enemy;
        }

        public void ClearEnemy()
        {
            this._enemy = null;
        }

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

        public void SetSpeed(int SpeedPercent)
        {
            this.Speed = Mathf.Max(0.2f, 100f / (100 + SpeedPercent));
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


        virtual public SkillState GetSkill(int priority)
        {
            List<SkillState> list = SelectSkillList.Where(m => m.SkillPanel.SkillData.SkillConfig.Priority >= priority && m.SkillPanel.SkillId != 9001)
                .OrderBy(m => m.UserCount * 1000 + m.Priority).ToList();

            foreach (SkillState state in list)
            {
                if (state.IsCanUse())
                {
                    state.UserCount = state.UserCount + 1;
                    return state;
                }
            }

            if (priority == 0)
            {
                SkillState normal = SelectSkillList.FirstOrDefault(m => m.SkillPanel.SkillId == 9001);
                if (normal!=null && normal.IsCanUse())
                {
                    return normal;
                }
            }

            return null;
        }

        public bool GetIsPause()
        {
            foreach (List<Effect> list in EffectMap.Values)
            {
                int mc = list.Where(m => m.Data.Config.Type == (int)EffectType.IgnorePause).Count();
                if (mc > 0)
                {
                    return false;
                }

                int count = list.Where(m => m.Data.Config.Type == (int)EffectType.Pause).Count();
                if (count > 0)
                {
                    return true;
                }
            }
            return false;
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
                //Debug.Log("Hero PhyDamage:" + this.AttributeBonus.GetAttackAttr(AttributeEnum.PhyDamage));

                if (this.AurasList != null)
                {
                    foreach (AAuras auras in this.AurasList)
                    {
                        auras.Do();
                    }
                }
            }

            //行动前计算buff
            foreach (List<Effect> list in EffectMap.Values)
            {
                foreach (Effect effect in list)
                {
                    effect.Do();
                    if (effect.Data.Config.Type == (int)EffectType.Pause)
                    {
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

            AttackLogic();
        }

        public virtual void AttackLogic()
        {
            //1. 控制前计算高优级技能
            SkillState skill = this.GetSkill(200);
            if (skill != null)
            {
                //Debug.Log("Hero Use Prioriry Skill:");
                skill.Do();
                return;
            }

            //2.是否有控制技能，不继续后续行动
            bool pause = GetIsPause();
            if (pause)
            {
                //Debug.Log("Hero Pause:");
                return;
            }

            //3. 优先攻击首要目标
            this.CalcEnemy();
            if (_enemy != null) {
                skill = this.GetSkill(0);
                if (skill != null)
                {
                    skill.Do();
                    return;
                }
            }

            //4. 攻击最近目标
            _enemy = this.FindNearestEnemy();
            if (_enemy != null)
            {
                skill = this.GetSkill(0);
                if (skill != null)
                {
                    skill.Do();
                    return;
                }
            }
            else {
                return;
            }

            //5. 移动到首要目标
            MoveToEnemy();
        }

        private void MoveToEnemy()
        {
            var enemys = FindNearestEnemys();

            for (int i = 0; i < enemys.Count; i++)
            {
                var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, enemys[i].Cell);
                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                {
                    this.Move(endPos);
                    return;
                }
            }
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
                int RemoveCount = list.Count - Math.Max(0, effectData.Max - 1);

                for (int i = 0; i < RemoveCount; i++)
                {
                    effectData.Layer = list[i].Data.Layer; //使用旧的FromId

                    list[i].Clear();
                }
                list.RemoveRange(0, RemoveCount);
            }
            else
            {
                effectData.Layer = list.Count; //每叠加一层，FromId+1
            }

            Effect effect = new Effect(this, effectData, total);
            list.Add(effect);
            // 立即运行类型，立即使用
            if (effect.Data.Config.RunType == 0)
            {
                effect.Do();
            }
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

        public APlayer FindNearestEnemy()
        {

            APlayer ret = null;

            //查找和自己不同类的,并且不是自己的主人/仆人
            var enemys = GameProcessor.Inst.PlayerManager.GetAllPlayers().FindAll(p => p.IsSurvice && p.GroupId != this.GroupId && !p.IsHide);

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

        public List<APlayer> FindNearestEnemys()
        {
            //查找和自己不同类的,并且不是自己的主人/仆人
            var enemys = GameProcessor.Inst.PlayerManager.GetAllPlayers().FindAll(p => p.IsSurvice && p.GroupId != this.GroupId && !p.IsHide);

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
            }

            return enemys.GetRange(0, Math.Min(enemys.Count, 3));
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
