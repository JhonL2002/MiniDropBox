﻿using Microsoft.Extensions.DependencyInjection;
using MiniDropBox.Application.Implementations;
using MiniDropBox.Application.Implementations.Trees;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.Trees;

namespace MiniDropBox.Application.Injections
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFolderService, FolderService>();
            services.AddScoped<IFolderTreeBuilderService, FolderTreeBuilderService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            return services;
        }
    }
}
