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
        public List<Equip> Bags { get; set; }

        public long LastOut { get; set; }

        public Hero():base()  {
            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);
            this.EventCenter.AddListener<HeroUseEquipEvent>(HeroUseEquip);
        }

        public override void Load()
        {
            base.Load();

            var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/HeroBox");
            var box = GameObject.Instantiate(boxPrefab, this.Transform).transform;
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

            if(this.Bags==null)
            {
                this.Bags = new List<Equip>();
            }
            if(this.EquipPanel==null)
            {
                this.EquipPanel = new Dictionary<int, Equip>();
            }
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

            this.Equip(e.Position, e.Equip);
        }

        private void Equip(int pos, Equip equip)
        {
            //装配包裹的
            Equip old;
            if (EquipPanel.TryGetValue(equip.Position, out old))
            {
                Bags.Add(old);
            }


            Bags.Remove(equip); //old move to bag

            EquipPanel[equip.Position] = equip; //new use to panel

            //替换属性
            foreach (var a in equip.AttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, equip.Position * 100 + AttributeFrom.EquipBase, a.Value);
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

        public void AddToBags(List<Equip> items)
        {
            int num = Mathf.Min( 50 - Bags.Count,items.Count);
            if (num > 0)
            {
                Bags.AddRange(items.GetRange(0, num));
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
