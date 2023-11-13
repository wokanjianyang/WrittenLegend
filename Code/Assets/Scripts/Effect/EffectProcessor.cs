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

        [LabelText("重设图片大小")]
        public bool NeedReSize = true;
        
        private Sprite[] imgs;
        private int currentIndex = 0;
        private int totalCount = 0;
        
        private bool hasEffect = false;

        private float currentTime = 0f;

        private float rotation = 0;

        private float AnimaTime = 0.75f;

        private void Start()
        {
            this.imgs = Resources.LoadAll<Sprite>(this.EffectPath);
            if (imgs != null && imgs.Length>0)
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
                if (this.currentTime > AnimaTime / this.totalCount)
                {
                    this.currentTime = 0;
                    var sprite = this.imgs[this.currentIndex++ % this.totalCount];
                    this.img_Effect.sprite = sprite;

                    if (rotation > 0)
                    {
                        this.img_Effect.rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotation);
                    }
                    if (this.NeedReSize)
                    {
                        this.img_Effect.SetNativeSize();
                    }
                    if (this.currentIndex >= this.totalCount)
                    {
                        this.hasEffect = this.Loop;
                        this.gameObject.SetActive(this.Loop);
                    }
                }
            }
        }



        public void SetData(bool loop, float rotation,float animaTime)
        {
            this.Loop = loop;
            this.rotation = rotation;
            if (animaTime > 0)
            {
                this.AnimaTime = animaTime;
            }
        }
    }
}
