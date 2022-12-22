using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Valet : BaseAttackSkill
    {
        public List<Valet> ValetList = new List<Valet>();
        public int MaxValet = 0;

        public Skill_Valet(APlayer player, SkillData skillData) : base(player, skillData)
        {
            MaxValet = skillData.EnemyMax;
        }

        public override List<AttackData> GetAllTargets()
        {
            Debug.Log($"使用技能:{(this.SkillData.Name)},施法目标为自己");

            List<AttackData> attackDatas = new List<AttackData>();
            attackDatas.Add(new AttackData()
            {
                Tid = this.SelfPlayer.ID,
                Ratio = 1
            });
            return attackDatas;
        }

        public override bool IsCanUse()
        {
            if (MaxValet > 0 && MaxValet > ValetList.Count)
            {
                return true;
            }
            return false;
        }

        public override void Do()
        {
            //销毁之前的
            foreach (Valet valet in ValetList)
            {
                valet.OnHit(SelfPlayer.ID, valet.HP);
            }
            ValetList.Clear();


            Hero hero = SelfPlayer as Hero;
            long baseAttr = hero.AttributeBonus.GetTotalAttr(AttributeEnum.SpiritAtt) * SkillData.Percent / 100 + SkillData.Damage;

            Dictionary<AttributeEnum, object> data = new Dictionary<AttributeEnum, object>();
            data[AttributeEnum.Color] = Color.white;
            data[AttributeEnum.Name] = "骷髅(" + SelfPlayer.Name + ")";
            data[AttributeEnum.Level] = 1;
            data[AttributeEnum.HP] = baseAttr * 10;
            data[AttributeEnum.PhyAtt] = baseAttr;
            data[AttributeEnum.Def] = baseAttr / 2;

            //创造新的
            for (int i = 0; i < MaxValet; i++)
            {
                Valet valet = GameProcessor.Inst.PlayerManager.LoadValet(SelfPlayer.ID, data);
                ValetList.Add(valet);
            }
        }
    }
}
