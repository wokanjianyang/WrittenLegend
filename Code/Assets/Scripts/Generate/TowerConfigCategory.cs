using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class TowerConfigCategory
    {
        public TowerConfig GetByFloor(long floor)
        {
            if (floor > ConfigHelper.Max_Floor)
            {
                floor = ConfigHelper.Max_Floor;
            }
            TowerConfig config = TowerConfigCategory.Instance.GetAll().Where(m => m.Value.StartLevel <= floor && m.Value.EndLevel >= floor).First().Value;
            return config;
        }

        public long GetMaxFloor()
        {
            return TowerConfigCategory.Instance.GetAll().Select(m => m.Value.EndLevel).Max();
        }
    }

    public class MonsterTowerHelper
    {
        public static List<Monster_Tower> BuildMonster(long floor)
        {
            if (floor >= ConfigHelper.Max_Floor)
            {
                return null;
            }

            List<Monster_Tower> monsters = new List<Monster_Tower>();

            if (floor <= TowerConfigCategory.Instance.GetMaxFloor())
            {
                long monsterQuantity = 1;
                //if (floor > 10000)
                //{
                //    monsterQuantity = (floor % 10) + 1;
                //}

                for (int i = 0; i < monsterQuantity; i++)
                {
                    var enemy = new Monster_Tower(floor, i);
                    monsters.Add(enemy);
                }
            }

            return monsters;
        }

        public static AttributeBonus BuildOffline(long Floor)
        {
            TowerConfig config = TowerConfigCategory.Instance.GetByFloor(Floor);

            long rise = Floor - config.StartLevel;

            long attr = config.StartAttr + (long)(rise * config.RiseAttr);
            long hp = config.StartHp + (long)(rise * config.RiseHp);
            long def = config.StartDef + (long)(rise * config.RiseDef);

            AttributeBonus attributeBonus = new AttributeBonus();

            attributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
            attributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
            attributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
            attributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
            attributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

            return attributeBonus;
        }

        public static void GetTowerSecond(long floor, out long secondExp, out long secondGold)
        {
            TowerConfig config = TowerConfigCategory.Instance.GetByFloor(floor);
            long rise = floor - config.StartLevel;

            secondExp = config.StartExp + (long)(rise * config.RiseExp);
            secondGold = config.StartGold + (long)(rise * config.RiseGold);
        }
    }
}
