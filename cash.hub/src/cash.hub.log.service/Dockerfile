FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/cash.hub.log.service/cash.hub.log.service.csproj", "src/cash.hub.log.service/"]
RUN dotnet restore "src/cash.hub.log.service/cash.hub.log.service.csproj"
COPY . .
WORKDIR "/src/src/cash.hub.log.service"
RUN dotnet build "cash.hub.log.service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "cash.hub.log.service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cash.hub.log.service.dll"]
