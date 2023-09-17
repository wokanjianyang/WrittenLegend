using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Exclusive : MonoBehaviour, IBattleLife
    {
        public Button Btn_Close;

        private List<Item_Achivement> activeItems = new List<Item_Achivement>();

        private GameObject ItemPrefab;

        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
        }

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ShowExclusive>(this.OnShowExclusive);


            ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item_Achievement");

            Init();
        }

        private void Init()
        {
            


        }

        public void OnShowExclusive(ShowExclusive e)
        {
            this.gameObject.SetActive(true);
        }


        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
