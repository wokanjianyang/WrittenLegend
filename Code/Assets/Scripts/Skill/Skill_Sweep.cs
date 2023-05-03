using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Sweep : BaseAttackSkill
    {
        public Skill_Sweep(APlayer player, SkillPanel skill) : base(player, skill)
        {
            this.skillGraphic = new SweepSkillGraphic(player, skill.SkillData.SkillConfig);
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();

            //Debug.Log($"��ȡ����:{(this.SkillPanel.SkillData.SkillConfig.Name)}ʩ��Ŀ��");

            //ʩ������Ϊ�Լ�
            APlayer target = SelfPlayer;

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(target.Cell, SkillPanel.Dis, SkillPanel.Area);
            float ratio = allAttackCells.Count;

            //���򣬴ӽ���Զ
            Vector3Int mainCell = SelfPlayer.Cell;
            Vector3Int selfCell = SelfPlayer.Cell;
            allAttackCells = allAttackCells.OrderBy(m => Mathf.Abs(m.x - mainCell.x) + Mathf.Abs(m.y - mainCell.y) + Mathf.Abs(m.z - mainCell.z)
            + Mathf.Abs(m.x - selfCell.x) + Mathf.Abs(m.y - selfCell.y) + Mathf.Abs(m.z - selfCell.z)).ToList();

            int EnemyNum = 0;


            foreach (var cell in allAttackCells)
            {
                if (EnemyNum >= SkillPanel.EnemyMax)
                {
                    break;
                }

                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy != null && enemy.GroupId != SelfPlayer.GroupId) //���ṥ��ͬ���Ա
                {
                    attackDatas.Add(new AttackData()
                    {
                        Tid = enemy.ID,
                        Ratio = ratio
                    });
                    EnemyNum++;
                }

                if (ratio > 0.1f)
                {
                    ratio -= 0.1f;
                }
            }

            return attackDatas;
        }
    }
}
