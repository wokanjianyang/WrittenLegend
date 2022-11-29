using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ET;

namespace Game
{
    public class Equip : Item
    {
        public int Position { get; set; }

        public SkillRune SkillRune { get; set; }
    }

    public class EquipHelper {

        public static List<Equip> DropEquip(DropConfig drop) {
            List<Equip> equips = new List<Equip>();

            for (int i = 0; i < drop.ItemList.Length; i++) {
                if (RandomHelper.RandomResult(drop.Rate[i])) {
                    Equip equip = BuildEquip(drop.ItemList[i]);

                    equips.Add(equip);
                }

                if (equips.Count >= drop.RandomType) {
                    break;
                }
            }

            return equips;
        }

        public static Equip BuildEquip(int id)
        {
            EquipConfig config = EquipConfigCategory.Instance.Get(id);

            Equip equip = new Equip();

            equip.ID = id;
            equip.Name = config.Name;
            equip.Des = "";
            equip.Level = config.LevelRequired;
            equip.Position = config.Position;
            equip.Gold = config.Price;
            equip.AttrList = new Dictionary<int,long>();
            for (int i = 0; i < config.AttributeBase.Length; i++)
            {
                equip.AttrList.Add(config.BaseArray[i], config.AttributeBase[i]);
            }

            //build Skill Runne
            SkillRune rune = new SkillRune();
            rune.SkillId = 2001;
            rune.Name = "火球术-多重";
            rune.Des = "增加火球的一个攻击目标";
            rune.Incre = 1;
            rune.Type = SkillRuneType.MaxNum;
            rune.SuitId = 0;

            equip.SkillRune = rune;

            return equip;
        }
    }
}
