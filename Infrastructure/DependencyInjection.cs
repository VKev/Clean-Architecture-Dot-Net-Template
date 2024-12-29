using System;
using Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Infrastructure.Configs;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName ?? "";
            if (solutionDirectory != null)
            {
                DotNetEnv.Env.Load(Path.Combine(solutionDirectory, ".env"));
            }
            services.AddSingleton<EnvironmentConfig>();

            using var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<AutoScaffold>>();
            var config = serviceProvider.GetRequiredService<EnvironmentConfig>();

            if (Environment.GetEnvironmentVariable("IS_SCAFFOLDING") != "true")
            {
                var scaffold = new AutoScaffold(logger)
                    .Configure(
                        config.DatabaseHost,
                        config.DatabasePort,
                        config.DatabaseName,
                        config.DatabaseUser,
                        config.DatabasePassword,
                        config.DatabaseProvider);

                scaffold.UpdateAppSettings();

                var autoMigration = new AutoMigration(logger);

                string currentHash = SchemaComparer.GenerateDatabaseSchemaHash(
                    config.DatabaseHost,
                    config.DatabasePort,
                    config.DatabaseName,
                    config.DatabaseUser,
                    config.DatabasePassword
                );

                if (!SchemaComparer.TryGetStoredHash(out string storedHash) || currentHash != storedHash)
                {
                    autoMigration.GenerateMigration();
                    logger.LogInformation("Database schema has changed. Performing scaffolding...");
                    SchemaComparer.SaveHash(currentHash);
                    if(Environment.GetEnvironmentVariable("IS_MIGRATING") == "true")
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
