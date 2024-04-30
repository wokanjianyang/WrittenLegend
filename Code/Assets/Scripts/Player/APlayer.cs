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

        public string Title { get; set; }

        public long Level { get; set; }
        public double HP { get; set; }
        public int Quality { get; set; }

        public bool IsHide { get; set; } = false;

        public long BirthDay { get; set; } = 0;

        public float MoveSpeed { get; private set; } = 1;
        public float AttckSpeed { get; private set; } = 1;

        public List<SkillData> SkillList { get; set; } = new List<SkillData>();

        [JsonIgnore]
        public PlayerType Camp { get; set; }

        public MondelType ModelType { get; set; } = MondelType.Nomal;

        public RuleType RuleType = RuleType.Normal;

        public int RingType { get; set; } = 0;

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
        public void ChangeMaxHp(int fromId, double total)
        {
            double PreMaxHp = this.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP);
            //Debug.Log("PreMaxHp:" + PreMaxHp);
            double rate = this.HP * 1f / PreMaxHp;
            //Debug.Log("rate:" + rate);
            //Debug.Log("effect maxHp Rate:" + total);

            this.AttributeBonus.SetAttr(AttributeEnum.PanelHp, fromId, total);

            double CurrentMaxHp = this.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
            //Debug.Log("CurrentMaxHp:" + CurrentMaxHp);
            double currentHp = CurrentMaxHp * rate;
            //Debug.Log("effect MaxHp:" + StringHelper.FormatNumber(currentHp));
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
            rect.localScale = UnityEngine.Vector3.one;
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

        public void SetAttackSpeed(int SpeedPercent)
        {
            this.AttckSpeed = Mathf.Max(0.2f, 100f / (100 + SpeedPercent));
        }
        public void SetMoveSpeed(int SpeedPercent)
        {
            this.MoveSpeed = Mathf.Max(0.2f, 100f / (100 + SpeedPercent));
        }

        public double GetRoleAttack(int role, bool haveBuff)
        {
            return DamageHelper.GetRoleAttack(this.AttributeBonus, role, haveBuff);
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

            long now = TimeHelper.ClientNowSeconds();

            foreach (SkillState state in list)
            {
                if (state.IsCanUse(now))
                {
                    state.UserCount = state.UserCount + 1;
                    return state;
                }
            }

            if (priority == 0)
            {
                SkillState normal = SelectSkillList.FirstOrDefault(m => m.SkillPanel.SkillId == 9001);
                if (normal != null && normal.IsCanUse(now))
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

        public void DoEffect(float time)
        {
            if (!this.IsSurvice) return;

            //光环
            //if (this.Camp == PlayerType.Hero)
            //{
            //    //Debug.Log("Hero Def:" + this.AttributeBonus.GetTotalAttr(AttributeEnum.Def));
            //    //Debug.Log("Hero PhyDamage:" + this.AttributeBonus.GetAttackAttr(AttributeEnum.PhyDamage));

            //    if (this.AurasList != null)
            //    {
            //        foreach (AAuras auras in this.AurasList)
            //        {
            //            auras.Do();
            //        }
            //    }
            //}

            //计算buff
            foreach (List<Effect> list in EffectMap.Values)
            {
                foreach (Effect effect in list)
                {
                    effect.Do(time);
                }
                list.RemoveAll(m => !m.Active);//移除已结束的
            }
        }

        public void AutoRestore()
        {
            //回血
            double restoreHp = AttributeBonus.GetAttackAttr(AttributeEnum.RestoreHp) +
                 AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP) / 100.0 * AttributeBonus.GetAttackAttr(AttributeEnum.RestoreHpPercent);
            if (restoreHp > 0)
            {
                this.OnRestore(this.ID, restoreHp);
            }
        }

        public virtual float DoEvent()
        {
            this.RoundCounter++;

            if (!this.IsSurvice) return 1f;

            //1.控制前计算高优级技能
            float attackIgnorePause = AttackIgnorePause();
            if (attackIgnorePause > 0)
            {
                return attackIgnorePause;
            }

            //2.判断控制
            if (GetIsPause())
            {
                return Math.Min(AttckSpeed, MoveSpeed);
            }

            //3.普通技能
            return AttackLogic();
        }

        public virtual float AttackIgnorePause()
        {
            SkillState skill = this.GetSkill(200);
            if (skill != null)
            {
                //Debug.Log("Player Use Prioriry Skill:" + skill.SkillPanel.SkillData.SkillConfig.Name);
                skill.Do();
                return AttckSpeed;
            }

            return 0f;
        }

        public virtual float AttackLogic()
        {
            //1. 优先攻击首要目标
            this.CalcEnemy();
            SkillState skill;
            if (_enemy != null)
            {
                skill = this.GetSkill(0);
                if (skill != null)
                {
                    skill.Do();
                    return AttckSpeed;
                }
            }

            //2. 攻击最近目标
            _enemy = this.FindNearestEnemy();
            if (_enemy != null)
            {
                skill = this.GetSkill(0);
                if (skill != null)
                {
                    skill.Do();
                    return AttckSpeed;
                }
            }
            else
            {
                return AttckSpeed;
            }

            //3. 移动到首要目标
            return MoveToEnemy();
        }

        private float MoveToEnemy()
        {
            var enemys = FindNearestEnemys();

            for (int i = 0; i < enemys.Count; i++)
            {
                var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, enemys[i].Cell);
                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                {
                    this.Move(endPos);
                    return MoveSpeed;
                }
            }
            return 1f;
        }

        public void RunEffect(APlayer attchPlayer, EffectData effectData, double damage, long rolePercent)
        {
            Effect effect = new Effect(this, effectData, damage, rolePercent, 0);
            effect.Do(1f);
        }
        public void AddEffect(APlayer attchPlayer, EffectData effectData, double damage, long rolePercent)
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
                    //effectData.Layer = list[i].Data.Layer; //使用旧的FromId

                    list[i].Clear();
                }
                list.RemoveRange(0, RemoveCount);
            }

            int layer = 0;
            if (list.Count > 0)
            {
                layer = (list[list.Count - 1].Layer + 1) % effectData.Max; //每叠加一层，FromId+1
            }

            Effect effect = new Effect(this, effectData, damage, rolePercent, layer);
            list.Add(effect);

            // 立即运行类型，立即使用
            if (effect.Data.Config.RunType == 0)
            {
                effect.Do(1f);
            }
        }

        public void Move(Vector3Int cell)
        {
            this.SetPosition(cell);
            var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
            this.Transform.DOKill(true);
            this.Transform.DOLocalMove(targetPos, MoveSpeed);
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

        public void OnRestore(int fromId, double hp)
        {
            if (this.RuleType == RuleType.Infinite && this.Camp == PlayerType.Enemy) //无尽的怪不能回血
            {
                return;
            }

            this.Logic.OnRestore(hp);
        }

        public void SetHP(double hp)
        {
            this.HP = hp;

        }

        public void ShowMiss()
        {
            this.EventCenter.Raise(new ShowMsgEvent()
            {
                Type = MsgType.Miss,
                Content = "MISS"
            });
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
