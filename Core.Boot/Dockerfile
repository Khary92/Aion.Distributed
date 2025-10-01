FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Core.Boot/Core.Boot.csproj", "Core.Boot/"]
COPY ["Core.Domain/Core.Domain.csproj", "Core.Domain/"]
COPY ["Core.Persistence/Core.Persistence.csproj", "Core.Persistence/"]
COPY ["Core.Tracing/Core.Tracing.csproj", "Core.Tracing/"]
COPY ["Service.Monitoring.Shared/Service.Monitoring.Shared.csproj", "Service.Monitoring.Shared/"]
COPY ["Core.Server/Core.Server.csproj", "Core.Server/"]
COPY ["Client.Proto/Client.Proto.csproj", "Client.Proto/"]
COPY ["Service.Proto.Shared/Service.Proto.Shared.csproj", "Service.Proto.Shared/"]

COPY ["Service.Proto.Shared/", "Service.Proto.Shared/"]
COPY ["Client.Proto/", "Client.Proto/"]

RUN dotnet restore "Core.Boot/Core.Boot.csproj"

COPY . .

WORKDIR "/src/Core.Boot"
RUN dotnet build "./Core.Boot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Core.Boot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# App-Binaries rein
COPY --from=publish /app/publish .

# Zertifikat als CRT für Truststore hinzufügen
#USER root
#COPY Certificates/server.crt /usr/local/share/ca-certificates/server.crt
#RUN update-ca-certificates

# optional wieder als non-root laufen lassen
# USER appuser

ENTRYPOINT ["dotnet", "Core.Boot.dll"]
