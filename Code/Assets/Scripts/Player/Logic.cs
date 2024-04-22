using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Logic : MonoBehaviour, IPlayer
    {
        /// <summary>
        /// 角色属性
        /// </summary>
        //private Dictionary<AttributeEnum, object> BaseAttributeMap = new Dictionary<AttributeEnum, object>();
        //private Dictionary<AttributeEnum, object> BattleAttributeMap = new Dictionary<AttributeEnum, object>();

        private Dictionary<int, Effect> EffectMap = new Dictionary<int, Effect>();

        public bool IsSurvice { get; private set; } = true;

        //private List<SDD.Events.Event> playerEvents = new List<SDD.Events.Event>();


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
            //设置名称
            SelfPlayer.EventCenter.Raise(new SetPlayerNameEvent
            {
                Name = SelfPlayer.Name
            });

            //设置等级

            SelfPlayer.EventCenter.Raise(new SetPlayerLevelEvent
            {
                Level = SelfPlayer.Level
            });

            //设置血量
            //this.SelfPlayer.SetHP(SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.HP));


            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
        }

        public void ResetData()
        {
            //var dict = new Dictionary<AttributeEnum, object>();
            //foreach (var kvp in BaseAttributeMap)
            //{
            //    dict[kvp.Key] = kvp.Value;
            //}
            SetData(null);
            IsSurvice = true;

            //BattleAttributeMap.Clear();

            SelfPlayer.HP = SelfPlayer.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP);
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
            //this.SelfPlayer.SetPosition(GameProcessor.Inst.PlayerManager.RandomCell(this.SelfPlayer.Cell));
        }

        public void OnDamage(DamageResult dr)
        {
            if (!IsSurvice)
            {
                return;
            }

            double currentHP = this.SelfPlayer.HP;

            currentHP -= dr.Damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }


            if (SelfPlayer.Camp == PlayerType.Hero)
            {
                //Debug.Log($"{(this.SelfPlayer.Name)} 受到伤害:{(damage)} ,剩余血量:{(currentHP)}");
            }

            this.SelfPlayer.SetHP(currentHP);

            this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent
            {
                Type = dr.Type,
                Content = "-" + StringHelper.FormatNumber(dr.Damage)
            });
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });

            if (currentHP <= 0)
            {
                IsSurvice = false;
                this.SelfPlayer.EventCenter.Raise(new DeadRewarddEvent
                {
                    FromId = dr.FromId,
                    ToId = SelfPlayer.ID
                });

                if (SelfPlayer.Camp != PlayerType.Hero)
                {
                    StartCoroutine(this.ClearPlayer());
                }

            }
        }

        private IEnumerator ClearPlayer()
        {
            yield return new WaitForSeconds(ConfigHelper.DelayShowTime);
            GameProcessor.Inst.PlayerManager.RemoveDeadPlayers(this.SelfPlayer);
            yield return null;
        }

        public void OnRestore(double hp)
        {
            double currentHP = this.SelfPlayer.HP;

            if (currentHP <= 0)
            {
                //?是否先判断死亡，再判断回复
                return;
            }

            double maxHp = this.SelfPlayer.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP);

            if (maxHp <= currentHP)
            {
                //满血不回复
                return;
            }

            currentHP += hp;
            if (maxHp <= currentHP)
            {
                currentHP = maxHp; //最多只能回复满血
            }

            if (SelfPlayer.Camp == PlayerType.Hero)
            {
                //Debug.Log($"{(this.SelfPlayer.Name)} 恢复生命:{(hp)} ,剩余血量:{(currentHP)}");
            }

            this.SelfPlayer.SetHP(currentHP);

            this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent
            {
                Type = MsgType.Restore,
                Content = StringHelper.FormatNumber(hp)
            });
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
        }

        //public void RaiseEvents()
        //{
        //    foreach(var e in this.playerEvents)
        //    {
        //        this.SelfPlayer.EventCenter.Raise(e);
        //    }
        //    this.playerEvents.Clear();
        //}

        //public int GetMaxHP()
        //{
        //    var baseValue = 0f;
        //    if (BaseAttributeMap.TryGetValue(AttributeEnum.HP, out var value))
        //    {
        //        baseValue = (float)Convert.ToDouble(value);
        //    }
        //    return (int)baseValue;
        //}


        //public float GetAttributeFloat(AttributeEnum attr)
        //{
        //    var baseValue = 0f;
        //    if (BaseAttributeMap.TryGetValue(attr, out var value))
        //    {
        //        baseValue = (float)Convert.ToDouble(value);
        //    }

        //    var battleValue = 0f;
        //    if (BattleAttributeMap.TryGetValue(attr, out var value2))
        //    {
        //        battleValue = (float)Convert.ToDouble(value2);
        //    }

        //    return baseValue + battleValue;
        //}

        public void AddBattleAttribute(AttributeEnum attr, float value)
        {
            //BattleAttributeMap.TryGetValue(attr, out var value2);
            //BattleAttributeMap[attr] = (float)Convert.ToDouble(value2) + value;
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
