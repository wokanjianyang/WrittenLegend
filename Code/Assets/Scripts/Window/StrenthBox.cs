using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class StrenthBox : MonoBehaviour, IPointerClickHandler
    {
        [Title("插槽")]
        [LabelText("类型")]
        public SlotType SlotType;

        // Start is called before the first frame update
        void Start()
        {
            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_White");
            var box = GameObject.Instantiate(prefab, this.transform);
            box.GetComponent<Com_Box>().tmp_Title.text = this.SlotType.ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //高亮，更换UI
            int position = ((int)SlotType);

            GameProcessor.Inst.EventCenter.Raise(new EquipStrengthSelectEvent() { Position = position });

            Debug.Log("qianghua dianji ");
        }
    }
}
