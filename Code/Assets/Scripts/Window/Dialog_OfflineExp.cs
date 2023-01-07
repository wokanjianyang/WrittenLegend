using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_OfflineExp : MonoBehaviour, IBattleLife
    {
        [LabelText("离线奖励提示")]
        public TextMeshProUGUI tmp_Msg;

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

            if (GameProcessor.Inst.PlayerManager.GetHero().LastOut > 0)
            {
                long offlineSecond = GameProcessor.Inst.CurrentTimeSecond - GameProcessor.Inst.PlayerManager.GetHero().LastOut;
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
            this.offlineSecond = os;
            this.gameObject.SetActive(true);
            var ticks = os * TimeSpan.TicksPerSecond;
            var dateTime = new DateTime(ticks);
            var t = dateTime.ToString("F");
            this.tmp_Msg.text = string.Format("本次离线时间:{0}\n奖励离线经验: {1}",t, os);

            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            hero.Exp += this.offlineSecond;
            hero.EventCenter.Raise(new HeroChangeEvent
            {
                Type = Hero.HeroChangeType.LevelUp
            });
        }
    }
}
