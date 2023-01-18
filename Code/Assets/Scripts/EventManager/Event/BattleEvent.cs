using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ChangePageEvent : SDD.Events.Event
    {
        public ViewPageType Page { get; set; }
    }

    public class ShowEquipDetailEvent : SDD.Events.Event
    {
        public Vector3 Position { get; set; }

        public Item Item { get; set; }

        public int BoxId { get; set; }

        public int EquipPosition { get; set; }
    }

    public class EquipOneEvent : SDD.Events.Event
    {
        public bool IsWear { get; set; } = true;

        public int Position { get; set; }
        public Item Item { get; set; }
        public int BoxId { get; set; }
    }


    public class BattleMsgEvent : SDD.Events.Event
    {
        public int RoundNum { get; set; }
        public int MonsterId { get; set; }

        public long Exp { get; set; }
        public long Gold { get; set; }

        public List<Item> Drops { get; set; }
    }

    public class SkillBookEvent : SDD.Events.Event
    {
        /// <summary>
        /// ����ѧϰ��������
        /// </summary>
        public bool IsLearn { get; set; }

        public Item Item { get; set; }
        public int BoxId { get; set; }
    }

    public class RecoveryEvent : SDD.Events.Event
    {
        public Item Item { get; set; }
        public int BoxId { get; set; }
    }
}