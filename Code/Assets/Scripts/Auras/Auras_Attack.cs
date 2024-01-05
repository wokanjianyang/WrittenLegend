using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Auras_Attack : AAuras
    {
        public Auras_Attack(APlayer player, AurasAttrConfig config) : base(player, config)
        {
        }


        public override void Do()
        {
            List<APlayer> enemys = GameProcessor.Inst.PlayerManager.GetAllPlayers().Where(m => m.GroupId != this.SelfPlayer.GroupId).ToList();

            double damage = this.SelfPlayer.GetRoleAttack(1, true) + this.SelfPlayer.GetRoleAttack(2, true) + this.SelfPlayer.GetRoleAttack(3, true);
            damage = damage * Config.AttrValue / 100;

            foreach (var enemy in enemys)
            {
                DamageResult dr = new DamageResult(0, damage, MsgType.Damage, RoleType.All); //光环伤害,来源为0
                enemy.OnHit(dr);
            }
        }
    }
}
