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
        [LabelText("��������")]
        public RectTransform scrollRect;

        [LabelText("װ������")]
        public Item_EquipCopy EquipCopy;

        public Dialog_BossInfo BossInfo;

        public Dialog_Phantom Phantom;

        public Dialog_BossFamily BossFamily;

        public Item_EquipCopy LegacyItem;
        public Dialog_Copy_Legacy LegacyDialog;

        public Item_EquipCopy MineItem;
        public Dialog_Mine MineDialog;



        void Start()
        {
        }

        void OnEnable()
        {
            User user = GameProcessor.Inst.User;

            if (user == null)
            {
                return;
            }

            long level = user.MagicLevel.Data;
            int mc = user.GetArtifactValue(ArtifactType.MineCount);
            if (level > 20000 || mc > 0)
            {
                MineItem.gameObject.SetActive(true);
            }
            else
            {
                MineItem.gameObject.SetActive(false);
            }

            int mapId = user.MapId;
            if (mapId >= 1070)
            {
                LegacyItem.gameObject.SetActive(true);
            }
            else
            {
                LegacyItem.gameObject.SetActive(false);
            }
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<CloseViewMoreEvent>(this.OnClose);
            GameProcessor.Inst.EventCenter.AddListener<CopyViewCloseEvent>(this.OnCopyViewClose);

            GameProcessor.Inst.EventCenter.AddListener<OpenMineEvent>(this.OpenMine);
            GameProcessor.Inst.EventCenter.AddListener<OpenLegacyEvent>(this.OpenLegacy);

            GameProcessor.Inst.EventCenter.AddListener<BattlerEndEvent>(this.OnBattlerEnd);
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

        public void StartBossFamily(int level, int rate)
        {
            User user = GameProcessor.Inst.User;

            long bossTicket = user.GetMaterialCount(ItemHelper.SpecialId_Boss_Ticket);

            if (bossTicket < rate)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "û���㹻��BOSS��ս��", ToastType = ToastTypeEnum.Failure });
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


        public void OnBattlerEnd(BattlerEndEvent e)
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

        private void OpenLegacy(OpenLegacyEvent e)
        {
            LegacyDialog.gameObject.SetActive(true);
        }

        public void StartLegacy(int mapId, int layer)
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new LegacyStartEvent() { MapId = mapId, Layer = layer });
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_More;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            scrollRect.gameObject.SetActive(true);
        }
    }
}
