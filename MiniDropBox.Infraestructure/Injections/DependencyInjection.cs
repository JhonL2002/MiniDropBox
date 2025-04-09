using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.FileServices;
using MiniDropBox.Application.Interfaces.Helpers;
using MiniDropBox.Application.Interfaces.UnitOfWork;
using MiniDropBox.Core.Repositories;
using MiniDropBox.Infraestructure.FileServices.CloudServices;
using MiniDropBox.Infraestructure.Helpers;
using MiniDropBox.Infraestructure.Repositories;
using MiniDropBox.Infraestructure.UnitOfWorkService;

namespace MiniDropBox.Infraestructure.Injections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordService, PasswordService>();

            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IFileStorageService<IFormFile>, BlobStorageService>();
            services.AddScoped<IFileRepository, FileRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
