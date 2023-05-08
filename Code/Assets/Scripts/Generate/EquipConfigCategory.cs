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
        public static Equip BuildEquip(int configId, int minQuality)
        {
            EquipConfig config = EquipConfigCategory.Instance.Get(configId);

            int runeId = config.RuneId;
            int suitId = config.SuitId;

            if (runeId == 0) //������ɴ���
            {
                SkillRuneConfig runeConfig = SkillRuneHelper.RandomRune();
                runeId = runeConfig.Id;

                if (suitId == 0)  //������ɴ���
                {
                    suitId = SkillSuitHelper.RandomSuit(runeConfig.SkillId).Id;
                }
            }

            Equip equip = new Equip(configId, runeId, suitId);

            //����Ʒ��
            equip.Quality = Math.Max(minQuality, RandomHelper.RandomQuality());

            //����Ʒ��,�����������
            for (int i = 0; i < equip.Quality; i++)
            {
                AttrEntryConfigCategory.Instance.Build(ref equip);
            }

            return equip;
        }

        public static Equip BuildCustomEquip(int configId)
        {
            EquipConfig config = EquipConfigCategory.Instance.Get(configId);

            int runeId = config.RuneId;
            int suitId = config.SuitId;

            if (runeId == 0) //������ɴ���
            {
                SkillRuneConfig runeConfig = SkillRuneHelper.RandomRune();
                runeId = runeConfig.Id;

                if (suitId == 0)  //������ɴ���
                {
                    suitId = SkillSuitHelper.RandomSuit(runeConfig.SkillId).Id;
                }
            }

            Equip equip = new Equip(configId, runeId, suitId);
            equip.Quality = 4;

            return equip;
        }
    }
}