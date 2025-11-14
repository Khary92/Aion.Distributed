# STAGE: Base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

ARG APP_UID=1000
ARG APP_GID=1000

RUN groupadd -g $APP_GID appgroup \
 && adduser --disabled-password --gecos "" --uid $APP_UID --gid $APP_GID appuser

RUN mkdir -p /app/DataProtection-Keys \
 && chown -R $APP_UID:$APP_GID /app/DataProtection-Keys

USER appuser
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
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.Admin.Web.dll"]