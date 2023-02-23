namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class RecoverySetting
    {
        private Dictionary<int, bool> ItemTypeDict = new Dictionary<int, bool>();  //���յ������ͣ�0 ��� 1 ͨ�õ��� 2װ��,3�����飩

        private Dictionary<int, bool> ItemQuanlityDict = new Dictionary<int, bool>();  //���յ���Ʒ��(1��ɫ 2��ɫ 3��ɫ 4��ɫ)

        private int Level = 0; //���յ��ߵȼ�

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
