using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ItouchIgnore
    {
        public TouchIgnoreType TouchType { get; }

        public void CheckPoint(Vector2 point);

        public bool RaycastFilter(Vector2 point);
    }
}
