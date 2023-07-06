using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TouchElement : MonoBehaviour,ItouchIgnore
    {
        [LabelText("触摸忽略类型")]
        public TouchIgnoreType TouchIgnore;

        TouchIgnoreType ItouchIgnore.TouchType { get => this.TouchIgnore; }

        private List<RectTransform> rectTransforms;

        // Start is called before the first frame update
        void Start()
        {
            this.rectTransforms = new List<RectTransform>();
            this.rectTransforms.Add(this.transform.GetComponent<RectTransform>());
            //this.rectTransforms.AddRange(this.transform.GetComponentsInChildren<RectTransform>());
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void CheckPoint(Vector2 point)
        {
            if (this.gameObject.activeSelf)
            {
                foreach (var rect in this.rectTransforms)
                {
                    var ret = RectTransformUtility.RectangleContainsScreenPoint(rect, point);
                    if (ret)
                    {
                        return;
                    }
                }

                this.gameObject.SetActive(false);
            }
        }

        public bool RaycastFilter(Vector2 point)
        {
            bool block = false;
            switch (this.TouchIgnore)
            {
                case TouchIgnoreType.HideWithTouchEmpty:
                    // block = true;
                    foreach (var rect in this.rectTransforms)
                    {
                        var ret = RectTransformUtility.RectangleContainsScreenPoint(rect, point);
                        
                        block = ret;
                        
                        break;
                    }
                    break;
                case TouchIgnoreType.HideWithCloseBtn:
                    block = false;
                    foreach (var rect in this.rectTransforms)
                    {
                        var ret = RectTransformUtility.RectangleContainsScreenPoint(rect, point);
                        if (ret)
                        {
                            block = true;
                            break;
                        }
                    }
                    break;
                default:
                    
                    break;
            }
            return block;
        }
    }
}
