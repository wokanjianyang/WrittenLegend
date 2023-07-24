using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_OfflineExp : MonoBehaviour, IBattleLife
    {
        [LabelText("离线奖励提示")]
        public Text tmp_Msg;

        [LabelText("领取按钮")]
        public Button btn_GetOfflineExp;

        private long offlineSecond = 0;

        // Start is called before the first frame update
        void Start()
        {
            this.gameObject.SetActive(false);

            this.btn_GetOfflineExp.onClick.AddListener(this.OnClick_GetOfflineExp);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public int Order => (int)ComponentOrder.Dialog;

        public void OnBattleStart()
        {

            if (GameProcessor.Inst.User.LastOut > 0)
            {
                long offlineSecond = GameProcessor.Inst.CurrentTimeSecond - GameProcessor.Inst.User.LastOut;
                if (offlineSecond > 0)
                {
                    this.OnShowOfflineExpEvent(offlineSecond);
                }
            }
        }

        private void OnClick_GetOfflineExp()
        {
            this.gameObject.SetActive(false);
        }
        private void OnShowOfflineExpEvent(long os)
        {
            User user = GameProcessor.Inst.User;

            LevelConfig levelConfig = LevelConfigCategory.Instance.Get(user.Level);

            this.offlineSecond = os;
            this.gameObject.SetActive(true);
            var ticks = os * TimeSpan.TicksPerSecond;
            var dateTime = new DateTime(ticks);
            var t = dateTime.ToString("F");

            var offlineExp = 1 * os;

            this.tmp_Msg.text = string.Format("本次离线时间:{0}\n奖励离线经验: {1}", t, offlineExp);


            user.Exp += offlineExp;
            user.EventCenter.Raise(new HeroChangeEvent
            {
                Type = UserChangeType.LevelUp
            });
        }
    }
}
