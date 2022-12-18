using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Logic : MonoBehaviour, IPlayer
    {
        /// <summary>
        /// 角色属性
        /// </summary>
        private Dictionary<AttributeEnum, object> BaseAttributeMap = new Dictionary<AttributeEnum, object>();
        private Dictionary<AttributeEnum, object> BattleAttributeMap = new Dictionary<AttributeEnum, object>();

        private Dictionary<int, Effect> EffectMap = new Dictionary<int, Effect>();

        public bool IsSurvice { get; private set; } = true;

        private List<SDD.Events.Event> playerEvents = new List<SDD.Events.Event>();


        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(Dictionary<AttributeEnum, object> dict)
        {
            if (dict != null)
            {
                foreach (var kvp in dict)
                {
                    BaseAttributeMap[kvp.Key] = kvp.Value;
                    if (kvp.Key > 0)
                    {
                        SelfPlayer.AttributeBonus.SetAttr(kvp.Key, AttributeFrom.HeroBase, Convert.ToInt64(kvp.Value));
                    }
                }
            }

            SelfPlayer.HP = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.HP);

            //设置背景  
            if (BaseAttributeMap.TryGetValue(AttributeEnum.Color, out var color))
            {
                if (color is Color _color)
                {
                    SelfPlayer.EventCenter.Raise(new SetBackgroundColorEvent
                    {
                        Color = _color
                    });
                }
            }

            //设置名称
            if (SelfPlayer.Name != null)
            {
                SelfPlayer.EventCenter.Raise(new SetPlayerNameEvent
                {
                    Name = SelfPlayer.Name
                });
            }

            //设置等级

            SelfPlayer.EventCenter.Raise(new SetPlayerLevelEvent
            {
                Level = SelfPlayer.Level.ToString()
            });

            //设置血量
            this.SelfPlayer.SetHP(SelfPlayer.HP);
        }

        public void ResetData()
        {
            var dict = new Dictionary<AttributeEnum, object>();
            foreach (var kvp in BaseAttributeMap)
            {
                dict[kvp.Key] = kvp.Value;
            }
            SetData(dict);
            IsSurvice = true;
            BattleAttributeMap.Clear();
        }

        public void OnDamage(int fromId,long damage)
        {
            long currentHP = this.SelfPlayer.HP;

            currentHP -= damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }

            AddBattleAttribute(AttributeEnum.HP, damage * -1);
            this.SelfPlayer.SetHP(currentHP);
            if (currentHP == 0)
            {
                IsSurvice = false;
                this.playerEvents.Add(new PlayerDeadEvent
                {
                    RoundNum = SelfPlayer.RoundCounter
                });
                this.playerEvents.Add(new ShowMsgEvent
                {
                    Content = "死亡"
                });
                this.playerEvents.Add(new DeadRewarddEvent
                {
                    FromId = fromId,
                    ToId = SelfPlayer.ID
                });
            }
            else
            {
                this.playerEvents.Add(new ShowMsgEvent
                {
                    Content = (damage * -1).ToString()
                });
            }
            AddBattleAttribute(AttributeEnum.HP, damage * -1);

            this.SelfPlayer.SetHP(currentHP);
        }

        public void RaiseEvents()
        {
            foreach(var e in this.playerEvents)
            {
                this.SelfPlayer.EventCenter.Raise(e);
            }
            this.playerEvents.Clear();
        }

        public int GetMaxHP()
        {
            var baseValue = 0f;
            if (BaseAttributeMap.TryGetValue(AttributeEnum.HP, out var value))
            {
                baseValue = (float)Convert.ToDouble(value);
            }
            return (int)baseValue;
        }


        public float GetAttributeFloat(AttributeEnum attr)
        {
            var baseValue = 0f;
            if (BaseAttributeMap.TryGetValue(attr, out var value))
            {
                baseValue = (float)Convert.ToDouble(value);
            }

            var battleValue = 0f;
            if (BattleAttributeMap.TryGetValue(attr, out var value2))
            {
                battleValue = (float)Convert.ToDouble(value2);
            }

            return baseValue + battleValue;
        }

        public void AddBattleAttribute(AttributeEnum attr, float value)
        {
            BattleAttributeMap.TryGetValue(attr, out var value2);
            BattleAttributeMap[attr] = (float)Convert.ToDouble(value2) + value;
        }

/*        private void SetHP(string hp)
        {
            SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent
            {
                HP = hp
            });
        }*/

        public APlayer SelfPlayer { get; set; }
        public void SetParent(APlayer player)
        {
            SelfPlayer = player;
        }
    }
}
