using System;
using Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            using var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<AutoScaffold>>();
            if (Environment.GetEnvironmentVariable("IS_SCAFFOLDING") != "true")
            {
                var scaffold = new AutoScaffold(logger)
                    .Configure(
                        "test",
                        28996,
                        "test",
                        "test",
                        "test",
                        "postgres")
                    .SetPaths("Entities", "Context", "MyDbContext", "Build/obj", "../Infrastructure/Infrastructure.csproj");

                scaffold.UpdateAppSettings();

                string currentHash = SchemaComparer.GenerateDatabaseSchemaHash(
                    "test",
                    28996,
                    "test",
                    "test",
                    "test"
                );

                if (!SchemaComparer.TryGetStoredHash(out string storedHash) || currentHash != storedHash)
                {
                    logger.LogInformation("Database schema has changed. Performing scaffolding...");
                    SchemaComparer.SaveHash(currentHash);
                    scaffold.Run();
                }
                else
                {
                    logger.LogInformation("Database schema is up to date. No scaffolding required.");
                }
            }
            return services;
        }
    }
}
