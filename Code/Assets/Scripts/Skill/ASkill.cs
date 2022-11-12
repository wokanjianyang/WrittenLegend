using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Game
{
    public class AttackData
    {
        public int Tid { get; set; }
        public float Ratio { get; set; }
    }
    abstract public class ASkill : IPlayer
    {
        protected float Value { get; private set; } = 0;
        public void Do(int tid)
        {
            this.PlayAnimation(tid);
        }

        virtual public void PlayAnimation(int tid)
        {
            
        }

        virtual public float CalcFormula(APlayer player,float ratio)
        {
            return 0;
        }

        virtual public List<AttackData> GetAllTargets(int tid)
        {
            return new List<AttackData>() {new AttackData()
            {
                Tid = tid,
                Ratio = 1
            }};
        }
        
        public APlayer SelfPlayer { get; set; }
        public void SetParent(APlayer player)
        {
            this.SelfPlayer = player;
        }
    }
}
