using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Numerics;

namespace dcld
{
    class clsCoefficientObject
    {
        private double _Float64 = 0.000;
        public double Float64                        // 64-bit floating point number
        {
            get { return _Float64; }
            set { _Float64 = value; return; }
        }

        private int _QFractionalBits = 15;              // Q-Format bit width (e.g. 15 for Q1.15)
        public int QFractionalBits
        {
            get { return _QFractionalBits; }
            set { _QFractionalBits = value; return; }
        }

        private int _QScaler = 0;                       // Q-Format bit-shift log_decade_scaler (number of bits shifted, where negative = multiply, positive = divide)
        public int QScaler
        {
            get { return _QScaler; }
            set { _QScaler = value; return; }
        }

        private bool _HasOutputScalingFactor = false;   // In case an output scaling factor is used, the scaled float value incorporates an additional factor
        internal bool HasOutputScalingFactor
        {
            get { return _HasOutputScalingFactor; }
            set { _HasOutputScalingFactor = value; return; }
        }

        private double _OutputScalingFactor = 0.000;   // In case an output scaling factor is used, the scaled float value incorporates an additional factor
        internal double OutputScalingFactor
        {
            get { return _OutputScalingFactor; }
            set { _OutputScalingFactor = value; return; }
        }

        private UInt32 _UInt = 0;
        public UInt32 UInt                           // Unsigned Integer number
        {
            get { return _UInt; }
        }

        private Int32 _Int = 0;
        public Int32 Int                           // Signed Integer number
        {
            get { return _Int; }
        }

        private string _Hex = "0000";
        public string Hex                           // Hexadezimal number
        {
            get { return _Hex; }
        }

        private string _Binary = "0000000000000000";
        public string Binary                         // Binary number
        {
            get { return _Binary; }
        }

        private double _FloatScaledFixedPoint = 0.0;
        public double FloatScaledFixedPoint           // 64-bit floating point number of the scaled fractional
        {
            get { return _FloatScaledFixedPoint; }
        }

        private double _QFractional = 0.0;
        public double QFractional               // Q-Format limited fractional number
        {
            get { return _QFractional; }
        }

        private double _FixedPointErr = 0.0;
        public double FixedPointErr          // Fixed point error of the scaled number
        {
            get { return _FixedPointErr; }
        }

        internal bool UpdateValues(bool pfloat_scaling = false)
        {
            int hex_len = 0, bin_len = 0, num_size = 0;

            try
            {
                // read new double precision floating point number

                if (_HasOutputScalingFactor)
                {
                    if ((float.IsNaN((float)_Float64)) || (_OutputScalingFactor == 0.0))
                    { return (false); }
                    else
                    { _FloatScaledFixedPoint = (_Float64 / _OutputScalingFactor) * Math.Pow(2, _QScaler); }
                }
                else
                {
                    if (float.IsNaN((float)_Float64))
                    { return (false); }
                    else
                    { _FloatScaledFixedPoint = _Float64 * Math.Pow(2, _QScaler); }
                }

                _QFractional = Convert.ToInt64(Math.Ceiling((FloatScaledFixedPoint) * Math.Pow(2, _QFractionalBits))) / Math.Pow(2, _QFractionalBits);
                if (_QFractional == 1.000) _QFractional = 1.0 - Math.Pow(2, -(_QFractionalBits));

                _FixedPointErr = (QFractional - FloatScaledFixedPoint) / FloatScaledFixedPoint;

                _Int = Convert.ToInt32(_QFractional * Math.Pow(2, _QFractionalBits));

                if (_Int < 0)
                {
                    _UInt = Convert.ToUInt32(Math.Pow(2, (_QFractionalBits + 1)) + _QFractional * Math.Pow(2, _QFractionalBits));
                }
                else
                {
                    _UInt = Convert.ToUInt32(_Int);
                }

                num_size = (int)(Math.Ceiling(_QFractionalBits / 8.0));
                hex_len = 2 * num_size;
                bin_len = 8 * num_size;

                if (pfloat_scaling)
                {
                    _Hex = NumberBaseConverter.Dec2Hex(_UInt, bin_len, true, false) + NumberBaseConverter.Dec2Hex(_QScaler, 16, true, false);
                    _Binary = NumberBaseConverter.Dec2Bin(_UInt, bin_len, false, false) + NumberBaseConverter.Dec2Bin(_QScaler, 16, false, false);
                }
                else 
                {
                    _Hex = _UInt.ToString("X" + hex_len);
                    _Binary = NumberBaseConverter.Dec2Bin(_UInt, bin_len, true, false);
                }

                return (true);
            }
            catch
            {
                return (false);
            }


        }

    }
}
