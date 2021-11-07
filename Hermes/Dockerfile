#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7001 5001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Hermes/Hermes.csproj", "Hermes/"]
RUN dotnet restore "Hermes/Hermes.csproj"
COPY . .
WORKDIR "/src/Hermes"
RUN dotnet build "Hermes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hermes.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && \
    apt-get install -y openssl && \     
    openssl genrsa -des3 -passout pass:"GuwyTUzzDDh3UCaCmuLk" -out server.pass.key 4096 && \
    openssl rsa -passin pass:"GuwyTUzzDDh3UCaCmuLk" -in server.pass.key -out server.key && \
    rm server.pass.key && \
    openssl req -new -key server.key -out server.csr \
        -subj "/C=SE/ST=SomeCity/L=SomeState/O=Hermes/CN=localhost.local" && \
    openssl x509 -req -days 36500 -in server.csr -signkey server.key -out server.crt && \
    openssl pkcs12 -export  -in server.crt -inkey server.key -passin pass:"GuwyTUzzDDh3UCaCmuLk" -passout pass:"GuwyTUzzDDh3UCaCmuLk" -out Server.pfx
ENTRYPOINT ["dotnet", "Hermes.dll"]