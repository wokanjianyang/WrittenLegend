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

        public Dialog_BossFamily BossFamily;

        public Item_EquipCopy MineItem;
        public Dialog_Mine MineDialog;

        void Start()
        {
            long level = GameProcessor.Inst.User.MagicLevel.Data;
            if (level > 20000)
            {
                MineItem.gameObject.SetActive(true);
            }
            else
            {
                MineItem.gameObject.SetActive(false);
            }
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<CloseViewMoreEvent>(this.OnClose);

            GameProcessor.Inst.EventCenter.AddListener<EndCopyEvent>(this.OnEndCopy);
            GameProcessor.Inst.EventCenter.AddListener<PhantomEndEvent>(this.OnPhantomEnd);
            GameProcessor.Inst.EventCenter.AddListener<CopyViewCloseEvent>(this.OnCopyViewClose);
            GameProcessor.Inst.EventCenter.AddListener<BossFamilyEndEvent>(this.OnBossFamilyEnd);

            GameProcessor.Inst.EventCenter.AddListener<OpenMineEvent>(this.OpenMine);

            GameProcessor.Inst.EventCenter.AddListener<DefendEndEvent>(this.OnDefendEnd);
        }

        public void OnClose(CloseViewMoreEvent e)
        {
            scrollRect.gameObject.SetActive(false);
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

        public void StartBossFamily(int level, int rate)
        {
            User user = GameProcessor.Inst.User;

            long bossTicket = user.GetMaterialCount(ItemHelper.SpecialId_Boss_Ticket);

            if (bossTicket < rate)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有足够的BOSS挑战卷", ToastType = ToastTypeEnum.Failure });
                return;
            }

            BossFamily.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = ItemHelper.SpecialId_Boss_Ticket,
                Quantity = rate
            });

            user.MagicRecord[AchievementSourceType.BossFamily].Data += rate;

            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new BossFamilyStartEvent() { Level = level, Rate = rate });
        }

        public void OnBossFamilyEnd(BossFamilyEndEvent e)
        {
            scrollRect.gameObject.SetActive(true);
        }

        public void OnDefendEnd(DefendEndEvent e)
        {
            scrollRect.gameObject.SetActive(true);
        }

        public void StartAnDian()
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new AnDianStartEvent() { });
        }

        public void StartDefend(int level)
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new DefendStartEvent());
        }

        public void StartHeroPhantom()
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new HeroPhatomStartEvent() { });
        }

        public void StartInfinite()
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new InfiniteStartEvent() { });
        }

        private void OpenMine(OpenMineEvent e)
        {
            MineDialog.gameObject.SetActive(true);
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_More;
        }
    }
}
