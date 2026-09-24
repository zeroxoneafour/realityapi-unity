using NativeWebSocket;
using Google.Protobuf;
using System.Threading.Tasks;
using zeroxoneafour.RealityAPIUnityClient.Protobuf;

namespace zeroxoneafour.RealityAPIUnityClient
{
    public class WebsocketHandler
    {
        private const string _wsUrl = "ws://10.89.53.91:65432";
        
        private WebSocket _websocket;

        public WebsocketHandler()
        {
            _websocket = new WebSocket(_wsUrl);
        }

        public WebsocketHandler(string wsUrl)
        {
            _websocket = new WebSocket(wsUrl);
        }

        private async Task<Packet> SendAndReceive<T>(T input) where T: IMessage<T>
        {
            var tcs = new TaskCompletionSource<byte[]>();
            WebSocketMessageEventHandler closure = (msg) => tcs.SetResult(msg);
            _websocket.OnMessage += closure;
            await _websocket.Send(input.ToByteArray());
            var res = await tcs.Task;
            _websocket.OnMessage -= closure;
            return Packet.Parser.ParseFrom(res);
        }

        public async Task<Packet> SayHello(string name)
        {
            var hello = new Hello
            {
                Name = name
            };
            return await SendAndReceive(hello);
        }

        public async Task<Packet> SendText(string text)
        {
            var textMsg = new Text
            {
                Text_ = text
            };
            return await SendAndReceive(textMsg);
        }

        public async Task<Packet> SendPosition(UnityEngine.Vector3 pos)
        {
            var position = new Protobuf.Vector3
            {
                X = pos.x,
                Y = pos.y,
                Z = pos.z
            };
            return await SendAndReceive(position);
        }
    }
}