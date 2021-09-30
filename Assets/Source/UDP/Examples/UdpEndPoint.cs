using System.Net;

namespace UdpMessaging
{
    [System.Serializable]
    public class UdpEndPoint
    {
        public string name = "";
        public string ip = "";
        public int port = 0;

        public UdpEndPoint(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public UdpEndPoint(string name, string ip, int port)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, IP: {1}, Port: {2}", name, ip, port);
        }

        public static implicit operator IPEndPoint(UdpEndPoint endpoint)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(0, endpoint.port);

            try
            {
                ipEndPoint.Address = IPAddress.Parse(endpoint.ip);
            }
            catch { }

            return ipEndPoint;
        }
    }
}