namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class RecoverySetting
    {
        public Dictionary<int, bool> EquipQuanlity { get; private set; } = new Dictionary<int, bool>();  //回收道具品质(1白色 2绿色 3蓝色 4紫色)
        public int EquipLevel { get; set; } = 0;

        public Dictionary<int, bool> SkillBookRole { get; private set; } = new Dictionary<int, bool>();
        public int SkillBookLevel { get; set; } = 0;//

        public int test = 10;

        public RecoverySetting() {
            this.test = 20;
        }

        public void SetEquipQuanlity(int quanlity, bool check)
        {
            this.EquipQuanlity[quanlity] = check;
        }

        public void SetSkillBookRole(int role, bool check)
        {
            this.SkillBookRole[role] = check;
        }


        public bool CheckRecovery(Item item)
        {
            if (item.Type == ItemType.Equip)
            {
                Equip equip = item as Equip;
                if ((EquipQuanlity.GetValueOrDefault(item.GetQuality(), false) || item.Level < EquipLevel) && equip.Part <= 10)
                {
                    return true;
                }
            }
            else if (item.Type == ItemType.SkillBox)
            {
                SkillBook skillBook = item as SkillBook;

                int role = skillBook.SkillConfig.Role;
                if (SkillBookRole.GetValueOrDefault(role, false) || item.Level < SkillBookLevel)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
