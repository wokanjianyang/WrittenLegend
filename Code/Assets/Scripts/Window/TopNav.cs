using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TopNav : MonoBehaviour, IBattleLife
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

        public Button Btn_Setting;

        private User user;

        public int Order => (int)ComponentOrder.TopNav;

        void Awake()
        {
            this.Btn_Setting.onClick.AddListener(this.OnClick_Setting);
        }

        public void OnBattleStart()
        {
            this.gameObject.SetActive(true);
            this.user = GameProcessor.Inst.User;
            this.tmp_Name.text = user.Name;

            this.OnHeroInfoUpdateEvent(null);

            this.tmp_Level.text = user.MagicLevel.Data + "级";
            this.tmp_BattlePower.text = $"战力：{user.AttributeBonus.GetPower()}";

            user.EventCenter.AddListener<SetPlayerLevelEvent>(this.OnSetPlayerLevelEvent);
            user.EventCenter.AddListener<UserInfoUpdateEvent>(this.OnHeroInfoUpdateEvent);
            user.EventCenter.AddListener<SetPlayerNameEvent>(this.OnSetPlayerNameEvent);
            user.EventCenter.AddListener<UserAttrChangeEvent>(this.OnShowPower);
        }

        public void OnClick_Setting()
        {
            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent() { IsOpen = true });
        }

        private void OnSetPlayerLevelEvent(SetPlayerLevelEvent e)
        {
            this.tmp_Level.text = StringHelper.FormatNumber(e.Level) + "级";
        }

        private void OnHeroInfoUpdateEvent(UserInfoUpdateEvent e)
        {
            long gold = this.user.MagicGold.Data;

            string goldText = gold > 100000000 ? StringHelper.FormatNumber(gold) : gold + "";

            this.tmp_Gold.text = $"金币:{goldText}";
            //
        }

        private void OnShowPower(UserAttrChangeEvent e)
        {
            this.tmp_BattlePower.text = $"战力：{user.AttributeBonus.GetPower()}";
        }

        private void OnSetPlayerNameEvent(SetPlayerNameEvent e)
        {
            this.tmp_Name.text = e.Name;
        }

    }
}
