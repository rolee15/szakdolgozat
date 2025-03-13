FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY [ "src/KanjiKa.Core/*.csproj", "KanjiKa.Core/" ]
COPY [ "src/KanjiKa.Data/*.csproj", "KanjiKa.Data/" ]
COPY [ "src/KanjiKa.Api/*.csproj", "KanjiKa.Api/" ]
RUN dotnet restore ./KanjiKa.Api/KanjiKa.Api.csproj
COPY src .
WORKDIR /src/KanjiKa.Api
RUN dotnet build KanjiKa.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KanjiKa.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish -p:UseAppHost=false

FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "KanjiKa.Api.dll" ]