using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public static class EffectLoader
    {
        public static EffectProcessor CreateEffect(string effectPath, bool loop)
        {
            EffectProcessor com = null;
            var prefab = Resources.Load<GameObject>("Prefab/Effect/"+effectPath);
            if (prefab)
            {
                com = GameObject.Instantiate(prefab).GetComponent<EffectProcessor>();
                com.SetData(effectPath,loop);
            }
            return com;
        }
    }
}
