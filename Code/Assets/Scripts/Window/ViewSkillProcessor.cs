using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewSkillProcessor : AViewPage
    {
        [Title("技能信息")]
        [LabelText("已学习技能")]
        public ScrollRect sr_AllSkill;

        [LabelText("已装配技能")]
        public Transform tran_EquipSkills;

        private List<Item_Skill> learnSkills;
        private List<Com_Skill> equipSkills;
        private GameObject bookPrefab;

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            this.learnSkills = new List<Item_Skill>();
            this.equipSkills = new List<Com_Skill>();

            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            hero.EventCenter.AddListener<HeroUpdateSkillEvent>(OnHeroUpdateSkillEvent);
            bookPrefab = Resources.Load<GameObject>("Prefab/Window/Item_Skill");

            foreach (var skill in hero.SkillPanel)
            {
                SkillToBattle(skill);
            }
            GameProcessor.Inst.PlayerManager.GetHero().InitPanelSkill();
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Skill;
        }

        private void OnHeroUpdateSkillEvent(HeroUpdateSkillEvent e)
        {
            SkillToBattle(e.SkillData);

            UserData.Save(); //修改技能后，存档
        }

        private void SkillToBattle(SkillData skill) {
            if (skill == null)
            {
                return;
            }

            if (skill.Status == SkillStatus.Learn)
            {
                this.equipSkills.RemoveAll(b => b.SkillData.SkillId == skill.SkillId);
                var learn = this.learnSkills.Find(s => s.SkillData.SkillId == skill.SkillId);
                if (learn != null)
                {
                    learn.SetItem(skill);
                }
                else
                {
                    this.CreateBook(skill);
                }
            }
            else
            {
                var learn = this.learnSkills.Find(s => s.SkillData.SkillId == skill.SkillId);
                if (learn != null)
                {
                    this.learnSkills.Remove(learn);
                    GameObject.Destroy(learn.gameObject);
                }
                var equip = this.equipSkills.Find(s => s.SkillData.SkillId == skill.SkillId);
                if (equip == null)
                {
                    var com = this.tran_EquipSkills.GetChild(this.equipSkills.Count).GetComponent<Com_Skill>();
                    com.SetItem(skill);
                    this.equipSkills.Add(com);
                }
            }
            //英雄重新加载已选择技能
            GameProcessor.Inst.PlayerManager.GetHero().InitPanelSkill();
        }

        private void CreateBook(SkillData skill)
        {
            var emptyBook = GameObject.Instantiate(bookPrefab);
            var com = emptyBook.GetComponent<Item_Skill>();
            com.SetItem(skill);
            emptyBook.transform.SetParent(this.sr_AllSkill.content);
            emptyBook.transform.localScale = Vector3.one;
            this.learnSkills.Add(com);
        }
    }
}
