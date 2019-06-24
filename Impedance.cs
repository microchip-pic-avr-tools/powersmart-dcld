using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace dcld
{
    public interface Impedance
    {
        Complex Z(Complex s);
    }

    class ParCkt : Impedance
    {
        Impedance[] zs;

        public Complex Z(Complex s)
        {
            return Complex.Reciprocal(zs.Select(z => Complex.Reciprocal(z.Z(s))).Aggregate(Complex.Add));
        }

        public ParCkt(Impedance[] _zs)
        {
            zs = _zs;
        }
    }

    class SerCkt : Impedance
    {
        Impedance[] zs;

        public Complex Z(Complex s)
        {
            return zs.Select(z => z.Z(s)).Aggregate(Complex.Add);
        }

        public SerCkt(Impedance[] _zs)
        {
            zs = _zs;
        }
    }

     class ZCap : Impedance
    {
        double C;
        double ESR;
        public Complex Z(Complex s)
        {
            return Complex.Reciprocal(C * s) + Complex.One * ESR;
        }

        public ZCap(double _C, double _ESR, int n=1)
        {
            C = _C * n;
            ESR = _ESR / n;
        }
    }

    class ZInd : Impedance
    {
        double L;
        double DCR;

        public Complex Z(Complex s)
        {
            return DCR * Complex.One + s * L;
        }

        public ZInd(double _L, double _DCR)
        {
            L = _L;
            DCR = _DCR;
        }

    }
}
