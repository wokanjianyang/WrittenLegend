using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewSkillProcessor : AViewPage
    {
        [Title("技能面板")]
        [LabelText("所有技能")]
        public ScrollRect sr_AllSkill;

        [LabelText("装载技能")]
        public Transform tran_EquipSkills;

        private List<Item_Skill> learnSkills;
        private List<Com_Skill> equipSkills;
        private GameObject bookPrefab;

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            this.learnSkills = new List<Item_Skill>();
            this.equipSkills = new List<Com_Skill>();

            var user = GameProcessor.Inst.User;
            user.EventCenter.AddListener<HeroUpdateSkillEvent>(OnHeroUpdateSkillEvent);
            bookPrefab = Resources.Load<GameObject>("Prefab/Window/Item_Skill");

            user.SkillList.Sort((a, b) => {
                return a.SkillConfig.Id.CompareTo(b.SkillConfig.Id);
            });
            foreach (var skill in user.SkillList)
            {
                SkillPanel skillPanel = new SkillPanel(skill, user.GetRuneList(skill.SkillId), user.GetSuitList(skill.SkillId));
                SkillToBattle(skillPanel);
            }
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Skill;
        }

        private void OnHeroUpdateSkillEvent(HeroUpdateSkillEvent e)
        {
            SkillToBattle(e.SkillPanel);

            //UserData.Save(); //修改技能后，存档
        }

        private void SkillToBattle(SkillPanel skill) {
            if (skill == null)
            {
                return;
            }

            if (skill.SkillData.Status == SkillStatus.Learn)
            {
                this.equipSkills.RemoveAll(b => b.SkillPanel.SkillId == skill.SkillId);
                var learn = this.learnSkills.Find(s => s.SkillPanel.SkillId == skill.SkillId);
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
                var learn = this.learnSkills.Find(s => s.SkillPanel.SkillId == skill.SkillId);
                if (learn != null)
                {
                    this.learnSkills.Remove(learn);
                    GameObject.Destroy(learn.gameObject);
                }
                var equip = this.equipSkills.Find(s => s.SkillPanel.SkillId == skill.SkillId);
                if (equip == null)
                {
                    var com = this.tran_EquipSkills.GetChild(this.equipSkills.Count).GetComponent<Com_Skill>();
                    com.SetItem(skill);
                    this.equipSkills.Add(com);
                }
            }
            //英雄重新加载已选择技能
            //GameProcessor.Inst.PlayerManager.GetHero().InitPanelSkill();
        }

        private void CreateBook(SkillPanel skill)
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
