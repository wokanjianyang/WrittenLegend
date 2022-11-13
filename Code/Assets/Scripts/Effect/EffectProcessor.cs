using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EffectProcessor : MonoBehaviour
    {
        [LabelText("特效路径")]
        public string EffectPath = "";

        [LabelText("是否循环播放")]
        public bool Loop = true;

        [LabelText("特效显示")]
        public Image img_Effect;

        private Sprite[] imgs;
        private int currentIndex = 0;
        private int totalCount = 0;
        
        private bool hasEffect = false;

        private const float frameTime = 1f/10;
        private float currentTime = 0f;

        
        private void Start()
        {
            this.imgs = Resources.LoadAll<Sprite>("UI/Buff斩杀素材/" + this.EffectPath);
            if (imgs != null)
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
                if (this.currentTime > frameTime)
                {
                    this.currentTime = 0;
                    var sprite = this.imgs[this.currentIndex++ % this.totalCount];
                    this.img_Effect.sprite = sprite;
                    this.img_Effect.SetNativeSize();
                    if (this.currentIndex >= this.totalCount)
                    {
                        this.hasEffect = this.Loop;
                        this.gameObject.SetActive(this.Loop);
                    }
                }
            }
        }

        public void SetData(string effectPath, bool loop)
        {
            this.EffectPath = effectPath;
            this.Loop = loop;
        }
    }
}
