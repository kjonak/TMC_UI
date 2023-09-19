using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API.Connection
{
    public partial class Connection : ObservableObject
    {
        [ObservableProperty]
        bool _IsConnected = false;

        private List<Action<byte[]>> callbacks = new List<Action<byte[]>>();
        public virtual void SendPacket(byte[] data)
        {
            throw new NotImplementedException();
        }

        public virtual void Disconnect()
        {
            throw new NotImplementedException();
        }

        public virtual void Connect()
        {
            throw new NotImplementedException();
        }

        protected void InvokeNewDataCallback(byte[] data)
        {
            foreach(var callback in callbacks)
                callback(data);
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
