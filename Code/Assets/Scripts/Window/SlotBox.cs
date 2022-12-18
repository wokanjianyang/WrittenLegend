using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class SlotBox : MonoBehaviour
    {
        [Title("≤Â≤€")]
        [LabelText("Œª÷√")]
        public SlotType SlotType;

        private Com_Box equip;

        // Start is called before the first frame update
        void Start()
        {
            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");
            var box = GameObject.Instantiate(prefab, this.transform);
            box.GetComponent<Com_Box>().tmp_Title.text = this.SlotType.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Equip(Com_Box equip)
        {
            this.equip = equip;
        }
        public void UnEquip()
        {
            this.equip = null;
        }
        public Com_Box GetEquip()
        {
            return this.equip;
        }
    }
}
