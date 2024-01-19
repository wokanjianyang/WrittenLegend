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

        public List<Button> Btn_Plan_List = new List<Button>();

        public Toggle toggle;

        public int Order => (int)ComponentOrder.Dialog;

        void Awake()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);

            for (int i = 0; i < Btn_Plan_List.Count; i++)
            {
                int index = i;
                Btn_Plan_List[i].onClick.AddListener(() =>
                {
                    ChangePlan(index);
                });
            }

            toggle.onValueChanged.AddListener((isOn) =>
            {
                GameProcessor.Inst.User.ExclusiveSetting = isOn;
            });
        }

        void Start()
        {
            toggle.isOn = GameProcessor.Inst.User.ExclusiveSetting;
        }

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ShowExclusiveEvent>(this.OnShowExclusive);

            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");

            SlotBox[] items = this.GetComponentsInChildren<SlotBox>();

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Init(prefab);
            }
        }

        private void ChangePlan(int i)
        {
            GameProcessor.Inst.EventCenter.Raise(new ChangeExclusiveEvent() { Index = i });
        }

        public void OnShowExclusive(ShowExclusiveEvent e)
        {
            this.gameObject.SetActive(true);
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
