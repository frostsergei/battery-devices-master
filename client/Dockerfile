FROM node:21.7-alpine3.18 AS base
WORKDIR /app/client

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS client-build

WORKDIR /app
COPY ["docs/", "/docs"]
COPY ["clientgen.nswag", "./"]

RUN apt update &&\
    apt -y install nodejs &&\
    apt -y install npm &&\
    npm install -g --silent nswag@14.0.7

RUN nswag run clientgen.nswag

FROM base AS final

WORKDIR /app/client
COPY ["client/package.json", "client/package-lock.json", "./"]

RUN npm i

COPY --from=client-build /app/client .
COPY ["client/", "./"]

ENTRYPOINT ["npm", "start"]
