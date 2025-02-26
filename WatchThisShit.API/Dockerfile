FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WatchThisShit.API/WatchThisShit.API.csproj", "WatchThisShit.API/"]
COPY ["WatchThisShit.Application/WatchThisShit.Application.csproj", "WatchThisShit.Application/"]
COPY ["WatchThisShit.Contracts/WatchThisShit.Contracts.csproj", "WatchThisShit.Contracts/"]
RUN dotnet restore "WatchThisShit.API/WatchThisShit.API.csproj"
COPY . .
WORKDIR "/src/WatchThisShit.API"
RUN dotnet build "WatchThisShit.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WatchThisShit.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WatchThisShit.API.dll"]
