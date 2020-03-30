using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace dcld
{
    public class clsCompensatorNPNZ
    {

        private bool _AutoUpdate = true;
        internal bool AutoUpdate           // When enabled, properties will be updated automatically
        {
            get { return _AutoUpdate; }
            set
            {
                _AutoUpdate = value;
                if (value)
                { // Enforce parameter refresh when enabled
                    Update();
                    //UpdateCoefficients();
                }
                return;
            }
        }

        public enum dcldControlerType : byte
        {
            DCLD_TYPE_UNDEFINED = 0,
            DCLD_TYPE_I = 1,
            DCLD_TYPE_II = 2,
            DCLD_TYPE_III = 3,
            DCLD_TYPE_IV = 4,
            DCLD_TYPE_V = 5,
            DCLD_TYPE_VI = 6
        }

        public enum dcldScalingMethod : byte
        {
            DCLD_SCLMOD_UNDEFINED = 0,
            DCLD_SCLMOD_SINGLE_BIT_SHIFT = 1,
            DCLD_SCLMOD_OUTPUT_SCALING_FACTOR = 2,
            DCLD_SCLMOD_DUAL_BIT_SHIFT = 3,
            DCLD_SCLMOD_DBLSCL_FLOAT = 4
        } ;

        public enum dcldDataStructureType : byte
        {
            DCLD_DST_UNDEFINED = 0,
            DCLD_DST_Q15 = 1,
            DCLD_DST_FFLOAT_Q15 = 2,
            DCLD_DST_FFLOAT = 3
        } ;

        private dcldDataStructureType _DataStructureType = dcldDataStructureType.DCLD_DST_Q15;
        internal dcldDataStructureType DataStructureType
        {
            get { return (_DataStructureType); }
        }

        private int _FilterOrder = 1;
        internal int FilterOrder
        {
            get { return _FilterOrder; }
            set { 

                _FilterOrder = value;

                int i = 0;

                OutputScalingFactor.Float64 = Math.Pow(2, -(_QFormat));

                Pole = new clsPoleZeroObject[_FilterOrder];                 // Indices for pole frequencies range from 0 to n-1
                Zero = new clsPoleZeroObject[_FilterOrder];                 // Indices for zero frequencies range from 1 to n-1 (Zero 0 is not initialized)

                CoeffA = new clsCoefficientObject[_FilterOrder + 1];        // Indices for A-coefficients range from 1 to n (A0 is not initialized)
                CoeffB = new clsCoefficientObject[_FilterOrder + 1];        // Indices for B-coefficients range from 0 to n

                for (i = 0; i < _FilterOrder; i++)
                {
                    Pole[i] = new clsPoleZeroObject();                      // Indices for pole frequencies range from 0 to n-1
                    Pole[i].Type = clsPoleZeroObject.PoleZeroType.Pole;
                    if (i > 0)
                    {
                        Zero[i] = new clsPoleZeroObject();           // Indices for zero frequencies range from 1 to n-1 (Zero 0 is not initialized)
                        Zero[i].Type = clsPoleZeroObject.PoleZeroType.Zero;
                    }
                }

                for (i = 0; i <= _FilterOrder; i++)
                {
                    CoeffB[i] = new clsCoefficientObject();                 // Indices for B-coefficients range from 0 to n
                    if (i > 0) CoeffA[i] = new clsCoefficientObject();      // Indices for A-coefficients range from 1 to n (A0 is not initialized)
                }

                UpdateCoefficients();                           // Update coefficients

                return; 
            }
        }

        private dcldScalingMethod _QScalingMethod = dcldScalingMethod.DCLD_SCLMOD_SINGLE_BIT_SHIFT;
        internal dcldScalingMethod ScalingMethod
        {
            get { return _QScalingMethod; }
            set { 
                    _QScalingMethod = value;
                    switch (_QScalingMethod)
                    { 
                        case dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT:
                            _DataStructureType = dcldDataStructureType.DCLD_DST_FFLOAT_Q15;
                            break;

                        default:
                            _DataStructureType = dcldDataStructureType.DCLD_DST_Q15;
                            break;
                    }
                    ScaleCoefficients(); 
                    return; 
                }
        }

        private string _QScalingMethodID = "";
        internal string ScalingMethodID
        {
            get { return _QScalingMethodID; }
            set { _QScalingMethodID = value; return; }
        }

        private int _QFormat = 15;
        internal int QFormat              // Q-number format used for coefficient generation
        {
            get { return _QFormat; }
            set
            {
                _QFormat = value;
                _QNumMax = ((Math.Pow(2, _QFormat) - 1) / Math.Pow(2, _QFormat));
                _QHexWidth = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(_QFormat) / 4.0));
                _QBinWidth = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(_QFormat) / 8.0)) * 8;

                UpdateCoefficients();

                return;
            }
        }

        private double _QNumMax = ((Math.Pow(2, 15) - 1) / Math.Pow(2, 15));
        internal double QNumMax           // Maximum positive fractional number based on Q-number format selected
        {
            get { return _QNumMax; }
        }

        private double _QNumMin = -(Math.Pow(2, 15) / Math.Pow(2, 15));
        internal double QNumMin           // Maximum negative fractional number based on Q-number format selected
        {
            get { return _QNumMin; }
        }

        private Int64 _QHexWidth = 4;
        internal Int64 QHexWidth            // Length of the HEX number based on Q-number format selected
        {
            get { return _QHexWidth; }
        }

        private Int64 _QBinWidth = 16;
        internal Int64 QBinWidth            // Length of the BIN number based on Q-number format selected
        {
            get { return _QBinWidth; }
        }

        private double _InputDataResolution = 12;
        internal double InputDataResolution     // Input data resolution in [Bit]
        {
            get { return _InputDataResolution; }
            set { _InputDataResolution = value; return; }
        }

        private double _SamplingFrequency = 1.000;
        internal double SamplingFrequency   // Sampling freq determining the convolution ratio of the digital filter
        {
            get { return _SamplingFrequency; }
            set { _SamplingFrequency = value; _SamplingInterval = (1 / _SamplingFrequency); UpdateCoefficients(); return; }
        }

        private double _SamplingInterval = 1.000;
        internal double SamplingInterval   // Sampling interval determed by sampling freq set
        {
            get { return _SamplingInterval; }
            set { _SamplingInterval = value; UpdateCoefficients(); return; }
        }

        private double _PrimaryZOH = 0.500;
        internal double PrimaryZOH   // Primary input-to-output delay in number of sampling intervals
        {
            get { return _PrimaryZOH; }
            set { _PrimaryZOH = value; UpdateCoefficients(); return; }
        }

        internal clsCoefficientObject OutputScalingFactor = new clsCoefficientObject();   // Output factor log_decade_scaler applied to A- and B-coefficients (single bit-shift + Output factor scaling method only)

        private double _InputGain = 1.000;
        internal double InputGain        // Scaling factor of the input data (affects cross-over freq of pole at the origin only)
        {
            get { return _InputGain; }
            set { _InputGain = value; UpdateCoefficients(); return; }
        }

        private bool _InputGainNormalization = false;
        internal bool InputGainNormalization
        {
            get { return _InputGainNormalization; }
            set { _InputGainNormalization = value; UpdateCoefficients(); return; }
        }

        private bool _IsBidirectional = false;
        internal bool IsBidirectional        // Specifies if input value has a static offset which will be subtracted from the input signal
        {
            get { return _IsBidirectional; }
            set { _IsBidirectional = value; return; }
        }

        private bool _FeedbackRecitification = false;
        internal bool FeedbackRecitification        // Specifies if bi-directional input signals will/can be inverted/rectified
        {
            get { return _FeedbackRecitification; }
            set { _FeedbackRecitification = value; return; }
        }

        private double _OutputGain = 1.000;
        internal double OutputGain        // Scaling factor of the output data (affects all coefficients)
        {
            get { return _OutputGain; }
            set { _OutputGain = value; UpdateCoefficients(); return; }
        }

        private bool _OutputGainNormalization = false;
        internal bool OutputGainNormalization
        {
            get { return _OutputGainNormalization; }
            set { _OutputGainNormalization = value; UpdateCoefficients(); return; }
        }

        private void DebugInfoAppend(string debug_msg)
        {
            _debugInfo.Append(debug_msg);
            if (_debugInfo.Length > 2)
                if (_debugInfo.ToString().Substring(0, 2) == "\r\n")
                    _debugInfo.Replace("\r\n", "", 0, 1);
        }

        private StringBuilder _debugInfo = new StringBuilder();
        internal StringBuilder DebugInfo
        {
            get { return (_debugInfo);  }
        }

        internal clsPoleZeroObject[] Pole;
        internal clsPoleZeroObject[] Zero;

        internal clsCoefficientObject[] CoeffA;              // Coefficient of control output filter term
        internal clsCoefficientObject[] CoeffB;              // Coefficient of error input filter term

        internal clsTransferFunction TransferFunction;       // Compensator Transfer Function Data Series

        // Control loop scalers used in assembly code generator
        private int _PreScaler = 0;
        internal int PreScaler
        {
            get { return (_PreScaler); }
        }

        private int _PostShiftA = 0;
        internal int PostShiftA
        {
            get { return (_PostShiftA); }
        }

        private int _PostShiftB = 0;
        internal int PostShiftB
        {
            get { return (_PostShiftB); }
        }

        private int _PostScaler;
        internal int PostScaler
        {
            get { return (_PostScaler); }
        }


        // Internal functions

        private int GetQNumberScaler(double float_number, double q_factor)
        {
            Int64 int_dummy = 0;
            double div_dummy = 0.0;

            _debugInfo.Append("GetQNumberScaler(float_number=" + float_number.ToString() + ", q_factor=" + q_factor.ToString() + ") => ");

            if (float.IsInfinity((float)float_number))
            {
                int_dummy = 0;
                _debugInfo.Append("false\r\n");
            }
            else
            { 
                if (float_number == -1.000)
                { // Special case => leave number as it is
                    int_dummy = 0;
                }
                else if ((Math.Abs(float_number) >= Math.Abs(q_factor)) && (q_factor != 0.0))
                { // Scale number down

                    if (float_number == 1.000) { float_number = q_factor; }

                    int_dummy = Convert.ToInt64(Math.Ceiling(Math.Abs(float_number / q_factor)));
                    if (int_dummy > 0) { div_dummy = Math.Ceiling(Math.Log(int_dummy, 2)); }
                    else { div_dummy = 0; }
                    int_dummy = -(int)div_dummy;
                }
                else if (float_number != 0.0)
                { // Scale number up
                    int_dummy = Convert.ToInt64(Math.Floor(Math.Abs(q_factor / float_number)));
                    if (int_dummy > 0) { div_dummy = Math.Floor(Math.Log(int_dummy, 2)); }
                    else { div_dummy = 0; }
                    int_dummy = (int)div_dummy;
                }
            }

            _debugInfo.Append(int_dummy.ToString() + "\r\n");
            return ((int)int_dummy);
        }

        private bool UpdateCoefficients()
        {

            int i=0;
            double _Ts = 0.0, _wp0s = 0.0;
            bool fres = true;

            if (!_AutoUpdate) return (fres);

            _Ts = _SamplingInterval;

            if (_InputGainNormalization)
            { _wp0s = (1.0 / _InputGain) * Pole[0].Radians; }
            else
            { _wp0s = Pole[0].Radians; }
            
            _debugInfo.Append("UpdateCoefficients() => " +
                "Filter Order = " + _FilterOrder.ToString() + "\r\n" +
                "SamplingInterval = " + _Ts.ToString() + "\r\n" +
                "Input Gain = " + _InputGain.ToString() + "\r\n" +
                "Output Gain = " + _OutputGain.ToString() + "\r\n" +
                "Zero-Pole (Hz/rad)= " + Pole[0].Frequency.ToString() + "/" + Pole[0].Radians.ToString() + "\r\n" +
                "Omega P0 = " + _wp0s.ToString() + "\r\n" +
                "");

            try
            {

                // Reset and initialize coefficient arrays
                OutputScalingFactor.QFractionalBits = _QFormat;
                OutputScalingFactor.Float64 = 0.000;

                for (i = 1; i < CoeffA.Length; i++)
                { CoeffA[i].QFractionalBits = _QFormat; CoeffA[i].Float64 = 0.000; }
                for (i = 0; i < CoeffB.Length; i++)
                { CoeffB[i].QFractionalBits = _QFormat; CoeffB[i].Float64 = 0.000; }

                _debugInfo.Append(
                    "Fractional Number Width = " + OutputScalingFactor.QFractionalBits.ToString() + "\r\n" +
                    "");


                // Calculate A and B coefficients to coefficent objects in accordance with selected filter order
                switch(_FilterOrder)
                {
                    case 1:
                        // Calculate control output term filter coefficients
                        CoeffA[1].Float64 = 1.000;

                        // Calculate erro input term filter coefficients
                        CoeffB[0].Float64 = (_wp0s * _Ts) / 2;
                        CoeffB[1].Float64 = (_wp0s * _Ts) / 2;

                        break;

                    case 2:
                        // Calculate control output term filter coefficients
                        CoeffA[1].Float64 = 4 / (_Ts * Pole[1].Radians + 2);
                        CoeffA[2].Float64 = (_Ts * Pole[1].Radians - 2) / (_Ts * Pole[1].Radians + 2);

                        // Calculate erro input term filter coefficients
                        CoeffB[0].Float64 = _wp0s * (_Ts * Pole[1].Radians * (_Ts * Zero[1].Radians + 2)) / (2 * (_Ts * Pole[1].Radians + 2) * Zero[1].Radians);
                        CoeffB[1].Float64 = _wp0s * (Math.Pow(_Ts, 2) * Pole[1].Radians) / (_Ts * Pole[1].Radians + 2);
                        CoeffB[2].Float64 = _wp0s * (_Ts * Pole[1].Radians * (_Ts * Zero[1].Radians - 2)) / (2 * (_Ts * Pole[1].Radians + 2) * Zero[1].Radians);
                        
                        break;

                    case 3:
                        // Calculate control output term filter coefficients
                        CoeffA[1].Float64 = (2 * _Ts * (Pole[2].Radians + Pole[1].Radians) - Math.Pow(_Ts, 2) * Pole[1].Radians * Pole[2].Radians + 12) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2));
                        CoeffA[2].Float64 = (2 * _Ts * (Pole[2].Radians + Pole[1].Radians) + Math.Pow(_Ts, 2) * Pole[1].Radians * Pole[2].Radians - 12) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2));
                        CoeffA[3].Float64 = ((_Ts * Pole[1].Radians - 2) * (_Ts * Pole[2].Radians - 2)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2));

                        // Calculate erro input term filter coefficients
                        CoeffB[0].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * (_Ts * Zero[1].Radians + 2) * (_Ts * Zero[2].Radians + 2)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * Zero[1].Radians * Zero[2].Radians);
                        CoeffB[1].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * (2 * _Ts * (Zero[2].Radians + Zero[1].Radians) + 3 * Math.Pow(_Ts, 2) * Zero[1].Radians * Zero[2].Radians - 4)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * Zero[1].Radians * Zero[2].Radians);
                        CoeffB[2].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * (-2 * _Ts * (Zero[2].Radians + Zero[1].Radians) + 3 * Math.Pow(_Ts, 2) * Zero[1].Radians * Zero[2].Radians - 4)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * Zero[1].Radians * Zero[2].Radians);
                        CoeffB[3].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * (_Ts * Zero[1].Radians - 2) * (_Ts * Zero[2].Radians - 2)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * Zero[1].Radians * Zero[2].Radians);

                        break;

                    case 4:
                        // Calculate control output term filter coefficients
                        CoeffA[1].Float64 = -(2 * (-4 * _Ts * (Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) + Math.Pow(_Ts, 3) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians - 16)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2));
                        CoeffA[2].Float64 = (4 * (Math.Pow(_Ts, 2) * (Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) - 12)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2));
                        CoeffA[3].Float64 = (2 * (-4 * _Ts * (Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) + Math.Pow(_Ts, 3) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians + 16)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2));
                        CoeffA[4].Float64 = ((_Ts * Pole[1].Radians - 2) * (_Ts * Pole[2].Radians - 2) * (_Ts * Pole[3].Radians - 2)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2));

                        // Calculate error input term filter coefficients
                        CoeffB[0].Float64 = (_Ts * _wp0s * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * (_Ts * Zero[1].Radians + 2) * (_Ts * Zero[2].Radians + 2) * (_Ts * Zero[3].Radians + 2)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians);
                        CoeffB[1].Float64 = (2 * _Ts * _wp0s * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * (Math.Pow(_Ts, 2) * (Zero[2].Radians * Zero[3].Radians + Zero[1].Radians * Zero[3].Radians + Zero[1].Radians * Zero[2].Radians) + Math.Pow(_Ts, 3) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians - 4)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians);
                        CoeffB[2].Float64 = (Math.Pow(_Ts, 2) * _wp0s * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * (3 * Math.Pow(_Ts, 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians - 4 * (Zero[3].Radians + Zero[2].Radians + Zero[1].Radians))) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians);
                        CoeffB[3].Float64 = (2 * _Ts * _wp0s * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * (-Math.Pow(_Ts, 2) * (Zero[2].Radians * Zero[3].Radians + Zero[1].Radians * Zero[3].Radians + Zero[1].Radians * Zero[2].Radians) + Math.Pow(_Ts, 3) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians + 4)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians);
                        CoeffB[4].Float64 = (_Ts * _wp0s * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * (_Ts * Zero[1].Radians - 2) * (_Ts * Zero[2].Radians - 2) * (_Ts * Zero[3].Radians - 2)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians);
                        
                        break;

                    case 5:
                        // Calculate control output term filter coefficients
                        CoeffA[1].Float64 = (-2 * Math.Pow(_Ts, 3) * (Pole[2].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians) + 4 * Math.Pow(_Ts, 2) * (Pole[3].Radians * Pole[4].Radians + Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[4].Radians + Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) + 24 * _Ts * (Pole[4].Radians + Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) - 3 * Math.Pow(_Ts, 4) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians + 80) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2));
                        CoeffA[2].Float64 = -(2 * (-2 * Math.Pow(_Ts, 3) * (Pole[2].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians) - 4 * Math.Pow(_Ts, 2) * (Pole[3].Radians * Pole[4].Radians + Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[4].Radians + Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) + 8 * _Ts * (Pole[4].Radians + Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) + Math.Pow(_Ts, 4) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians + 80)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2));
                        CoeffA[3].Float64 = (2 * (2 * Math.Pow(_Ts, 3) * (Pole[2].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians) - 4 * Math.Pow(_Ts, 2) * (Pole[3].Radians * Pole[4].Radians + Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[4].Radians + Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) - 8 * _Ts * (Pole[4].Radians + Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) + Math.Pow(_Ts, 4) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians + 80)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2));
                        CoeffA[4].Float64 = (-2 * Math.Pow(_Ts, 3) * (Pole[2].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians) - 4 * Math.Pow(_Ts, 2) * (Pole[3].Radians * Pole[4].Radians + Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[4].Radians + Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) + 24 * _Ts * (Pole[4].Radians + Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) + 3 * Math.Pow(_Ts, 4) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians - 80) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2));
                        CoeffA[5].Float64 = ((_Ts * Pole[1].Radians - 2) * (_Ts * Pole[2].Radians - 2) * (_Ts * Pole[3].Radians - 2) * (_Ts * Pole[4].Radians - 2)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2));

                        // Calculate error input term filter coefficients
                        CoeffB[0].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * (_Ts * Zero[1].Radians + 2) * (_Ts * Zero[2].Radians + 2) * (_Ts * Zero[3].Radians + 2) * (_Ts * Zero[4].Radians + 2)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians);
                        CoeffB[1].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * (6 * Math.Pow(_Ts, 3) * (Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians) + 4 * Math.Pow(_Ts, 2) * (Zero[3].Radians * Zero[4].Radians + Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[4].Radians + Zero[2].Radians * Zero[3].Radians + Zero[1].Radians * Zero[3].Radians + Zero[1].Radians * Zero[2].Radians) - 8 * _Ts * (Zero[4].Radians + Zero[3].Radians + Zero[2].Radians + Zero[1].Radians) + 5 * Math.Pow(_Ts, 4) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians - 48)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians);
                        CoeffB[2].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * (2 * Math.Pow(_Ts, 3) * (Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians) - 4 * Math.Pow(_Ts, 2) * (Zero[3].Radians * Zero[4].Radians + Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[4].Radians + Zero[2].Radians * Zero[3].Radians + Zero[1].Radians * Zero[3].Radians + Zero[1].Radians * Zero[2].Radians) - 8 * _Ts * (Zero[4].Radians + Zero[3].Radians + Zero[2].Radians + Zero[1].Radians) + 5 * Math.Pow(_Ts, 4) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + 16)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians);
                        CoeffB[3].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * (-2 * Math.Pow(_Ts, 3) * (Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians) - 4 * Math.Pow(_Ts, 2) * (Zero[3].Radians * Zero[4].Radians + Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[4].Radians + Zero[2].Radians * Zero[3].Radians + Zero[1].Radians * Zero[3].Radians + Zero[1].Radians * Zero[2].Radians) + 8 * _Ts * (Zero[4].Radians + Zero[3].Radians + Zero[2].Radians + Zero[1].Radians) + 5 * Math.Pow(_Ts, 4) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + 16)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians);
                        CoeffB[4].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * (-6 * Math.Pow(_Ts, 3) * (Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians) + 4 * Math.Pow(_Ts, 2) * (Zero[3].Radians * Zero[4].Radians + Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[4].Radians + Zero[2].Radians * Zero[3].Radians + Zero[1].Radians * Zero[3].Radians + Zero[1].Radians * Zero[2].Radians) + 8 * _Ts * (Zero[4].Radians + Zero[3].Radians + Zero[2].Radians + Zero[1].Radians) + 5 * Math.Pow(_Ts, 4) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians - 48)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians);
                        CoeffB[5].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * (_Ts * Zero[1].Radians - 2) * (_Ts * Zero[2].Radians - 2) * (_Ts * Zero[3].Radians - 2) * (_Ts * Zero[4].Radians - 2)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians);
                        
                        break;

                    case 6:
                        // Calculate control output term filter coefficients
                        CoeffA[1].Float64 = -(4 * (Math.Pow(_Ts, 4) * (Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians) - 4 * Math.Pow(_Ts, 2) * (Pole[4].Radians * Pole[5].Radians + Pole[3].Radians * Pole[5].Radians + Pole[2].Radians * Pole[5].Radians + Pole[1].Radians * Pole[5].Radians + Pole[3].Radians * Pole[4].Radians + Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[4].Radians + Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) - 16 * _Ts * (Pole[5].Radians + Pole[4].Radians + Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) + Math.Pow(_Ts, 5) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians - 48)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2));
                        CoeffA[2].Float64 = (2 * Math.Pow(_Ts, 4) * (Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians) + 12 * Math.Pow(_Ts, 3) * (Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[2].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[4].Radians * Pole[5].Radians + Pole[2].Radians * Pole[3].Radians * Pole[5].Radians + Pole[1].Radians * Pole[3].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[5].Radians + Pole[2].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians) + 8 * Math.Pow(_Ts, 2) * (Pole[4].Radians * Pole[5].Radians + Pole[3].Radians * Pole[5].Radians + Pole[2].Radians * Pole[5].Radians + Pole[1].Radians * Pole[5].Radians + Pole[3].Radians * Pole[4].Radians + Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[4].Radians + Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) - 80 * _Ts * (Pole[5].Radians + Pole[4].Radians + Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) - 5 * Math.Pow(_Ts, 5) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians - 480) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2));
                        CoeffA[3].Float64 = (8 * (Math.Pow(_Ts, 4) * (Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians) - 4 * Math.Pow(_Ts, 2) * (Pole[4].Radians * Pole[5].Radians + Pole[3].Radians * Pole[5].Radians + Pole[2].Radians * Pole[5].Radians + Pole[1].Radians * Pole[5].Radians + Pole[3].Radians * Pole[4].Radians + Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[4].Radians + Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) + 80)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2));
                        CoeffA[4].Float64 = (2 * Math.Pow(_Ts, 4) * (Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians) - 12 * Math.Pow(_Ts, 3) * (Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[2].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[4].Radians * Pole[5].Radians + Pole[2].Radians * Pole[3].Radians * Pole[5].Radians + Pole[1].Radians * Pole[3].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[5].Radians + Pole[2].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians) + 8 * Math.Pow(_Ts, 2) * (Pole[4].Radians * Pole[5].Radians + Pole[3].Radians * Pole[5].Radians + Pole[2].Radians * Pole[5].Radians + Pole[1].Radians * Pole[5].Radians + Pole[3].Radians * Pole[4].Radians + Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[4].Radians + Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) + 80 * _Ts * (Pole[5].Radians + Pole[4].Radians + Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) + 5 * Math.Pow(_Ts, 5) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians - 480) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2));
                        CoeffA[5].Float64 = (4 * (-Math.Pow(_Ts, 4) * (Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[4].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[5].Radians + Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians) + 4 * Math.Pow(_Ts, 2) * (Pole[4].Radians * Pole[5].Radians + Pole[3].Radians * Pole[5].Radians + Pole[2].Radians * Pole[5].Radians + Pole[1].Radians * Pole[5].Radians + Pole[3].Radians * Pole[4].Radians + Pole[2].Radians * Pole[4].Radians + Pole[1].Radians * Pole[4].Radians + Pole[2].Radians * Pole[3].Radians + Pole[1].Radians * Pole[3].Radians + Pole[1].Radians * Pole[2].Radians) - 16 * _Ts * (Pole[5].Radians + Pole[4].Radians + Pole[3].Radians + Pole[2].Radians + Pole[1].Radians) + Math.Pow(_Ts, 5) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians + 48)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2));
                        CoeffA[6].Float64 = ((_Ts * Pole[1].Radians - 2) * (_Ts * Pole[2].Radians - 2) * (_Ts * Pole[3].Radians - 2) * (_Ts * Pole[4].Radians - 2) * (_Ts * Pole[5].Radians - 2)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2));

                        // Calculate error input term filter coefficients
                        CoeffB[0].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians * (_Ts * Zero[1].Radians + 2) * (_Ts * Zero[2].Radians + 2) * (_Ts * Zero[3].Radians + 2) * (_Ts * Zero[4].Radians + 2) * (_Ts * Zero[5].Radians + 2)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians);
                        CoeffB[1].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians * (4 * Math.Pow(_Ts, 4) * (Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians) + 4 * Math.Pow(_Ts, 3) * (Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians) - 16 * _Ts * (Zero[5].Radians + Zero[4].Radians + Zero[3].Radians + Zero[2].Radians + Zero[1].Radians) + 3 * Math.Pow(_Ts, 5) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians - 64)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians);
                        CoeffB[2].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians * (10 * Math.Pow(_Ts, 4) * (Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians) - 4 * Math.Pow(_Ts, 3) * (Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians) - 24 * Math.Pow(_Ts, 2) * (Zero[4].Radians * Zero[5].Radians + Zero[3].Radians * Zero[5].Radians + Zero[2].Radians * Zero[5].Radians + Zero[1].Radians * Zero[5].Radians + Zero[3].Radians * Zero[4].Radians + Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[4].Radians + Zero[2].Radians * Zero[3].Radians + Zero[1].Radians * Zero[3].Radians + Zero[1].Radians * Zero[2].Radians) - 16 * _Ts * (Zero[5].Radians + Zero[4].Radians + Zero[3].Radians + Zero[2].Radians + Zero[1].Radians) + 15 * Math.Pow(_Ts, 5) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + 160)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians);
                        CoeffB[3].Float64 = _wp0s * (2 * Math.Pow(_Ts, 2) * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians * (-4 * Math.Pow(_Ts, 2) * (Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians) + 16 * (Zero[5].Radians + Zero[4].Radians + Zero[3].Radians + Zero[2].Radians + Zero[1].Radians) + 5 * Math.Pow(_Ts, 4) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians);
                        CoeffB[4].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians * (-10 * Math.Pow(_Ts, 4) * (Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians) - 4 * Math.Pow(_Ts, 3) * (Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians) + 24 * Math.Pow(_Ts, 2) * (Zero[4].Radians * Zero[5].Radians + Zero[3].Radians * Zero[5].Radians + Zero[2].Radians * Zero[5].Radians + Zero[1].Radians * Zero[5].Radians + Zero[3].Radians * Zero[4].Radians + Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[4].Radians + Zero[2].Radians * Zero[3].Radians + Zero[1].Radians * Zero[3].Radians + Zero[1].Radians * Zero[2].Radians) - 16 * _Ts * (Zero[5].Radians + Zero[4].Radians + Zero[3].Radians + Zero[2].Radians + Zero[1].Radians) + 15 * Math.Pow(_Ts, 5) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians - 160)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians);
                        CoeffB[5].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians * (-4 * Math.Pow(_Ts, 4) * (Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians) + 4 * Math.Pow(_Ts, 3) * (Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[4].Radians * Zero[5].Radians + Zero[1].Radians * Zero[4].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[3].Radians * Zero[5].Radians + Zero[1].Radians * Zero[2].Radians * Zero[5].Radians + Zero[2].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[3].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[4].Radians + Zero[1].Radians * Zero[2].Radians * Zero[3].Radians) - 16 * _Ts * (Zero[5].Radians + Zero[4].Radians + Zero[3].Radians + Zero[2].Radians + Zero[1].Radians) + 3 * Math.Pow(_Ts, 5) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians + 64)) / ((_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians);
                        CoeffB[6].Float64 = _wp0s * (_Ts * Pole[1].Radians * Pole[2].Radians * Pole[3].Radians * Pole[4].Radians * Pole[5].Radians * (_Ts * Zero[1].Radians - 2) * (_Ts * Zero[2].Radians - 2) * (_Ts * Zero[3].Radians - 2) * (_Ts * Zero[4].Radians - 2) * (_Ts * Zero[5].Radians - 2)) / (2 * (_Ts * Pole[1].Radians + 2) * (_Ts * Pole[2].Radians + 2) * (_Ts * Pole[3].Radians + 2) * (_Ts * Pole[4].Radians + 2) * (_Ts * Pole[5].Radians + 2) * Zero[1].Radians * Zero[2].Radians * Zero[3].Radians * Zero[4].Radians * Zero[5].Radians);

                        break;

                    default:
                        break;

                }

                // Perform output gain scaling
                if (_OutputGainNormalization) 
                { 
                    for (i = 1; i < CoeffA.Length; i++)
                    { CoeffA[i].Float64 *= _OutputGain; }
                    for (i = 0; i < CoeffB.Length; i++)
                    { CoeffB[i].Float64 *= _OutputGain; }
                }

                // generate debug info on coefficient calculation
                if (_FilterOrder > 1)
                {
                    for (i = 1; i < _FilterOrder; i++)
                    { _debugInfo.Append("Pole[" + i.ToString() + "]     (float64) = " + Pole[i].Frequency.ToString() + "/" + Pole[i].Radians.ToString() + "\r\n"); }
                    for (i = 1; i < _FilterOrder; i++)
                    { _debugInfo.Append("Zero[" + i.ToString() + "]     (float64) = " + Zero[i].Frequency.ToString() + "/" + Zero[i].Radians.ToString() + "\r\n"); }
                }

                for (i = 1; i < CoeffA.Length; i++)
                { _debugInfo.Append("CoeffA[" + i.ToString() + "]   (float64) = " + CoeffA[i].Float64.ToString() + "\r\n"); }
                for (i = 0; i < CoeffB.Length; i++)
                { _debugInfo.Append("CoeffB[" + i.ToString() + "]   (float64) = " + CoeffB[i].Float64.ToString() + "\r\n"); }

                // scale determined coefficients to Qxx Number format selected using the selected scaling method
                ScaleCoefficients();

                // calculate numeric results of transfer function based on recently selected pole & zero locations
                fres = UpdateTransferFunction();


                return (fres);

            }
            catch (Exception ex)
            {
                _debugInfo.Append("UpdateCoefficients() Error (0x" + ex.HResult.ToString("X") + "): " + ex.Message + "\r\n");
                fres = false;
                return (fres);
            }

        }

        private bool UpdateTransferFunction()
        {
            double start_f = 0.0, stop_f = 0.0;
            double decades = 0.0;
            int datapoints = 0;
            bool fres = false;

            if (Math.Floor(Math.Log10(_SamplingFrequency)) != Math.Round(Math.Log10(_SamplingFrequency), 1))
            {
                decades = Math.Floor(Math.Log10(_SamplingFrequency)) + 1;
            }
            else 
            {
                decades = Math.Floor(Math.Log10(_SamplingFrequency));
            }

            start_f = 1.0;
            stop_f = Math.Pow(10, decades);
            datapoints = 1601; //401;

            if (_InputGain == 0.0) return (false);

            TransferFunction.Pole = Pole;
            TransferFunction.Zero = Zero;
            if (_InputGainNormalization)
            { TransferFunction.InputGain = 1.0; }
            else
            { TransferFunction.InputGain = _InputGain; }
            TransferFunction.SamplingFrequency = _SamplingFrequency;
            TransferFunction.PrimaryZOH = _PrimaryZOH;

            //stop_f = 10000000;
            try
            {
                TransferFunction.CreateTransferFunction(start_f, stop_f, datapoints);
                fres = true;
            }
            catch
            {
                fres = false;
            }

            return (fres);

        }

        private bool ScaleCoefficients()
        {
            int i = 0;
            bool UpdateValueResult = true;
            double ACoeffMax = 0.0, BCoeffMax = 0.0;
            int _QScalerSingle = 0, _QScalerDualCoeffA = 0, _QScalerDualCoeffB = 0;

            if (!_AutoUpdate) return (true);

            _debugInfo.Append("ScaleCoefficients() => \r\n");

            try
            {

                // Search coefficient arrays for largest number
                for (i = 1; i < CoeffA.Length; i++)
                { if (Math.Abs(ACoeffMax) < Math.Abs(CoeffA[i].Float64)) ACoeffMax = CoeffA[i].Float64; }
                for (i = 0; i < CoeffB.Length; i++)
                { if (Math.Abs(BCoeffMax) < Math.Abs(CoeffB[i].Float64)) BCoeffMax = CoeffB[i].Float64; }


                // Select scaled number 

                switch (_QScalingMethod)
                {
                    case dcldScalingMethod.DCLD_SCLMOD_SINGLE_BIT_SHIFT:

                        // Determine Common Single Bit-Shift Coefficient Scaler
                        if (Math.Abs(ACoeffMax) < Math.Abs(BCoeffMax)) ACoeffMax = BCoeffMax;

                        _QScalerSingle = GetQNumberScaler(ACoeffMax, _QNumMax);
                        _QScalerDualCoeffA = 0;
                        _QScalerDualCoeffB = 0;

                        for (i = 1; i < CoeffA.Length; i++)
                        { 
                            CoeffA[i].QScaler = _QScalerSingle; 
                            CoeffA[i].HasOutputScalingFactor = false; 
                            CoeffA[i].OutputScalingFactor = 0.000;
                            UpdateValueResult &= CoeffA[i].UpdateValues(false);
                        }
                        for (i = 0; i < CoeffB.Length; i++)
                        { 
                            CoeffB[i].QScaler = _QScalerSingle; 
                            CoeffB[i].HasOutputScalingFactor = false; 
                            CoeffB[i].OutputScalingFactor = 0.000;
                            UpdateValueResult &= CoeffB[i].UpdateValues(false);
                        }

                        // Control loop scalers for assembly code generator
                        _PreScaler = (_QFormat - Convert.ToInt32(_InputDataResolution));
                        _PostShiftA = CoeffA[1].QScaler;
                        _PostShiftB = 0;
                        _PostScaler = 0;

                        break;

                    case dcldScalingMethod.DCLD_SCLMOD_OUTPUT_SCALING_FACTOR:

                        // Determine Common Single Coefficient Scaler Factor
                        if (Math.Abs(ACoeffMax) < Math.Abs(BCoeffMax)) ACoeffMax = BCoeffMax;

                        // Determine and scale control output scaling factor
                        OutputScalingFactor.Float64 = (ACoeffMax / _QNumMax);
                        OutputScalingFactor.QScaler = GetQNumberScaler(OutputScalingFactor.Float64, _QNumMax);
                        OutputScalingFactor.HasOutputScalingFactor = false;
                        OutputScalingFactor.OutputScalingFactor = 0.000;
                        UpdateValueResult &= OutputScalingFactor.UpdateValues(false);

                         // Determine Common Single Bit-Shift Coefficient Scaler
                        ACoeffMax = 0;
                        BCoeffMax = 0;

                        // Search coefficient arrays for largest number
                        for (i = 1; i < CoeffA.Length; i++)
                        { if (Math.Abs(ACoeffMax) < Math.Abs(CoeffA[i].Float64 / OutputScalingFactor.Float64)) { ACoeffMax = CoeffA[i].Float64 / OutputScalingFactor.Float64; } }
                        for (i = 0; i < CoeffB.Length; i++)
                        { if (Math.Abs(ACoeffMax) < Math.Abs(CoeffB[i].Float64 / OutputScalingFactor.Float64)) { BCoeffMax = CoeffB[i].Float64 / OutputScalingFactor.Float64; } }

                        if (Math.Abs(ACoeffMax) < Math.Abs(BCoeffMax)) ACoeffMax = BCoeffMax;

                        _QScalerSingle = GetQNumberScaler(ACoeffMax, _QNumMax);
                        _QScalerDualCoeffA = 0;
                        _QScalerDualCoeffB = 0;

                        for (i = 1; i < CoeffA.Length; i++)
                        { 
                            CoeffA[i].QScaler = _QScalerSingle; 
                            CoeffA[i].HasOutputScalingFactor = true; 
                            CoeffA[i].OutputScalingFactor = OutputScalingFactor.Float64;
                            UpdateValueResult &= CoeffA[i].UpdateValues(false);
                        }
                        for (i = 0; i < CoeffB.Length; i++)
                        { 
                            CoeffB[i].QScaler = _QScalerSingle; 
                            CoeffB[i].HasOutputScalingFactor = true; 
                            CoeffB[i].OutputScalingFactor = OutputScalingFactor.Float64;
                            UpdateValueResult &= CoeffB[i].UpdateValues(false);
                        }

                        // Control loop scalers for assembly code generator
                        _PreScaler = (_QFormat - Convert.ToInt32(_InputDataResolution));
                        _PostShiftA = OutputScalingFactor.QScaler;
                        _PostShiftB = 0;
                        _PostScaler = OutputScalingFactor.Int;

                        break;

                    case dcldScalingMethod.DCLD_SCLMOD_DUAL_BIT_SHIFT:

                        // Determine A-Coefficient Scaler
                        _QScalerDualCoeffA = GetQNumberScaler(ACoeffMax, _QNumMax);

                        for (i = 1; i < CoeffA.Length; i++)
                        { 
                            CoeffA[i].QScaler = _QScalerDualCoeffA; 
                            CoeffA[i].HasOutputScalingFactor = false; 
                            CoeffA[i].OutputScalingFactor = 0.000;
                            UpdateValueResult &= CoeffA[i].UpdateValues(false);
                        }

                        // Determine B-Coefficient Scaler
                        _QScalerDualCoeffB = GetQNumberScaler(BCoeffMax, _QNumMax);

                        for (i = 0; i < CoeffB.Length; i++)
                        { 
                            CoeffB[i].QScaler = _QScalerDualCoeffB; 
                            CoeffB[i].HasOutputScalingFactor = false; 
                            CoeffB[i].OutputScalingFactor = 0.000;
                            UpdateValueResult &= CoeffB[i].UpdateValues(false);
                        }

                        // Control loop scalers for assembly code generator
                        _PreScaler = (_QFormat - Convert.ToInt32(_InputDataResolution));
                        _PostShiftA = CoeffA[1].QScaler;
                        _PostShiftB = CoeffB[0].QScaler;
                        _PostScaler = 0;

                        break;

                    case dcldScalingMethod.DCLD_SCLMOD_DBLSCL_FLOAT:

                        _QScalerDualCoeffA = 0;
                        _QScalerDualCoeffB = 0;

                        for (i = 1; i < CoeffA.Length; i++)
                        {
                            CoeffA[i].QScaler = GetQNumberScaler(CoeffA[i].Float64, _QNumMax);
                            CoeffA[i].HasOutputScalingFactor = false; 
                            CoeffA[i].OutputScalingFactor = 0.000;
                            UpdateValueResult &= CoeffA[i].UpdateValues(true);
                        }

                        for (i = 0; i < CoeffB.Length; i++)
                        {
                            CoeffB[i].QScaler = GetQNumberScaler(CoeffB[i].Float64, _QNumMax);
                            CoeffB[i].HasOutputScalingFactor = false;
                            CoeffB[i].OutputScalingFactor = 0.000;
                            UpdateValueResult &= CoeffB[i].UpdateValues(true);
                        }

                        // Control loop scalers for assembly code generator
                        _PreScaler = (_QFormat - Convert.ToInt32(_InputDataResolution));
                        _PostShiftA = 0;
                        _PostShiftB = 0;
                        _PostScaler = 0;

                        break;

                    default:
                        break;
                }

                // generate debug information
                if (!UpdateValueResult)
                { _debugInfo.Append("Coefficient values update failed\r\n"); }
                else
                { _debugInfo.Append("Coefficient values have been updated successfully\r\n"); }

                for (i = 1; i < CoeffA.Length; i++)
                {
                    if (CoeffA[i].HasOutputScalingFactor)
                    { _debugInfo.Append("CoeffA[" + i.ToString() + "]   (scaled)  = " +
                        "(0x" + CoeffA[i].Hex + ") " +
                        CoeffA[i].QFractional.ToString() + " x " + 
                        CoeffA[i].OutputScalingFactor.ToString() + 
                        " x 2^(" + CoeffA[i].QScaler.ToString() + ")" + 
                        "\r\n"); }
                    else
                    { _debugInfo.Append("CoeffA[" + i.ToString() + "]   (scaled)  = " +
                        "(0x" + CoeffA[i].Hex + ") " +
                        CoeffA[i].QFractional.ToString() + 
                        " x 2^(" + CoeffA[i].QScaler.ToString() + ")" +
                        "\r\n");
                    }
                }
                for (i = 0; i < CoeffB.Length; i++)
                {
                    if (CoeffB[i].HasOutputScalingFactor)
                    { _debugInfo.Append("CoeffB[" + i.ToString() + "]   (scaled)  = " +
                        "(0x" + CoeffB[i].Hex + ") " +
                        CoeffB[i].QFractional.ToString() + " x " + 
                        CoeffB[i].OutputScalingFactor.ToString() + 
                        " x 2^(" + CoeffB[i].QScaler.ToString() + ")" +
                        "\r\n");
                    }
                    else
                    { _debugInfo.Append("CoeffB[" + i.ToString() + "]   (scaled)  = " +
                        "(0x" + CoeffB[i].Hex + ") " +
                        CoeffB[i].QFractional.ToString() + 
                        " x 2^(" + CoeffB[i].QScaler.ToString() + ")" + 
                        "\r\n"); }
                }

                return (true);
            }
            catch (Exception ex)
            {
                _debugInfo.Append("ScaleCoefficients() Error (0x" + ex.HResult.ToString("X") + "): " + ex.Message + "\r\n");
                return (false);
            }

        }

        public void Update()
        {
            bool bdum = _AutoUpdate;
            bool fres = false;

            _AutoUpdate = true;
            _debugInfo.Clear();
            _debugInfo.Append("nPnZ Generation Debug Info:\r\n");
            fres = UpdateCoefficients();
            _debugInfo.Append("nPnZ debug info complete\r\n");
            _AutoUpdate = bdum;

            return;
        }


        // Instance Constructor.
        public clsCompensatorNPNZ()
        {
            // Initialize values here, if necessary
            TransferFunction = new clsTransferFunction();

            return;

        }

    }
}
