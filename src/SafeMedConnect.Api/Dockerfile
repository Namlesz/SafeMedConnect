FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SafeMedConnect.Api/SafeMedConnect.Api.csproj", "src/SafeMedConnect.Api/"]
RUN dotnet restore "src/SafeMedConnect.Api/SafeMedConnect.Api.csproj"
COPY . .
WORKDIR "/src/src/SafeMedConnect.Api"
RUN dotnet build "SafeMedConnect.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SafeMedConnect.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SafeMedConnect.Api.dll"]
