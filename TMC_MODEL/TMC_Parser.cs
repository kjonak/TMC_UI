using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMC_API
{
    public class TMC_Parser
    {
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
            LEN_MSB,
            LEN_LSB,
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
                            rx_state = RX_STATE.LEN_MSB;
                        }

                        else
                            rx_state = RX_STATE.H1;

                        break;
                    }
                case RX_STATE.LEN_MSB:
                    {
                        payload_len = b << 8;
                        rx_state = RX_STATE.LEN_LSB;
                        payload[received_bytes++] = b;

                        break;
                    }
                case RX_STATE.LEN_LSB:
                    {
                        payload_len |= b;
                        payload[received_bytes++] = b;
                        packet_len = payload_len + 7;
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
            Console.WriteLine("Valid message received");
        }


    }
}
