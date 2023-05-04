using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace Game
{
    public class Hero : APlayer
    {
        public long Exp { get; set; }

        public long UpExp { get; set; }

        public long Gold { get; set; }

        public long Essence { get; set; }

        public int MapId { get; set; }

        public int LastCityId { get; set; }

        public long Power { get; set; }

        public long SecondExpTick { get; set; }

        public IDictionary<int, Equip> EquipPanel { get; set; } = new Dictionary<int, Equip>();

        [JsonIgnore]
        public IDictionary<int, int> equipRecord { get; set; } = new Dictionary<int, int>();

        public RecoverySetting RecoverySetting { get; set; } = new RecoverySetting();


        /// <summary>
        /// ����
        /// </summary>
        public List<BoxItem> Bags { get; set; } = new List<BoxItem>();

        public IDictionary<int,bool> GiftList { get; set; } = new Dictionary<int, bool>();

        public long LastOut { get; set; }

        private bool isInLevelUp;

        /// <summary>
        /// �޾�������
        /// </summary>
        public int TowerFloor = 1;

        public Hero():base()  {
            this.EventCenter.AddListener<HeroChangeEvent>(HeroChange);
            this.EventCenter.AddListener<HeroUseEquipEvent>(HeroUseEquip);
            this.EventCenter.AddListener<HeroUnUseEquipEvent>(HeroUnUseEquip);
            this.EventCenter.AddListener<HeroUseSkillBookEvent>(HeroUseSkillBook);
            this.GroupId = 1;
        }

        public override void Load()
        {
            base.Load();

            //var boxPrefab = Resources.Load<GameObject>("Prefab/Effect/HeroBox");
            //var box = GameObject.Instantiate(boxPrefab, this.Transform).transform;
            //box.SetParent(this.GetComponent<PlayerUI>().image_Background.transform); 
        }

        public void Init() {
            this.Camp = PlayerType.Hero;

            //���ø�������ֵ
            SetLevelConfigAttr();
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.Test, 10000);
            AttributeBonus.SetAttr(AttributeEnum.AttIncrea, AttributeFrom.Test, 400);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.Test, 10000);
            AttributeBonus.SetAttr(AttributeEnum.ExpIncrea, AttributeFrom.Test, 1000);

            //������ǰѪ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //���û���ѡ��
            RecoverySetting.SetType(2, true);
            RecoverySetting.SetQuanlity(1, true);
            RecoverySetting.SetQuanlity(2, true);
            RecoverySetting.SetQuanlity(3, true);
            //RecoverySetting.SetQuanlity(4, true);
            RecoverySetting.SetLevel(100);
        }

        /// <summary>
        /// ���������ѡ���ܷŵ�SkillIdList
        /// </summary>
        public void InitPanelSkill()
        {
            base.LoadSkill();
        }

        public void UpdatePlayerInfo() {
            GameProcessor.Inst.PlayerInfo.UpdateAttrInfo(this);
        }

        private void HeroChange(HeroChangeEvent e)
        {
            switch (e.Type)
            {
                case HeroChangeType.LevelUp:
                    if(!this.isInLevelUp)
                    {
                        this.isInLevelUp = true;
                        GameProcessor.Inst.StartCoroutine(LevelUp());
                    }
                    break;
            }
        }

        private void HeroUseEquip(HeroUseEquipEvent e)
        {
            int PanelPosition = e.Position;
            Equip equip = e.Equip;

            EquipPanel[PanelPosition] = equip;

            //�滻����
            foreach (var a in equip.GetTotalAttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase,PanelPosition, a.Value);
            }

            //��ʾ���µ�Ѫ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //�����������
            UpdatePlayerInfo();
        }

        private void HeroUnUseEquip(HeroUnUseEquipEvent e)
        {
            EquipPanel.Remove(e.Position);

            //�滻����
            foreach (var a in e.Equip.GetTotalAttrList)
            {
                AttributeBonus.SetAttr((AttributeEnum)a.Key, AttributeFrom.EquipBase, e.Position, 0);
            }

            //��ʾ���µ�Ѫ��
            SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));

            //�����������
            UpdatePlayerInfo();
        }

        private void HeroUseSkillBook(HeroUseSkillBookEvent e)
        {
            SkillBook Book = e.Item as SkillBook;

            if (e.IsLearn)
            {
                //��һ��ѧϰ��������������
                SkillData skillData = new SkillData(Book.ConfigId,0);
                skillData.Status = SkillStatus.Learn;
                skillData.Level = 1;
                skillData.Exp = 0;

                this.SkillList.Add(skillData);

                SkillPanel skillPanel = new SkillPanel(skillData, GetRuneList(skillData.SkillId), GetSuitList(skillData.SkillId));
                this.EventCenter.Raise(new HeroUpdateSkillEvent()
                {
                    SkillPanel = skillPanel
                });
            }
            else
            {
                SkillData skillData = this.SkillList.Find(b => b.SkillId == Book.ConfigId);
                skillData.AddExp(Book.ItemConfig.UseParam);

                SkillPanel skillPanel = new SkillPanel(skillData, GetRuneList(skillData.SkillId), GetSuitList(skillData.SkillId));
                this.EventCenter.Raise(new HeroUpdateSkillEvent()
                {
                    SkillPanel = skillPanel
                });
            }
        }

        public void BuildReword()
        {
            Dictionary<int, RewardConfig> rewordList = RewardConfigCategory.Instance.GetAll();
            foreach (int rewordId in rewordList.Keys)
            {
                if (!GiftList.Keys.Contains(rewordId))
                {
                    RewardConfig config = rewordList[rewordId];

                    BoxItem boxItem = new BoxItem();

                    if (config.type == 4) //���
                    {
                        GiftPack gift = new GiftPack(config.ItemId);
                        boxItem.Item = gift;
                        boxItem.Number = 1;
                        boxItem.BoxId = -1;
                    }

                    this.Bags.Add(boxItem);

                    GiftList.Add(rewordId, true);
                }
            }
        }

        public override List<SkillRune> GetRuneList(int skillId)
        {
            List<SkillRune> list = new List<SkillRune>();

            //����װ���Ĵ����ӳ�
            List<Equip> skillList = this.EquipPanel.Where(m => m.Value.SkillRuneConfig.SkillId == skillId).Select(m => m.Value).ToList();

            //����������,�����жѵ�����
            var runeGroup = skillList.GroupBy(m => m.RuneConfigId);
            foreach (IGrouping<int, Equip> runeItem in runeGroup)
            {
                SkillRune skillRune = new SkillRune(runeItem.Key, runeItem.Count());
                list.Add(skillRune);
            }

            return list;
        }

        public override List<SkillSuit> GetSuitList(int skillId)
        {
            List<SkillSuit> list = new List<SkillSuit>();

            //����װ������װ�ӳ�
            List<Equip> skillList = this.EquipPanel.Where(m => m.Value.SkillSuitConfig.SkillId == skillId).Select(m => m.Value).ToList();
            var suitGroup = skillList.GroupBy(m => m.SkillRuneConfig.SuitId);

            foreach (var suitItem in suitGroup)
            {
                if (suitItem.Count() >= 4)
                {  //4���ų���,����ֻ����һ������Ч
                    SkillSuit suit = new SkillSuit(suitItem.Key);
                    list.Add(suit);
                }
            }

            return list;
        }


        private void SetLevelConfigAttr()
        {
            LevelConfig config = LevelConfigCategory.Instance.Get(Level);
            AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, config.HP);
            AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, config.PhyAtt);
            AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, config.Def);

            if (this.TowerFloor > 1)
            {
                TowerConfig towerConfig = TowerConfigCategory.Instance.Get(this.TowerFloor - 1);
                AttributeBonus.SetAttr(AttributeEnum.SecondExp, AttributeFrom.Tower, towerConfig.TotalExp);
            }

            UpExp = config.Exp;
        }

        IEnumerator LevelUp()
        {
            while (this.Exp >= this.UpExp)
            {
                Exp -= UpExp;
                Level++;

                //Add Base Attr
                SetLevelConfigAttr();
                //�����ָ���Ѫ��
                SetHP(AttributeBonus.GetTotalAttr(AttributeEnum.HP));
                EventCenter.Raise(new SetPlayerLevelEvent { Level = Level.ToString() });
                EventCenter.Raise(new HeroInfoUpdateEvent());

                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
            this.isInLevelUp = false;
        }

        public enum HeroChangeType
        {
            LevelUp = 0,
            AttrChange = 1
        }
    }

}
