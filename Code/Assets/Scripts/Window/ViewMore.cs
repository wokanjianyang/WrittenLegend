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

        void Start()
        {
  
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();
            GameProcessor.Inst.EventCenter.AddListener<EndCopyEvent>(this.OnEndCopy);
        }

        public void SelectMap(int mapId)
        {
            scrollRect.gameObject.SetActive(false);
            BossInfo.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new StartCopyEvent() { MapId = mapId });
        }

        public void OnEndCopy(EndCopyEvent e) {
            scrollRect.gameObject.SetActive(true);
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_More;
        }
    }
}
