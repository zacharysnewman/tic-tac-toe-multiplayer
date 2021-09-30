using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdpMessaging
{
    public class AdvancedUdpComponent : MonoBehaviour
    {
        // There can only be one receiver on a single device/computer
        public bool useReceiver = true;

        private void Awake()
        {
            AdvancedUdp.StartUdpClient();

            if (useReceiver)
                AdvancedUdp.StartListening();
        }

        private void OnApplicationQuit()
        {
            AdvancedUdp.DefaultStop();
            AdvancedUdp.OnMessageReceived.RemoveAllListeners();
        }
    }
}
