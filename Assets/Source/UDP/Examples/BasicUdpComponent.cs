using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdpMessaging
{
    public class BasicUdpComponent : MonoBehaviour
    {
        [Tooltip("There can only be one receiver on a single device/computer.")]
        public bool useListener = true;
        public bool receiveMessagesFromSelf = false;
        public int listenPort = 7987;

        private async void Awake()
        {
            BasicUdp.StartClient();

            if (useListener)
            {
                await BasicUdp.StartListening(listenPort, !receiveMessagesFromSelf);
            }
        }

        private void OnApplicationQuit()
        {
            BasicUdp.StopAll();
        }
    }
}