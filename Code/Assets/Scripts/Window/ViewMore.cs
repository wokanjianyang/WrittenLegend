using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewMore : AViewPage
    {
        [LabelText("副本容器")]
        public RectTransform scrollRect;

        [LabelText("装备副本")]
        public Item_EquipCopy EquipCopy;

        public Dialog_BossInfo BossInfo;

        public Dialog_Phantom Phantom;


        void Start()
        {
  
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();
            GameProcessor.Inst.EventCenter.AddListener<EndCopyEvent>(this.OnEndCopy);
            GameProcessor.Inst.EventCenter.AddListener<PhantomEndEvent>(this.OnPhantomEnd);
        }

        public void SelectMap(int mapId,int rate)
        {
            scrollRect.gameObject.SetActive(false);
            BossInfo.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new StartCopyEvent() { MapId = mapId ,Rate= rate});
        }

        public void SelectPhantomMap(int configId)
        {
            scrollRect.gameObject.SetActive(false);
            Phantom.gameObject.SetActive(false);
            GameProcessor.Inst.EventCenter.Raise(new PhantomStartEvent() { PhantomId = configId });
        }

        public void OnEndCopy(EndCopyEvent e) {
            scrollRect.gameObject.SetActive(true);
        }
        public void OnPhantomEnd(PhantomEndEvent e)
        {
            scrollRect.gameObject.SetActive(true);
        }
        

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_More;
        }
    }
}
