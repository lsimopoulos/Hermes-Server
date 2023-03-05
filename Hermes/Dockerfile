#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7001 55556

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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
COPY Hermes/Server.pfx /app/Server.pfx


ENTRYPOINT ["dotnet", "Hermes.dll"]