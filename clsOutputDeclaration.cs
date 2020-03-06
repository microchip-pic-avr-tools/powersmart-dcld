using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcld
{
    class clsOutputDeclaration
    {
        // Private Variables
        //private bool ParameterUpdate = false;

        // Feedback Type Declaration ~~~~~~~~~~~~~~~~~~~

        internal enum dcldOutputType : byte
        {
            DCLD_OUT_TYPE_UNDEFINED = 0,
            DCLD_OUT_TYPE_FIXED_FREQUENCY = 1,
            DCLD_OUT_TYPE_PHASE_SHIFTED_PWM = 2,
            DCLD_OUT_TYPE_VARIABLE_FREQUENCY = 3,
            DCLD_OUT_TYPE_DIGITAL_SOURCE = 4
        }

        private dcldOutputType _OutputType = dcldOutputType.DCLD_OUT_TYPE_UNDEFINED;
        internal dcldOutputType OutputType
        {
            get { return (_OutputType); }
            set { _OutputType = value; return; }
        }

        // Feedback Type Declaration End ~~~~~~~~~~~~~~~

        // PWM Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private double _PWMclock = (4e+9);
        internal double PWMClock
        {
            get { return (_PWMclock); }
            set { _PWMclock = value; refreshPWM(); return; }
        }

        private double _PWMClkDiv = 1.0;
        internal double PWMClockDivider
        {
            get { return (_PWMClkDiv); }
            set { _PWMClkDiv = value; refreshPWM(); return; }
        }

        private double _PWMRes = (250e-12);
        internal double PWMResolution
        {
            get { return (_PWMRes); }
        }

        private double _PWMBitRes = 16.0;
        internal double PWMBitResolution
        {
            get { return (_PWMBitRes); }
            set { _PWMBitRes = value; refreshPWM(); return; }
        }

        private double _PWMfreq = 250000.0;
        internal double PWMFrequency
        {
            get { return (_PWMfreq); }
            set { _PWMfreq = value; refreshPWM(); return; }
        }

        private double _PWMper = (4e-6);
        internal double PWMPeriod
        {
            get { return (_PWMper); }
        }

        private Int64 _PWMcount = 8000;
        internal Int64 PWMCount
        {
            get { return (_PWMcount); }
        }

        private double _Gain = 1.000;
        internal double Gain
        {
            get { return (_Gain); }
        }

        private void refreshPWM()
        {

            if (_PWMClkDiv == 0.0) _PWMClkDiv = 1;
            _PWMRes = 1.0 / (_PWMclock / _PWMClkDiv);

            switch(_OutputType)
            {
                case dcldOutputType.DCLD_OUT_TYPE_FIXED_FREQUENCY:
                    _PWMper = 1.0 / _PWMfreq; 
                    _PWMcount = Convert.ToInt64(Math.Floor(_PWMper / _PWMRes) + 1);
                    break;

                case dcldOutputType.DCLD_OUT_TYPE_PHASE_SHIFTED_PWM:
                    _PWMper = 1.0 / _PWMfreq; 
                    _PWMcount = Convert.ToInt64((Math.Floor(_PWMper / _PWMRes) + 1) / 2.0);
                    break;

                case dcldOutputType.DCLD_OUT_TYPE_VARIABLE_FREQUENCY:
                    _PWMcount = Convert.ToInt64(Math.Pow(2.0, _PWMBitRes)-1);
                    _PWMper = _PWMRes * _PWMcount;
                    _PWMfreq = 1.0 / _PWMper;
                    break;

                default:
                    _PWMcount = Convert.ToInt64(Math.Pow(2.0, _PWMBitRes) - 1);
                    _PWMper = _PWMRes * _PWMcount;
                    _PWMfreq = 1.0 / _PWMper;
                    break;
            }

            _Gain = _PWMcount / (Math.Pow(2.0, _PWMBitRes)-1);

        }

    }
}
