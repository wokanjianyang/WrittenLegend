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

        public Item_EquipCopy LegacyItem;
        public Dialog_Copy_Legacy LegacyDialog;

        public Item_EquipCopy MineItem;

        public Item_EquipCopy BabelItem;

        public Item_EquipCopy InfiniteItem;

        public Item_EquipCopy PillItem;
        public Map_Dialog_Pill MapDialogPill;

        public Map_Dialog_Babel MapDialogBabel;

        public Item_EquipCopy MythItem;
        public Map_Dialog_Myth MapDialogMyth;

        public Button Btn_World;
        public Map_Dialog_World MapDialogWorld;

        public Button Btn_Festive;
        public Map_Dialog_Festive MapDialogFestive;

        public Button Btn_Shengxiao;
        public Map_Dialog_Shengxiao MapDialogShengxiao;

        public Button Btn_Spirit;
        public Map_Dialog_Spirit MapDialogSpirit;

        public Text Txt_Limit;

        void Start()
        {

            Btn_World.onClick.AddListener(OnClick_World);
            Btn_Festive.onClick.AddListener(OnClick_Festive);
            Btn_Shengxiao.onClick.AddListener(OnClick_Shengxiao);
            Btn_Spirit.onClick.AddListener(OnClick_Spirit);
        }

        void OnEnable()
        {
            User user = GameProcessor.Inst.User;

            if (user == null)
            {
                return;
            }

            long level = user.MagicLevel.Data;
            int mc = user.GetArtifactValue(ArtifactType.MineCount) + user.GetLimitMineCount2();
            if (level > 20000 || mc > 0)
            {
                MineItem.gameObject.SetActive(true);
            }
            else
            {
                MineItem.gameObject.SetActive(false);
            }

            if (level > 30000 || user.Cycle.Data > 0)
            {
                InfiniteItem.gameObject.SetActive(true);
            }
            else
            {
                InfiniteItem.gameObject.SetActive(false);
            }

            if (level > 50000 || user.Cycle.Data > 0)
            {
                BabelItem.gameObject.SetActive(true);
            }
            else
            {
                BabelItem.gameObject.SetActive(false);
            }

            if (user.Cycle.Data > 0)
            {
                PillItem.gameObject.SetActive(true);
                MythItem.gameObject.SetActive(true);
            }
            else
            {
                PillItem.gameObject.SetActive(false);
                MythItem.gameObject.SetActive(false);
            }

            if (user.Cycle.Data >= 4)
            {
                Btn_World.gameObject.SetActive(true);
            }
            else
            {
                Btn_World.gameObject.SetActive(false);
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

            //如果是节日期间

            if (DropLimitConfigCategory.Instance.CheckIsTime() && user.Cycle.Data >= 1)
            {
                Btn_Festive.gameObject.SetActive(true);
            }
            else
            {
                Btn_Festive.gameObject.SetActive(false);
            }

            if (user.Cycle.Data >= 10)
            {
                Btn_Shengxiao.gameObject.SetActive(true);
            }
            else
            {
                Btn_Shengxiao.gameObject.SetActive(false);
            }

            if (user.Cycle.Data >= 15)
            {
                Btn_Spirit.gameObject.SetActive(true);
            }
            else
            {
                Btn_Spirit.gameObject.SetActive(false);
            }
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<CloseViewMoreEvent>(this.OnClose);
            GameProcessor.Inst.EventCenter.AddListener<CopyViewCloseEvent>(this.OnCopyViewClose);

            GameProcessor.Inst.EventCenter.AddListener<OpenLegacyEvent>(this.OpenLegacy);
            GameProcessor.Inst.EventCenter.AddListener<OpenPillEvent>(this.OpenPill);
            GameProcessor.Inst.EventCenter.AddListener<OpenBabelEvent>(this.OpenBabel);
            GameProcessor.Inst.EventCenter.AddListener<OpenMythEvent>(this.OpenMyth);

            GameProcessor.Inst.EventCenter.AddListener<BattlerEndEvent>(this.OnBattlerEnd);
        }

        private void OnClick_World()
        {
            this.MapDialogWorld.gameObject.SetActive(true);
        }
        private void OnClick_Festive()
        {
            this.MapDialogFestive.gameObject.SetActive(true);
        }

        private void OnClick_Shengxiao()
        {
            this.MapDialogShengxiao.gameObject.SetActive(true);
        }

        private void OnClick_Spirit()
        {
            this.MapDialogSpirit.gameObject.SetActive(true);
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
            AppHelper.CopyCount += rate * 10;

            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new BossFamilyStartEvent() { Level = level, Rate = rate });

            GameProcessor.Inst.SaveData();
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

        private void OpenLegacy(OpenLegacyEvent e)
        {
            LegacyDialog.gameObject.SetActive(true);
        }

        public void StartLegacy(int mapId, int layer)
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new LegacyStartEvent() { MapId = mapId, Layer = layer });
        }

        private void OpenPill(OpenPillEvent e)
        {
            MapDialogPill.gameObject.SetActive(true);
        }

        private void OpenBabel(OpenBabelEvent e)
        {
            MapDialogBabel.gameObject.SetActive(true);
        }

        private void OpenMyth(OpenMythEvent e)
        {
            MapDialogMyth.gameObject.SetActive(true);
        }

        public void StartPill(int layer, int type)
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new PillStartEvent() { Layer = layer, Type = type });
        }

        public void StartMyth(int id)
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new MythStartEvent() { Id = id });
        }

        public void StartFestive(int id)
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new FestiveStartEvent() { Id = id });
        }

        public void StartShengxiao(int id)
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new ShengxiaoStartEvent() { Id = id });
        }

        public void StartSpirit(int id)
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new SpiritStartEvent() { Id = id });
        }

        public void StartWorld(int id)
        {
            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new WorldStartEvent() { Id = id });
        }


        public void StartBabel()
        {
            User user = GameProcessor.Inst.User;

            if (user.BabelData.Data >= ConfigHelper.BabelMax)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "你已经通关了，请等待开放上限", ToastType = ToastTypeEnum.Failure });
                return;
            }


            scrollRect.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new BabelStartEvent() { });
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
