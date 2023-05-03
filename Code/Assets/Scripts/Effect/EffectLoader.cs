using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public static class EffectLoader
    {
        public static EffectProcessor CreateEffect(string skillName)
        {
            EffectProcessor com = null;
            var prefab = Resources.Load<GameObject>("Prefab/Effect/"+skillName);
            if (prefab)
            {
                com = GameObject.Instantiate(prefab).GetComponent<EffectProcessor>();
            }
            else
            {
                prefab = Resources.Load<GameObject>("Prefab/Effect/通用");
                if (prefab)
                {
                    com = GameObject.Instantiate(prefab).GetComponent<EffectProcessor>();
                    com.EffectPath += skillName;
                }
            }
            return com;
        }
    }
}
