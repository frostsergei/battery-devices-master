FROM node:20-alpine3.18 AS base
WORKDIR /app/client

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.18 AS client-build
RUN apk add --upgrade --no-cache \
        nodejs \
        npm &&\
    npm install -g --silent nswag@14.0.7

# Generate typescript clients
WORKDIR /app
COPY --from=battery-devices-master-server /docs/api /docs/api
COPY ["docs/settings/clientgen.nswag", "./"]
RUN nswag run clientgen.nswag

ARG SERVER_IP=http://localhost
ENV SERVER_IP=$SERVER_IP

# Replace API_BASE_URL with server url
WORKDIR /app/client/src
COPY ["client/src/env.template.ts", "./"]
RUN apk add --upgrade --no-cache \
        envsubst
RUN envsubst < env.template.ts > env.ts

FROM base AS final

WORKDIR /app/client
COPY ["client/package.json", "client/package-lock.json", "./"]

RUN npm i

COPY --from=client-build /app/client .
COPY ["client/", "./"]

ENTRYPOINT ["npm", "start"]
