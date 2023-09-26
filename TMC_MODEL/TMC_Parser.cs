using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API
{
    public partial class TMC_Parser : ObservableObject
    {
        [ObservableProperty]
        int invalidPackets = 0;
        private TMC_Model _tmc;
        private Connection.Connection _connection;
        public TMC_Parser(TMC_Model tmc_api, Connection.Connection connection)
        {
            _tmc = tmc_api;
            _connection = connection;
            if (!connection.IsConnected)
            {
                connection.Connect();
            }

            tmc_api.RequestControlMatrix = () => SendRequestPacket(REQUEST.CL_MATRIX);
            tmc_api.RequestPID = () => SendRequestPacket(REQUEST.PID);
            tmc_api.RequestLimits = () => SendRequestPacket(REQUEST.LIMITS);
            tmc_api.RequesTaskFrequency = () => SendRequestPacket(REQUEST.TASK_FREQUENCY);

            tmc_api.SendControlMatrix = () => { byte[] b = new byte[48 * sizeof(float)]; Buffer.BlockCopy(tmc_api.CTRLMatrix.ToArray(), 0, b, 0, 48 * sizeof(float)); SendServicePacket(SERVICE.CL_MATRIX, b); };

            tmc_api.SendPID = () =>
            {
                var roll = tmc_api.PIDRoll.Serialize(); var pitch = tmc_api.PIDPitch.Serialize(); var yaw = tmc_api.PIDYaw.Serialize(); var vertical = tmc_api.PIDVertical.Serialize();
                byte[] b = new byte[(5 * 3 + 4) * sizeof(float)];
                Buffer.BlockCopy(roll, 0, b, 0, 5 * sizeof(float)); Buffer.BlockCopy(pitch, 0, b, 5 * sizeof(float), 5 * sizeof(float)); Buffer.BlockCopy(yaw, 0, b, 2 * 5 * sizeof(float), 5 * sizeof(float)); Buffer.BlockCopy(vertical, 0, b, 3 * 5 * sizeof(float), 3 * sizeof(float));
                SendServicePacket(SERVICE.PID, b);
            };
            tmc_api.SendLimits = () =>
            {
                var r = tmc_api.Limit_AngularVelocityRoll.Serialize(); var p = tmc_api.Limit_AngularVelocityPitch.Serialize(); var y = tmc_api.Limit_AngularVelocityYaw.Serialize();
                var r_a = BitConverter.GetBytes(tmc_api.Limit_AngleRoll); var p_a = BitConverter.GetBytes(tmc_api.Limit_AnglePitch);
                var s_v = tmc_api.Limit_VerticalVelocity.Serialize(); var s_lon = tmc_api.Limit_LongitudinalVelocity.Serialize(); var s_lat = tmc_api.Limit_LateralVelocity.Serialize();
                byte[] b = new byte[14 * sizeof(float)];
                Buffer.BlockCopy(r_a, 0, b, 0, r_a.Length); Buffer.BlockCopy(p_a, 0, b, p_a.Length, p_a.Length);
                Buffer.BlockCopy(r, 0, b, r_a.Length + p_a.Length, r.Length); Buffer.BlockCopy(p, 0, b, r_a.Length + p_a.Length + r.Length, p.Length); Buffer.BlockCopy(y, 0, b, r_a.Length + p_a.Length + r.Length + p.Length, y.Length);
                Buffer.BlockCopy(s_v, 0, b, r_a.Length + p_a.Length + r.Length + p.Length + y.Length, s_v.Length); Buffer.BlockCopy(s_lon, 0, b, r_a.Length + p_a.Length + r.Length + p.Length + y.Length + s_v.Length, s_lon.Length);
                Buffer.BlockCopy(s_lat, 0, b, r_a.Length + p_a.Length + r.Length + p.Length + y.Length + s_v.Length + s_lon.Length, s_lat.Length);
                SendServicePacket(SERVICE.LIMITS, b);
            };

            tmc_api.SendSticks = (float[] x) => {
                byte[] b = new byte[x.Length*sizeof(float)];
                Buffer.BlockCopy(x.ToArray(), 0, b, 0, x.Length * sizeof(float));
                var data = Protocol.CreateMsg((byte)TYPE.STICKS, b);
                _connection.SendPacket(data);
            };

            connection.RegisterNewDataCallback(HandleNewBytes);
        }

        private void HandleNewBytes(byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                HandleByte(b);
            }
        }

        private enum RX_STATE
        {
            H1,
            H2,
            LEN,
            PAYLOAD,
            CHECKSUM_MSB,
            CHECKSUM_LSB
        };

        RX_STATE rx_state = RX_STATE.H1;
        int payload_len;
        int packet_len;
        int received_bytes;
        int received_payload;
        byte[] payload = new byte[255 + 7];
        UInt16 received_checksum;
        private void HandleByte(byte b)
        {

            switch (rx_state)
            {
                case RX_STATE.H1:
                    {
                        received_bytes = 0;
                        if (b == 0x69)
                        {
                            rx_state = RX_STATE.H2;
                            payload[received_bytes++] = b;
                        }
                        break;
                    }
                case RX_STATE.H2:
                    {
                        if (b == 0x68)
                        {
                            payload[received_bytes++] = b;
                            rx_state = RX_STATE.LEN;
                        }

                        else
                            rx_state = RX_STATE.H1;

                        break;
                    }
                case RX_STATE.LEN:
                    {
                        payload_len = b;
                        payload[received_bytes++] = b;
                        packet_len = payload_len + 6;
                        if (payload_len > payload.Length)

                            rx_state = RX_STATE.H1;
                        else
                        {
                            rx_state = RX_STATE.PAYLOAD;
                            received_payload = 0;
                        }
                        break;
                    }
                case RX_STATE.PAYLOAD:
                    {
                        payload[received_bytes++] = b;
                        received_payload++;
                        if (received_payload > payload_len)
                            rx_state = RX_STATE.CHECKSUM_MSB;
                        break;
                    }
                case RX_STATE.CHECKSUM_MSB:
                    {
                        payload[received_bytes++] = b;
                        received_checksum = (ushort)(b << 8);
                        rx_state = RX_STATE.CHECKSUM_LSB;
                        break;

                    }
                case RX_STATE.CHECKSUM_LSB:
                    {
                        payload[received_bytes++] = b;
                        received_checksum |= (ushort)(b);
                        var crc = Protocol.CalculateChecksum(payload, received_bytes-2);
               
                        if (received_checksum == crc)
                        {
                            HandleNewValidMessage(payload);
                        }
                        else
                            InvalidPackets++;
                        rx_state = RX_STATE.H1;
                        break;

                    }
                default:
                    {
                        rx_state = RX_STATE.H1;
                        break;
                    }
            }

        }



        private void HandleNewValidMessage(byte[] data)
        {
            // Console.WriteLine("Valid message received");
            var type = (TYPE)data[3];
            int len = (int)data[2];

            byte[] payload = new byte[len];
            Buffer.BlockCopy(data, 4, payload, 0, len);


            switch(type)
            {
                case TYPE.REQUEST:
                    {
                        HandleNewRequestPacket(payload);
                        break;
                    }
                case TYPE.SERVICE:
                    {
                        break;
                    }
                case TYPE.TELEMETRY:
                    {
                        HandleNewTelemetryPacket(payload);
                        break;
                    }
                default:
                    break;
            }
        }

        void SendRequestPacket(REQUEST type)
        {
            
            var data = Protocol.CreateMsg((byte)TYPE.REQUEST, new byte[] {(byte)type});
            _connection.SendPacket(data);
        }

        void SendServicePacket(SERVICE type, byte[] data)
        {
            var b = new byte[data.Length + 1];
            b[0] = (byte)type;
            Buffer.BlockCopy(data,0,b,1, data.Length);
            var buff = Protocol.CreateMsg((byte)TYPE.SERVICE,b);
            _connection.SendPacket(buff);

        }
        void HandleNewRequestPacket(byte[] data)
        {
            var type = (REQUEST)data[0];
            switch (type)
            {
                case REQUEST.PID:
                    {
                        Console.WriteLine("PIDs received!");
                        byte[] buf = new byte[5 * sizeof(float)];
                        Buffer.BlockCopy(data, 1, buf, 0, 5 * sizeof(float));
                        _tmc.PIDRoll.Decode(buf);
                        Buffer.BlockCopy(data, 1 + 5 * sizeof(float) * 1, buf, 0, 5 * sizeof(float));
                        _tmc.PIDPitch.Decode(buf);
                        Buffer.BlockCopy(data, 1 + 5 * sizeof(float) * 2, buf, 0, 5 * sizeof(float));
                        _tmc.PIDYaw.Decode(buf);
                        Buffer.BlockCopy(data, 1 + 5 * sizeof(float) * 3, buf, 0, 4 * sizeof(float));
                        _tmc.PIDRoll.Decode(buf);
                        break;
                    }

                case REQUEST.CL_MATRIX:
                    {
                        ObservableCollection<float> v = new ObservableCollection<float>();

                        for (int i = 0; i < 48; i++)
                        {
                            float value = BitConverter.ToSingle(data, 1 + i * 4);
                            v.Add(value);
                        }
                        _tmc.CTRLMatrix = v;
                        Console.WriteLine("Control Matrix received!");
                        break;
                    }
                case REQUEST.LIMITS:
                    {
                        int i = 0;
                        _tmc.Limit_AngleRoll = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_AnglePitch = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_AngularVelocityRoll.Min = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_AngularVelocityRoll.Max = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_AngularVelocityPitch.Min = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_AngularVelocityPitch.Max = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_AngularVelocityYaw.Min = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_AngularVelocityYaw.Max = BitConverter.ToSingle(data, 1 + i++ * 4);

                        _tmc.Limit_VerticalVelocity.Min = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_VerticalVelocity.Max = BitConverter.ToSingle(data, 1 + i++ * 4);

                        _tmc.Limit_LongitudinalVelocity.Min = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_LongitudinalVelocity.Max = BitConverter.ToSingle(data, 1 + i++ * 4);

                        _tmc.Limit_LateralVelocity.Min = BitConverter.ToSingle(data, 1 + i++ * 4);
                        _tmc.Limit_LateralVelocity.Max = BitConverter.ToSingle(data, 1 + i++ * 4);
                        Console.WriteLine("Limits received!");
                        break;
                    }
                case REQUEST.TASK_FREQUENCY:
                    {
                        Console.WriteLine("Frequency received!");
                        break;
                    }
                default : break;

            }
        }
        void HandleNewTelemetryPacket(byte[] data)
        {
            ObservableCollection < TMC_Variables.Variable > v = new ObservableCollection < TMC_Variables.Variable >();
            for (int i =0; i< (int)TMC_Variables.NAMES.VAR_SYS_COUNT; i++)
            {
                float d = BitConverter.ToSingle(data, i * 4);
                _tmc.Variables[i].Value = d;
            }
        }

    }
}
