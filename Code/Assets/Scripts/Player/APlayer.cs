using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using SDD.Events;
using TMPro;
using Newtonsoft.Json;
using System.Linq;

namespace Game
{
    abstract public class APlayer
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }
        public long HP { get; set; }

        public IDictionary<int,int> SkillIdList { get; set; }

        [JsonIgnore]
        public PlayerType Camp { get; set; }

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
                return this.Logic.IsSurvice;
            }
        }
        [JsonIgnore]
        public List<SkillState> SelectSkillList { get; set; }

        private Dictionary<int, List<Effect>> EffectMap = new Dictionary<int, List<Effect>>();

        [JsonIgnore]
        public int GroupId { get; set; }

        public APlayer()
        {
            this.EventCenter = new EventManager();
            this.AttributeBonus = new AttributeBonus();
            this.SkillIdList = new Dictionary<int,int>();
            //this.Load();
        }


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
            LoadSkill();
        }

        public void LoadSkill()
        {
            SelectSkillList = new List<SkillState>();
            //加载已选择的技能
            foreach (int position in SkillIdList.Keys)
            {
                LoadSkill(position, SkillIdList[position]);
            }
            //默认增加普通攻击
            LoadSkill(9, 9001);
        }

        private void LoadSkill(int position, int skillId)
        {
            SkillConfig config = SkillConfigCategory.Instance.Get(skillId);

            SkillData data = new SkillData(skillId);
            data.SkillId = config.Id;
            data.Name = config.Name;
            data.CD = config.CD;
            data.Des = config.Des;
            data.Dis = config.Dis;
            data.Center = (SkillCenter)Enum.Parse(typeof(SkillCenter), config.Center);
            data.Area = (AttackGeometryType)Enum.Parse(typeof(AttackGeometryType), config.Area);
            data.EnemyMax = config.EnemyMax;
            data.Percent = config.Percent;
            data.Damage = config.Damage;
            data.Type = config.Type;
            data.Priority = config.Priority;

            SkillState skill = new SkillState(this, data, position);
            SelectSkillList.Add(skill);
        }

        public SkillState GetSkill()
        {
            List<SkillState> list = SelectSkillList.OrderBy(m => m.Priority).ToList();
            foreach (SkillState state in list)
            {
                if (state.IsCanUse())
                {
                    return state;
                }
            }
            return null;
        }

        public void DoEvent()
        {
            this.RoundCounter++;
            if (!this.IsSurvice) return;
            var up = this.Cell + Vector3Int.up;
            var down = this.Cell + Vector3Int.down;
            var right = this.Cell + Vector3Int.right;
            var left = this.Cell + Vector3Int.left;

            var fourSide = new List<Vector3Int>()
            {
                up, down, right, left
            };

            var nearestEnemy = this.FindNearestEnemy();

            //先判断是否有需要释放的技能
            //SkillProcessor skillProcessor = this.GetComponent<SkillProcessor>();

            SkillState skill = this.GetSkill();
            List<AttackData> targets = skill.GetAllTarget();

            if (targets.Count > 0)
            {  //使用技能
                Debug.Log($"使用技能:{(skill.Data.Name)}");
                skill.Do(targets);
                this.EventCenter.Raise(new ShowAttackIcon { NeedShow = true });
            }
            else
            {
                this.EventCenter.Raise(new ShowAttackIcon { NeedShow = false });

                //移动
                if (nearestEnemy == null)
                {
                    return;
                }
                var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, nearestEnemy.Cell);
                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                {
                    this.Move(endPos);
                }
            }

            //行动后计算buff
            foreach (List<Effect> list in EffectMap.Values) {
                foreach (Effect effect in list) {
                    //DO Effect
                    effect.Do();
                }
                list.RemoveAll(m => m.Duration <= 0);//移除已结束的
            }
        }

        public void AddEffect(int EffectId)
        {
            this.AddEffect(EffectId, 1);
        }

        public void RunEffect(int EffectId) {
            EffectConfig config = EffectConfigCategory.Instance.Get(EffectId);

            Effect effect = new Effect(this);

            effect.Level = config.Level;
            effect.CreateTime = DateTime.Now.Ticks;
            effect.Duration = config.Duration;
            effect.Config = config;

            effect.Do();
        }
        public void AddEffect(int EffectId, int level)
        {

            if (!EffectMap.TryGetValue(EffectId, out List<Effect> list))
            {
                list = new List<Effect>();
            }

            EffectConfig config = EffectConfigCategory.Instance.Get(EffectId);

            if (list.Count >= config.Max)
            {  //移除旧的
                list.RemoveRange(0, list.Count - config.Max + 1);
            }

            Effect effect = new Effect(this);
            effect.Level = level;
            effect.CreateTime = DateTime.Now.Ticks;
            effect.Duration = config.Duration;
            effect.Config = config;

            list.Add(effect);
        }

        public void Move(Vector3Int cell)
        {
            this.EventCenter.Raise(new ShowMsgEvent
            {
                Content = "移动"
            });
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
            //查找和自己不同类的,并且不是自己的主人/仆人
            var enemys = GameProcessor.Inst.PlayerManager.GetAllPlayers().FindAll(p => p.Camp != this.Camp  && p.IsSurvice && p.GroupId != this.GroupId);

            if (enemys.Count <= 0)
            {
                return null;
            }

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

            return enemys[0];
        }

        public void OnHit(int tid, params long[] damages)
        {
            foreach (var d in damages)
            {
                this.Logic.OnDamage(tid, d);
            }
        }

        public void SetHP(long hp) {
            this.HP = hp;

            EventCenter.Raise(new SetPlayerHPEvent
            {
                HP = hp.ToString()
            });
        }

        public T GetComponent<T>()
        {
            return this.Transform.GetComponent<T>();
        }
    }
}
