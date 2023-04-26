using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Valet : BaseAttackSkill
    {
        public List<Valet> ValetList = new List<Valet>();
        public int MaxValet = 0;

        public Skill_Valet(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
            MaxValet = skillPanel.EnemyMax;
        }

        public override List<AttackData> GetAllTargets()
        {
            Debug.Log($"ʹ�ü���:{(this.SkillPanel.SkillData.SkillConfig.Name)},ʩ��Ŀ��Ϊ�Լ�");

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
            //����֮ǰ��
            foreach (Valet valet in ValetList)
            {
                valet.OnHit(SelfPlayer.ID, valet.HP);
            }
            ValetList.Clear();

            long baseAttr = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.SpiritAtt) * (SkillPanel.Percent + SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.InheritIncrea)) / 100 + SkillPanel.Damage;

            Dictionary<AttributeEnum, object> data = new Dictionary<AttributeEnum, object>();
            data[AttributeEnum.Color] = Color.white;
            data[AttributeEnum.Name] = "����(" + SelfPlayer.Name + ")";
            data[AttributeEnum.Level] = 1;
            data[AttributeEnum.HP] = baseAttr * 10;
            data[AttributeEnum.PhyAtt] = baseAttr;
            data[AttributeEnum.Def] = baseAttr / 2;

            //�����µ�
            for (int i = 0; i < MaxValet; i++)
            {
                Valet valet = GameProcessor.Inst.PlayerManager.LoadValet(SelfPlayer.ID, data);
                ValetList.Add(valet);
            }
        }

        public void ClearValet() {
            //����֮ǰ��
            foreach (Valet valet in ValetList)
            {
                valet.OnHit(SelfPlayer.ID, valet.HP);
            }
            ValetList.Clear();
        }
    }
}
