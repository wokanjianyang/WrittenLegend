using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    [Serializable]
    public class PetSpeicalItemSelectEvent : UnityEvent<int> { } // 支持int和string参数

    public class Item_Pet_Speical : MonoBehaviour, IPointerClickHandler
    {

        public Text Txt_Name;

        private int Id = 0;
        private string[] names = { "天龙", "神凤", "圣麟" };

        [SerializeField]
        private PetSpeicalItemSelectEvent _onValueChanged = new PetSpeicalItemSelectEvent();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void OnEnable()
        {

        }

        public void AddListener(UnityAction<int> callback)
        {
            _onValueChanged.AddListener(callback);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_onValueChanged != null)
            {
                _onValueChanged.Invoke(Id);
            }
        }

        public void SetContent(int i)
        {
            this.Id = i;

            User user = GameProcessor.Inst.User;

            int level = user.GetPetSpeicalLevel(Id);
            int layer = user.GetPetSpeicalLayer(Id);

            this.Txt_Name.text = names[i - 1] + layer + "阶" + level + "级";

        }
    }
}
