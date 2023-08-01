using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TopNav : MonoBehaviour,IBattleLife
    {
        [Title("顶部导航")]
        [LabelText("名称")]
        public Text tmp_Name;

        [LabelText("等级")]
        public Text tmp_Level;

        [LabelText("战力")]
        public Text tmp_BattlePower;

        [LabelText("金币")]
        public Text tmp_Gold;

        private User user;

        public int Order => (int)ComponentOrder.TopNav;

        public void OnBattleStart()
        {
            this.user = GameProcessor.Inst.User;
            this.tmp_Name.text = user.Name;
            this.tmp_Level.text = $"{user.Level}级";
            this.OnHeroInfoUpdateEvent(null);
            this.tmp_BattlePower.text = $"战力：{user.AttributeBonus.GetPower()}"; 
            user.EventCenter.AddListener<SetPlayerLevelEvent>(this.OnSetPlayerLevelEvent);
            user.EventCenter.AddListener<UserInfoUpdateEvent>(this.OnHeroInfoUpdateEvent);
            user.EventCenter.AddListener<SetPlayerNameEvent>(this.OnSetPlayerNameEvent);
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

        private void OnHeroInfoUpdateEvent(UserInfoUpdateEvent e)
        {
            this.tmp_Gold.text = $"金币:{this.user.Gold}";
            this.tmp_BattlePower.text = $"战力：{user.AttributeBonus.GetPower()}";
        }
        
        private void OnSetPlayerNameEvent(SetPlayerNameEvent e)
        {
            this.tmp_Name.text = e.Name;
        }

    }
}
