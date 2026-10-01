#!/bin/sh
cd "$(dirname $0)/Runtime/Protobuf"
protoc --csharp_out=. realityapi.proto
