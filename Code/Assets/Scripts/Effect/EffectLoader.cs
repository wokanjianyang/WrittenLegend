using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public static class EffectLoader
    {
        public static EffectProcessor CreateEffect(string skillName,bool loop)
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
            com.Loop = loop;
            return com;
        }

        public static EffectShield CreateEffectShield(string skillName, int duration)
        {
            EffectShield com = null;
            var prefab = Resources.Load<GameObject>("Prefab/Effect/盾");

            if (prefab)
            {
                com = GameObject.Instantiate(prefab).GetComponent<EffectShield>();
                com.SetData("UI/Buff斩杀素材/" + skillName);
            }

            return com;
        }
    }
}
