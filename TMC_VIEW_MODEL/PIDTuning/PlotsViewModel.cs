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
       



        PlotModel GeneratePlot(string S1_Title, string S2_Title, double y_min, double y_max)
        {
            var mdl = new PlotModel();
            
            //mdl.PlotAreaBackground = OxyColor.FromArgb(0, 100, 0, 0);
            //mdl.Background = OxyColor.FromArgb(0,100,0,0);
            SolidColorBrush fcolor = (SolidColorBrush)Application.Current.FindResource("ForegroundBrush");
            OxyColor foregroundColor = OxyColor.FromArgb(fcolor.Color.A, fcolor.Color.R, fcolor.Color.G, fcolor.Color.B);
            OxyColor GridLineColor = OxyColor.FromArgb((byte)(fcolor.Color.A * 0.8), fcolor.Color.R, fcolor.Color.G, fcolor.Color.B);

            mdl.TextColor = foregroundColor;
            mdl.PlotAreaBorderColor = OxyColors.Transparent;

            ObservableCollection<DataPoint> data1 = new ObservableCollection<DataPoint>();
            ObservableCollection<DataPoint> data2 = new ObservableCollection<DataPoint>();
            LineSeries s1 = new LineSeries();
            LineSeries s2 = new LineSeries();
            s1.Title = S1_Title;
            s2.Title = S2_Title;
            s1.ItemsSource = data1;
            s2.ItemsSource = data2;
            s1.LineStyle = LineStyle.Solid;
            s2.LineStyle = LineStyle.Solid;
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
            this.TMC_Model.PropertyChanged += TMC_Model_PropertyChanged;
            startTime = DateTime.Now;
            lastUpdate = DateTime.Now;
        }

        public override void OnHide()
        {
            this.TMC_Model.PropertyChanged -= TMC_Model_PropertyChanged;
        }
        public PlotsViewModel(TMC_Model TMC_Model) : base(TMC_Model)
        {
            Roll = GeneratePlot("Roll", "Desired roll", -45, 45);
            Pitch = GeneratePlot("Pitch", "Desired pitch", -45, 45);
            Yaw = GeneratePlot("Yaw", "Desired yaw", -45, 45);
            VerticalVelocity = GeneratePlot("Vertical speed", "Desired VS", - 1, 1);

            AngularRoll = GeneratePlot("p", "Desired p", -45, 45);
            AngularPitch = GeneratePlot("q","Desired q", -45, 45);
            AngularYaw = GeneratePlot("p", "Desired r", -45, 45);
            


        }

        void UpdateCollection (ref ObservableCollection<DataPoint> collection, double value, double time)
        {
            
            
            collection.Add(new DataPoint(time, value));
            if(collection.Count > 500) 
            {
                collection.RemoveAt(0);
            }
            
        }

       
        private void TMC_Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            
            if (e.PropertyName != nameof(TMC_Model.AttitudeE))
                return;

            bool update_plots = false;
            if((DateTime.Now - lastUpdate)>new TimeSpan(0,0,0,0,33))
            {
                lastUpdate = DateTime.Now;
                update_plots = true;
            }
            double time = (DateTime.Now - startTime).TotalSeconds;

            if (RollVisible)
            {
                ObservableCollection<DataPoint> s1 = ((ObservableCollection<DataPoint>)((LineSeries)Roll.Series[0]).ItemsSource);
                ObservableCollection<DataPoint> s2 = ((ObservableCollection<DataPoint>)((LineSeries)Roll.Series[1]).ItemsSource);
                ObservableCollection<DataPoint> s3 = ((ObservableCollection<DataPoint>)((LineSeries)AngularRoll.Series[0]).ItemsSource);
                ObservableCollection<DataPoint> s4 = ((ObservableCollection<DataPoint>)((LineSeries)AngularRoll.Series[1]).ItemsSource);
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
                ObservableCollection<DataPoint> s1 = ((ObservableCollection<DataPoint>)((LineSeries)Pitch.Series[0]).ItemsSource);
                ObservableCollection<DataPoint> s2 = ((ObservableCollection<DataPoint>)((LineSeries)Pitch.Series[1]).ItemsSource);
                ObservableCollection<DataPoint> s3 = ((ObservableCollection<DataPoint>)((LineSeries)AngularPitch.Series[0]).ItemsSource);
                ObservableCollection<DataPoint> s4 = ((ObservableCollection<DataPoint>)((LineSeries)AngularPitch.Series[1]).ItemsSource);
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
                ObservableCollection<DataPoint> s1 = ((ObservableCollection<DataPoint>)((LineSeries)Yaw.Series[0]).ItemsSource);
                ObservableCollection<DataPoint> s2 = ((ObservableCollection<DataPoint>)((LineSeries)Yaw.Series[1]).ItemsSource);
                ObservableCollection<DataPoint> s3 = ((ObservableCollection<DataPoint>)((LineSeries)AngularYaw.Series[0]).ItemsSource);
                ObservableCollection<DataPoint> s4 = ((ObservableCollection<DataPoint>)((LineSeries)AngularYaw.Series[1]).ItemsSource);
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
                ObservableCollection<DataPoint> s1 = ((ObservableCollection<DataPoint>)((LineSeries)VerticalVelocity.Series[0]).ItemsSource);
                ObservableCollection<DataPoint> s2 = ((ObservableCollection<DataPoint>)((LineSeries)VerticalVelocity.Series[1]).ItemsSource);
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
