using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace UdpMessaging
{
    public static class BasicUdp
    {
        private static event Action<byte[]> onMessageReceived;
        private static List<Action<byte[]>> subscribers = new List<Action<byte[]>>();
        private static UdpClient udpClient;
        private static bool isInitialized = false;
        private static bool isListening = false;

        public static void Subscribe(Action<byte[]> subscriber)
        {
            BasicUdp.onMessageReceived += subscriber;
            BasicUdp.subscribers.Add(subscriber);
        }
        public static void Unsubscribe(Action<byte[]> subscriber)
        {
            BasicUdp.onMessageReceived -= subscriber;
            BasicUdp.subscribers.Remove(subscriber);
        }
        public static void UnsubscribeAll()
        {
            var subscribers = new List<Action<byte[]>>(BasicUdp.subscribers);
            foreach (var subscriber in subscribers)
            {
                BasicUdp.Unsubscribe(subscriber);
            }
        }
        public static void InvokeOnMessageReceived(byte[] message)
        {
            if(BasicUdp.onMessageReceived != null)
            {
                BasicUdp.onMessageReceived.Invoke(message);
            }
        }

        // Initializes client for sending and/or receiving
        public static void StartClient()
        {
            if (BasicUdp.isInitialized)
                return;

            BasicUdp.udpClient = new UdpClient();
            BasicUdp.udpClient.EnableBroadcast = true;

            BasicUdp.isInitialized = true;
        }

        // Initializes listening for messages
        public static async Task StartListening(int port, bool rejectMessagesFromSelf = true)
        {
            if (BasicUdp.isListening)
                return;

            if (!BasicUdp.isInitialized)
                throw new Exception("UDP Client not initialized. Must be initialized before Listening.");

            BasicUdp.isListening = true;
            BasicUdp.udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            await ReceiveMessage(rejectMessagesFromSelf);
        }

        private static async Task<(IPEndPoint, byte[])> ClientReceiveAsync()
        {
            var result = await BasicUdp.udpClient.ReceiveAsync();
            return (result.RemoteEndPoint, result.Buffer);
        }
        private static async Task ReceiveMessage(bool rejectMessagesFromSelf)
        {
            while (BasicUdp.isListening)
            {
                var (from, data) = await BasicUdp.ClientReceiveAsync();
                bool isLocalIP = BasicUdp.LocalIP == from.Address.ToString();
                bool isPublicIP = BasicUdp.PublicIP == from.Address.ToString();
                bool rejectMessage = rejectMessagesFromSelf && (isLocalIP || isPublicIP);

                if (data.Length <= 0 || rejectMessage)
                    continue;

                BasicUdp.InvokeOnMessageReceived(data);
            }
        }

        public static void Broadcast(byte[] data, int port)
        {
            Send(data, new IPEndPoint(IPAddress.Broadcast, port));
        }

        public static void Send(byte[] data, IPEndPoint endPoint)
        {
            if (!BasicUdp.isInitialized)
                throw new Exception("UDP Client not initialized. Must be initialized before Sending.");

            if (data.Length > 1024)
                throw new Exception("Exceeded maximum message size of 1024 bytes. Message size: " + data.Length);

            BasicUdp.udpClient.Send(data, data.Length, endPoint);
        }

        // When stopping all or portions of the system
        public static void StopAll()
        {
            BasicUdp.StopListening();
            BasicUdp.StopClient();
            BasicUdp.UnsubscribeAll();
        }

        // Stop listening for broadcasts
        public static void StopListening()
        {
            BasicUdp.isListening = false;
        }

        // Stop client and close connection
        public static void StopClient()
        {
            if (!BasicUdp.isInitialized)
                return;

            BasicUdp.udpClient.Close();
            BasicUdp.isInitialized = false;
        }

        // Getting local and public IP's
        private static string myLocalIp = "";
        private static string myPublicIp = "";
        private static bool gettingPublicIP = false;

        private static string LocalIP
        {
            get
            {
                if (BasicUdp.myLocalIp == "")
                    BasicUdp.myLocalIp = BasicUdp.GetLocalIpAddress();

                return BasicUdp.myLocalIp;
            }
        }

        private static string PublicIP
        {
            get
            {
                if (BasicUdp.myPublicIp == "" && !BasicUdp.gettingPublicIP)
                    BasicUdp.GetPublicIpAddress();

                return BasicUdp.myPublicIp;
            }
        }

        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static async void GetPublicIpAddress()
        {
            BasicUdp.gettingPublicIP = true;

            var result = "0.0.0.0";
            try
            {
                result = await new HttpClient().GetStringAsync("http://ipinfo.io/ip");
            }
            catch { }

            BasicUdp.myPublicIp = result.Trim();
            BasicUdp.gettingPublicIP = false;
        }
    }
}