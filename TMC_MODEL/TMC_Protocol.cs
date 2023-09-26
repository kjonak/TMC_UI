using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API
{
    //internal static class TO_OKON
    //{
    //    public static byte MSG_OKON_REQUEST = 0x0;
    //    public static byte MSG_OKON_SERVICE = 0x1;
    //    public static byte MSG_OKON_STICKS = 0x2;
    //    public static byte MSG_OKON_MODE = 0x3;
    //    public static byte MSG_OKON_CL_STATUS = 0x4;

    //}

    public enum TYPE
    {
        REQUEST,
        SERVICE,
        STICKS,
        TELEMETRY
    }
    public enum REQUEST
    {
         PID = 0x0,
         CL_MATRIX = 0x1,
         TASK_FREQUENCY,
         LIMITS
    }

    public enum SERVICE
    {
        PID = 0x0,
        CL_MATRIX = 0x1,
        TASK_FREQUENCY,
        LIMITS
    }

    //internal static class TO_OKON_SERVICE
    //{
    //    public static byte MSG_OKON_SERVICE_ENTER = 0x0;
    //    public static byte MSG_OKON_SERVICE_REBOOT = 0x1;
    //    public static byte MSG_OKON_SERVICE_UPDATE_CL_MATRIX = 0x2;
    //    public static byte MSG_OKON_SERVICE_ENABLE_DIRECT_MOTORS_CTRL = 0x3;
    //    public static byte MSG_OKON_SERVICE_DISABLE_DIRECT_MOTORS_CTRL = 0x4;
    //    public static byte MSG_OKON_SERVICE_DIRECT_THRUSTERS_CTRL = 0x5;
    //    public static byte MSG_OKON_SERVICE_DIRECT_MATRIX_THRUSTERS_CTRL = 0x6;
    //    public static byte MSG_OKON_SERVICE_SAVE_SETTINGS = 0x7;
    //    public static byte MSG_OKON_SERVICE_LOAD_SETTINGS = 0x8;
    //    public static byte MSG_OKON_SERVICE_NEW_PIDS = 0x9;
    //}

    //internal static class FROM_OKON
    //{
    //    public static byte MSG_FROM_OKON_PID = 0x0;
    //    public static byte MSG_FROM_OKON_CL_MATRIX = 0x1;
    //    public static byte MSG_FROM_OKON_HEART_BEAT = 0x2;
    //    public static byte MSG_FROM_OKON_SERVICE_CONFIRM = 0x3;
    //}

    public static class Protocol
    {
        

        public static byte[] CreateMsg(byte msg_type, byte[]? data)
        {
            int data_len = 0;
            if(data != null)
                data_len = data.Length;

            byte[] msg = new byte[data_len + 6];
            if (data != null)
            { 
                for (int i = 0; i < data_len; i++)
                {
                    msg[i + 4] = data[i];
                }
            }
            msg[0] = 0x69;
            msg[1] = 0x68;
            msg[2] = (byte)(data_len);
            msg[3] = msg_type;
           
            UInt16 checksum = CalculateChecksum(msg,  data_len + 4);
            msg[data_len + 4] = (byte)(checksum >> 8);
            msg[data_len + 5] = (byte)(checksum);
            return msg;
        }

        public static ushort CalculateChecksum(byte[] data, int length)
        {
            byte x;
            ushort crc = 0xFFFF;

            for (int i = 0; i < length; i++)
            {

                x = ((byte)((crc >> 8) ^ data[i]));

                x ^= (byte)(x >> 4);
                crc = (ushort)((crc << 8) ^ ((ushort)(x << 12)) ^ ((ushort)(x << 5)) ^ ((ushort)x));
            }

            return crc;
        }
    }
}

