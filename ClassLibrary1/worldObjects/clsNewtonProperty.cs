using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameLogic
{
    public class clsNewtonianProperty
    {
        public float baseValue { get; set; }
        public float addedValue { get; set; }
        public float totalValue
        {
            get
            {
                return baseValue + addedValue;
            }
        }

        public clsNewtonianProperty(float baseValue, float addedValue = 0f)
        {
            this.baseValue = baseValue;
            this.addedValue = addedValue;
        }
        
    }
}
