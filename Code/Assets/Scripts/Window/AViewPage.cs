using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    abstract public class AViewPage : MonoBehaviour, IBattleLife
    {

        // Start is called before the first frame update
        virtual public void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnChangePageEnvent(ChangePageEvent e)
        {
            this.transform.localScale = this.CheckPageType(e.Page) ? Vector3.one : Vector3.zero;
        }

        protected abstract bool CheckPageType(ViewPageType page);

        virtual public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ChangePageEvent>(this.OnChangePageEnvent);
        }

        public int Order
        {
            get
            {
                return (int)ComponentOrder.ViewPage;
            }
        }
    }
}
