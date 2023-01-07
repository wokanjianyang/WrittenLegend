using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Com_Progress : MonoBehaviour, IBattleLife
    {
        [Title("进度条")]
        [LabelText("进度图片")]
        public Image img_Progress;

        [LabelText("进度数字")]
        public TextMeshProUGUI tmp_Progress;

        [LabelText("进度条类型")]
        public ProgressType ProgressType;

        private Hero hero;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public int Order => (int)ComponentOrder.Progress;

        public void OnBattleStart()
        {
            this.hero = GameProcessor.Inst.PlayerManager.GetHero();
            switch (this.ProgressType)
            {
                case ProgressType.PlayerExp:
                    this.OnHeroInfoUpdateEvent(null);
                    hero.EventCenter.AddListener<HeroInfoUpdateEvent>(this.OnHeroInfoUpdateEvent);
                    break;
                case ProgressType.SkillExp:
                    break;
            }
        }
        public void SetProgress(long current, long total)
        {
            var value = current * 1f / total;
            if(value>1)
            {
                value = 1;
                current = total;
            }
            this.img_Progress.fillAmount = value;
            switch(this.ProgressType)
            {
                case ProgressType.PlayerExp:
                    this.tmp_Progress.text = string.Format("EXP:{0}/{1}",current,total);
                    break;
                default:
                    this.tmp_Progress.text = string.Format("{0}/{1}", current, total);
                    break;
            }
        }
        private void OnHeroInfoUpdateEvent(HeroInfoUpdateEvent e)
        {
            this.SetProgress(this.hero.Exp, this.hero.UpExp);
        }
    }
}