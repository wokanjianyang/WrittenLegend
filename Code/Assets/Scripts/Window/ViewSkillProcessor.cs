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
        [Title("������Ϣ")]
        [LabelText("���м���")]
        public ScrollRect sr_AllSkill;




        public override void OnBattleStart()
        {
            base.OnBattleStart();

        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Skill;
        }
    }
}
