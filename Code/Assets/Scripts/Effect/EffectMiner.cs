using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EffectMiner : MonoBehaviour
    {
        [LabelText("特效路径")]
        public string EffectPath = "";

        [LabelText("特效显示")]
        public Image img_Effect;

        private Sprite[] imgs;
        private int currentIndex = 0;
        private int totalCount = 0;

        private float currentTime = 0f;

        private float rotation = 0;

        private float AnimaTime = 0.8f;

        private void Start()
        {
            this.imgs = Resources.LoadAll<Sprite>(this.EffectPath);
            if (imgs != null && imgs.Length > 0)
            {
                this.totalCount = this.imgs.Length;
            }
        }

        private void Update()
        {

            this.currentTime += Time.deltaTime;
            if (this.currentTime > AnimaTime / this.totalCount)
            {
                this.currentTime = 0;
                var sprite = this.imgs[this.currentIndex++ % this.totalCount];
                this.img_Effect.sprite = sprite;

                if (rotation > 0)
                {
                    this.img_Effect.rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotation);
                }
            }
        }
    }
}
