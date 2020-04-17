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

        internal enum dcldConverterType : byte
        {
            DCLD_CONVERTER_BUCK = 0,
            DCLD_CONVERTER_BOOST = 1,
            DCLD_CONVERTER_BUCK_BOOST = 2
        }

        private dcldConverterType _ConverterType = dcldConverterType.DCLD_CONVERTER_BUCK;
        internal dcldConverterType ConverterType
        {
            get { return(_ConverterType); }
            set { _ConverterType = value; return; }
        }

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

        // Output Type Declaration End ~~~~~~~~~~~~~~~

        internal enum dcldDeviceType : byte
        {
            DCLD_DEVICE_TYPE_UNDEFINED = 0,
            DCLD_DEVICE_TYPE_P33FJ = 1,
            DCLD_DEVICE_TYPE_P33EP = 2,
            DCLD_DEVICE_TYPE_P33C = 3
        }

        private dcldDeviceType _DeviceType = dcldDeviceType.DCLD_DEVICE_TYPE_P33C;
        internal dcldDeviceType DeviceType
        {
            get { return (_DeviceType); }
            set { _DeviceType = value; refreshPWM(); return; }
        }

        internal bool SetDeviceType (string DeviceTypeString)
        {
            if (DeviceTypeString.Trim().Length == 0)
                return (false);

            if (DeviceTypeString.Contains("dsPIC33C"))
                _DeviceType = dcldDeviceType.DCLD_DEVICE_TYPE_P33C;
            else if (DeviceTypeString.Contains("dsPIC33EP"))
                _DeviceType = dcldDeviceType.DCLD_DEVICE_TYPE_P33EP;
            else if (DeviceTypeString.Contains("dsPIC33FJ"))
                _DeviceType = dcldDeviceType.DCLD_DEVICE_TYPE_P33FJ;
            else
                return (false);

            return (true);
        }
        
        internal string DeviceTypeString
        {
            get {

                switch (_DeviceType)
                {
                    case dcldDeviceType.DCLD_DEVICE_TYPE_P33C:
                        return ("dsPIC33C");
                    case dcldDeviceType.DCLD_DEVICE_TYPE_P33EP:
                        return ("dsPIC33EP");
                    case dcldDeviceType.DCLD_DEVICE_TYPE_P33FJ:
                        return ("dsPIC33FJ");
                    default:
                        return("");
                }
            }
        }


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

        internal double PWMResolution
        {
            get { 
                if (_PWMClkDiv == 0.0) _PWMClkDiv = 1.0;
                return (1.0 / (_PWMclock / _PWMClkDiv)); 
            }
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

        internal double PWMPeriod
        {
            get { return (1.0 / _PWMfreq); }
        }

        internal Int64 PWMPeriodCount
        {
            get { return (Convert.ToInt64(Math.Round((PWMPeriod / PWMResolution), 0))); }
        }

        private double _PWMduty = (0.30);
        internal double PWMDutyCycle
        {
            get { return (_PWMduty); }
            set { _PWMduty = value; refreshPWM(); return; }
        }

        internal Int64 PWMDutyCycleCount
        {
            get { return (Convert.ToInt64((double)_PWMduty * PWMPeriodCount)); }
        }

        private double _Gain = 1.000;
        internal double Gain
        {
            get { return (_Gain); }
        }

        private double _NominalOutput = 0.0;
        internal double NominalOutput
        {
            get { return (_NominalOutput); }
        }

        private double _NominalInputVoltage = 0.0;
        internal double NominalInputVoltage
        {
            get { return (_NominalInputVoltage); }
            set { _NominalInputVoltage = value; refreshDutyCycle(); return; }
        }

        private double _NominalOutputVoltage = 0.0;
        internal double NominalOutputVoltage
        {
            get { return (_NominalOutputVoltage); }
            set { _NominalOutputVoltage = value; refreshDutyCycle(); return; }
        }

        private double _NominalEfficiency = 1.0;
        internal double NominalEfficiency
        {
            get { return (_NominalEfficiency); }
            set {
                double dbl_dum = value;
                if (dbl_dum > 1.0) dbl_dum = 1.0;
                if (dbl_dum < 0.0) dbl_dum = 0.0;
                _NominalEfficiency = dbl_dum; 
                refreshDutyCycle(); 
                return; 
            }
        }

        private double _WindingRatioPrimary = 1.0;
        internal double WindingRatioPrimary
        {
            get { return (_WindingRatioPrimary); }
            set {
                double _dbl_dum = 0.0;
                _dbl_dum = value;
                if (_dbl_dum == 0.0) _dbl_dum = 1;
                _WindingRatioPrimary = _dbl_dum; 
                refreshDutyCycle(); 
                return;
            }
        }

        private double _WindingRatioSecondary = 1.0;
        internal double WindingRatioSecondary
        {
            get { return (_WindingRatioSecondary); }
            set {
                double _dbl_dum = 0.0;
                _dbl_dum = value;
                if (_dbl_dum == 0.0) _dbl_dum = 1;
                _WindingRatioSecondary = _dbl_dum; 
                refreshDutyCycle(); 
                return;
            }
        }

        internal double WindingRatio
        {
            get { return (_WindingRatioPrimary / _WindingRatioSecondary); }
        }

        private void refreshDutyCycle()
        {
            double _voutX = 0.0, _wr = 0.0;

            _wr = (_WindingRatioPrimary / _WindingRatioSecondary);

            switch (_ConverterType)
            {
                case clsOutputDeclaration.dcldConverterType.DCLD_CONVERTER_BUCK: // Buck/Forward type
                    _voutX = _NominalOutputVoltage / _wr;
                    PWMDutyCycle = (_voutX / _NominalInputVoltage) / _NominalEfficiency;
                    break;
                case clsOutputDeclaration.dcldConverterType.DCLD_CONVERTER_BOOST: // Boost type
                    _voutX = _NominalOutputVoltage / _wr;
                    PWMDutyCycle = ((_voutX - _NominalInputVoltage) / _voutX) / _NominalEfficiency; 
                    break;
                case clsOutputDeclaration.dcldConverterType.DCLD_CONVERTER_BUCK_BOOST: // Buck/Boost type
                    _voutX = _NominalOutputVoltage / _wr;
                    PWMDutyCycle = (_voutX / (_voutX + _NominalInputVoltage)) / _NominalEfficiency;
                    break;
                default:
                    break;

            }

        }

        private void refreshPWM()
        {

            switch(_OutputType)
            {
                case dcldOutputType.DCLD_OUT_TYPE_FIXED_FREQUENCY:
                    _NominalOutput = (PWMDutyCycle * PWMPeriodCount);
                    break;

                case dcldOutputType.DCLD_OUT_TYPE_PHASE_SHIFTED_PWM:
                    _NominalOutput = (PWMPeriodCount / 2);
                    break;

                case dcldOutputType.DCLD_OUT_TYPE_VARIABLE_FREQUENCY:
                    _NominalOutput = PWMPeriodCount;
                    break;

                default:
                    break;
            }

            _Gain = PWMPeriodCount / (Math.Pow(2.0, _PWMBitRes)-1);

        }

    }
}
