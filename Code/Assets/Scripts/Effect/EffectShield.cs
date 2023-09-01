using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EffectShield : MonoBehaviour
    {
        private string EffectPath = "";

        [LabelText("特效显示")]
        public Image img_Effect;

        [LabelText("重设图片大小")]
        public bool NeedReSize = true;

        private Sprite[] imgs;
        private int currentIndex = 0;
        private int totalCount = 0;

        private bool hasEffect = false;

        private float currentTime = 0f;

        private void Start()
        {
            this.imgs = Resources.LoadAll<Sprite>(this.EffectPath);
            if (imgs != null && imgs.Length > 0)
            {
                this.hasEffect = this.imgs.Length > 0;
                this.totalCount = this.imgs.Length;
            }
        }

        private void Update()
        {
            if (this.hasEffect)
            {
                this.currentTime += Time.deltaTime;

                if (this.currentTime > 1f / this.totalCount)
                {
                    this.currentTime = 0;

                    var sprite = this.imgs[this.currentIndex < this.totalCount ? this.currentIndex : (this.totalCount - 1)];
                    this.img_Effect.sprite = sprite;
                    this.img_Effect.SetNativeSize();

                    this.currentIndex++;
                }
            }
        }
        public void SetData(string effectPath)
        {
            this.EffectPath = effectPath;
        }
    }
}
