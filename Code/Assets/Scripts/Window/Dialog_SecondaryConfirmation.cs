using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_SecondaryConfirmation : MonoBehaviour, IBattleLife
    {
        public Text txt_Msg;
        
        private Action doneAction;
        private Action cancleAction;

        void Start()
        {
        }
        public void OnBattleStart()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.AddListener<SecondaryConfirmationEvent>(this.OnSecondaryConfirmationEvent);
            GameProcessor.Inst.ShowSecondaryConfirmationDialog += this.OnShow;
        }

        public int Order => (int)ComponentOrder.Dialog;

        private void OnShow(string msg,Action done, Action cancle)
        {
            this.gameObject.SetActive(true);
            this.txt_Msg.text = msg;
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
