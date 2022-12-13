using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipConfigCategory
    {

    }

    public class EquipHelper
    {
        public static List<Equip> DropEquip(DropConfig drop)
        {
            List<Equip> equips = new List<Equip>();

            for (int i = 0; i < drop.ItemList.Length; i++)
            {
                if (RandomHelper.RandomResult(drop.Rate[i]))
                {
                    Equip equip = BuildEquip(drop.ItemList[i]);

                    equips.Add(equip);
                }

                if (equips.Count >= drop.RandomType)
                {
                    break;
                }
            }

            return equips;
        }

        public static Equip BuildEquip(int ConfigId)
        {
            EquipConfig config = EquipConfigCategory.Instance.Get(ConfigId);

            Equip equip = new Equip();

            equip.Id = IdGenerater.Instance.GenerateId();
            equip.ConfigId = ConfigId;
            equip.Name = config.Name;
            equip.Des = config.Name;
            equip.Level = config.LevelRequired;
            equip.Position = config.Position;
            equip.Gold = config.Price;
            equip.MaxNum = 1;
            equip.BaseAttrList = new Dictionary<int, long>();
            for (int i = 0; i < config.AttributeBase.Length; i++)
            {
                equip.BaseAttrList.Add(config.BaseArray[i], config.AttributeBase[i]);
            }

            //生成品质
            equip.Quality = RandomHelper.RandomQuality();

            //根据品质,生成随机属性
            for (int i = 0; i < equip.Quality; i++)
            {
                AttrEntryConfigCategory.Instance.Build(ref equip);
            }

            //根据基础属性和词条属性，计算总属性
            equip.AttrList = new Dictionary<int, long>();
            foreach (int attrId in equip.BaseAttrList.Keys)
            {
                equip.AttrList[attrId] = equip.BaseAttrList[attrId];
            }

            for (int i = 0; i < equip.AttrEntryList.Count; i++)
            {
                int attrId = equip.AttrEntryList[i].Key;
                long val = 0;
                if (equip.AttrList.TryGetValue(attrId, out val))
                {
                }
                equip.AttrList[attrId] = val + equip.AttrEntryList[i].Value;
            }


            //build Skill Runne
            equip.SkillRune = SkillRuneConfigCategory.Instance.Build();

            return equip;
        }
    }
}