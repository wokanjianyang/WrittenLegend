using System;
using System.Linq;

namespace Game
{

    public partial class DropConfigCategory 
    {
        public DropConfig GetByMonsterId(int monsterId)
        {
            return this.dict.Where(m => m.Value.MonsterID == monsterId).First().Value;
        }
    }
}
