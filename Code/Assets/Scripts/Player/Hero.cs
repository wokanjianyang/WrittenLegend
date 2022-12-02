using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class Hero : APlayer
    {
        public long Exp { get; set; }

        public long UpExp { get; set; }

        public long Gold { get; set; }

        public long Essence { get; set; }

        public int MapId { get; set; }

        public int LastCityId { get; set; }

        public long Power { get; set; }

        public IDictionary<int, Equip> EquipPanel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Item> Bags { get; set; }

        public long LastOut { get; set; }
        public override void Load()
        {
            base.Load();

            var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/HeroBox");
            var box = GameObject.Instantiate(boxPrefab).transform;
            box.SetParent(this.GetComponent<PlayerUI>().image_Background.transform);

            this.Camp = PlayerType.Hero;

            //设置各种属性值
    
            SetLevelConfigAttr();
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.Test, 15);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //加载技能
            if (SkillIdList == null)
            {
                SkillIdList = new Dictionary<int, int>();
            }
            if (SkillIdList.Count == 0) {
                SkillIdList.Add(1, 1001);  //基础剑术
                SkillIdList.Add(2, 2001);  //火球
                SkillIdList.Add(3, 3001);  //灵魂火符
            }

            this.Bags = new List<Item>();
            this.EquipPanel = new Dictionary<int, Equip>();

            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);
            this.EventCenter.AddListener<HeroUseEquipEvent>(HeroUseEquip);
        }

        private void HeroChange(HeroChangeEvent e)
        {
            switch (e.Type)
            {
                case HeroChangeType.LevelUp:
                    LevelUp();
                    break;
            }
        }

        private void HeroUseEquip(HeroUseEquipEvent e)
        {
            Equip equip = Bags.FindLast(m => m.ID == e.EquipId) as Equip;

/*            if (equip.ID == 0)
                return;*/

            int postion = equip.Position;

            Equip old;
            if (EquipPanel.TryGetValue(postion, out old))
            {
                Bags.Add(old); //old move to bag
            }

            EquipPanel[postion] = equip; //new use to panel

            //替换属性
            foreach (var a in equip.AttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, postion * 100 + AttributeFrom.EquipBase, a.Value);
            }

        }

        private void SetLevelConfigAttr()
        {
            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);
            UpExp = config.Exp;
        }

        private void LevelUp()
        {
            if (this.Exp >= this.UpExp)
            {
                Exp -= UpExp;
                Level++;

                //Add Base Attr
                SetLevelConfigAttr();
                //升级恢复满血量
                SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });
            }
        }

        public List<SkillData> GetSelectSkills() {


            return null;
        }
        public enum HeroChangeType
        {
            LevelUp = 0,
            AttrChange = 1
        }
    }

}
