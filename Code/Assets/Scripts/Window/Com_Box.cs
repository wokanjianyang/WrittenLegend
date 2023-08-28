using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Com_Box : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
    {

        [Title("物品格")]
        [LabelText("道具名")]
        public Text tmp_Title;

        [LabelText("数量")]
        public Text tmp_Count;

        public GameObject go_Lock;

        public BoxItem Item { get; private set; }
        public int boxId { get; private set; }

        public int EquipPosition { get; private set; }
        public long Count { get; private set; }

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
            if (this.Item == null) return;
            GameProcessor.Inst.EventCenter.Raise(new ShowEquipDetailEvent()
            {
                Position = this.transform.position,
                Item = this.Item.Item,
                BoxId = this.boxId,
                EquipPosition = this.EquipPosition
            });
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
        public void SetItem(BoxItem item)
        {
            this.tmp_Title.text = item.Item.Name;

            this.Item = item;

            this.Count = item.MagicNubmer.Data;
            
            this.go_Lock.gameObject.SetActive(item.Item.IsLock);

            this.tmp_Count.transform.gameObject.SetActive(this.Count > 1);
            this.tmp_Count.text = this.Count.ToString();
        }

        public void SetBoxId(int id)
        {
            this.boxId = id;
        }

        public void SetEquipPosition(int position)
        {
            this.EquipPosition = position;
        }

        public void AddStack(long quantity)
        {
            this.Count+= quantity;
            this.tmp_Count.transform.gameObject.SetActive(this.Count != 1);
            this.tmp_Count.text = this.Count.ToString();
        }

        public void RemoveStack(long quantity)
        {
            this.Count -= quantity;
            this.tmp_Count.transform.gameObject.SetActive(this.Count != 1);
            this.tmp_Count.text = this.Count.ToString();
        }

        public void SetLock(bool isLock)
        {
            this.go_Lock.gameObject.SetActive(isLock);
        }
    }
}