using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API.Connection
{
    public partial class Connection : ObservableObject
    {

        private double _TX_Rate;
        public double TX_Rate { get => _TX_Rate; private set { _TX_Rate = value; OnPropertyChanged(); } }

        private double _RX_Rate;
        public double RX_Rate { get => _RX_Rate; private set { _RX_Rate = value; OnPropertyChanged(); } }

        private bool _IsConnected = false;
        public bool IsConnected {  get {  return _IsConnected; } protected set { _IsConnected = value; OnPropertyChanged("IsConnected"); } }

        private List<Action<byte[]>> callbacks = new List<Action<byte[]>>();
       

        public virtual void Disconnect()
        {
            throw new NotImplementedException();
        }

        public virtual void Connect()
        {
            throw new NotImplementedException();
        }
 

        int _rx_packets, _tx_packets;
        DateTime _last_update;
        protected void InvokeNewDataCallback(byte[] data)
        {
            mut.WaitOne();
            _rx_packets += data.Length;
            mut.ReleaseMutex();
            foreach (var callback in callbacks)
                callback(data);
        }

        bool _calculate_stats;
        protected void StartCalculatingStats()
        {
            _calculate_stats = true;
            Thread x = new Thread(_task);
            x.Start();
        }
        protected void StopCalculatingStats()
        {
            _calculate_stats = false;
        }

        public virtual void SendPacket(byte[] data)
        {
            mut.WaitOne();
            _tx_packets += data.Length;
            mut.ReleaseMutex();
        }
        private static Mutex mut = new Mutex();

        void _task()
        {
            while(_calculate_stats)
            {
                if ((DateTime.Now - _last_update) > TimeSpan.FromSeconds(0.2))
                {
                    double dt = (DateTime.Now - _last_update).TotalSeconds * 1000;
                    mut.WaitOne();
                    RX_Rate = (_rx_packets / dt);
                    TX_Rate = (_tx_packets / dt);// (DateTime.Now - _last_update).TotalSeconds / 1000;
                    _rx_packets = 0;
                    _tx_packets = 0;
                    mut.ReleaseMutex();
                    _last_update = DateTime.Now;
                }
                Thread.Sleep(100);
            }
        }

        public void RegisterNewDataCallback(Action<byte[]> callback)
        {
            foreach (var action in callbacks)
            {
                if (action == callback)
                {
                    return;
                }
            }
            Debug.WriteLine("Added callback");
            callbacks.Add(callback);
        }
        public void DeleteNewDataCallback(Action<byte[]> callback)
        {
            foreach (var action in callbacks)
            {
                if (action == callback)
                {
                    callbacks.Remove(action);
                    Debug.WriteLine("Callback removed");
                    return;
                }
            }
        }

    }
}
