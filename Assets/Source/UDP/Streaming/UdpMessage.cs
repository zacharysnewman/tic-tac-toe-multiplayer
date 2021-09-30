using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdpMessaging
{
    [Serializable]
    public class UdpMessage
    {
        public int ID;
        public int totalPackets;
        public UdpPacket[] packets = new UdpPacket[0];

        public UdpMessage(int newID, int newTotalPackets)
        {
            ID = newID;
            totalPackets = newTotalPackets;
            packets = new UdpPacket[totalPackets];

            for (int i = 0; i < packets.Length; i++)
            {
                packets[i] = new UdpPacket();
            }
        }
    }
}