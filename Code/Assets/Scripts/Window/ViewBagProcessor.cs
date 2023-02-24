using SA.Android.Utilities;
using SA.CrossPlatform.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Game.PocketAD;

namespace Game
{
    public class ViewBagProcessor : AViewPage
    {
        [Title("��������")]
        [LabelText("������Ʒ")]
        public Toggle toggle_All;

        [LabelText("װ��")]
        public Toggle toggle_Equip;

        [LabelText("����")]
        public Toggle toggle_Prop;

        [LabelText("����")]
        public Toggle toggle_Forging;

        [Title("��������")]
        [LabelText("����")]
        public ScrollRect sr_Bag;

        [Title("������Ϣ")]
        [LabelText("����")]
        public Button btn_PlayerAttribute;

        [LabelText("�ƺ�")]
        public Button btn_PlayerTitle;

        private List<Com_Box> items;


        // Start is called before the first frame update
        void Start()
        {
            this.btn_PlayerTitle.onClick.AddListener(this.OnClick_PlayerTitle);
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Bag;
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<EquipOneEvent>(this.OnEquipOneEvent);
            GameProcessor.Inst.EventCenter.AddListener<SkillBookEvent>(this.OnSkillBookEvent);
            GameProcessor.Inst.EventCenter.AddListener<RecoveryEvent>(this.OnRecoveryEvent);
            GameProcessor.Inst.EventCenter.AddListener<AutoRecoveryEvent>(this.OnAutoRecoveryEvent);

            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            hero.EventCenter.AddListener<HeroBagUpdateEvent>(this.OnHeroBagUpdateEvent);

            this.items = new List<Com_Box>();

            var emptyPrefab = Resources.Load<GameObject>("Prefab/Window/Box_Empty");

            for (var i = 0; i < PlayerHelper.bagMaxCount; i++)
            {
                var empty = GameObject.Instantiate(emptyPrefab, (this.sr_Bag.content));
                empty.name = "Box_" + i;
            }

            if (hero.Bags!=null)
            {
                //��ʼ��Ⱦ����,��������
                hero.Bags.Sort((x,y)=> x.Item.Type.CompareTo(y.Item.Type));
                for (int BoxId = 0; BoxId < hero.Bags.Count; BoxId++)
                {
                    BoxItem item = hero.Bags[BoxId];
                    item.BoxId = BoxId;

                    Com_Box box = this.CreateBox(item);
                    box.transform.SetParent(this.sr_Bag.content.GetChild(BoxId));
                    box.transform.localPosition = Vector3.zero;
                    box.transform.localScale = Vector3.one;
                    box.SetBoxId(BoxId);
                    this.items.Add(box);
                }
            }

            if(hero.EquipPanel!=null)
            {
                //��ʼ��Ⱦ����װ��
                foreach(var kvp in hero.EquipPanel)
                {
                    this.CreateEquipPanelItem(kvp.Key,kvp.Value);
                }
            }
        }
        private Com_Box CreateBox(BoxItem item)
        {

            GameObject prefab = null;
            switch (item.Item.GetQuality())
            {
                case 0:
                case 1:
                    {
                        prefab = Resources.Load<GameObject>("Prefab/Window/Box_White");
                    }
                    break;
                case 2:
                    {
                        prefab = Resources.Load<GameObject>("Prefab/Window/Box_Green");
                    }
                    break;
                case 3:
                    {
                        prefab = Resources.Load<GameObject>("Prefab/Window/Box_Blue");
                    }
                    break;
                case 4:
                    {
                        prefab = Resources.Load<GameObject>("Prefab/Window/Box_Pink");
                    }
                    break;
            }
            var go = GameObject.Instantiate(prefab);
            var comItem = go.GetComponent<Com_Box>();
            comItem.SetBoxId(item.BoxId);
            comItem.SetItem(item.Item);
            return comItem;
        }
        private void OnEquipOneEvent(EquipOneEvent e)
        {
            var equip = e.Item as Equip;

            if (e.IsWear)
            {
                this.WearEquipment(equip, e.BoxId);
            }
            else
            {
                this.RmoveEquipment(e.Position, equip);
            }
            UserData.Save();
        }
        private void OnSkillBookEvent(SkillBookEvent e)
        {
            var hero = GameProcessor.Inst.PlayerManager.GetHero();

            UseBoxItem(e.BoxId);

            hero.EventCenter.Raise(new HeroUseSkillBookEvent
            {
                IsLearn = e.IsLearn,
                Item = e.Item
            });
        }

        private void OnRecoveryEvent(RecoveryEvent e) {
            var hero = GameProcessor.Inst.PlayerManager.GetHero();

            UseBoxItem(e.BoxId);

            hero.Gold += e.Item.Gold;
            hero.EventCenter.Raise(new HeroInfoUpdateEvent());
        }

        private void OnAutoRecoveryEvent(AutoRecoveryEvent e)
        {
            Hero hero = GameProcessor.Inst.PlayerManager.GetHero();

            List<BoxItem> recoveryList = hero.Bags.Where(m => hero.RecoverySetting.CheckRecovery(m.Item)).ToList();

            foreach (BoxItem box in recoveryList)
            {
                hero.Gold += box.Item.Gold * box.Number;

                Log.Debug("�Զ�����:" + box.Item.Name + " " + box.Number + "��");

                box.Number = 0;
                UseBoxItem(box.BoxId);

                hero.EventCenter.Raise(new HeroInfoUpdateEvent());
            }
        }
        private void UseBoxItem(int boxId)
        {
            var hero = GameProcessor.Inst.PlayerManager.GetHero();

            //�߼�����
            BoxItem boxItem = hero.Bags.Find(m => m.BoxId == boxId);
            Com_Box boxUI = this.items.Find(m => m.boxId == boxId);

            if (boxItem == null)
            {
                Log.Debug("����Ʒ�Ѿ���ʹ����");
                return;
            }
            boxItem.RemoveStack();
            boxUI.RemoveStack();

            //�ù��ˣ��Ƴ�����
            if (boxItem.Number <= 0)
            {
                hero.Bags.Remove(boxItem);

                this.items.Remove(boxUI);
                GameObject.Destroy(boxUI.gameObject);

            }
        }

        private void AddBoxItem(Item newItem)
        {
            var hero = GameProcessor.Inst.PlayerManager.GetHero();

            BoxItem boxItem = hero.Bags.Find(m => !m.IsFull() && m.Item.ConfigId == newItem.ConfigId);  //ͬ����Ʒ������û�����ѵ��ĸ���

            if (boxItem != null)
            {  //�ѵ�UI
                var item = this.items.Find(b => b.boxId == boxItem.BoxId);
                item.AddStack();

                boxItem.AddStack();
            }
            else
            {
                int lastBoxId = GetNextBoxId();

                if (lastBoxId < 0) { //�����Ѿ�����
                    return;
                }

                boxItem = new BoxItem();
                boxItem.Item = newItem;
                boxItem.Number = 1;
                boxItem.BoxId = lastBoxId;

                var item = this.CreateBox(boxItem);
                item.transform.SetParent(this.sr_Bag.content.GetChild(lastBoxId));
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.SetBoxId(lastBoxId);
                this.items.Add(item);

                hero.Bags.Add(boxItem);
            }
        }

        private void WearEquipment(Equip equip,int BoxId)
        {
            var hero = GameProcessor.Inst.PlayerManager.GetHero();

            int Part = equip.Part;
            //����һ�δ�����¼������������������
            if (!hero.equipRecord.ContainsKey(Part))
            {
                hero.equipRecord[Part] = 0;
            }
            hero.equipRecord[Part]++;
            int PartIndex = hero.equipRecord[Part] % equip.Position.Length;
            int Position = equip.Position[PartIndex];

            //�Ӱ����Ƴ�
            UseBoxItem(BoxId);

            //������ھ�װ�������ӵ�����
            if (hero.EquipPanel.ContainsKey(Position))
            {
                AddBoxItem(hero.EquipPanel[Position]);
            }

            //������������
            this.CreateEquipPanelItem(Position, equip);

            //֪ͨӢ�۸�������
            hero.EventCenter.Raise(new HeroUseEquipEvent
            {
                Position = Position,
                Equip = equip
            });

            //if (equiped != null)
            //{
            //    for (var i = 0; i < PlayerHelper.bagMaxCount; i++)
            //    {
            //        var com = this.items.FirstOrDefault(c => c.boxId == i);
            //        if (com == null)
            //        {
            //            equiped.transform.SetParent(this.sr_Bag.content.GetChild(i));
            //            equiped.transform.localPosition = Vector3.zero;
            //            equiped.SetBoxId(i);
            //            this.items.Add(equiped);
            //            break;
            //        }
            //    }
            //}

            //var comItem = this.items.FirstOrDefault(i => i.boxId == boxId);
            //if (comItem == null)
            //{
            //    BoxItem boxItem = new BoxItem();
            //    boxItem.Item = equip;
            //    boxItem.Number = 1;
            //    boxItem.BoxId = boxId;

            //    comItem = this.CreateBox(boxItem);
            //}
            //else
            //{
            //    this.items.Remove(comItem);
            //}
            //slots[emptySlotIndex].Equip(comItem);
            //comItem.transform.SetParent(slots[emptySlotIndex].transform);
            //comItem.transform.localPosition = Vector3.zero;
            //comItem.transform.localScale = Vector3.one;
            //comItem.SetBoxId(-1);

            //if (position == 0)
            //{  
            //    var hero = GameProcessor.Inst.PlayerManager.hero;
            //    equip.Position = equip.Position + emptySlotIndex * PlayerHelper.MAX_EQUIP_COUNT;
            //    hero.EventCenter.Raise(new HeroUseEquipEvent
            //    {
            //        Position = equip.Position + emptySlotIndex * PlayerHelper.MAX_EQUIP_COUNT,
            //        Equip = equip
            //    });
            //}
            //else
            //{
            //    //ԭ����װ���ˣ�ֻ����ʾ
            //}
        }

        private void CreateEquipPanelItem(int Position, Equip equip)
        {
            var hero = GameProcessor.Inst.PlayerManager.GetHero();

            var slot = this.transform.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == Position).First();

            //���ɸ���
            BoxItem boxItem = new BoxItem();
            boxItem.Item = equip;
            boxItem.Number = 1;
            boxItem.BoxId = -1;

            Com_Box comItem = this.CreateBox(boxItem);
            comItem.transform.SetParent(slot.transform);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;
            comItem.SetBoxId(-1);
            comItem.SetEquipPosition(Position);

            //����
            slot.Equip(comItem);
        }

        private void RmoveEquipment(int position, Equip equip)
        {
            //װ����ж��
            var slot = this.transform.GetComponentsInChildren<SlotBox>().Where(s => (int)s.SlotType == position).First();
            Com_Box comItem = slot.GetEquip();
            slot.UnEquip();
            GameObject.Destroy(comItem.gameObject);

            //comItem.transform.SetParent(this.sr_Bag.content.GetChild(10));
            //comItem.transform.localPosition = Vector3.zero;
            //comItem.transform.localScale = Vector3.one;
            //comItem.SetBoxId(i);


            //װ���ƶ�����������
            AddBoxItem(equip);

            //֪ͨӢ�۸�������
            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            hero.EventCenter.Raise(new HeroUnUseEquipEvent()
            {
                Position = position,
                Equip = equip
            });

            UserData.Save();
        }
        private void OnHeroBagUpdateEvent(HeroBagUpdateEvent e)
        {
            Hero hero = GameProcessor.Inst.PlayerManager.GetHero();
            if (hero.Bags != null)
            {
                var newItems = e.ItemList;

                foreach (var newItem in newItems)
                {
                    AddBoxItem(newItem);
                }
            }
        }

        public int GetNextBoxId()
        {
            for (int boxId = 0; boxId < 50; boxId++)
            {
                if (this.items.Find(m => m.boxId == boxId) == null)
                {
                    return boxId;
                }
            }
            return -1;
        }

        private AdStateCallBack adStateCallBack;
        public void OnClick_PlayerTitle()
        {
            adStateCallBack += OnAdStateCallBack;
            string title = "��ʾ���";
            string message = "������Ƶ������";
            var builder = new UM_NativeDialogBuilder(title, message);
            builder.SetPositiveButton("��", () => {
                Log.Debug("Okay button pressed");

                PocketAD.Inst.ShowAD("�ƺ�", adStateCallBack);
            });

            var dialog = builder.Build();
            dialog.Show();


        }

        public void OnAdStateCallBack(int rv, AdStateEnum state, AdTypeEnum adType)
        {

        }
    }
}