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
        public Button Btn_OK;

        public Text Txt_Msg;
        public ScrollRect Container;
        //public RectTransform Container;


        private List<Box_Drop> ItemList = new List<Box_Drop>();

        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
            Btn_OK.onClick.AddListener(OnClick_Close);
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

            this.Txt_Msg.text = "获取金币:" + StringHelper.FormatNumber(e.Gold) + "，经验：" + StringHelper.FormatNumber(e.Exp);

            for (int i = 0; i < e.Items.Count; i++)
            {
                Box_Drop box = PrefabHelper.Instance().CreateBoxDrop(Container.content, e.Items[i]);

                ItemList.Add(box);
            }
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

    }
}
