using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcld
{
    class clsFeedbackDeclaration
    {

        // Private Variables
        private bool ParameterUpdate = false;

        // Feedback Type Declaration ~~~~~~~~~~~~~~~~~~~

        internal enum dcldFeedbackType : byte
        {
            DCLD_FB_TYPE_UNDEFINED = 0,
            DCLD_FB_TYPE_VOLTAGE_DIVIDER = 1,
            DCLD_FB_TYPE_SHUNT_AMPLIFIER = 2,
            DCLD_FB_TYPE_CURRENT_TRANSFORMER = 3,
            DCLD_FB_TYPE_DIGITAL_SOURCE = 4
        }

        private dcldFeedbackType _FeedbackType = dcldFeedbackType.DCLD_FB_TYPE_UNDEFINED;
        internal dcldFeedbackType FeedbackType
        {
            get { return (_FeedbackType); }
            set { _FeedbackType = value; return; }
        }

        // Feedback Type Declaration End ~~~~~~~~~~~~~~~

        // ADC Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private double _ADCRes = 12.0;
        internal double ADCResolution
        {
            get { return (_ADCRes); }
            set { _ADCRes = value; refreshADC(); return; }
        }

        private double _ADCRef = 3.300;
        internal double ADCReference
        {
            get { return (_ADCRef); }
            set { _ADCRef = value; refreshADC(); return; }
        }

        private bool _ADCIsDiff = false;
        internal bool ADCIsDifferential
        {
            get { return (_ADCIsDiff); }
            set { _ADCIsDiff = value; refreshADC(); return; }
        }

        private double _ADCGran = 0.0;
        internal double ADCGranulatiry
        {
            get { return (_ADCGran); }
        }

        private double _ADCMin = 0.0;
        internal double ADCMinimum
        {
            get { return (_ADCMin); }
        }

        private double _ADCMax = 0.0;
        internal double ADCMaximum
        {
            get { return (_ADCMax); }
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void refreshADC()
        {

            // Update ADC parameters
            if (!_ADCIsDiff)
            {
                _ADCMin = 0;
                _ADCMax = (Math.Pow(2, _ADCRes) - 1);
                _ADCGran = _ADCRef / Math.Pow(2, _ADCRes);
            }
            else
            {
                _ADCMin = (-Math.Pow(2, (_ADCRes - 1)));
                _ADCMax = (Math.Pow(2, _ADCRes - 1) - 1);
                _ADCGran = _ADCRef / Math.Pow(2, _ADCRes);
            }

            // Update Feedback parameters
            if (!ParameterUpdate) // Guarding condition to prevent recirsive calls
            {
                switch(_FeedbackType)
                {
                    case dcldFeedbackType.DCLD_FB_TYPE_VOLTAGE_DIVIDER:
                        refreshVoltageDivider();
                        break;
                    case dcldFeedbackType.DCLD_FB_TYPE_SHUNT_AMPLIFIER:
                        refreshCurrentSense();
                        break;
                    case dcldFeedbackType.DCLD_FB_TYPE_CURRENT_TRANSFORMER:
                        refreshCurrentTransformer();
                        break;
                    case dcldFeedbackType.DCLD_FB_TYPE_DIGITAL_SOURCE:
                        refreshDigitalSource();
                        break;
                    case dcldFeedbackType.DCLD_FB_TYPE_UNDEFINED:
                        _FeedbackGain = 1.000;
                        break;
                }
                
            }

            return;
        }
        // ADC Properties End ~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Voltage Divider ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private double _VDivR1 = 12000;
        internal double VoltageDividerR1
        {
            get { return (_VDivR1); }
            set { _VDivR1 = value; refreshVoltageDivider(); return; }
        }

        private double _VDivR2 = 2200;
        internal double VoltageDividerR2
        {
            get { return (_VDivR2); }
            set { _VDivR2 = value; refreshVoltageDivider(); return; }
        }

        private double _VDivGain = 1.000;
        internal double VoltageDividerFeedbackGain
        {
            get { 
                _VDivGain = _VDivGamp * (_VDivR2 / (_VDivR1 + _VDivR2)); 
                return (_VDivGain); 
            }
        }

        private double _VDivGamp = 1.000;
        internal double VoltageDividerAmplifierGain
        {
            get { return (_VDivGamp); }
            set { _VDivGamp = value; refreshVoltageDivider(); return; }
        }

        private double _VDVinMax = 1.000;
        internal double VoltageDividerSourceMaximum
        {
            get { return (_VDVinMax); }
        }

        private double _VDVin = 0.0;
        internal double VoltageDividerSenseVoltage
        {
            get { return (_VDVin); }
            set { _VDVin = value; refreshVoltageDivider(); return; }
        }

        private double _VDVfb = 1.000;
        internal double VoltageDividerFeedbackVoltage
        {
            get { _VDVfb = _VDVin * VoltageDividerFeedbackGain; return (_VDVfb); }
        }
        
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void refreshVoltageDivider()
        {
            // Amplifer Gain * (lower resistance / (upper resistance + lower resistance))
            _FeedbackGain = _VDivGamp * (_VDivR2 / (_VDivR1 + _VDivR2));

            if (!_ADCIsDiff) _VDVinMax = _ADCRef / _FeedbackGain;
            else _VDVinMax = 0.5 * _ADCRef / _FeedbackGain;

            _VDVfb = _VDVin * _FeedbackGain;
            if (_ADCGran != 0.0)
                _FeedbackValue = Convert.ToInt32(Math.Ceiling(_VDVfb / _ADCGran));

            return;
        }
        // Voltage Divider End ~~~~~~~~~~~~~~~~~~~~~~~~~

        // Shunt Amplifier ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private double _CSRshunt = 0.010;
        internal double CurrentSenseRshunt
        {
            get { return (_CSRshunt); }
            set { _CSRshunt = value; refreshCurrentSense(); return; }
        }

        private double _CSGamp = 20.000;
        internal double CurrentSenseAmplifierGain
        {
            get { return (_CSGamp); }
            set { _CSGamp = value; refreshCurrentSense();  return; }
        }

        private double _CSGain = 1.000;
        internal double CurrentSenseFeedbackGain
        {
            get
            {
                _CSGain = _CSRshunt * _CSGamp;
                return (_CSGain);
            }
        }

        private double _CSIinMax = 1.000;
        internal double CurrentSenseSourceMaximum
        {
            get { return (_CSIinMax); }
        }

        private double _CSIin = 0.0;
        internal double CurrentSenseSenseCurrent
        {
            get { return (_CSIin); }
            set { _CSIin = value; refreshCurrentSense(); return; }
        }

        private double _CSVfb = 1.000;
        internal double CurrentSenseFeedbackVoltage
        {
            get { _CSVfb = _CSIin * CurrentSenseFeedbackGain; return (_CSVfb); }
        }
        
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void refreshCurrentSense()
        {
            // Shunt Resistance * Amplifier Gain
            _FeedbackGain = _CSRshunt * _CSGamp; 

            if (!_ADCIsDiff) _CSIinMax = _ADCRef / _FeedbackGain;
            else _CSIinMax = 0.5 * _ADCRef / _FeedbackGain;

            _CSVfb = _CSIin * _FeedbackGain;
            if (_ADCGran != 0.0)
                _FeedbackValue = Convert.ToInt32(Math.Ceiling(_CSVfb / _ADCGran));

            return;
        }
        // Shunt Amplifier End ~~~~~~~~~~~~~~~~~~~~~~~~~

        // Current Sense Transformer ~~~~~~~~~~~~~~~~~~~
        private double _CTRB = 10.0;
        internal double CurrentTransformerBurdenResistance
        {
            get { return (_CTRB); }
            set { _CTRB = value; refreshCurrentTransformer(); return; }
        }

        private double _CTWR = 50.000;
        internal double CurrentTransformerWindingRatio
        {
            get { return (_CTWR); }
            set { _CTWR = value; refreshCurrentTransformer(); return; }
        }

        private double _CTGain = 1.000;
        internal double CurrentTransformerFeedbackGain
        {
            get
            {
                _CTGain = (1.0 / _CTWR) * _CTRB;
                return (_CTGain);
            }
        }

        private double _CTIinMax = 1.000;
        internal double CurrentTransformerSourceMaximum
        {
            get { return (_CTIinMax); }
        }

        private double _CTIin = 0.0;
        internal double CurrentTransformerSenseCurrent
        {
            get { return (_CTIin); }
            set { _CTIin = value; refreshCurrentTransformer(); return; }
        }

        private double _CTVfb = 1.000;
        internal double CurrentTransformerFeedbackVoltage
        {
            get { _CTVfb = _CTIin * CurrentTransformerFeedbackGain; return (_CSVfb); }
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void refreshCurrentTransformer()
        {
            // 1/[secondary windings] * R_burden
            _FeedbackGain = (1.0 / _CTWR) * _CTRB; 

            if (!_ADCIsDiff) _CTIinMax = _ADCRef / _FeedbackGain;
            else _CTIinMax = 0.5 * _ADCRef / _FeedbackGain;

            _CTVfb = _CTIin * _FeedbackGain;
            if (_ADCGran != 0.0)
                _FeedbackValue = Convert.ToInt32(Math.Ceiling(_CTVfb / _ADCGran));


            return;
        }
        // Current Sense Transformer End ~~~~~~~~~~~~~~~

        // Digital Source ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private double _DSres = 12.0;
        internal double DigitalSourceResolution
        {
            get { return (_DSres); }
            set { _DSres = value; refreshDigitalSource(); return; }
        }
        private double _DSInMax = 1.000;
        internal double DigitalSourceSourceMaximum
        {
            get { return (_DSInMax); }
        }

        private double _DSIn = 1.000;
        internal double DigitalSourceValue
        {
            get { return (_DSIn); }
            set { _DSIn = value;  return; }
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void refreshDigitalSource()
        {
            double gdiv = 0.0;

            if (!_ADCIsDiff) gdiv = 1.0;
            else gdiv = 2.0;

            _FeedbackGain = (_ADCRes / _DSres);

            // Calculate maximum input value
            _DSInMax = (Math.Pow(2.0, _ADCRes) - 1) / gdiv;

            _FeedbackValue = Convert.ToInt32(_DSIn);
        }
        // Digital Source End ~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Feedback Gain Value ~~~~~~~~~~~~~~~~~~~~~~~~~
        private double _FeedbackGain = 1.000;
        internal double FeedbackGain
        {
            get { return (_FeedbackGain); }
            set { _FeedbackGain = value; return; }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // Feedback Value ~~~~~~~~~~~~~~~~~~~~~~~~~
        private int _FeedbackValue = 4095;
        internal int FeedbackValue
        {
            get { return (_FeedbackValue); }
            set { _FeedbackValue = value; return; }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        // METHODS =====================================


        // METHODS END =================================


    }
}
