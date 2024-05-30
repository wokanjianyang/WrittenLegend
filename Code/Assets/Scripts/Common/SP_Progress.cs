using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SP_Progress : MonoBehaviour
    {
        [Title("进度条")]
        [LabelText("进度背景")]
        public Image img_Progress;

        [LabelText("进度文本")]
        public Text tmp_Progress;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int Order => (int)ComponentOrder.Progress;

        public void SetProgress(double current, double total)
        {
            double value = current / total;
            if (value > 1)
            {
                value = 1f;
            }
            this.img_Progress.fillAmount = (float)value;
            this.tmp_Progress.text = StringHelper.FormatNumber(current);
        }
    }
}