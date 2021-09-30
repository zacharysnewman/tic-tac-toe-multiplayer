using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UdpMessaging;

public class UdpTest : MonoBehaviour
{
    public UdpEndPoint[] endPoints = { };
    public string myName = "Zack";
    public string message = "Yoho!";

    void Start()
    {
        BasicUdp.Subscribe(OnMessage);
        BasicUdp.GetPublicIpAddress();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && message != "")
        {
            var msg = myName + ": " + message;

            foreach (var endPoint in endPoints)
            {
                try
                {
                    BasicUdp.Send(UdpConvert.ToBytes(msg), endPoint);
                }
                catch (Exception e) { Debug.Log(e); }
            }
            message = "";
        }
    }

    void OnMessage(byte[] message)
    {
        Debug.Log(UdpConvert.FromBytes<string>(message));
    }
}
