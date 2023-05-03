using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TopNav : MonoBehaviour,IBattleLife
    {
        [Title("顶部导航")]
        [LabelText("用户名")]
        public Text tmp_Name;

        [LabelText("等级")]
        public Text tmp_Level;

        [LabelText("战力")]
        public Text tmp_BattlePower;

        [LabelText("金币")]
        public Text tmp_Gold;

        private Hero hero;

        public int Order => (int)ComponentOrder.TopNav;

        public void OnBattleStart()
        {
            this.hero = GameProcessor.Inst.PlayerManager.GetHero();
            this.tmp_Name.text = hero.Name;
            this.tmp_Level.text = $"{hero.Level}级";
            this.OnHeroInfoUpdateEvent(null);
            this.tmp_BattlePower.text = $"战力：{hero.AttributeBonus.GetPower()}"; 
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
            this.tmp_BattlePower.text = $"战力：{hero.AttributeBonus.GetPower()}";
        }
    }
}
