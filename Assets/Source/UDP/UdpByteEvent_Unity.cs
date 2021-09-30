// Obviously can't be used outside of Unity since it relies on UnityEngine.Events namespace
using UnityEngine.Events;

namespace UdpMessaging
{
    public class UDPByteEvent : UnityEvent<byte[]> { }
}
