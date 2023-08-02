using UnityEngine;

namespace Game
{
    public class Dialog_SecondaryConfirmation : MonoBehaviour, IBattleLife
    {
        

        void Start()
        {
            this.gameObject.SetActive(false);
        }
        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<SecondaryConfirmationEvent>(this.OnSecondaryConfirmationEvent);
        }

        public int Order => (int)ComponentOrder.Dialog;

        private void OnSecondaryConfirmationEvent(SecondaryConfirmationEvent e)
        {
            this.gameObject.SetActive(true);
        }

        public void OnClick_Done()
        {
            this.gameObject.SetActive(false);
            GameProcessor.Inst.OnSecondaryConfirmationDoneAction?.Invoke();
        }

        public void OnClick_Cancle()
        {
            this.gameObject.SetActive(false);
            GameProcessor.Inst.OnSecondaryConfirmationCancleAction?.Invoke();
        }
    }
}
