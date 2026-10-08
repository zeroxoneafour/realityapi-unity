using System;
using System.Threading.Tasks;
using UnityEngine.Analytics;
using zeroxoneafour.RealityAPIUnityClient.Protobuf;

namespace zeroxoneafour.RealityAPIUnityClient
{
    class RealityAPIException : Exception {}

    public class RealityAPIClient {
        const DeviceType DEV_TYPE = DeviceType.Vr;
        const string DEV_ID = "1";

        private WebsocketHandler _wsHandler;

        public RealityAPIClient()
        {
            _wsHandler = new WebsocketHandler();
        }

        public RealityAPIClient(string serverUrl)
        {
            _wsHandler = new WebsocketHandler(serverUrl);
        }

        public async Task Init()
        {
            await _wsHandler.Connect();
        }

        public async Task CleanUp()
        {
            await _wsHandler.Disconnect();
        }

        public async Task<UnityEngine.Vector3> RequestGlobalPosition(UnityEngine.Vector3 localPos)
        {
            var vector = new Protobuf.Vector3()
            {
                X = localPos.x,
                Y = localPos.y,
                Z = localPos.z,
            };
            // Must not also set Heartbeat: the server treats any packet with a
            // heartbeat field as a pure keepalive and replies without a position.
            var packet = new Packet()
            {
                Devicetype = DEV_TYPE,
                Id = DEV_ID,
                Position = vector,
            };
            var retPacket = await _wsHandler.SendPacket(packet);
            return new UnityEngine.Vector3()
            {
                x = retPacket.Position.X,
                y = retPacket.Position.Y,
                z = retPacket.Position.Z,
            };
        }

        public async Task SendHeartbeat()
        {
            await _wsHandler.SendPacket(new Packet()
            {
                Devicetype = DEV_TYPE,
                Id = DEV_ID,
                Heartbeat = new Heartbeat()
                {
                    CurrentTime = (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds
                }
            });
        }
    }
}