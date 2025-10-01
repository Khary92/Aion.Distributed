using Global.Settings.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace Core.Persistence.DbContext;

public class DesignTimeDbContextFactory(IOptions<DatabaseSettings> databaseConfiguration)
    : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(databaseConfiguration.Value.ConnectionString);

        return new AppDbContext(optionsBuilder.Options, databaseConfiguration);
    }
}