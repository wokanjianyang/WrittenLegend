using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Logic : MonoBehaviour,IPlayer
    {
        /// <summary>
        /// 角色属性
        /// </summary>
        private Dictionary<AttributeEnum, object> BaseAttributeMap = new Dictionary<AttributeEnum, object>();
        private Dictionary<AttributeEnum, object> BattleAttributeMap = new Dictionary<AttributeEnum, object>();

        public bool IsSurvice { get; private set; } = true;


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
                }
            }
            
            //设置背景  
            if(BaseAttributeMap.TryGetValue(AttributeEnum.Color,out var color))
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
            if (BaseAttributeMap.TryGetValue(AttributeEnum.Name, out var name))
            {
                SelfPlayer.EventCenter.Raise(new SetPlayerNameEvent
                {
                    Name = name.ToString()
                });
            }
            
            //设置等级
            if (BaseAttributeMap.TryGetValue(AttributeEnum.Level, out var level))
            {
                SelfPlayer.EventCenter.Raise(new SetPlayerLevelEvent
                {
                    Level = level.ToString()
                });
            }
            
            //设置血量
            if (BaseAttributeMap.TryGetValue(AttributeEnum.HP, out var hp))
            {
                SetHP(hp.ToString());
            }
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

        public void OnDamage(float damage)
        {
            var currentHP = GetAttributeFloat(AttributeEnum.HP);
            currentHP -= damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }

            AddBattleAttribute(AttributeEnum.HP,damage*-1);
            SetHP(currentHP.ToString());
            if (currentHP == 0)
            {
                IsSurvice = false;
                SelfPlayer.EventCenter.Raise(new PlayerDeadEvent
                {
                    RoundNum = SelfPlayer.RoundCounter
                });
                SelfPlayer.EventCenter.Raise(new ShowMsgEvent
                {
                    Content = "死亡"
                });
            }
            else
            {
                SelfPlayer.EventCenter.Raise(new ShowMsgEvent
                {
                    Content = (damage*-1).ToString()
                });
                
            }
            AddBattleAttribute(AttributeEnum.HP,damage*-1);
            SetHP(currentHP.ToString());
        }

        public float GetAttributeFloat(AttributeEnum attr)
        {
            var baseValue = 0f;
            if (BaseAttributeMap.TryGetValue(attr, out var value))
            {
                baseValue =  (float)Convert.ToDouble(value);
            }
            
            var battleValue = 0f;
            if (BattleAttributeMap.TryGetValue(attr, out var value2))
            {
                battleValue =  (float)Convert.ToDouble(value2);
            }

            return baseValue+battleValue;
        }

        public void AddBattleAttribute(AttributeEnum attr, float value)
        {
            BattleAttributeMap.TryGetValue(attr, out var value2);
            BattleAttributeMap[attr] = (float)Convert.ToDouble(value2) + value;
        }

        private void SetHP(string hp)
        {
            SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent
            {
                HP = hp
            });
        }

        public APlayer SelfPlayer { get; set; }
        public void SetParent(APlayer player)
        {
            SelfPlayer = player;
        }
    }
}
