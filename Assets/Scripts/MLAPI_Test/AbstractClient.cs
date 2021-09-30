using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using System;
using Events.UnityEvents;
using Events.TransportEvents;

public class AbstractClient : IDisposable
{
    private NetworkDriver m_Driver;
    private NetworkConnection m_Connection;

    #region Public Interface

    public AbstractClient()
    {
        EventAggregator.Get<DestroyEvent>().Subscribe(Dispose);
        EventAggregator.Get<UpdateEvent>().Subscribe(CheckForMessages);
    }

    public void DefaultConnect()
    {
        Connect(NetworkEndPoint.LoopbackIpv4, 9000);
    }

    public void Connect(NetworkEndPoint endpoint, ushort port)
    {
        m_Driver = NetworkDriver.Create();
        m_Connection = default(NetworkConnection);

        endpoint.Port = port;
        m_Connection = m_Driver.Connect(endpoint);
    }

    public void Disconnect()
    {
        m_Connection.Disconnect(m_Driver);
        OnDisconnect();
    }

    public void Dispose()
    {
        m_Driver.Dispose();
    }

    #endregion

    #region Triggers

    private void OnConnect()
    {
        EventAggregator.Get<ClientConnectEvent>().Publish();
    }

    private void OnReceiveBytes(byte[] bytes)
    {
        Debug.Log(bytes);
        EventAggregator.Get<ClientReceiveBytesEvent>().Publish(bytes);
    }

    private void OnDisconnect()
    {
        m_Connection = default(NetworkConnection);
        EventAggregator.Get<ClientDisconnectEvent>().Publish();
    }

    #endregion


    private void CheckForMessages()
    {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            return;
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;

        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");
                OnConnect();
                byte[] value = { 1, 2, 3 };
                m_Driver.BeginSend(m_Connection, out var writer);
                writer.WriteBytes(new NativeArray<byte>(value, Allocator.Temp));
                m_Driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                NativeArray<byte> bytes = new NativeArray<byte>();
                stream.ReadBytes(bytes);
                OnReceiveBytes(bytes.ToArray());
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                OnDisconnect();
            }
        }
    }
}
