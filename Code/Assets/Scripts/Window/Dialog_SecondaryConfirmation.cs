using System;
using UnityEngine;

namespace Game
{
    public class Dialog_SecondaryConfirmation : MonoBehaviour, IBattleLife
    {
        private Action doneAction;
        private Action cancleAction;

        void Start()
        {
            this.gameObject.SetActive(false);
        }
        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<SecondaryConfirmationEvent>(this.OnSecondaryConfirmationEvent);
            GameProcessor.Inst.ShowSecondaryConfirmationDialog += this.OnShow;
        }

        public int Order => (int)ComponentOrder.Dialog;

        private void OnShow(Action done, Action cancle)
        {
            this.gameObject.SetActive(true);
            this.doneAction = done;
            this.cancleAction = cancle;
        }
        
        private void OnSecondaryConfirmationEvent(SecondaryConfirmationEvent e)
        {
            this.gameObject.SetActive(true);
        }

        public void OnClick_Done()
        {
            this.gameObject.SetActive(false);
            this.doneAction?.Invoke();
        }

        public void OnClick_Cancle()
        {
            this.gameObject.SetActive(false);
            this.cancleAction?.Invoke();
        }
    }
}
