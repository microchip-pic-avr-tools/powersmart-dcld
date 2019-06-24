namespace dcld
{
    partial class frmChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.VerticalLineAnnotation verticalLineAnnotation1 = new System.Windows.Forms.DataVisualization.Charting.VerticalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.VerticalLineAnnotation verticalLineAnnotation2 = new System.Windows.Forms.DataVisualization.Charting.VerticalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation horizontalLineAnnotation1 = new System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.VerticalLineAnnotation verticalLineAnnotation3 = new System.Windows.Forms.DataVisualization.Charting.VerticalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChart));
            this.chartBode = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartBode)).BeginInit();
            this.SuspendLayout();
            // 
            // chartBode
            // 
            verticalLineAnnotation1.AxisXName = "chrtMag\\rX";
            verticalLineAnnotation1.ClipToChartArea = "chrtMag";
            verticalLineAnnotation1.IsInfinitive = true;
            verticalLineAnnotation1.LineColor = System.Drawing.Color.Red;
            verticalLineAnnotation1.Name = "AnnoCrossoverMag";
            verticalLineAnnotation1.YAxisName = "chrtMag\\rY";
            verticalLineAnnotation2.AxisXName = "chrtPhase\\rX";
            verticalLineAnnotation2.ClipToChartArea = "chrtPhase";
            verticalLineAnnotation2.IsInfinitive = true;
            verticalLineAnnotation2.LineColor = System.Drawing.Color.Red;
            verticalLineAnnotation2.Name = "AnnoCrossoverPhase";
            verticalLineAnnotation2.YAxisName = "chrtPhase\\rY";
            horizontalLineAnnotation1.AxisXName = "chrtPhase\\rX";
            horizontalLineAnnotation1.ClipToChartArea = "chrtPhase";
            horizontalLineAnnotation1.IsInfinitive = true;
            horizontalLineAnnotation1.LineColor = System.Drawing.Color.DarkRed;
            horizontalLineAnnotation1.Name = "AnnoPhaseMargin";
            horizontalLineAnnotation1.YAxisName = "chrtPhase\\rY";
            verticalLineAnnotation3.AxisXName = "chrtMag\\rX";
            verticalLineAnnotation3.ClipToChartArea = "chrtMag";
            verticalLineAnnotation3.IsInfinitive = true;
            verticalLineAnnotation3.LineColor = System.Drawing.Color.DarkRed;
            verticalLineAnnotation3.Name = "AnnoGainMargin";
            verticalLineAnnotation3.YAxisName = "chrtMag\\rY";
            this.chartBode.Annotations.Add(verticalLineAnnotation1);
            this.chartBode.Annotations.Add(verticalLineAnnotation2);
            this.chartBode.Annotations.Add(horizontalLineAnnotation1);
            this.chartBode.Annotations.Add(verticalLineAnnotation3);
            this.chartBode.BackColor = System.Drawing.Color.Transparent;
            this.chartBode.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartBode.BorderlineColor = System.Drawing.Color.Transparent;
            this.chartBode.BorderSkin.BackColor = System.Drawing.Color.Transparent;
            this.chartBode.BorderSkin.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartBode.BorderSkin.PageColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisX.InterlacedColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.IsLogarithmic = true;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.MajorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MajorGrid.LineWidth = 2;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.Maximum = 100000D;
            chartArea1.AxisX.Minimum = 100D;
            chartArea1.AxisX.MinorGrid.Enabled = true;
            chartArea1.AxisX.MinorGrid.Interval = double.NaN;
            chartArea1.AxisX.MinorGrid.IntervalOffset = double.NaN;
            chartArea1.AxisX.MinorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.MinorTickMark.Interval = double.NaN;
            chartArea1.AxisX.MinorTickMark.IntervalOffset = double.NaN;
            chartArea1.AxisX.MinorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MinorTickMark.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisX.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.AcrossAxis;
            chartArea1.AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.Title = "Frequency [Hz]";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX2.Crossing = -1.7976931348623157E+308D;
            chartArea1.AxisX2.IsMarginVisible = false;
            chartArea1.AxisX2.IsStartedFromZero = false;
            chartArea1.AxisX2.MajorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX2.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX2.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX2.MajorTickMark.Enabled = false;
            chartArea1.AxisX2.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX2.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX2.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX2.MinorGrid.Interval = double.NaN;
            chartArea1.AxisX2.MinorGrid.IntervalOffset = double.NaN;
            chartArea1.AxisX2.MinorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX2.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX2.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX2.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX2.MinorTickMark.Interval = double.NaN;
            chartArea1.AxisX2.MinorTickMark.IntervalOffset = double.NaN;
            chartArea1.AxisX2.MinorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX2.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX2.MinorTickMark.LineColor = System.Drawing.Color.Gainsboro;
            chartArea1.AxisY.Interval = 10D;
            chartArea1.AxisY.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MajorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Interval = 0D;
            chartArea1.AxisY.MajorTickMark.IntervalOffset = 0D;
            chartArea1.AxisY.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.Maximum = 60D;
            chartArea1.AxisY.Minimum = -60D;
            chartArea1.AxisY.MinorGrid.Interval = double.NaN;
            chartArea1.AxisY.MinorGrid.IntervalOffset = double.NaN;
            chartArea1.AxisY.MinorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.MinorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.MinorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.Title = "Magnitude [dB]";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY2.MajorGrid.Interval = 0D;
            chartArea1.AxisY2.MajorGrid.IntervalOffset = 0D;
            chartArea1.AxisY2.MajorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY2.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY2.MajorTickMark.Enabled = false;
            chartArea1.AxisY2.MajorTickMark.Interval = 0D;
            chartArea1.AxisY2.MajorTickMark.IntervalOffset = 0D;
            chartArea1.AxisY2.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY2.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY2.MajorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY2.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY2.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY2.MinorTickMark.LineColor = System.Drawing.Color.Silver;
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.IsSameFontSizeForAllAxes = true;
            chartArea1.Name = "chrtMag";
            chartArea2.AxisX.IsLogarithmic = true;
            chartArea2.AxisX.IsStartedFromZero = false;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisX.MajorGrid.LineWidth = 2;
            chartArea2.AxisX.Maximum = 300000D;
            chartArea2.AxisX.Minimum = 100D;
            chartArea2.AxisX.MinorGrid.Enabled = true;
            chartArea2.AxisX.MinorGrid.Interval = double.NaN;
            chartArea2.AxisX.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisX.Title = "Frequency [Hz]";
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisX2.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisX2.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY.Interval = 30D;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY.Maximum = 180D;
            chartArea2.AxisY.Minimum = -180D;
            chartArea2.AxisY.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY.Title = "Phase[°]";
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.AxisY2.MinorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea2.IsSameFontSizeForAllAxes = true;
            chartArea2.Name = "chrtPhase";
            this.chartBode.ChartAreas.Add(chartArea1);
            this.chartBode.ChartAreas.Add(chartArea2);
            this.chartBode.Cursor = System.Windows.Forms.Cursors.Cross;
            this.chartBode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartBode.Location = new System.Drawing.Point(0, 0);
            this.chartBode.Name = "chartBode";
            series1.ChartArea = "chrtMag";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.Blue;
            series1.Name = "srsMagPlant";
            series2.ChartArea = "chrtPhase";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.Blue;
            series2.Name = "srsPhasePlant";
            series3.ChartArea = "chrtMag";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Color = System.Drawing.Color.Green;
            series3.Name = "srsMagComp";
            series4.ChartArea = "chrtPhase";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Color = System.Drawing.Color.Green;
            series4.Name = "srsPhaseComp";
            series5.ChartArea = "chrtMag";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series5.Color = System.Drawing.Color.Red;
            series5.Name = "srsMagOpenLoop";
            series6.ChartArea = "chrtPhase";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series6.Color = System.Drawing.Color.Red;
            series6.Name = "srsPhaseOpenLoog";
            this.chartBode.Series.Add(series1);
            this.chartBode.Series.Add(series2);
            this.chartBode.Series.Add(series3);
            this.chartBode.Series.Add(series4);
            this.chartBode.Series.Add(series5);
            this.chartBode.Series.Add(series6);
            this.chartBode.Size = new System.Drawing.Size(1094, 641);
            this.chartBode.TabIndex = 1;
            this.chartBode.Text = "chartBode";
            title1.DockedToChartArea = "chrtMag";
            title1.Name = "Magnitude";
            title2.DockedToChartArea = "chrtMag";
            title2.Name = "Phase";
            this.chartBode.Titles.Add(title1);
            this.chartBode.Titles.Add(title2);
            // 
            // frmChart
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 641);
            this.Controls.Add(this.chartBode);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmChart";
            this.Text = "frmChart";
            this.Load += new System.EventHandler(this.frmChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartBode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartBode;

    }
}