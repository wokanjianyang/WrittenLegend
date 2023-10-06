using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SlotBox : MonoBehaviour
    {
        [Title("插槽")]
        [LabelText("类型")]
        public SlotType SlotType;

        private Com_Box equip;

        private Com_Box baseInfo;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(GameObject prefab)
        {
            var box = GameObject.Instantiate(prefab, this.transform);
            baseInfo = box.GetComponent<Com_Box>();
            baseInfo.tmp_Title.text = this.SlotType.ToString();
        }

        public void Equip(Com_Box equip)
        {
            this.equip = equip;
            baseInfo.gameObject.SetActive(false);
        }
        public void UnEquip()
        {
            this.equip = null;
            baseInfo.gameObject.SetActive(true);
        }
        public Com_Box GetEquip()
        {
            return this.equip;
        }
    }
}
