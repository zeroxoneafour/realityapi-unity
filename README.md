# Reality API Unity Client

## Important - Requires Google.Protobuf DLL (included) and [the NativeWebSockets package](https://github.com/endel/NativeWebSocket) (not included)!

Implementation of the [Reality API Client](https://github.com/LilG0053/RealityAPIServer) in C# for Unity. Not tested.

To use, currently create a `WebsocketHandler` and call its methods. All methods are `async` and return a `Packet`, which can further be clarified into the different types outlined in the file `realityapi.proto`.

I don't actually know how to use Unity, but I do know how to write C#, so this code is hypothetically functional but likely broken. Ping me on discord (zeroxoneafour) for issues.

Installing this package is left as an exercise to the developer.