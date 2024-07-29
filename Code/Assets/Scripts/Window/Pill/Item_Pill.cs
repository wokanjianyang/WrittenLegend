using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Pill : MonoBehaviour
    {
        public Toggle toggle;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {

        }

        public void Active(bool isOn)
        {
            this.toggle.isOn = isOn;
        }
    }
}