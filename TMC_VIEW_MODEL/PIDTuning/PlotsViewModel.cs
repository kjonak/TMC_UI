using CommunityToolkit.Mvvm.ComponentModel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TMC_API;

namespace TMC_VIEW_MODEL.PIDTuning
{
    public partial class PlotsViewModel : BaseTMCViewModel
    {

        private DateTime startTime = DateTime.MinValue;
        private DateTime lastUpdate = DateTime.MinValue;

        [ObservableProperty]
        PlotModel _Roll;
        [ObservableProperty]
        PlotModel _Pitch;
        [ObservableProperty]
        PlotModel _Yaw;
        [ObservableProperty]
        PlotModel _VerticalVelocity;

        [ObservableProperty]
        PlotModel _AngularRoll;
        [ObservableProperty]
        PlotModel _AngularPitch;
        [ObservableProperty]
        PlotModel _AngularYaw;


        [ObservableProperty]
        bool _RollVisible;
        [ObservableProperty]
        bool _PitchVisible;
        [ObservableProperty]
        bool _YawVisible;
        [ObservableProperty]
        bool _VerticalSpeedVisible;
       



        PlotModel GeneratePlot(string S1_Title, string S2_Title, double y_min, double y_max, OxyThickness margins)
        {
            var mdl = new PlotModel();
            mdl.PlotMargins = margins; 
            //mdl.PlotAreaBackground = OxyColor.FromArgb(0, 100, 0, 0);
            //mdl.Background = OxyColor.FromArgb(0,100,0,0);
            SolidColorBrush fcolor = (SolidColorBrush)Application.Current.FindResource("ForegroundBrush");
            OxyColor foregroundColor = OxyColor.FromArgb(fcolor.Color.A, fcolor.Color.R, fcolor.Color.G, fcolor.Color.B);
            OxyColor GridLineColor = OxyColor.FromArgb((byte)(fcolor.Color.A * 0.8), fcolor.Color.R, fcolor.Color.G, fcolor.Color.B);

            mdl.TextColor = foregroundColor;
            mdl.PlotAreaBorderColor = OxyColors.Transparent;

            List<DataPoint> data1 = new List<DataPoint>();
            List<DataPoint> data2 = new List<DataPoint>();
            LineSeries s1 = new LineSeries();
            LineSeries s2 = new LineSeries();
            s1.Title = S1_Title;
            s2.Title = S2_Title;
            s1.ItemsSource = data1;
            s2.ItemsSource = data2;
            s1.LineStyle = LineStyle.Solid;
            s2.LineStyle = LineStyle.Solid;
            s1.CanTrackerInterpolatePoints = false; s2.CanTrackerInterpolatePoints = false;
            mdl.Series.Add(s1);
            mdl.Series.Add(s2);
            
            mdl.Legends.Add(new Legend() { LegendPosition = LegendPosition.TopRight, LegendPlacement = LegendPlacement.Outside, LegendBorderThickness = 0, LegendOrientation = LegendOrientation.Horizontal});

            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, AxislineStyle = LineStyle.Solid };
            var yAxis = new LinearAxis { Position = AxisPosition.Left, AxislineStyle = LineStyle.Solid };
            xAxis.IsZoomEnabled = false;
            xAxis.IsPanEnabled = false;
            yAxis.IsZoomEnabled = false;
            yAxis.IsPanEnabled = false;

            xAxis.AxislineColor = OxyColors.Transparent;
            yAxis.AxislineColor = OxyColors.Transparent;

            yAxis.MajorGridlineColor = GridLineColor;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MajorGridlineThickness = 2;
            yAxis.TicklineColor = OxyColors.Transparent;

            yAxis.MinorGridlineColor = GridLineColor;
            yAxis.MinorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineThickness = 1.2;

            yAxis.MajorStep = (y_max - y_min) / 4;
            yAxis.MinorStep = (y_max - y_min) / 4 / 2;

            yAxis.Minimum = y_min;
            yAxis.Maximum = y_max;
            
            xAxis.TicklineColor = foregroundColor;

            mdl.Axes.Add(xAxis);
            mdl.Axes.Add(yAxis);
            return mdl;
        }

        public override void OnShow()
        {
            startTime = DateTime.Now;
            lastUpdate = DateTime.Now;
            List<DataPoint> s1 = ((List<DataPoint>)((LineSeries)Roll.Series[0]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)Roll.Series[1]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)AngularRoll.Series[0]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)AngularRoll.Series[1]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)Pitch.Series[0]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)Pitch.Series[1]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)AngularPitch.Series[0]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)AngularPitch.Series[1]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)Yaw.Series[0]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)Yaw.Series[1]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)AngularYaw.Series[0]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)AngularYaw.Series[1]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)VerticalVelocity.Series[0]).ItemsSource);
            s1.Clear();
            s1 = ((List<DataPoint>)((LineSeries)VerticalVelocity.Series[1]).ItemsSource);
            s1.Clear();
            samples_hz = 0;
            calc_samples = true;
            first_sample = true;
            samples_cnt = 0;
            TMC_Model.Variables[(int)TMC_Variables.NAMES.VAR_SYS_ATTITUDE_E_y].Changed += TMC_Model_PropertyChanged;
        }

        private int samples_cnt = 0;
        private int samples_hz = 0;
        private bool calc_samples = false;
        private bool first_sample = false;
        public override void OnHide()
        {
            TMC_Model.Variables[(int)TMC_Variables.NAMES.VAR_SYS_ATTITUDE_E_y].Changed -= TMC_Model_PropertyChanged;
        }
        public PlotsViewModel(TMC_Model TMC_Model) : base(TMC_Model)
        {
            var left = new OxyThickness(30, 0,0,30);
            var right = new OxyThickness(30,0,0,30);
            Roll = GeneratePlot("Roll", "Desired roll", -45, 45, left);
            Pitch = GeneratePlot("Pitch", "Desired pitch", -45, 45, left);
            Yaw = GeneratePlot("Yaw", "Desired yaw", -45, 45, left);
            VerticalVelocity = GeneratePlot("Vertical speed", "Desired VS", - 1, 1, left);

            AngularRoll = GeneratePlot("p", "Desired p", -45, 45, right);
            AngularPitch = GeneratePlot("q","Desired q", -45, 45, right);
            AngularYaw = GeneratePlot("r", "Desired r", -45, 45, right);
            


        }

        void UpdateCollection (ref List<DataPoint> collection, double value, double time)
        {
            
            
            collection.Add(new DataPoint(time, value));
            if(collection.Count > 150) 
            {
                collection.RemoveAt(0);
            }
            
        }

     
        private void TMC_Model_PropertyChanged()
        {

            if (calc_samples)
            {
                if(first_sample)
                {
                    lastUpdate = DateTime.Now;
                    first_sample = false;
                }

                samples_hz++;
                double _dt = (DateTime.Now - lastUpdate).TotalMilliseconds;
                if (_dt >= 2000)
                {
                    samples_hz = (int)((double)samples_hz / _dt * 1000) / 30 ;
                    calc_samples = false;
                    samples_cnt = 0;
                }
                return;
            }
            samples_cnt++;
            if (samples_cnt < samples_hz)
                return;
            samples_cnt = 0;


            //if (dt < 10 )
            //{
            //    return;
            //}

            //Console.WriteLine(dt + "\n");
            lastUpdate = DateTime.Now;
            bool update_plots = true;
            double time = (DateTime.Now - startTime).TotalSeconds;

            if (RollVisible)
            {
                List<DataPoint> s1 = ((List<DataPoint>)((LineSeries)Roll.Series[0]).ItemsSource);
                List<DataPoint> s2 = ((List<DataPoint>)((LineSeries)Roll.Series[1]).ItemsSource);
                List<DataPoint> s3 = ((List<DataPoint>)((LineSeries)AngularRoll.Series[0]).ItemsSource);
                List<DataPoint> s4 = ((List<DataPoint>)((LineSeries)AngularRoll.Series[1]).ItemsSource);
                if (s1 != null && s2 != null && s3 != null && s4 != null)
                {
                    UpdateCollection(ref s1, Rad2Deg(TMC_Model.AttitudeE.X), time);
                    UpdateCollection(ref s2, Rad2Deg(TMC_Model.DesiredAttitudeE.X), time);
                    UpdateCollection(ref s3, Rad2Deg(TMC_Model.AngularVelocity.X), time);
                    UpdateCollection(ref s4, Rad2Deg(TMC_Model.DesiredAngularVelocity.X), time);
                    Roll.InvalidatePlot(update_plots);
                    AngularRoll.InvalidatePlot(update_plots);
                }
            }

            if(PitchVisible)
            {
                List<DataPoint> s1 = ((List<DataPoint>)((LineSeries)Pitch.Series[0]).ItemsSource);
                List<DataPoint> s2 = ((List<DataPoint>)((LineSeries)Pitch.Series[1]).ItemsSource);
                List<DataPoint> s3 = ((List<DataPoint>)((LineSeries)AngularPitch.Series[0]).ItemsSource);
                List<DataPoint> s4 = ((List<DataPoint>)((LineSeries)AngularPitch.Series[1]).ItemsSource);
                if (s1 != null && s2 != null && s3 != null && s4 != null)
                {
                    UpdateCollection(ref s1, Rad2Deg(TMC_Model.AttitudeE.Y), time);
                    UpdateCollection(ref s2, Rad2Deg(TMC_Model.DesiredAttitudeE.Y), time);
                    UpdateCollection(ref s3, Rad2Deg(TMC_Model.AngularVelocity.Y), time);
                    UpdateCollection(ref s4, Rad2Deg(TMC_Model.DesiredAngularVelocity.Y), time);
                    Pitch.InvalidatePlot(update_plots);
                    AngularPitch.InvalidatePlot(update_plots);
                }
            }
            if(YawVisible)
            {
                List<DataPoint> s1 = ((List<DataPoint>)((LineSeries)Yaw.Series[0]).ItemsSource);
                List<DataPoint> s2 = ((List<DataPoint>)((LineSeries)Yaw.Series[1]).ItemsSource);
                List<DataPoint> s3 = ((List<DataPoint>)((LineSeries)AngularYaw.Series[0]).ItemsSource);
                List<DataPoint> s4 = ((List<DataPoint>)((LineSeries)AngularYaw.Series[1]).ItemsSource);
                if (s1 != null && s2 != null && s3 != null && s4 != null)
                {
                    
                    UpdateCollection(ref s1, Rad2Deg(TMC_Model.AttitudeE.Z), time);
                    UpdateCollection(ref s2, Rad2Deg(TMC_Model.DesiredAttitudeE.Z), time);
                    UpdateCollection(ref s3, Rad2Deg(TMC_Model.AngularVelocity.Z), time);
                    UpdateCollection(ref s4, Rad2Deg(TMC_Model.DesiredAngularVelocity.Z), time);
                    Yaw.InvalidatePlot(update_plots);
                    AngularYaw.InvalidatePlot(update_plots);
                }

            }
            if(VerticalSpeedVisible)
            {
                List<DataPoint> s1 = ((List<DataPoint>)((LineSeries)VerticalVelocity.Series[0]).ItemsSource);
                List<DataPoint> s2 = ((List<DataPoint>)((LineSeries)VerticalVelocity.Series[1]).ItemsSource);
                if (s1 != null && s2 != null)
                {
                    UpdateCollection(ref s1, Rad2Deg(TMC_Model.VerticalVelocity), time);
                    UpdateCollection(ref s2, Rad2Deg(TMC_Model.DesiredVerticalVelocity), time);

                    VerticalVelocity.InvalidatePlot(update_plots);
                
                }

            }

        }


        double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180);
        }
    }
}
