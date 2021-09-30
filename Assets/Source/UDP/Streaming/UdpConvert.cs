using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace UdpMessaging
{
    public static class UdpConvert
    {
        private static int currentMessageID = int.MinValue;
        private static int maxPacketSize = 500; // Does not include 12 byte header
        private static int headerSize = 12;

        public static List<byte[]> ToPacketList(byte[] data)
        {
            int messageID = GetMessageID();
            byte[] messageIDBytes = BitConverter.GetBytes(messageID);//ToBytes(messageID);

            int totalPackets = Mathf.CeilToInt((float)data.Length / (float)maxPacketSize);

            byte[] totalPacketsBytes = BitConverter.GetBytes(totalPackets);
            List<byte[]> packetList = new List<byte[]>();

            for (int currentPacket = 0; currentPacket < totalPackets; currentPacket++)
            {
                byte[] currentPacketBytes = BitConverter.GetBytes(currentPacket);
                byte[] packetHeader = CombineBytes(messageIDBytes, currentPacketBytes, totalPacketsBytes);
                byte[] packetData = SubBytes(data, currentPacket * maxPacketSize, maxPacketSize);
                byte[] headerAndPacket = CombineBytes(packetHeader, packetData);
                packetList.Add(headerAndPacket);
            }

            return packetList;
        }

        private static int GetMessageID()
        {
            int messageID = currentMessageID;

            if (currentMessageID < int.MaxValue)
                currentMessageID++;
            else
                currentMessageID = int.MinValue;

            return messageID;
        }

        public static byte[] SubBytes(byte[] data, int startIndex, int length)
        {
            byte[] subBytes;

            if (data.Length - startIndex - length >= 0)
                subBytes = new byte[length];
            else
                subBytes = new byte[maxPacketSize + (data.Length - startIndex - length)];

            for (int i = startIndex, s = 0; i < startIndex + length && i < data.Length; i++, s++)
            {
                subBytes[s] = data[i];
            }

            return subBytes;
        }

        public static byte[] CombineBytes(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static byte[] CombineBytes(byte[] first, byte[] second, byte[] third)
        {
            return CombineBytes(CombineBytes(first, second), third);
        }

        public static byte[] ToBytes(object source)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                return stream.ToArray();
            }
        }

        public static T FromBytes<T>(byte[] data)
        {
            try
            {
                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream(data))
                {
                    return (T)formatter.Deserialize(stream);
                }
            }
            catch (SerializationException e)
            {
                Debug.Log("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
        }

        public static byte[] UdpMessageToBytes(UdpMessage message)
        {
            byte[] data = new byte[0];

            foreach (UdpPacket p in message.packets)
            {
                data = CombineBytes(data, p.data);
            }

            return data;
        }

        public static UdpPacket BytesToUDPPacket(byte[] data)
        {
            UdpPacket packet = new UdpMessaging.UdpPacket();
            packet.messageID = BitConverter.ToInt32(data, 0);
            packet.packetID = BitConverter.ToInt32(data, 4);
            packet.totalPackets = BitConverter.ToInt32(data, 8);
            packet.data = SubBytes(data, headerSize, data.Length - headerSize);

            return packet;
        }
    }
}