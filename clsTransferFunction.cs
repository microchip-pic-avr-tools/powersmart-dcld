using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

using System.Collections;

namespace dcld
{
    class clsTransferFunction
    {

        //Data Buffer;
        private double _StartFrequency = 1.0;
        internal double StartFrequency
        {
            get { return (_StartFrequency); }
            private set { _StartFrequency = value; return; }
        }

        private double _StopFrequency = 100000.0;
        internal double StopFrequency
        {
            get { return (_StopFrequency); }
            private set { _StopFrequency = value; return; }
        }

        private double _SamplingFrequency = 100000.0;
        internal double SamplingFrequency
        {
            get { return (_SamplingFrequency); }
            set { _SamplingFrequency = value; return; }
        }

        private double _PrimaryZOH = 0.500;
        internal double PrimaryZOH   // Primary input-to-output delay in number of sampling intervals
        {
            get { return _PrimaryZOH; }
            set { _PrimaryZOH = value; return; }
        }

        private int _DataPoints = 401;
        internal int DataPoints
        {
            get { return (_DataPoints); }
            set { _DataPoints = value; return; }
        }

        private double[] _FrequencyPoint;
        internal double[] FrequencyPoint
        {
            get { return (_FrequencyPoint); }
            private set { _FrequencyPoint = value; return; }
        }

        private Complex[] _s;

        private Complex[] _TFs_output;
        internal Complex[] TFs_output
        {
            get { return (_TFs_output); }
            private set { _TFs_output = value; return; }
        }

        private Complex[] _TFz_output;
        internal Complex[] TFz_output
        {
            get { return (_TFz_output); }
            private set { _TFz_output = value; return; }
        }

        private double[] _MagGain_s;
        internal double[] MagGain_s
        {
            get { return (_MagGain_s); }
            private set { _MagGain_s = value; return; }
        }

        private double[] _MagPhase_z;
        internal double[] MagPhase_z
        {
            get { return (_MagPhase_z); }
            private set { _MagPhase_z = value; return; }
        }

        private double[] _MagGain_z;
        internal double[] MagGain_z
        {
            get { return (_MagGain_z); }
            private set { _MagGain_z = value; return; }
        }

        private double[] _MagPhase_s;
        internal double[] MagPhase_s
        {
            get { return (_MagPhase_s); }
            private set { _MagPhase_s = value; return; }
        }

        private double _InputGain;
        internal double InputGain
        {
            get { return _InputGain; }
            set { _InputGain = value; return; }
        }
        
        private clsPoleZeroObject[] _Pole;
        internal clsPoleZeroObject[] Pole
        {
            get { return _Pole; }
            set { _Pole = value; return; }
        }

        private clsPoleZeroObject[] _Zero;
        internal clsPoleZeroObject[] Zero
        {
            get { return _Zero; }
            set { _Zero = value; return; }
        }

        private bool _UnwrapPhase = true;
        internal bool UnwrapPhase
        {
            get { return _UnwrapPhase; }
            set { _UnwrapPhase = value; return; }
        }

        // Instance Constructor.
        public clsTransferFunction()
        {
            // Initialize values here, if necessary
            if (_s != null)
            { 
                if (_s.Length > 0)
                { 
                    _TFs_output = new Complex[_s.Length];

                    _MagGain_s = new double[_s.Length];
                    _MagPhase_s = new double[_s.Length];

                    _TFz_output = new Complex[_s.Length];

                    _MagGain_z = new double[_s.Length];
                    _MagPhase_z = new double[_s.Length];
                
                }
            }

            return;

        }

        internal Complex GetTFDataPoint_s(double frequency)
        { 
            int i = 0;
            Complex numerator, denominator, tf_output;
            Complex __s;

            __s = new Complex(0, 2 * Math.PI * frequency);

            numerator = 1;
            denominator = 1;

            for (i = 1; i < _Zero.Length; i++)   // First zero is empty => Index represents filter order
            {
                numerator = Complex.Multiply(numerator, Complex.Add(Complex.Divide(__s, _Zero[i].Radians), Complex.One));
            }

            for (i = 1; i < _Pole.Length; i++)   // Pole "ZERO" is pole at the origin and is considered at the end
            {
                denominator = Complex.Multiply(denominator, Complex.Add(Complex.Divide(__s, _Pole[i].Radians), Complex.One));
            }

            tf_output = Complex.Multiply(_InputGain, (Complex.Multiply(Complex.Divide(_Pole[0].Radians, __s), Complex.Divide(numerator, denominator))));

            return (tf_output);
        
        }

        internal Complex GetTFDataPoint_z(double frequency)
        {
            int i = 0;
            Complex numerator, denominator, tf_output;
            Complex __z;

            __z = new Complex (0, 2 * Math.PI * frequency);

            numerator = Complex.Exp(-2 * 2 * Math.PI * frequency);

            numerator = 0;
            denominator = 1;

            for (i = 1; i < _Zero.Length; i++)   // First zero is empty => Index represents filter order
            {
                numerator += Complex.Multiply(numerator, Complex.Add(Complex.Divide(__z, _Zero[i].Radians), Complex.One));
            }

            for (i = 1; i < _Pole.Length; i++)   // Pole "ZERO" is pole at the origin and is considered at the end
            {
                denominator = Complex.Multiply(denominator, Complex.Add(Complex.Divide(__z, _Pole[i].Radians), Complex.One));
            }

            tf_output = Complex.Multiply((1.0 / _InputGain), (Complex.Multiply(Complex.Divide(_Pole[0].Radians, __z), Complex.Divide(numerator, denominator))));

            return (tf_output);

        }

        internal bool CreateTransferFunction(double start_f = 1.0, double stop_f = 100000.0, int datapoints = 401)
        {
            int i = 0;
            double r = 0.0; 

            double prevPhase = 0.0;
            double PhaseOffset = 0.0;
            double phase = 0.0;

            Complex s_to_z, TF_point;

            r = Math.Pow(stop_f / start_f, 1.0 / datapoints);
            _FrequencyPoint = Enumerable.Range(0, datapoints).Select(n => (start_f * Math.Pow(r, n))).ToArray();
            _s = _FrequencyPoint.Select(__f => new Complex(0, 2 * Math.PI * __f)).ToArray();

            _TFs_output = new Complex[_s.Length];
            _MagGain_s = new double[_s.Length];
            _MagPhase_s = new double[_s.Length];

            _TFz_output = new Complex[_s.Length];
            _MagGain_z = new double[_s.Length];
            _MagPhase_z = new double[_s.Length];

            _StartFrequency = start_f;
            _StopFrequency = stop_f;
            _DataPoints = datapoints;

            for (i = 0; i < _s.Length; i++)
            {
                _TFs_output[i] = GetTFDataPoint_s(FrequencyPoint[i]);

                MagGain_s[i] = 20.0 * Math.Log10(_TFs_output[i].Magnitude);

                phase = _TFs_output[i].Phase * 180 / Math.PI;

                if (_UnwrapPhase)
                { 
                    // Unwarpping phase 
                    if (phase - prevPhase > 180) PhaseOffset -= 360;
                    if (phase - prevPhase < -180) PhaseOffset += 360;
                    prevPhase = phase;
                }
                else { PhaseOffset = 0.0; }

                _MagPhase_s[i] = phase + PhaseOffset;

                // Generating z-transfer function results -2*PI()*(B7/_fsam)
                s_to_z = new Complex((-2.0 * Math.PI * (FrequencyPoint[i] / _SamplingFrequency)), (-2.0 * Math.PI * (FrequencyPoint[i] / _SamplingFrequency)));
                TF_point = TFs_output[i];
                _TFz_output[i] = Complex.Multiply(TF_point, Complex.Exp(s_to_z));

                //_TFs_output[i] = GetTFDataPoint_s(FrequencyPoint[i]/_SamplingFrequency);

            }


            prevPhase = 0;
/*
            double dum = 0.0;
            double f_point = 0.0;
*/
            double f_nyq = 0.0;

            for (i = 0; i < _TFz_output.Length; i++)
            {

                f_nyq = SamplingFrequency / 2;

                /*
                    f_point = (FrequencyPoint[i] - Math.Floor(FrequencyPoint[i] / _SamplingFrequency) * _SamplingFrequency);
                    dum = (1 - (f_point / f_nyq));
                    dum = Math.Exp(-2.0 * Math.PI * f_point / _SamplingFrequency);
                */
//                MagGain_z[i] = 20.0 * Math.Log10(_TFz_output[i].Magnitude * (1 - 2 * Math.Atan((FrequencyPoint[i] / f_nyq)/2)));
//                MagGain_z[i] = MagGain_z[i] / Math.Abs(1 - 2 * Math.Atan((FrequencyPoint[i] / f_nyq)/2));

                MagGain_z[i] = 20.0 * Math.Log10(_TFz_output[i].Magnitude);

                phase = _TFz_output[i].Phase * 180 / Math.PI;

                if (_UnwrapPhase)
                {
                    // Unwarpping phase 
                    if (phase - prevPhase > 180) PhaseOffset -= 360;
                    if (phase - prevPhase < -180) PhaseOffset += 360;
                    prevPhase = phase;

                }
                else { PhaseOffset = 0.0; }

                _MagPhase_z[i] = phase + PhaseOffset;

            }


            return (true);
        }


    }
}
