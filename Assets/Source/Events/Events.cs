using System.Collections;
using System.Collections.Generic;

namespace Events
{
    namespace UnityEvents
    {
        public class AwakeEvent : EventSync { }
        public class StartEvent : EventSync { }
        public class UpdateEvent : EventSync { }
        public class FixedUpdateEvent : EventSync { }
        public class LateUpdateEvent : EventSync { }
        public class DestroyEvent : EventSync { }
        public class ApplicationQuitEvent : EventSync { }
    }
    namespace InputEvents
    {
        public class NewGameEvent : EventSync { }
        public class ActivateTileEvent : EventSync<Coordinates> { }
    }
    namespace TransportEvents
    {
        public class ClientConnectEvent : EventSync { }
        public class ClientReceiveBytesEvent : EventSync<byte[]> { }
        public class ClientDisconnectEvent : EventSync { }
    }
    namespace StateEvents
    {
        public class StateChangedEvent : EventSync<GameState> { }
    }
}