using System;
using System.Threading.Tasks;
using UnityEngine.Analytics;
using zeroxoneafour.RealityAPIUnityClient.Protobuf;

namespace zeroxoneafour.RealityAPIUnityClient
{
    class RealityAPIException : Exception {}

    class RealityAPIClient {
        const DeviceType DEV_TYPE = DeviceType.Vr;
        const string DEV_ID = "1";

        private WebsocketHandler _wsHandler;

        public RealityAPIClient()
        {
            
        }

        public async Task<UnityEngine.Vector3> RequestGlobalPosition(UnityEngine.Vector3 localPos)
        {
            var vector = new Protobuf.Vector3()
            {
                X = localPos.x,
                Y = localPos.y,
                Z = localPos.z,
            };
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
    }
}