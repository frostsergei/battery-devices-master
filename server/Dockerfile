FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["server/server.csproj", "server/"]

RUN dotnet restore "server/server.csproj"

COPY ["server/", "server/"]
COPY ["apigen.nswag", "BatteryDevicesMaster.sln", "./"]

RUN dotnet build "server/server.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN apt update &&\
    apt -y install nodejs &&\
    apt -y install npm &&\
    npm install -g --silent nswag@14.0.7 

RUN nswag run apigen.nswag &&\
    sed 's/\\n[[:space:]]*"/"/g' /docs/api/api.yaml > /docs/api/api.g.yaml &&\
    mv /docs/api/api.g.yaml /docs/api/api.yaml

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "server/server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

RUN apt update&&\
    apt -y install curl

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "server.dll"]
