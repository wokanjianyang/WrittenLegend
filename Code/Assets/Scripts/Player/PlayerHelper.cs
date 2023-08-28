using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class PlayerHelper
    {
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
            {nameof(AttributeEnum.CritDamage), "暴伤加成" },
            {nameof(AttributeEnum.CritRateResist), "抗暴率" },
            {nameof(AttributeEnum.CritDamageResist), "爆伤减免" },
            {nameof(AttributeEnum.DamageIncrea), "伤害增加" },
            {nameof(AttributeEnum.DamageResist), "伤害减少" },
            {nameof(AttributeEnum.AttIncrea), "攻击加成" },
            {nameof(AttributeEnum.HpIncrea), "生命加成" },
            {nameof(AttributeEnum.DefIncrea), "防御加成" },
            {nameof(AttributeEnum.InheritIncrea), "继承加成" },
            {nameof(AttributeEnum.ExpIncrea), "经验加成" },
            {nameof(AttributeEnum.BurstIncrea), "爆率加成" },
            {nameof(AttributeEnum.GoldIncrea), "金币加成" },
            {nameof(AttributeEnum.SecondExp), "经验收益" },
            {nameof(AttributeEnum.SecondGold), "金币收益" },
            {nameof(AttributeEnum.RestoreHp), "每秒点数回血" },
            {nameof(AttributeEnum.RestoreHpPercent), "每秒上限回血" },
            {nameof(AttributeEnum.QualityIncrea), "品质加成" },

            {nameof(AttributeEnum.PhyAttIncrea), "物攻加成" },
            {nameof(AttributeEnum.MagicAttIncrea),"魔法加成" },
            {nameof(AttributeEnum.SpiritAttIncrea), "道术加成" },

            {nameof(AttributeEnum.EquipBaseIncrea), "装备基础属性提升" },
            {nameof(AttributeEnum.EquipQualityIncrea), "装备品质属性提升" },
        };
    }
}
