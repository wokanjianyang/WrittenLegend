using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Map : BaseAttackSkill
    {
        public Skill_Map(APlayer player, SkillPanel skill) : base(player, skill)
        {
            this.skillGraphic = new AttackSkillGraphic(player, skill.SkillData.SkillConfig);
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();

            //Debug.Log($"��ȡ����:{(this.SkillData.Name)}ʩ��Ŀ��");

            //ʩ������Ϊ�Լ�
            APlayer target = SelfPlayer;

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(target.Cell, SkillPanel.Dis, SkillPanel.Area);

            foreach (var cell in allAttackCells)
            {
                attackDatas.Add(new AttackData()
                {
                    Tid = 0,
                    Ratio = 0
                });
            }

            return attackDatas;
        }

        public void Run(APlayer enemy)
        {
            if (enemy.GroupId == this.SelfPlayer.GroupId)
            {  //���Զ�������˺�
                return;
            }
            long damage = this.SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt) - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.Def);
            damage = damage > 1 ? damage : 1;

            enemy.OnHit(SelfPlayer.ID, damage);
        }

        public override void Do()
        {
            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SkillPanel.Dis, SkillPanel.Area);

            foreach (var cell in allAttackCells)
            {
                MapCell mapCell = GameProcessor.Inst.MapData.GetMapCell(cell);
                if (mapCell != null) //���ڵ�ͼ��Ե��ʱ��
                {
                    mapCell.AddSkill(this);
                }
                this.skillGraphic.PlayAnimation(cell);
            }

        }
    }
}
