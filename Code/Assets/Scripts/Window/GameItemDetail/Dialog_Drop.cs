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

            this.Txt_Msg.text = e.Message;

            Dictionary<string, int> mergeDict = new Dictionary<string, int>();

            for (int i = 0; i < e.Items.Count; i++)
            {
                Item item = e.Items[i];

                if (item.Type == ItemType.Card)
                {
                    MergeDict(mergeDict, "Í¼¼ø");
                }
                else if (item.Type == ItemType.Fashion)
                {
                    MergeDict(mergeDict, "Ê±×°");
                }
                else
                {
                    Box_Drop box = PrefabHelper.Instance().CreateBoxDrop(Container.content, item);
                    ItemList.Add(box);
                }
            }

            foreach (var sp in mergeDict)
            {
                Box_Drop box = PrefabHelper.Instance().CreateBoxDrop(Container.content, sp.Key, 1, sp.Value);
                ItemList.Add(box);
            }
        }

        private void MergeDict(Dictionary<string, int> dict, string name)
        {
            if (!dict.ContainsKey(name))
            {
                dict[name] = 0;
            }

            dict[name]++;
        }



        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

    }
}
