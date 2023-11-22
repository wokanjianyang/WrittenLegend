using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Com_Progress : MonoBehaviour, IBattleLife
    {
        [Title("进度条")]
        [LabelText("进度背景")]
        public Image img_Progress;

        [LabelText("进度文本")]
        public Text tmp_Progress;

        [LabelText("进度条类型")]
        public ProgressType ProgressType;

        private User user;

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
            this.user = GameProcessor.Inst.User;
            switch (this.ProgressType)
            {
                case ProgressType.PlayerExp:
                    this.OnHeroInfoUpdateEvent(null);
                    user.EventCenter.AddListener<UserInfoUpdateEvent>(this.OnHeroInfoUpdateEvent);
                    break;
                case ProgressType.SkillExp:
                    break;
            }
        }
        public void SetProgress(double current, double total)
        {
            double value = current / total;
            if (value > 1)
            {
                value = 1f;
                //current = total;
            }
            this.img_Progress.fillAmount = (float)value;
            switch (this.ProgressType)
            {
                case ProgressType.PlayerExp:
                    this.tmp_Progress.text = string.Format("EXP:{0}/{1}", StringHelper.FormatNumber(current), StringHelper.FormatNumber(total));
                    break;
                default:
                    this.tmp_Progress.text = string.Format("{0}/{1}", StringHelper.FormatNumber(current), StringHelper.FormatNumber(total));
                    break;
            }
        }
        private void OnHeroInfoUpdateEvent(UserInfoUpdateEvent e)
        {
            this.SetProgress(this.user.MagicExp.Data, this.user.MagicUpExp.Data);
        }
    }
}