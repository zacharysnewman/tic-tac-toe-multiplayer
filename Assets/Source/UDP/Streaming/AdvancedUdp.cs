using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace UdpMessaging
{

    public static class AdvancedUdp
    {
        public static UDPByteEvent OnMessageReceived = new UDPByteEvent(); // for use inside of Unity
                                                                           //public static event Action<byte[]> OnMessageReceived; // for use outside of Unity (although it can be used in Unity as well)
        private static UdpClient udpClient;
        private static int PORT = 8051;
        private static bool isInitialized = false;
        private static bool isListening = false;

        private static List<UdpMessage> incompleteMessages = new List<UdpMessage>();

        // Initializes both the client and broadcast listening
        public static void DefaultStart()
        {
            StartUdpClient();
            StartListening();
        }

        // Initializes client for sending and/or receiving
        public static void StartUdpClient()
        {
            if (isInitialized)
                return;

            udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;

            isInitialized = true;
        }


        // Initializes listening for broadcast messages
        public static void StartListening()
        {
            if (isListening)
                return;

            if (!isInitialized)
                throw new Exception("UDP Client not initialized. Must be initialized before Listening.");

            isListening = true;

            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

            var from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {
                while (isListening)
                {
                    var data = udpClient.Receive(ref from);

                    OnPacketReceived(data);
                }
            });
        }


        public static Task SendAsync(byte[] data)
        {
            if (!isInitialized)
                throw new Exception("UDP Client not initialized. Must be initialized before Sending.");

            return Task.Run(() =>
            {
                var packets = UdpConvert.ToPacketList(data);

                foreach (byte[] packet in packets)
                {
                    udpClient.Send(packet, packet.Length, new IPEndPoint(IPAddress.Broadcast, PORT));
                }
            });
        }

        // Send method can be called from anywhere
        public static void Send(byte[] data)
        {
            if (!isInitialized)
                throw new Exception("UDP Client not initialized. Must be initialized before Sending.");

            var packets = UdpConvert.ToPacketList(data);

            foreach (byte[] packet in packets)
            {
                udpClient.Send(packet, packet.Length, new IPEndPoint(IPAddress.Broadcast, PORT));
            }
        }


        // When stopping all or portions of the system
        public static void DefaultStop()
        {
            StopListening();
            StopUdpClient();
        }

        // Stop listening for broadcasts
        public static void StopListening()
        {
            isListening = false;
        }

        // Stop client and close connection
        public static void StopUdpClient()
        {
            if (!isInitialized)
                return;

            udpClient.Close();
            isInitialized = false;
        }


        // Handle large incoming messages and put them back together
        private static void OnPacketReceived(byte[] bytePacket)
        {
            AddPacketToMessageList(bytePacket);
        }

        private static void AddPacketToMessageList(byte[] bytePacket)
        {
            UdpPacket packet = UdpConvert.BytesToUDPPacket(bytePacket);

            foreach (UdpMessage msg in incompleteMessages)
            {
                if (msg.ID == packet.messageID)
                {
                    if (msg.packets.Length == 0)
                    {
                        msg.packets = new UdpPacket[packet.totalPackets];
                    }

                    msg.packets[packet.packetID] = packet;

                    if (IsCompleteMessage(msg))
                    {
                        byte[] byteMessage = UdpConvert.UdpMessageToBytes(msg);
                        OnMessageReceived.Invoke(byteMessage);
                        incompleteMessages.Remove(msg);
                    }

                    return;
                }
            }

            // If the message can't be found
            UdpMessage newMessage = new UdpMessage(packet.messageID, packet.totalPackets);
            newMessage.packets[packet.packetID] = packet;

            if (IsCompleteMessage(newMessage))
            {
                byte[] byteMessage = UdpConvert.UdpMessageToBytes(newMessage);
                OnMessageReceived.Invoke(byteMessage);
            }
            else
            {
                incompleteMessages.Add(newMessage);
            }
        }

        private static bool IsCompleteMessage(UdpMessage message)
        {
            foreach (UdpPacket packet in message.packets)
            {
                if (packet.totalPackets == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}