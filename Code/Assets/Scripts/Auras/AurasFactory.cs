using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AurasFactory
    {




        public static AAuras BuildAuras(APlayer player, int atid, long level)
        {
            AAuras auras = null;

            AurasAttrConfig config = AurasAttrConfigCategory.Instance.Get(atid);

            if (config.Type == (int)AurasType.AutoDamage)
            {
                auras = new Auras_Attack(player, config);
            }
            else if (config.Type == (int)AurasType.AutoRestore)
            {
                auras = new Auras_Restore(player, config);
            }
            else
            {
                auras = new Auras_Base(player, config);
            }
            return auras;
        }
    }
}
