using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdpMessaging
{
    [Serializable]
    public class UdpPacket
    {
        public int messageID;
        public int packetID;
        public int totalPackets;

        public byte[] data;

        public UdpPacket()
        {
            messageID = 0;
            packetID = 0;
            totalPackets = 0;

            data = new byte[0];
        }

        public override string ToString()
        {
            return string.Format("Packet - Message #: {0}, Packet #: {1}, Packets: {2}, Bytes: {3}", messageID, packetID, totalPackets, data.Length);
        }
    }
}