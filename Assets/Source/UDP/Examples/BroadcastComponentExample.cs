﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdpMessaging
{
    public class BroadcastComponentExample : MonoBehaviour
    {
        [Tooltip("There can only be one receiver on a single device/computer.")]
        public bool useListener = true;
        public bool receiveMessagesFromSelf = false;
        public bool useBroadcastHandshake = false;
        public int listenPort = 7987;
        public int broadcastPort = 7987;

        private async void Awake()
        {
            BasicUdp.StartClient();

            if (useListener)
            {
                await BasicUdp.StartListening(listenPort, !receiveMessagesFromSelf);
            }
        }

        public void Update()
        {
            // Refresh Handshake by Pinging everyone (Maybe actually calculate ping here?)
            if (useBroadcastHandshake && Time.frameCount % 60 == 0)
            {
                BasicUdp.Broadcast(new byte[0], broadcastPort);
            }
        }

        private void OnApplicationQuit()
        {
            BasicUdp.StopAll();
        }
    }
}