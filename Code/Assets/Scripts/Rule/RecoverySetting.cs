namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class RecoverySetting
    {
        private Dictionary<int, bool> ItemTypeDict = new Dictionary<int, bool>();  //回收道具类型（0 金币 1 通用道具 2装备,3技能书）

        private Dictionary<int, bool> ItemQuanlityDict = new Dictionary<int, bool>();  //回收道具品质(1白色 2绿色 3蓝色 4紫色)

        private int Level = 0; //回收道具等级

        public void SetType(int type, bool check) {
            this.ItemTypeDict[type] = check;
        }

        public void SetQuanlity(int quanlity, bool check) {
            this.ItemQuanlityDict[quanlity] = check;
        }

        public void SetLevel(int level) {
            this.Level = level;
        }

        public bool CheckRecovery(Item item)
        {
            if (ItemTypeDict.GetValueOrDefault((int)item.Type, false)
                && ItemQuanlityDict.GetValueOrDefault(item.GetQuality(), false)
                && item.Level <= Level)
            {
                return true;
            }
            return false;
        }
    }
}
