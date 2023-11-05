using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipStrengthConfigCategory
    {

        public EquipStrengthConfig GetByPositioin(int position)
        {
            try
            {
                return this.GetAll().Where(m => m.Value.Position == position).First().Value;
            }
            catch 
            {

            }

            return null;
        }
    }

}
