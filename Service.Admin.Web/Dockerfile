# STAGE: Base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

ARG APP_UID=1000
ENV APP_UID=$APP_UID

# Benutzer anlegen
RUN adduser --disabled-password --gecos "" --uid $APP_UID appuser

# Verzeichnis vorbereiten mit korrekten Rechten
RUN mkdir -p /app/DataProtection-Keys && chown -R $APP_UID /app/DataProtection-Keys
EXPOSE 5000
WORKDIR /app

# STAGE: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Service.Admin.Web/Service.Admin.Web.csproj", "Service.Admin.Web/"]
COPY ["Service.Admin.Proto/Service.Admin.Proto.csproj", "Service.Admin.Proto/"]
COPY ["Service.Proto.Shared/Service.Proto.Shared.csproj", "Service.Proto.Shared/"]
RUN dotnet restore "Service.Admin.Web/Service.Admin.Web.csproj"
COPY . .
WORKDIR "/src/Service.Admin.Web"
RUN dotnet build "./Service.Admin.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# STAGE: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Service.Admin.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# STAGE: Final
FROM base AS final
WORKDIR /app

# Copy app binaries
COPY --from=publish /app/publish .

# Zertifikat (CRT) einspielen und trusten
#USER root
#COPY Certificates/server.crt /usr/local/share/ca-certificates/server.crt
#RUN update-ca-certificates

# Zurück zu normalem User
#USER appuser

ENTRYPOINT ["dotnet", "Service.Admin.Web.dll"]
