FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080  

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Core.Boot/Core.Boot.csproj", "Core.Boot/"]
COPY ["Core.Domain/Core.Domain.csproj", "Core.Domain/"]
COPY ["Core.Persistence/Core.Persistence.csproj", "Core.Persistence/"]
COPY ["Core.Server/Core.Server.csproj", "Core.Server/"]
COPY ["Proto.Shared/Proto.Shared.csproj", "Proto.Shared/"]
RUN install 
RUN dotnet restore "Core.Boot/Core.Boot.csproj"
COPY . .
WORKDIR "/src/Core.Boot"
RUN dotnet build "./Core.Boot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Core.Boot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.Boot.dll"]
