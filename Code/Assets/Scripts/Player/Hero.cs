using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;

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

        public IDictionary<int, Equip> EquipPanel { get; set; } = new Dictionary<int, Equip>();

        [JsonIgnore]
        public IDictionary<int, int> equipRecord { get; set; } = new Dictionary<int, int>();

        public List<SkillData> SkillPanel { get; set; } = new List<SkillData>();



        /// <summary>
        /// 包裹
        /// </summary>
        public List<BoxItem> Bags { get; set; } = new List<BoxItem>();

        public long LastOut { get; set; }

        private bool isInLevelUp;

        public Hero():base()  {
            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);
            this.EventCenter.AddListener<HeroUseEquipEvent>(HeroUseEquip);
            this.EventCenter.AddListener<HeroUnUseEquipEvent>(HeroUnUseEquip);
            this.EventCenter.AddListener<HeroUseSkillBookEvent>(HeroUseSkillBook);
            this.GroupId = 1;
        }

        public override void Load()
        {
            base.Load();

            //var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/HeroBox");
            //var box = GameObject.Instantiate(boxPrefab, this.Transform).transform;
            //box.SetParent(this.GetComponent<PlayerUI>().image_Background.transform); 
        }

        public void Init() {
            this.Camp = PlayerType.Hero;

            //设置各种属性值
            SetLevelConfigAttr();
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.Test, 1000);
            AttributeBonus.SetAttr(AttributeEnum.ExpIncrea, AttributeFrom.Test, 1000);

            //回满当前血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
        }

        /// <summary>
        /// 加载面板已选择技能放到SkillIdList
        /// </summary>
        public void InitPanelSkill()
        {
            SkillIdList = new Dictionary<int, int>();

            List<SkillData> list = SkillPanel.FindAll(m => m.Status == SkillStatus.Equip);

            for (int i = 0; i < list.Count; i++)
            {
                SkillIdList.Add(i, list[i].SkillId);
            }

            base.LoadSkill();
        }

        public void UpdatePlayerInfo() {
            GameProcessor.Inst.PlayerInfo.UpdateAttrInfo(this);
        }

        private void HeroChange(HeroChangeEvent e)
        {
            switch (e.Type)
            {
                case HeroChangeType.LevelUp:
                    if(!this.isInLevelUp)
                    {
                        this.isInLevelUp = true;
                        GameProcessor.Inst.StartCoroutine(LevelUp());
                    }
                    break;
            }
        }

        private void HeroUseEquip(HeroUseEquipEvent e)
        {
            int PanelPosition = e.Position;
            Equip equip = e.Equip;

            EquipPanel[PanelPosition] = equip;

            //替换属性
            foreach (var a in equip.GetTotalAttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase,PanelPosition, a.Value);
            }

            //显示最新的血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //更新属性面板
            UpdatePlayerInfo();
        }

        private void HeroUnUseEquip(HeroUnUseEquipEvent e)
        {
            EquipPanel.Remove(e.Position);

            //替换属性
            foreach (var a in e.Equip.GetTotalAttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, e.Position, 0);
            }

            //显示最新的血量
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //更新属性面板
            UpdatePlayerInfo();
        }

        private void HeroUseSkillBook(HeroUseSkillBookEvent e)
        {
            SkillBook Book = e.Item as SkillBook;

            if (e.IsLearn)
            {
                //第一次学习，创建技能数据
                SkillData skillData = new SkillData(Book.ConfigId);
                skillData.Status = SkillStatus.Learn;
                skillData.Level = 1;
                skillData.Exp = 0;
                skillData.UpExp = Book.SkillConfig.Exp;

                this.SkillPanel.Add(skillData);
                this.EventCenter.Raise(new HeroUpdateSkillEvent()
                {
                    SkillData = skillData
                });
            }
            else
            {
                SkillData skillData = this.SkillPanel.Find(b => b.SkillId == Book.ConfigId);
                skillData.AddExp(Book.ItemConfig.UseParam);
                this.EventCenter.Raise(new HeroUpdateSkillEvent()
                {
                    SkillData = skillData
                });
            }
        }

        private void RebuildSkill(ref SkillData skill)
        {
            int skillId = skill.SkillId;

            List<Equip> skillList = this.EquipPanel.Where(m => m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value).ToList();

            //按单件分组
            var runeGroup = skillList.GroupBy(m => m.RuneConfigId);
            foreach (var runeList in runeGroup)
            {
                SkillRuneConfig SkillRuneConfig = runeList.ElementAt(0).SkillRuneConfig;

                int RuneCount = Mathf.Min(SkillRuneConfig.Max, runeList.Count());

                skill.CD += SkillRuneConfig.CD * RuneCount;
                skill.Dis += SkillRuneConfig.Dis * RuneCount;
                skill.Damage += SkillRuneConfig.Damage * RuneCount;
                skill.Percent += SkillRuneConfig.Percent * RuneCount;
                skill.EnemyMax += SkillRuneConfig.EnemyMax * RuneCount;
            }

            //按套装分组
            var suitGroup = skillList.GroupBy(m => m.SkillRuneConfig.SuitId);

            foreach (var suitList in suitGroup)
            {
                if (suitList.Count() >= 4)
                {  //4件才成套,并且只能有一套能生效
                    SkillSuitConfig SkillSuitConfig = suitList.ElementAt(0).SkillSuitConfig;

                    skill.CD += SkillSuitConfig.CD;
                    skill.Dis += SkillSuitConfig.Dis;
                    skill.Damage += SkillSuitConfig.Damage;
                    skill.Percent += SkillSuitConfig.Percent;
                    skill.EnemyMax += SkillSuitConfig.EnemyMax;
                }
            }
        }

       
        private void SetLevelConfigAttr()
        {
            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);
            UpExp = config.Exp;
        }

        IEnumerator LevelUp()
        {
            while (this.Exp >= this.UpExp)
            {
                Exp -= UpExp;
                Level++;

                //Add Base Attr
                SetLevelConfigAttr();
                //升级恢复满血量
                SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });
                EventCenter.Raise(new HeroInfoUpdateEvent());

                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;
        }

        public enum HeroChangeType
        {
            LevelUp = 0,
            AttrChange = 1
        }
    }

}
