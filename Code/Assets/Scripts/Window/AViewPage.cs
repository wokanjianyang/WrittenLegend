using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    abstract public class AViewPage : MonoBehaviour, IBattleLife
    {

        private bool isInit = false;

        private void OnChangePageEnvent(ChangePageEvent e)
        {
            this.gameObject.SetActive(this.CheckPageType(e.Page));
            if (!isInit)
            {
                isInit = true;
                this.OnInit();
            }
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
        
        virtual public void OnInit()
        {
            
        }

        virtual public void OnOpen()
        {
            
        }
    }
}
