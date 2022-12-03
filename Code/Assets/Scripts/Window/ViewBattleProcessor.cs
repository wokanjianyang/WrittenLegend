using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ViewBattleProcessor : AViewPage
    {
        private bool isViewMapShowing = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }


        protected override bool CheckPageType(ViewPageType page)
        {
            var ret = page == ViewPageType.View_Battle;
            if(ret)
            {
                GameProcessor.Inst.Resume();
            }
            else if (this.isViewMapShowing)
            {
                GameProcessor.Inst.Pause();
            }
            this.isViewMapShowing = ret;
            return ret;
        }
    }
}
