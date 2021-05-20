# Hermes-Server
A  chat server implemented with [gRPC](https://grpc.io) in [.NET](https://dotnet.microsoft.com/) and C#.

## Requirements ##
[Docker](https://docs.docker.com/get-docker/) 

## How to run ##

### Docker ###
In  the root  of repo folder and enter the following commands:

```
 docker build -f Hermes\Dockerfile --tag hermes-server .
 docker run -it --name hermes hermes-server -p 5001:5001 -p 7001:7001
```
