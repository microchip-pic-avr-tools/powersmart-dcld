using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms.DataVisualization.Charting;

//using System.ComponentModel;


namespace dcld
{
    class clsPoleZeroObject
    {
    
        private double _Frequency = 1.000;          // Frequency locations of filter poles
        internal double Frequency 
        {
            get { return _Frequency; }
            set { 
                _Frequency = value; 
                _Radians = 2 * Math.PI * _Frequency;
                return;
            }
        }

        private double _Radians = (2 * Math.PI);
        internal double Radians             // Cross-Over Frequency of pole at the origin
        {
            get { return _Radians; }
            set { _Radians = value; _Frequency = _Radians / (2 * Math.PI); return; }
        }

        public enum PoleZeroType { Pole, Zero };
        private PoleZeroType _Type = PoleZeroType.Pole;
        internal PoleZeroType Type             // Cross-Over Frequency of pole at the origin
        {
            get { return _Type; }
            set { _Type = value; return; }
        }

//        internal Annotation PosAnnotation; => annotations are handled by the main window

    }
}
