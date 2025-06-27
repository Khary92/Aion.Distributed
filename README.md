# Aion.Distributed

# How to use ef core migration

- change appsettings.json connection string to localhost
- Start database in docker
- run dotnet ef migrations add InitialCreate --project Core.Persistence
- change appsettings.json connection string back to postgres
- ????
- Profit