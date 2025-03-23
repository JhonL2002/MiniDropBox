using Microsoft.Extensions.DependencyInjection;
using MiniDropBox.Core.Repositories;
using MiniDropBox.Infraestructure.Repositories;

namespace MiniDropBox.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            //Change this when you implement a real data persistence
            services.AddSingleton<IFolderRepository, FolderRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();

            return services;
        }
    }
}
