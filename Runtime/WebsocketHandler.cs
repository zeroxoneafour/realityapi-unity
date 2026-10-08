using NativeWebSocket;
using Google.Protobuf;
using System.Threading.Tasks;
using zeroxoneafour.RealityAPIUnityClient.Protobuf;
using UnityEngine.PlayerLoop;
using Google.Protobuf.WellKnownTypes;
using System;

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

        public async Task Connect()
        {
            // NativeWebSocket's Connect() does not return once the handshake
            // succeeds -- it internally runs the socket's whole receive loop
            // and only completes when the connection closes. So it must be
            // fired off rather than awaited, and readiness tracked separately
            // via OnOpen/OnError/OnClose.
            var tcs = new TaskCompletionSource<bool>();

            void OnOpen() => tcs.TrySetResult(true);
            void OnError(string msg) => tcs.TrySetException(new Exception(msg));
            void OnClose(WebSocketCloseCode code) => tcs.TrySetException(new Exception($"Socket closed before opening: {code}"));

            _websocket.OnOpen += OnOpen;
            _websocket.OnError += OnError;
            _websocket.OnClose += OnClose;

            _ = _websocket.Connect();

            try
            {
                await tcs.Task;
            }
            finally
            {
                _websocket.OnOpen -= OnOpen;
                _websocket.OnError -= OnError;
                _websocket.OnClose -= OnClose;
            }
        }

        public async Task Disconnect()
        {
            await _websocket.Close();
        }

        private async Task<Packet> SendAndReceive<T>(T input) where T: IMessage<T>
        {
            // Connect() already owns the socket's single receive loop, so we
            // must not call Receive() again here -- just wait for the next
            // OnMessage instead of pumping a second concurrent receive.
            var tcs = new TaskCompletionSource<byte[]>();
            WebSocketMessageEventHandler closure = (msg) => tcs.TrySetResult(msg);
            _websocket.OnMessage += closure;
            try
            {
                await _websocket.Send(input.ToByteArray());
                var res = await tcs.Task;
                return Packet.Parser.ParseFrom(res);
            }
            finally
            {
                _websocket.OnMessage -= closure;
            }
        }

        // obsoleted i suppose?
        /*
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
        */

        public async Task<Packet> SendPacket(Packet packet)
        {
            return await SendAndReceive(packet);
        }
    }
}