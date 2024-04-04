using Microsoft.EntityFrameworkCore;
using Persistence;

namespace MediConnect.API;

public static class MigrationConfiguration
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}