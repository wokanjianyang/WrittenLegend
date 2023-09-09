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

        public Button BtnBossFamily;

        void Start()
        {
            BtnBossFamily.onClick.AddListener(OnClickBossFamily);
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();


            GameProcessor.Inst.EventCenter.AddListener<EndCopyEvent>(this.OnEndCopy);
            GameProcessor.Inst.EventCenter.AddListener<PhantomEndEvent>(this.OnPhantomEnd);
            GameProcessor.Inst.EventCenter.AddListener<CopyViewCloseEvent>(this.OnCopyViewClose);
            GameProcessor.Inst.EventCenter.AddListener<BossFamilyEndEvent>(this.OnBossFamilyEnd);

        }

        public void SelectMap(int mapId, int rate)
        {
            scrollRect.gameObject.SetActive(false);
            BossInfo.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new StartCopyEvent() { MapId = mapId, Rate = rate });
        }

        public void SelectPhantomMap(int configId)
        {
            scrollRect.gameObject.SetActive(false);
            Phantom.gameObject.SetActive(false);
            GameProcessor.Inst.EventCenter.Raise(new PhantomStartEvent() { PhantomId = configId });
        }

        public void OnCopyViewClose(CopyViewCloseEvent e)
        {
            scrollRect.gameObject.SetActive(false);
            Phantom.gameObject.SetActive(false);
        }

        public void OnEndCopy(EndCopyEvent e)
        {
            scrollRect.gameObject.SetActive(true);
        }
        public void OnPhantomEnd(PhantomEndEvent e)
        {
            scrollRect.gameObject.SetActive(true);
        }

        public void OnClickBossFamily()
        {
            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认挑战？", true, () =>
             {
                 User user = GameProcessor.Inst.User;

                 long bossTicket = user.GetMaterialCount(ItemHelper.SpecialId_Boss_Ticket);

                 if (bossTicket <= 0)
                 {

                     GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的BOSS挑战卷", ToastType = ToastTypeEnum.Failure });
                     return;
                 }

                 GameProcessor.Inst.EventCenter.Raise(new MaterialUseEvent()
                 {
                     MaterialId = ItemHelper.SpecialId_Boss_Ticket,
                     Quantity = 1
                 });

                 user.MagicRecord[AchievementSourceType.BossFamily].Data++;

                 scrollRect.gameObject.SetActive(false);

                 GameProcessor.Inst.EventCenter.Raise(new BossFamilyStartEvent() { });

             }, null);
        }
        public void OnBossFamilyEnd(BossFamilyEndEvent e)
        {
            scrollRect.gameObject.SetActive(true);
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_More;
        }
    }
}
