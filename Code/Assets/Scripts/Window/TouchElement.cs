using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TouchElement : MonoBehaviour,ItouchIgnore
    {
        [LabelText("µã»÷ºöÂÔÀàÐÍ")]
        public TouchIgnoreType TouchIgnore;

        TouchIgnoreType ItouchIgnore.TouchType { get => this.TouchIgnore; }

        private List<RectTransform> rectTransforms;

        // Start is called before the first frame update
        void Start()
        {
            this.rectTransforms = new List<RectTransform>();
            this.rectTransforms.Add(this.transform.GetComponent<RectTransform>());
            this.rectTransforms.AddRange(this.transform.GetComponentsInChildren<RectTransform>());
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void CheckPoint(Vector2 point)
        {
            foreach(var rect in this.rectTransforms)
            {
                var ret = RectTransformUtility.RectangleContainsScreenPoint(rect, point);
                if (ret)
                {
                    return;
                }
            }

            this.transform.localScale = Vector3.zero;
        }
    }
}
