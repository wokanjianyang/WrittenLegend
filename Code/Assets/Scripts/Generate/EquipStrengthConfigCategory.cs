using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipStrengthConfigCategory
    {

        public EquipStrengthConfig GetByPositioinAndLevel(int position, int level)
        {
            try
            {
                return this.GetAll().Where(m => m.Value.Level == level && m.Value.Position == position).First().Value;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }

}
