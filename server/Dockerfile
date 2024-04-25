FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.18 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.18 AS build
RUN apk add --upgrade --no-cache \
        nodejs \
        npm &&\
    npm install -g --silent nswag@14.0.7

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["server/server.csproj", "server/"]

RUN dotnet restore "server/server.csproj"

COPY ["server/", "server/"]
COPY ["docs/settings/apigen.nswag", "BatteryDevicesMaster.sln", "./"]

RUN dotnet build "server/server.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN nswag run apigen.nswag &&\
    sed 's/\\n[[:space:]]*"/"/g' /docs/api/api.yaml > /docs/api/api.g.yaml &&\
    mv /docs/api/api.g.yaml /docs/api/api.yaml

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "server/server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

RUN apk add --upgrade --no-cache \
        curl

WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /docs /docs

ENTRYPOINT ["dotnet", "server.dll"]
