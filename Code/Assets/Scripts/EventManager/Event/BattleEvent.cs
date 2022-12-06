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
    }

    public class EquipOneEvent : SDD.Events.Event
    {
        public Item Item { get; set; }
        public int BoxId { get; set; }

    }
}
