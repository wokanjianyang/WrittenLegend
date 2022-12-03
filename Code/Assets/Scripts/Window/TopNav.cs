using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TopNav : MonoBehaviour,IBattleLife
    {
        [Title("顶部导航")]
        [LabelText("用户名")]
        public TextMeshProUGUI tmp_Name;

        [LabelText("等级")]
        public TextMeshProUGUI tmp_Level;

        [LabelText("战力")]
        public TextMeshProUGUI tmp_BattlePower;

        [LabelText("金币")]
        public TextMeshProUGUI tmp_Gold;

        [LabelText("经验条")]
        public Image img_Exp;

        private Hero hero;

        public int Order => (int)ComponentOrder.TopNav;

        public void OnBattleStart()
        {
            this.hero = UserData.Load();
            this.tmp_Name.text = hero.Name;
            this.tmp_Level.text = $"{hero.Level}级";
            this.OnHeroInfoUpdateEvent(null);
            hero.EventCenter.AddListener<SetPlayerLevelEvent>(this.OnSetPlayerLevelEvent);
            hero.EventCenter.AddListener<HeroInfoUpdateEvent>(this.OnHeroInfoUpdateEvent);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnSetPlayerLevelEvent(SetPlayerLevelEvent e)
        {
            this.tmp_Level.text = $"{e.Level}级";
        }

        private void OnHeroInfoUpdateEvent(HeroInfoUpdateEvent e)
        {
            this.tmp_Gold.text = $"金币:{this.hero.Gold}";
            this.img_Exp.fillAmount = this.hero.Exp * 1f / this.hero.UpExp;
        }
    }
}
