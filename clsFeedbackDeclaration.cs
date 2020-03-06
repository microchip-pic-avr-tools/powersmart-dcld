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

        private double _VDivGamp = 1.000;
        internal double VoltageDividerAmplifierGain
        {
            get { return (_VDivGamp); }
            set { _VDivGamp = value; refreshVoltageDivider(); return; }
        }

        private double _VDVin = 1.000;
        internal double VoltageDividerSourceMaximum
        {
            get { return (_VDVin); }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void refreshVoltageDivider()
        {
            // Amplifer Gain * (lower resistance / (upper resistance + lower resistance))
            _FeedbackGain = _VDivGamp * (_VDivR2 / (_VDivR1 + _VDivR2));

            if (!_ADCIsDiff) _VDVin = _ADCRef / _FeedbackGain;
            else _VDVin = 0.5 * _ADCRef / _FeedbackGain;

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

        private double _CSIin = 1.000;
        internal double CurrentSenseSourceMaximum
        {
            get { return (_CSIin); }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void refreshCurrentSense()
        {
            // Shunt Resistance * Amplifier Gain
            _FeedbackGain = _CSRshunt * _CSGamp; 

            if (!_ADCIsDiff) _CSIin = _ADCRef / _FeedbackGain;
            else _CSIin = 0.5 * _ADCRef / _FeedbackGain;

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
        private double _CTIin = 1.000;
        internal double CurrentTransformerSourceMaximum
        {
            get { return (_CTIin); }
        }
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void refreshCurrentTransformer()
        {
            // 1/[secondary windings] * R_burden
            _FeedbackGain = (1.0 / _CTWR) * _CTRB; 

            if (!_ADCIsDiff) _CTIin = _ADCRef / _FeedbackGain;
            else _CTIin = 0.5 * _ADCRef / _FeedbackGain;

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
        private double _DSIn = 1.000;
        internal double DigitalSourceSourceMaximum
        {
            get { return (_DSIn); }
        }

        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void refreshDigitalSource()
        {
            double gdiv = 0.0;

            if (!_ADCIsDiff) gdiv = 1.0;
            else gdiv = 2.0;

            _FeedbackGain = (_ADCRes / _DSres);

            // Calculate maximum input value
            _DSIn = (Math.Pow(2.0, _ADCRes) - 1) / gdiv;
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


        // METHODS =====================================


        // METHODS END =================================


    }
}
