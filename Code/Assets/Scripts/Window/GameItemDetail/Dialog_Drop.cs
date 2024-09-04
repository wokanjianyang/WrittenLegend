using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Drop : MonoBehaviour, IBattleLife
    {
        public Button Btn_Close;

        public Text Txt_Msg;
        public RectTransform Container;


        private List<Gift_Item> ItemList = new List<Gift_Item>();

        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
        }


        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ShowDropEvent>(this.OnShow);
        }

        private void Init()
        {
            //clear
            foreach (var si in ItemList)
            {
                GameObject.Destroy(si.gameObject);
            }
            ItemList.Clear();
        }

        public void OnShow(ShowDropEvent e)
        {
            this.Init();
            this.gameObject.SetActive(true);


      

        }


        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

    }
}
