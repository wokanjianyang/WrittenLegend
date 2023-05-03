using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TopNav : MonoBehaviour,IBattleLife
    {
        [Title("��������")]
        [LabelText("�û���")]
        public Text tmp_Name;

        [LabelText("�ȼ�")]
        public Text tmp_Level;

        [LabelText("ս��")]
        public Text tmp_BattlePower;

        [LabelText("���")]
        public Text tmp_Gold;

        private Hero hero;

        public int Order => (int)ComponentOrder.TopNav;

        public void OnBattleStart()
        {
            this.hero = GameProcessor.Inst.PlayerManager.GetHero();
            this.tmp_Name.text = hero.Name;
            this.tmp_Level.text = $"{hero.Level}��";
            this.OnHeroInfoUpdateEvent(null);
            this.tmp_BattlePower.text = $"ս����{hero.AttributeBonus.GetPower()}"; 
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
            this.tmp_BattlePower.text = $"ս����{hero.AttributeBonus.GetPower()}";
        }
    }
}
