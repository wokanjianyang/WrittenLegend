using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class Com_Box : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
    {

        [Title("格子信息")]
        [LabelText("道具名称")]
        public TextMeshProUGUI tmp_Title;

        [LabelText("道具数量")]
        public TextMeshProUGUI tmp_Count;

        public int itemId;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowEquipDetailEvent()
            {
                Position = this.transform.position,
                ItemId = this.itemId,
                BoxId = this.transform.GetComponent<Com_Item>().boxId
            });
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

    }
}