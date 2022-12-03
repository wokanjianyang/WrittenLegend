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
        [Title("��������")]
        [LabelText("�û���")]
        public TextMeshProUGUI tmp_Name;

        [LabelText("�ȼ�")]
        public TextMeshProUGUI tmp_Level;

        [LabelText("ս��")]
        public TextMeshProUGUI tmp_BattlePower;

        [LabelText("���")]
        public TextMeshProUGUI tmp_Gold;

        [LabelText("������")]
        public Image img_Exp;

        private Hero hero;

        public int Order => (int)ComponentOrder.TopNav;

        public void OnBattleStart()
        {
            this.hero = UserData.Load();
            this.tmp_Name.text = hero.Name;
            this.tmp_Level.text = $"{hero.Level}��";
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
            this.tmp_Level.text = $"{e.Level}��";
        }

        private void OnHeroInfoUpdateEvent(HeroInfoUpdateEvent e)
        {
            this.tmp_Gold.text = $"���:{this.hero.Gold}";
            this.img_Exp.fillAmount = this.hero.Exp * 1f / this.hero.UpExp;
        }
    }
}
