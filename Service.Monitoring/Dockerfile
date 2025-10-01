# --- Base Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Service.Monitoring/Service.Monitoring.csproj", "Service.Monitoring/"]
COPY ["Service.Monitoring.Shared/Service.Monitoring.Shared.csproj", "Service.Monitoring.Shared/"]
COPY ["Service.Admin.Proto/Service.Admin.Proto.csproj", "Service.Admin.Proto/"]

RUN dotnet restore "Service.Monitoring/Service.Monitoring.csproj"
COPY . .
WORKDIR "/src/Service.Monitoring"
RUN dotnet build "./Service.Monitoring.csproj" -c $BUILD_CONFIGURATION -o /app/build

# --- Publish Stage ---
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Service.Monitoring.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --- Final Stage ---
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

# Root-Zertifikat hinzufügen (im CRT-Format, nicht PFX!)
#USER root
#COPY Certificates/server.crt /usr/local/share/ca-certificates/server.crt
#RUN update-ca-certificates

# PFX nur als Volume mounten, nicht ins Image packen
# -> docker-compose.yml: mount ./Certificates/server.pfx:/app/server.pfx

ENTRYPOINT ["dotnet", "Service.Monitoring.dll"]
