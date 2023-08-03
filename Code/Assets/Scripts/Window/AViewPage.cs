using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    abstract public class AViewPage : MonoBehaviour, IBattleLife
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnChangePageEnvent(ChangePageEvent e)
        {
            this.gameObject.SetActive(this.CheckPageType(e.Page));
            if (this.CheckPageType(e.Page))
            {
                this.OnOpen();
            }
        }

        protected abstract bool CheckPageType(ViewPageType page);

        virtual public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ChangePageEvent>(this.OnChangePageEnvent);
            this.OnChangePageEnvent(new ChangePageEvent
            {
                Page = ViewPageType.View_Battle
            });
        }

        public int Order
        {
            get
            {
                return (int)ComponentOrder.ViewPage;
            }
        }

        virtual public void OnOpen()
        {
            
        }
    }
}
