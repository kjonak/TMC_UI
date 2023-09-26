using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMC_API;
using static TMC_API.TMC_Variables;

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

        [ObservableProperty]
        bool _serviceModeOn = false;

        [ObservableProperty]
        ObservableCollection<TMC_Variables.Variable> _Variables;

        [ObservableProperty]
        ObservableCollection<float> _CTRLMatrix = new ObservableCollection<float>();

        [ObservableProperty]
        PID _PIDRoll = new();

        [ObservableProperty]
        PID _PIDPitch = new();

        [ObservableProperty]
        PID _PIDYaw = new();

        [ObservableProperty]
        PID _PIDVertical = new();

        [ObservableProperty]
        Limit _Limit_AngularVelocityRoll = new();

        [ObservableProperty]
        Limit _Limit_AngularVelocityPitch = new();

        [ObservableProperty]
        Limit _Limit_AngularVelocityYaw = new();

        [ObservableProperty]
        Limit _Limit_VerticalVelocity = new();

        [ObservableProperty]
        Limit _Limit_LongitudinalVelocity = new();

        [ObservableProperty]
        Limit _Limit_LateralVelocity = new();

        [ObservableProperty]
        float _Limit_AngleRoll;
        [ObservableProperty]
        float _Limit_AnglePitch;


        public Vector3 AttitudeE 
        { 
            get 
            { var v = new Vector3();
                v.X = Variables[(int)NAMES.VAR_SYS_ATTITUDE_E_r].Value;
                v.Y = Variables[(int)NAMES.VAR_SYS_ATTITUDE_E_p].Value;
                v.Z = Variables[(int)NAMES.VAR_SYS_ATTITUDE_E_y].Value;
                return v;
            } 
        }

        public Vector3 DesiredAttitudeE
        {
            get
            {
               var v = new Vector3();
                v.X = Variables[(int)NAMES.VAR_SYS_DESIRED_ATTITUDE_E_r].Value;
                v.Y = Variables[(int)NAMES.VAR_SYS_DESIRED_ATTITUDE_E_p].Value;
                v.Z = Variables[(int)NAMES.VAR_SYS_DESIRED_ATTITUDE_E_y].Value;
                return v;
            }
        }

        public Vector3 AngularVelocity
        {
            get
            {
                var v = new Vector3();
                v.X = Variables[(int)NAMES.VAR_SYS_GYRO_r].Value;
                v.Y = Variables[(int)NAMES.VAR_SYS_GYRO_p].Value;
                v.Z = Variables[(int)NAMES.VAR_SYS_GYRO_y].Value;
                return v;
            }
        }

        public Vector3 DesiredAngularVelocity
        {
            get
            {
                var v = new Vector3();
                v.X = Variables[(int)NAMES.VAR_SYS_DESIRED_GYRO_r].Value;
                v.Y = Variables[(int)NAMES.VAR_SYS_DESIRED_GYRO_p].Value;
                v.Z = Variables[(int)NAMES.VAR_SYS_DESIRED_GYRO_y].Value;
                return v;
            }
        }

        public float VerticalVelocity
        {
            get { return -1; }
        }

        public float DesiredVerticalVelocity
        {
            get { return -1; }
        }

        public Action? RequestControlMatrix;
        public Action? RequestPID;
        public Action? RequesTaskFrequency;
        public Action? RequestLimits;


        public Action? SendControlMatrix;
        public Action? SendPID;
        public Action? SendTaskFrequency;
        public Action? SendLimits;
        public Action<float[]>? SendSticks;



        public TMC_Model()
        {
            Variables = new ObservableCollection<TMC_Variables.Variable>();

            for(int i =0; i < (int)TMC_Variables.NAMES.VAR_SYS_COUNT;i++)
            {
                Variables.Add(new TMC_Variables.Variable() { Name = (TMC_Variables.NAMES)i, Value = 0 });
            }
            CTRLMatrix = new ObservableCollection<float>();
            for (int i = 0; i < 48; i++)
                CTRLMatrix.Add(0);// = 0;

            Variables[(int)NAMES.VAR_SYS_DESIRED_ATTITUDE_E_y].PropertyChanged += (sender, e) => { Variables[(int)NAMES.VAR_SYS_DESIRED_ATTITUDE_E_y].Changed?.Invoke();};
            Variables[(int)NAMES.VAR_SYS_ATTITUDE_E_p].PropertyChanged += (sender, e) => { OnPropertyChanged(nameof(AttitudeE)); };
            Variables[(int)NAMES.VAR_SYS_GYRO_p].PropertyChanged += (sender, e) => { OnPropertyChanged(nameof(AngularVelocity)); };
            Variables[(int)NAMES.VAR_SYS_DESIRED_GYRO_p].PropertyChanged += (sender, e) => { OnPropertyChanged(nameof(DesiredAngularVelocity)); };

        }
        private void task()
        {
            int i = 0;
            while(true)
            {

                i++;
                if (i > 50)
                    i = -50;
                Thread.Sleep(200);
            }
        }


    }
}



/*
 * 
 * 
 * 
        [ObservableProperty]
        Quaternion _AttitudeQ;

        [ObservableProperty]
        Quaternion _DesiredAttitudeQ;

        [ObservableProperty]
        Vector3 _AttitudeE;

        [ObservableProperty]
        Vector3 _DesiredAttitudeE;

        [ObservableProperty]
        Vector3 _AngularVelocity;

        [ObservableProperty]
        Vector3 _DesiredAngularVelocity;

        [ObservableProperty]
        float _VerticalVelocity;

        [ObservableProperty]
        float _DesiredVerticalVelocity;

        [ObservableProperty]
        float _BatteryLevel;

        [ObservableProperty]
        float _OnboardTemperature;

        [ObservableProperty]
        float _CPUTemperature;

        [ObservableProperty]
        float _OnboardHhummidity;

        [ObservableProperty]
        Vector<float> _ActuatorsOutput = new(8);

        [ObservableProperty]
        Vector<float> _StickInputs = new(16);
*/