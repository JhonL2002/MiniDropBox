using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiniDropBox.Infraestructure.Data
{
    public static class DatabaseProvider
    {
        public static IServiceCollection AddDatabaseProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                string connectionString;
                try
                {
                    connectionString = configuration["SQL_CONNECTION_STRING"]!;
                    options.UseSqlServer(connectionString, sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("MiniDropBox.Infraestructure");
                    });
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
            return services;
        }
    }
}
