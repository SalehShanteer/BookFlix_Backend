using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services;
using BookFlix.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BookFlix.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IFileService<Book>, BookFileService>();
            services.AddScoped<IFileService<User>, UserFileService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserLogService, UserLogService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICurrentUserContext, CurrentUserContext>();

            return services;
        }
    }
}
