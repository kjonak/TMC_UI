using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.AppSettings
{
    public partial class AppSettings : ObservableObject
    {
        [ObservableProperty]
        string _UDP_IP = "0.0.0.0";

        [ObservableProperty]
        int _UDP_Listen_Port = 5685;
        [ObservableProperty]
        int _UDP_Target_Port = 5685;

        [ObservableProperty]
        int _BaudRate = 115200;

        [ObservableProperty]
        string _StreamIP = "0.0.0.0";
        [ObservableProperty]
        int _StreamPort = 0;

        [ObservableProperty]
        double _ControllerInterval = 50;

        [ObservableProperty]
        ControllersSettings _ControllersSettings = new ControllersSettings();

        [ObservableProperty]
        int _ConnectionMethod = 0;
        public void Save(string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(AppSettings));
                xmls.Serialize(sw, this);
            }
        }

        public static AppSettings? Read(string filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    try
                    {
                        XmlSerializer xmls = new XmlSerializer(typeof(AppSettings));

                        AppSettings ret = xmls.Deserialize(sr) as AppSettings;

                        return ret;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return null;
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }
    }
}
