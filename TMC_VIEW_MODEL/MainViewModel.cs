using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services.AppSettings;
using Services.Dialogs;
using Services.Dialogs.SettingsDialog.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using TMC_API;
using TMC_API.Connection;
using TMC_VIEW_MODEL;


namespace TMC_VIEW_MODEL
{
    public class ControlWriter : TextWriter
    {
        public Action<string> Writer;
  

        public override void Write(char value)
        {
            if(Writer != null)
                Writer(value.ToString());
          
        }

        public override void Write(string value)
        {
            if (Writer != null)
                Writer(value);
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }

    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private TMC_Parser? _TMC_Parser;

        [ObservableProperty]
        private TMC_Model _TMC_Model;


        [ObservableProperty]
        private SetupViewModel _SetupVM;

        [ObservableProperty]
        private PIDTuningViewModel _PIDTuningVM;

        [ObservableProperty]
        private BaseTMCViewModel _ControlVM;

        [ObservableProperty]
        private BaseTMCViewModel _SelectedVM;
        [ObservableProperty]
        ControllerViewModel _ControllerVM;

        private ControlWriter _ControlWriter = new ControlWriter();
        private string _diary;
        public string Diary
        {
            get { return _diary; }
            set 
            { 
                string prefix = DateTime.Now.ToString("T") + ": ";
                string text;// = prefix + value ;
                if (value != "\r\n")
                {
                    text = prefix + value;
                }
                else
                     text = value;
                if (_diary != null) 
                    _diary += text; 
                else 
                    _diary = text; 
                OnPropertyChanged("Diary"); 
            }
        }

        [RelayCommand]
        private void ChangeSelectedVM(string arg)
        {
            SelectedVM.OnHide();
            //string arg = "PID";
            switch(arg)
            {
                case "Setup":
                    SelectedVM = SetupVM; break;
                case "PID":
                    SelectedVM = PIDTuningVM; break;
                case "Control":
                    SelectedVM  = ControlVM; break;
            }
            SelectedVM.OnShow();
        }
        private void InitializeConnection(Connection connection)
        {
            Connection.Connect();
            if(Connection.IsConnected)
            {
                TMC_Parser = new TMC_Parser(TMC_Model, connection);
                Console.WriteLine("Connection established!");
            } else
            {
                Console.WriteLine("Could not establish connection");
            }
                
        }

        [ObservableProperty]
        Connection connection;

        [RelayCommand]
        private void ConnectUDP()
        {
            Connection = new NetworkConnection(AppSettings.UDP_IP, AppSettings.UDP_Target_Port, AppSettings.UDP_Listen_Port);
            InitializeConnection(Connection);
            Console.WriteLine("Listening on: "+AppSettings.UDP_Listen_Port + " Sending to: " +AppSettings.UDP_IP+":"+ AppSettings.UDP_Target_Port);
        }
        [RelayCommand]
        private void Disconnect()
        {
            Connection.Disconnect();
            Console.WriteLine("Connection closed");
        }
        [RelayCommand]
        private void ConnectSerial(string portName)
        {
            if(portName == null)
            {
                Console.WriteLine("Please select serial port");
                return;
            }
            Console.WriteLine("Opening serial port " + portName);
            Connection = new SerialConnection(portName, AppSettings.BaudRate);
            InitializeConnection(Connection);

        }

        [RelayCommand]
        private void OpenSettings()
        {
           
            DialogBaseViewModel vm = new SettingsDialogViewModel(AppSettings);
            DialogService.OpenDialog(vm);
        }
        [ObservableProperty]
        AppSettings _appSettings;
        void write(string value)
        {
            Diary = value;
        }
        public void OnClose()
        {
            Connection.Disconnect();
        }
        public MainViewModel(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _diary = new string("");
            Connection = new Connection();
            _ControlWriter.Writer = write;
            Console.SetOut(_ControlWriter);
            TMC_Model = new TMC_Model();

            ControllerVM = new ControllerViewModel(TMC_Model, _appSettings);
            SetupVM = new SetupViewModel(TMC_Model);
            PIDTuningVM = new PIDTuningViewModel(TMC_Model);
            ControlVM = new ControlViewModel(TMC_Model);

            SelectedVM = PIDTuningVM;

        }

    }
}
