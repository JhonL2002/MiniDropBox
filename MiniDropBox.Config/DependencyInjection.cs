using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniDropBox.Application.Interfaces.FileServices;
using MiniDropBox.Application.Interfaces.UnitOfWork;
using MiniDropBox.Core.Repositories;
using MiniDropBox.Infraestructure.FileServices;
using MiniDropBox.Infraestructure.Repositories;
using MiniDropBox.Infraestructure.UnitOfWork;

namespace MiniDropBox.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            //Change this when you implement a real data persistence
            services.AddSingleton<IFolderRepository, FolderRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IUserRoleRepository, UserRoleRepository>();

            services.AddSingleton<UserRepository>();
            services.AddSingleton<UserRoleRepository>();

            services.AddSingleton<IRoleRepository, RoleRepository>();
            services.AddSingleton<IFileStorageService<IFormFile>, LocalFileStorageService>();
            services.AddSingleton<IFileRepository, FileRepository>();

            services.AddSingleton<IUnitOfWork, InMemoryUnitOfWork>();
            return services;
        }
    }
}
