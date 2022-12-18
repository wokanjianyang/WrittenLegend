using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class TouchIgnore : MonoBehaviour, IPointerClickHandler, ICanvasRaycastFilter
    {
        public Transform Top;
        void Start()
        {
            
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            var coms = Top.GetComponentsInChildren<MonoBehaviour>();
            var ignores = coms.Where(com => com is ItouchIgnore).ToList();
            if (ignores != null && ignores.Count > 0)
            {
                foreach (var ignore in ignores)
                {
                    if (ignore is ItouchIgnore _ignore)
                    {
                        switch (_ignore.TouchType)
                        {
                            case TouchIgnoreType.HideWithTouchEmpty:
                                _ignore.CheckPoint(eventData.position);
                                break;
                        }
                    }
                }
            }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            var coms = Top.GetComponentsInChildren<MonoBehaviour>();
            var ignores = coms.Where(com => com is ItouchIgnore).ToList();
            if (ignores != null && ignores.Count > 0)
            {
                foreach (var ignore in ignores)
                {
                    if (!ignore.gameObject.activeSelf) continue;
                    if (ignore is ItouchIgnore _ignore)
                    {
                        return _ignore.RaycastFilter(sp);
                    }
                }
            }
            return true;
        }
    }
}