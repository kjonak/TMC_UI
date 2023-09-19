using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMC_API;

namespace TMC_API
{
    /*
     * This class contains data sent by the TMC.
     * It also allows to send control signals to the boat. 
     * 
     */
    public partial class TMC_Model : ObservableObject
    {
        [ObservableProperty]
        private bool _IsConnected = false;

        //private TMC_Connection _connection;
        //private TMC_Parser _parser;

        [ObservableProperty]
        private Quaternion _AttitudeQ;

        [ObservableProperty]
        private Quaternion _DesiredAttitudeQ;

        [ObservableProperty]
        private Vector3 _AttitudeE;

        [ObservableProperty]
        private Vector3 _DesiredAttitudeE;

        [ObservableProperty]
        private Vector3 _AngularVelocity;

        [ObservableProperty]
        private Vector3 _DesiredAngularVelocity;

        [ObservableProperty]
        private float _VerticalVelocity;

        [ObservableProperty]
        private float _DesiredVerticalVelocity;

        [ObservableProperty]
        private float _BatteryLevel;

        [ObservableProperty]
        private float _OnboardTemperature;

        [ObservableProperty]
        private float _CPUTemperature;

        [ObservableProperty]
        private float _OnboardHhummidity;

        [ObservableProperty]
        private Vector<float> _ActuatorsOutput = new(8);

        [ObservableProperty]
        private Vector<float> _StickInputs = new(16);

        [ObservableProperty]
        private Vector<float> _CTRLMatrix = new(48);

        [ObservableProperty]
        private PID _PIDRoll = new();

        [ObservableProperty]
        private PID _PIDPitch = new();

        [ObservableProperty]
        private PID _PIDYaw = new();

        [ObservableProperty]
        private PID _PIDVertical = new();

        [ObservableProperty]
        private Limit _Limit_AngularVelocityRoll = new();

        [ObservableProperty]
        private Limit _Limit_AngularVelocityPitch = new();

        [ObservableProperty]
        private Limit _Limit_AngularVelocityYaw = new();

        [ObservableProperty]
        private Limit _Limit_VerticalVelocity = new();
        [ObservableProperty]
        private Limit _Limit_LongitudinalVelocity = new();
        [ObservableProperty]
        private Limit _Limit_LateralVelocity = new();

        [ObservableProperty]
        private float _Limit_AngleRoll;
        [ObservableProperty]
        private float _Limit_AnglePitch;





        public void SendSticks(float[] sticks)
        {

        }

        public void SendPids() 
        { 
        }

        public void SendSUMatrix()
        {

        }

        public TMC_Model()
        {
            Thread x = new Thread(task);
            //x.Start();
            PIDPitch.KI = 100;
        }
        private void task()
        {
            int i = 0;
            while(true)
            {
                float j = i / (float)100;
                AttitudeE = new Vector3(j,j,j);
                DesiredAttitudeE = new Vector3(j,j,j);
                AngularVelocity = new Vector3(j, j, j);
                DesiredAngularVelocity = new Vector3(j, j, j);
                VerticalVelocity = j;
                DesiredVerticalVelocity = j;
                BatteryLevel = i;


                i++;
                if (i > 50)
                    i = -50;
                Thread.Sleep(200);
            }
        }
        //public TMC_Model(TMC_Connection connection)
        //{
        //    _connection = connection;
        //    _parser = new TMC_Parser(this);
        //    _connection.NewDataCallback = _parser.HandleNewBytes;
        //}


    }
}