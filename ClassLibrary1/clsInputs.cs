using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gameLogic;

namespace GameLogic
{
    class clsInputs
    {
        private float _steering;
        private float _accelerator;
        private float _breakPedal;
        private ShifterPosition _shifterPosition;


        public float steering
        {
            get
            {
                return _steering;
            }
            set
            {
                if (value > 1) value = 1;
                if (value < -1) value = -1;
                _steering = value;
            }
        }

        public float accelerator
        {
            get
            {
                return _steering;
            }
            set
            {
                if (value > 1) value = 1;
                if (value < 0) value = 0;
                _accelerator = value;
            }
        }

        public float breakPedal
        {
            get
            {
                return _breakPedal;
            }
            set
            {
                if (value > 1) value = 1;
                if (value < 0) value = 0;
                _breakPedal = value;
            }
        }

        public ShifterPosition shiftSelector
        {
            get
            {
                return _shifterPosition;
            }
            set
            {
                _shifterPosition = value;
            }
        }
    }
}
