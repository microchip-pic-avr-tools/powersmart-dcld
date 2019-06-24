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

using System.Windows.Forms.DataVisualization.Charting;

using System.Collections;

namespace dcld
{
    public partial class frmChart : Form
    {

        System.Windows.Forms.DataVisualization.Charting.Cursor xPosPhase, xPosGain;
        
        public frmChart()
        {
            InitializeComponent();
//            dataCaps.Rows.Add(100, 10, 1);
//            model = new Model(Convert.ToDouble(txtFreqStart.Text), Convert.ToDouble(txtFreqStop.Text), Convert.ToInt32(txtFreqPoints.Text));
            model = new Model(100, 1000000, 801);



            UpdateCharts(true);

            ListPZs = new BindingList<PoleZero>();
//            lstPolesZeroes.DataSource = ListPZs;
            ListPZs.AllowEdit = true;
            ListPZs.AllowNew = true;
            ListPZs.AllowRemove = true;

            chartBode.ChartAreas[0].AxisX.IsLogarithmic = true;
            chartBode.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Number;
            chartBode.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;

            chartBode.ChartAreas[0].AxisX.IsMarginVisible = true;
            chartBode.ChartAreas[0].AxisX.IsStartedFromZero = false;

            chartBode.ChartAreas[0].AxisX.MajorTickMark.Enabled = true;
            chartBode.ChartAreas[0].AxisX.MinorTickMark.Enabled = true;

            chartBode.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chartBode.ChartAreas[0].AxisX.MinorGrid.Enabled = true;

            chartBode.ChartAreas[0].AxisX.IsInterlaced = false;


            xPosGain = new System.Windows.Forms.DataVisualization.Charting.Cursor();
            xPosPhase = new System.Windows.Forms.DataVisualization.Charting.Cursor();

            chartBode.ChartAreas[1].AxisX.IsLogarithmic = true;
            chartBode.ChartAreas[1].AxisX.IntervalType = DateTimeIntervalType.Number;
            chartBode.ChartAreas[1].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;

            chartBode.ChartAreas[1].AxisX.IsMarginVisible = true;
            chartBode.ChartAreas[1].AxisX.IsStartedFromZero = false;

            chartBode.ChartAreas[1].AxisX.MajorTickMark.Enabled = true;
            chartBode.ChartAreas[1].AxisX.MinorTickMark.Enabled = true;

            chartBode.ChartAreas[1].AxisX.MajorGrid.Enabled = true;
            chartBode.ChartAreas[1].AxisX.MinorGrid.Enabled = true;


            
            chartBode.ChartAreas[0].AxisX.Minimum = 100; // Convert.ToInt32(txtFreqStart.Text);
            chartBode.ChartAreas[0].AxisX.Maximum = 1000000; // Convert.ToInt32(txtFreqStop.Text);
            chartBode.ChartAreas[1].AxisX.Minimum = chartBode.ChartAreas[0].AxisX.Minimum;
            chartBode.ChartAreas[1].AxisX.Maximum = chartBode.ChartAreas[0].AxisX.Maximum;

            return;
        }

        private void frmChart_Load(object sender, EventArgs e)
        {
            
        }

        private void UpdateCharts(bool UpdatePlant = false)
        {
            int i = 0;

            if (UpdatePlant)
            {
                chartBode.Series[0].Points.DataBindXY(model.f, model.Plant.Magnitude);
                chartBode.Series[1].Points.DataBindXY(model.f, model.Plant.Phase);
            }

            chartBode.Series[2].Points.DataBindXY(model.f, model.Compensation.Magnitude);
            chartBode.Series[3].Points.DataBindXY(model.f, model.Compensation.Phase);

            chartBode.Series[4].Points.DataBindXY(model.f, model.OpenLoop.Magnitude);
            chartBode.Series[5].Points.DataBindXY(model.f, model.OpenLoop.Phase);

            /*
            if(UpdatePlant)
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
            }

            chart1.Series[2].Points.Clear();
            chart1.Series[3].Points.Clear();

            chart1.Series[4].Points.Clear();
            chart1.Series[5].Points.Clear();

            if(UpdatePlant)
            {
                model.freq.Zip(model.Plant.Magnitude, (freq, TFs_output) => chart1.Series[0].Points.AddXY(freq, TFs_output)).ToArray();
                model.freq.Zip(model.Plant.Phase, (freq, TFs_output) => chart1.Series[1].Points.AddXY(freq, TFs_output)).ToArray();
            }

            model.freq.Zip(model.Compensation.Magnitude, (freq, TFs_output) => chart1.Series[2].Points.AddXY(freq, TFs_output)).ToArray();
            model.freq.Zip(model.Compensation.Phase, (freq, TFs_output) => chart1.Series[3].Points.AddXY(freq, TFs_output)).ToArray();

            model.freq.Zip(model.OpenLoop.Magnitude, (freq, TFs_output) => chart1.Series[4].Points.AddXY(freq, TFs_output)).ToArray();
            model.freq.Zip(model.OpenLoop.Phase, (freq, TFs_output) => chart1.Series[5].Points.AddXY(freq, TFs_output)).ToArray();
            */


            double GainMin = 1000;
            int GainMinIdx = -1;

            for (i = 0; i < model.f.Length; i++)
            {
                if (Math.Abs(model.OpenLoop.Magnitude[i]) < GainMin)
                {
                    GainMinIdx = i;
                    GainMin = Math.Abs(model.OpenLoop.Magnitude[i]);
                }
            }

//            txtCrossover.Text = Convert.ToString(model.freq[GainMinIdx]);
//            txtPhaseMargin.Text = Convert.ToString(model.OpenLoop.Phase[GainMinIdx] + 180);

            chartBode.Annotations.FindByName("AnnoCrossoverMag").AnchorDataPoint = chartBode.Series[4].Points[GainMinIdx];
            chartBode.Annotations.FindByName("AnnoCrossoverPhase").AnchorDataPoint = chartBode.Series[5].Points[GainMinIdx];
            chartBode.Annotations.FindByName("AnnoPhaseMargin").AnchorDataPoint = chartBode.Series[5].Points[GainMinIdx];

            /*

            for (i=0; i < model.freq.Length; i++)
            {
                if (UpdatePlant)
                {
                    chart1.Series[0].Points.AddXY(model.freq[i], model.Plant.Magnitude[i]);
                    chart1.Series[1].Points.AddXY(model.freq[i], model.Plant.Phase[i]);
                }

                chart1.Series[2].Points.AddXY(model.freq[i], model.Compensation.Magnitude[i]);
                chart1.Series[3].Points.AddXY(model.freq[i], model.Compensation.Phase[i]);

                chart1.Series[4].Points.AddXY(model.freq[i], model.OpenLoop.Magnitude[i]);
                chart1.Series[5].Points.AddXY(model.freq[i], model.OpenLoop.Phase[i]);
            }
            */

            return;
        }

        public abstract class TransferFunction
        {


            public TransferFunction(Complex[] _s)
            {
                s = _s;
                y = new Complex[s.Length];

                Magnitude = new double[s.Length];
                Phase = new double[s.Length];
            }

            public void UpdateMagPhase()
            {
                int i = 0;

                double prevPhase = 0;
                double PhaseOffset = 0;

                for (i = 0; i < s.Length; i++)
                {
                    double phase;
                    //TFs_output[i] = TF(s[i]);
                    Magnitude[i] = 20.0 * Math.Log10(y[i].Magnitude);

                    phase = y[i].Phase * 180 / Math.PI;
                    if (phase - prevPhase > 180) PhaseOffset -= 360;
                    if (phase - prevPhase < -180) PhaseOffset += 360;

                    Phase[i] = phase + PhaseOffset;
                    prevPhase = phase;
                }
            }

            public abstract void Update();

            public Complex[] s;

            public Complex[] y;

            public double[] Magnitude;

            public double[] Phase;
        }

        public class TFStatic : TransferFunction
        {
            public TFStatic(Complex[] _s, Complex[] _y)
                : base(_s)
            {
                y = _y;

                UpdateMagPhase();
            }

            public override void Update()
            {
                throw new NotImplementedException();
            }
        }

        public class TFH : TransferFunction
        {
            public delegate Complex H(Complex s);

            public static Complex Identity(Complex s)
            {
                return Complex.One;
            }

            public TFH(Complex[] _s, H _TF)
                : base(_s)
            {
                TF = _TF;
            }

            public TFH(Complex[] _s)
                : base(_s)
            {
                TF = Identity;
            }

            public H TF;

            public override void Update()
            {
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    y[i] = TF(s[i]);
                }

                UpdateMagPhase();
            }

        }

        public class TFPolesZeroes : TFH
        {
            public Complex[] poles;
            public Complex[] zeroes;
            public double gain;

            public Complex TFPZ(Complex s)
            {
                int i = 0;
                Complex numerator = gain, denominator = s;

                for (i = 0; i < zeroes.Length; i++)
                {
                    numerator *= s / zeroes[i] + Complex.One;
                }

                for (i = 0; i < poles.Length; i++)
                {
                    denominator *= s / poles[i] + Complex.One;
                }

                return numerator / denominator;
            }

            public TFPolesZeroes(Complex[] _s)
                : base(_s)
            {
                TF = TFPZ;
                gain = 1;

                zeroes = new Complex[0];
                poles = new Complex[0];
            }
        }

        public class TFBuckVM : TFH
        {
            public Impedance Ind, Outcaps;
            double gain;

            public Complex TFBuck(Complex s)
            {
                return gain * Outcaps.Z(s) / (Ind.Z(s) + Outcaps.Z(s));
            }

            public TFBuckVM(Complex[] _s, Impedance _Ind, Impedance _Outcaps, double _gain)
                : base(_s)
            {
                TF = TFBuck;
                gain = _gain;
                Ind = _Ind;
                Outcaps = _Outcaps;
            }
        }

        public class TFCompound : TransferFunction
        {
            public TransferFunction a, b;

            public TFCompound(Complex[] _s, TransferFunction _a, TransferFunction _b)
                : base(_s)
            {
                a = _a;
                b = _b;
            }

            public override void Update()
            {
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    y[i] = a.y[i] * b.y[i];
                }

                UpdateMagPhase();
            }
        }

        private class Model
        {

            public static Complex Identity(Complex s)
            {
                return s;
            }

            public TransferFunction Plant, OpenLoop;
            public TFPolesZeroes Compensation;

            public double[] f { get; private set; }


            public Complex[] s;

            public void SetFreqParams(double start_f, double stop_f, int datapoints)
            {
                double r = Math.Pow(stop_f / start_f, 1.0 / datapoints);

                f = Enumerable.Range(0, datapoints + 1).Select(n => start_f * Math.Pow(r, n)).ToArray();
                s = f.Select(_f => new Complex(0, 2 * Math.PI * _f)).ToArray();
            }

            public Model(double start_f, double end_f, int datapoints)
            {
                SetFreqParams(start_f, end_f, datapoints);

                Plant = new TFH(s, TFH.Identity);
                Compensation = new TFPolesZeroes(s);
                //OpenLoop = new TransferFunction(s, _s => Compensation.TF(_s) * Plant.TF(_s) );
                OpenLoop = new TFCompound(s, Compensation, Plant);
            }
        }

        Model model;

        private class PoleZero : INotifyPropertyChanged
        {
            private double _f;
            public double f
            {
                set
                {
                    _f = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("freq"));
                }
                get { return _f; }
            }

            public Complex s
            {
                get
                {
                    return 2 * Math.PI * _f * Complex.One;
                }
            }

            public enum _type { pole, zero };
            public _type type;

            public event PropertyChangedEventHandler PropertyChanged;

            //public List<Annotation> annotations;

            public Annotation PhaseAnnotation;
            public Annotation MagAnnotation;

            public PoleZero(double __f, _type T)
            {
                f = __f;
                type = T;
            }

            public static PoleZero Pole(double __f = 1000.0)
            {
                return new PoleZero(__f, _type.pole);
            }

            public static PoleZero Zero(double __f = 1000.0)
            {
                return new PoleZero(__f, _type.zero);
            }

            override public string ToString()
            {
                if (type == _type.pole)
                {
                    return string.Format("Pole: {0}", _f);
                }
                else
                {
                    return string.Format("Zero: {0}", _f);
                }
            }
        }

        BindingList<PoleZero> ListPZs;

        private void UpdateCompensation(Model m)
        {
            //PoleZero[] pzs = lstPolesZeroes.Items.OfType<PoleZero>().ToArray();

            m.Compensation.gain = 56;//Math.Pow(10, Convert.ToDouble(txtCompGain.Text) / 20);

            m.Compensation.poles = ListPZs.Where(pz => pz.type == PoleZero._type.pole).Select(_p => _p.s).ToArray();
            m.Compensation.zeroes = ListPZs.Where(pz => pz.type == PoleZero._type.zero).Select(_z => _z.s).ToArray();

            m.Compensation.Update();
            m.OpenLoop.Update();
        }

        /*
        private void UpdateChart(double[] freq, Complex[] TFs_output, int SeriesIndex)
        {
            int i;
            double[] mag, phase;
            double unwrap_carry = 0.0;

            chart1.Series[SeriesIndex].Points.Clear();
            chart1.Series[SeriesIndex+1].Points.Clear();
            
            mag = TFs_output.Select(m => 20.0 * Math.Log10(m.Magnitude)).ToArray();
            phase = TFs_output.Select(p => p.Phase * 180 / Math.PI).ToArray();

            for (i = 0; i < freq.Length; i++)
            { 
                // unwrapping
                if( i > 0 )
                {
                    if ( phase[i-1] - phase[i] > 180.0 )
                    {
                        unwrap_carry += 360;
                    }
                    else if(phase[i-1] - phase[i] < -180.0 )
                    {
                        unwrap_carry -= 360;
                    }
                }


                chart1.Series[SeriesIndex].Points.AddXY(freq[i], mag[i]);
                chart1.Series[SeriesIndex + 1].Points.AddXY(freq[i], phase[i] + unwrap_carry);
            }
        }
        */


/*
        private void btnUpdatePlant_Click(object sender, EventArgs e)
        {
            model.SetFreqParams(Convert.ToDouble(txtFreqStart.Text), 100, 100000);
                //Convert.ToDouble(txtFreqStop.Text),
                //Convert.ToInt32(txtFreqPoints.Text));

            //model.Compensation.s = model.s;
            //model.OpenLoop.s = model.s;

            List<ZCap> LOutcaps = new List<ZCap>();
            ParCkt Outcaps;

            //List<Tuple<Double, Double, int>> OutCaps = new List<Tuple<double, double, int>>();
            //Tuple<Double, Double> Inductor = new Tuple<double, double>(Convert.ToDouble(txtIndL.Text) * 1e-6, Convert.ToDouble(txtIndDCR.Text) * 1e-3);

            foreach (DataGridViewRow row in dataCaps.Rows)
            {
                double C, ESR;
                int n;

                C = Convert.ToDouble(row.Cells[0].Value) * 1e-6;
                ESR = Convert.ToDouble(row.Cells[1].Value) * 1e-3;
                n = Convert.ToInt32(row.Cells[2].Value);

                if (double.IsNaN(C) || double.IsNaN(ESR) || n == 0) continue;
                LOutcaps.Add(new ZCap(C, ESR, n));
            }

            Outcaps = new ParCkt(LOutcaps.ToArray());

            ZInd Ind = new ZInd(Convert.ToDouble(txtIndL.Text) * 1e-6, Convert.ToDouble(txtIndDCR.Text) * 1e-3);

            model.Plant = new TFBuckVM(model.s, Ind, Outcaps, Convert.ToDouble(txtModulatorGain.Text));
            model.Plant.Update();
            model.Compensation.Update();
            model.OpenLoop = new TFCompound(model.s, model.Compensation, model.Plant);
            model.OpenLoop.Update();
            UpdateCharts(true);

            chartBode.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(txtFreqStart.Text);
            chartBode.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(txtFreqStop.Text);

            chartBode.ChartAreas[1].AxisX.Minimum = Convert.ToDouble(txtFreqStart.Text);
            chartBode.ChartAreas[1].AxisX.Maximum = Convert.ToDouble(txtFreqStop.Text);

            if ((chartBode.ChartAreas[1].AxisX.Maximum / chartBode.ChartAreas[1].AxisX.Minimum) > 100.0)
            {
                chartBode.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                chartBode.ChartAreas[1].AxisX.MinorGrid.Enabled = false;
            }
            else
            {
                chartBode.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
                chartBode.ChartAreas[1].AxisX.MinorGrid.Enabled = true;
            }


            chartBode.Refresh();

            //chart1.Series[0].Points.DataBindXY(model.freq, model.Plant.Magnitude);
            //chart1.Series[1].Points.DataBindXY(model.freq, model.Plant.Phase);

            //chart1.ChartAreas[0].RecalculateAxesScale();
            //chart1.ChartAreas[1].RecalculateAxesScale();

            //chart1.DataBind();

            /*
            double compGain = Math.Pow(Convert.ToDouble(txtCompGain.Text) / 20.0, 10);

            List<Double> poles = new List<double>(), zeroes = new List<double>();

            foreach( PoleZero pz in lstPolesZeroes.Items.OfType<PoleZero>() )
            {
                if(pz.type == PoleZero._pztype.pole)
                {
                    poles.Add(2 * Math.PI * pz.freq);
                }
                else if(pz.type == PoleZero._pztype.zero)
                {
                    zeroes.Add(2 * Math.PI * pz.freq);
                }
            }


            f_Compensation = doCompensation(compGain, ListModule.OfSeq(poles), ListModule.OfSeq(zeroes), ListModule.OfSeq(s)).ToArray();

            f_System = f_Plant.Zip(f_Compensation, (_p, _c) => _c * _p).ToArray();

            UpdateCharts(freq, f_Plant, f_Compensation, f_System);
            */
    //    }
    
        private VerticalLineAnnotation GetVline(int chartArea, PoleZero pz)
        {
            VerticalLineAnnotation v = new VerticalLineAnnotation();

            v.AxisX = chartBode.ChartAreas[chartArea].AxisX;
            v.AxisY = chartBode.ChartAreas[chartArea].AxisY;

            v.IsSizeAlwaysRelative = true;

            v.ClipToChartArea = chartBode.ChartAreas[chartArea].Name;

            v.AllowSelecting = false; // true;
            v.AllowMoving = true;

            v.LineColor = (pz.type == PoleZero._type.pole) ? Color.Blue : Color.Green;

            v.LineWidth = 1;

            v.IsInfinitive = true;

            v.X = pz.f;
            v.Y = 0;

            v.Tag = pz;

            return v;
        }

        private void btnAddPole_Click(object sender, EventArgs e)
        {
            PoleZero p = PoleZero.Pole();
            ListPZs.Add(p);
            //lstPolesZeroes.DataSource = ListPZs;

            VerticalLineAnnotation v_mag = GetVline(0, p);
            VerticalLineAnnotation v_phase = GetVline(1, p);

            chartBode.Annotations.Add(v_mag);
            chartBode.Annotations.Add(v_phase);
            p.MagAnnotation = v_mag;
            p.PhaseAnnotation = v_phase;
            //p.annotations.Add(v);

            UpdateCompensation(model);

            UpdateCharts();

        }

        private void btnAddZero_Click(object sender, EventArgs e)
        {
            PoleZero z = PoleZero.Zero();
            ListPZs.Add(z);
            //lstPolesZeroes.DataSource = ListPZs;

            VerticalLineAnnotation v_mag = GetVline(0, z);
            VerticalLineAnnotation v_phase = GetVline(1, z);

            chartBode.Annotations.Add(v_mag);
            chartBode.Annotations.Add(v_phase);
            z.MagAnnotation = v_mag;
            z.PhaseAnnotation = v_phase;

            UpdateCompensation(model);
            UpdateCharts();
        }

        private void chartBode_AnnotationSelectionChanged(object sender, EventArgs e)
        {
            /*   Annotation a = (Annotation)sender;

               //PoleZero pz = (PoleZero)a.Tag;

               int index = lstPolesZeroes.Items.IndexOf(pz);

               lstPolesZeroes.SelectedIndex = index;*/

        }

        private void chartBode_AnnotationPositionChanging(object sender, AnnotationPositionChangingEventArgs e)
        {
            Annotation a = (Annotation)sender;

            PoleZero pz = (PoleZero)a.Tag;

            pz.f = a.X;

            if (!pz.MagAnnotation.Equals(sender)) pz.MagAnnotation.X = a.X;
            if (!pz.PhaseAnnotation.Equals(sender)) pz.PhaseAnnotation.X = a.X;

            UpdateCompensation(model);
            UpdateCharts();
            chartBode.Update();

            /*            chart1.Series[2].Points.DataBindXY(model.freq, model.Compensation.Magnitude);
                        chart1.Series[3].Points.DataBindXY(model.freq, model.Compensation.Phase);

                        chart1.Series[4].Points.DataBindXY(model.freq, model.OpenLoop.Magnitude);
                        chart1.Series[5].Points.DataBindXY(model.freq, model.OpenLoop.Phase); */

            //chart1.ChartAreas[0].RecalculateAxesScale();
            //chart1.ChartAreas[1].RecalculateAxesScale();
        }

        private void chartBode_AnnotationPositionChanged(object sender, EventArgs e)
        {
            UpdateCompensation(model);
            UpdateCharts();
        }

        private void chartBode_MouseMove(object sender, MouseEventArgs e)
        {
//            lblFrequency.Text = chartBode.ChartAreas[0].CursorX.Position.ToString();
//            lblGain.Text = chartBode.ChartAreas[0].CursorY.Position.ToString();

        }
    
    }
}
