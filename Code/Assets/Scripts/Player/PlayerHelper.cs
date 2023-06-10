using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class PlayerHelper
    {
        public const int MAX_EQUIP_COUNT = 8;

        public const int Max_Level = 350;

        public static Dictionary<string, string> PlayerAttributeMap = new Dictionary<string, string>()
        {
            {nameof(AttributeEnum.Color),"颜色" },
            {nameof(AttributeEnum.Name),"名称" },
            {nameof(AttributeEnum.Level),"等级" },
            {nameof(AttributeEnum.Exp), "经验值" },
            {nameof(AttributeEnum.Power), "战力" },
            {nameof(AttributeEnum.HP), "生命值" },
            {nameof(AttributeEnum.PhyAtt), "物理攻击" },
            {nameof(AttributeEnum.MagicAtt),"魔法攻击" },
            {nameof(AttributeEnum.SpiritAtt), "道术攻击" },
            {nameof(AttributeEnum.Def), "防御" },
            {nameof(AttributeEnum.Speed), "攻速" },
            {nameof(AttributeEnum.Lucky), "幸运" },
            {nameof(AttributeEnum.CritRate), "暴击率" },
            {nameof(AttributeEnum.CritDamage), "暴害增加" },
            {nameof(AttributeEnum.CritRateResist), "抗暴" },
            {nameof(AttributeEnum.CritDamageResist), "爆伤减免" },
            {nameof(AttributeEnum.DamageIncrea), "伤害增加" },
            {nameof(AttributeEnum.DamageResist), "伤害减少" },
            {nameof(AttributeEnum.AttIncrea), "攻击加成" },
            {nameof(AttributeEnum.HpIncrea), "生命加成" },
            {nameof(AttributeEnum.InheritIncrea), "继承加成" },
            {nameof(AttributeEnum.ExpIncrea), "经验加成" },
            {nameof(AttributeEnum.BurstIncrea), "爆率加成" },
            {nameof(AttributeEnum.GoldIncrea), "金币加成" },
            {nameof(AttributeEnum.SecondExp), "泡点经验" },
            {nameof(AttributeEnum.RestoreHp), "每秒回血" },
            {nameof(AttributeEnum.RestoreHpPercent), "每秒回血上限" },
        };
    }
}
