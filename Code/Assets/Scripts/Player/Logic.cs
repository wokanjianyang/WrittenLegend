using System;
using System.Collections.Generic;
using SDD.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Logic : MonoBehaviour,IPlayer
    {
        /// <summary>
        /// 角色属性
        /// </summary>
        private Dictionary<AttributeEnum, object> AttributeMap = new Dictionary<AttributeEnum, object>();

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
                    this.AttributeMap[kvp.Key] = kvp.Value;
                }
            }
            
            //设置背景
            if(this.AttributeMap.TryGetValue(AttributeEnum.Color,out var color))
            {
                if (color is Color _color)
                {
                    this.SelfPlayer.EventCenter.Raise(new SetBackgroundColorEvent()
                    {
                        Color = _color
                    });
                }
            }
            
            //设置名称
            if (this.AttributeMap.TryGetValue(AttributeEnum.Name, out var name))
            {
                this.SelfPlayer.EventCenter.Raise(new SetPlayerNameEvent()
                {
                    Name = name.ToString()
                });
            }
            
            //设置等级
            if (this.AttributeMap.TryGetValue(AttributeEnum.Level, out var level))
            {
                this.SelfPlayer.EventCenter.Raise(new SetPlayerLevelEvent()
                {
                    Level = level.ToString()
                });
            }
            
            //设置血量
            if (this.AttributeMap.TryGetValue(AttributeEnum.HP, out var hp))
            {
                this.SetHP(hp.ToString());
            }
        }

        public void OnDamage(float damage)
        {
            var currentHP = this.GetAttributeFloat(AttributeEnum.HP);
            currentHP -= damage;
            if (currentHP < 0)
            {
                currentHP = 0;
                this.IsSurvice = false;
            }
            this.AddBattleAttribute(AttributeEnum.HP,currentHP);
            this.SetHP(currentHP.ToString());
        }

        public float GetAttributeFloat(AttributeEnum attr)
        {
            if (this.AttributeMap.TryGetValue(attr, out var value))
            {
                return (float)Convert.ToDouble(value);
            }

            return 0;
        }

        public void AddBattleAttribute(AttributeEnum attr, float value)
        {
            this.AttributeMap[attr] = value;
        }

        private void SetHP(string hp)
        {
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent()
            {
                HP = hp.ToString()
            });
        }

        public APlayer SelfPlayer { get; set; }
        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
        }
    }
}
