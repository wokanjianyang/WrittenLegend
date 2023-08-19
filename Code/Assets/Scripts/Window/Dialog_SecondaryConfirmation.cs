﻿using System;
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
            GameProcessor.Inst.EventCenter.AddListener<SecondaryConfirmTextEvent>(this.OnSecondaryConfirmTextEvent);
            GameProcessor.Inst.EventCenter.AddListener<SecondaryConfirmCloseEvent>(this.OnSecondaryConfirmClose);

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

        private void OnSecondaryConfirmTextEvent(SecondaryConfirmTextEvent e)
        {
            this.txt_Msg.text = e.Text;
        }

        private void OnSecondaryConfirmClose(SecondaryConfirmCloseEvent e)
        {
            this.gameObject.SetActive(false);
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
