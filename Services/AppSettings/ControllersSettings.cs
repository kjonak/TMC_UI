using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Services.AppSettings
{

    [Serializable]
    public partial class ControllersSettings : ObservableObject
    {
        public enum KeyboardControls
        {
         ROLL_LEFT,
         ROLL_RIGHT,
         PITCH_UP,
         PITCH_DOWN,
         YAW_LEFT,
         YAW_RIGHT,
         FORWAD,
         BACKWARD,
         UP,
         DOWN,
         LEFT,
         RIGHT
        }

        [ObservableProperty]
        Key rollLeftKey = Key.A;
        [ObservableProperty]
        Key rollRightKey = Key.D;
        [ObservableProperty]
        Key pitchUpKey = Key.S;
        [ObservableProperty]
        Key pitchDownKey = Key.W;
        [ObservableProperty]
        Key yawLeftKey = Key.A;
        [ObservableProperty]
        Key yawRightKey = Key.D;
        [ObservableProperty]
        Key forwardKey = Key.Up;
        [ObservableProperty]
        Key backwardKey = Key.Down;
        [ObservableProperty]
        Key downKey = Key.LeftCtrl;
        [ObservableProperty]
        Key upKey = Key.LeftShift;
        [ObservableProperty]
        Key leftKey = Key.Left;
        [ObservableProperty]
        Key rightKey = Key.Right;

        [ObservableProperty]
        string rollKeyPad = "rightThumbX";
        [ObservableProperty]
        string pitchKeyPad = "rightThumbY";
        [ObservableProperty]
        string yawKeyPad = "rightThumbY";
        [ObservableProperty]
        string forwardKeyPad = "leftThumbY";
        [ObservableProperty]
        string upKeyPad = "leftTrigger";
        [ObservableProperty]
        string downKeyPad = "rightTrigger";
        [ObservableProperty]
        string leftKeyPad = "leftTrigger";
        [ObservableProperty]
        string rightKeyPad = "rightTrigger";

       
       // [XmlIgnoreAttribute]
        public ObservableCollection<Key> KeyboardAssignment { get; set; }

        //[XmlIgnoreAttribute]
        //[ObservableProperty]
        //Dictionary<string, string> _PadAssignment;

        public void RaisePropertyChanged(string name)
        {
            OnPropertyChanged(name);
        }
        
  

        public void LoadDefaultValues()
        {
            KeyboardAssignment = new ObservableCollection<Key>
            {
                RollLeftKey,
                RollRightKey,
                PitchUpKey,
                PitchDownKey,
                YawLeftKey,
                YawRightKey,
                ForwardKey,
                BackwardKey,
                DownKey,
                UpKey
            };
        }
        public ControllersSettings()
        {
            KeyboardAssignment = new ObservableCollection<Key>();
            //  if(KeyboardAssignment == null) 
            //KeyboardAssignment = new ObservableCollection<Key>
            //{
            //    RollLeftKey,
            //    RollRightKey,
            //    PitchUpKey,
            //    PitchDownKey,
            //    YawLeftKey,
            //    YawRightKey,
            //    ForwardKey,
            //    BackwardKey,
            //    DownKey,
            //    UpKey
            //};

            //PadAssignment = new Dictionary<string, string>();
            //PadAssignment.Add("roll", rollKeyPad);
            //PadAssignment.Add("pitch", pitchKeyPad);
            //PadAssignment.Add("yaw", yawKeyPad);
            //PadAssignment.Add("forward", forwardKeyPad);
            //PadAssignment.Add("down", upKeyPad);
            //PadAssignment.Add("up", downKeyPad);
            //PadAssignment.Add("left", upKeyPad);
            //PadAssignment.Add("right", downKeyPad);


        }
    }
}
