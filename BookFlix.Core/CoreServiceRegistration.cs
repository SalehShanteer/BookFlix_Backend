using BookFlix.Core.Decorators;
using BookFlix.Core.Models;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<BookService>();
            services.AddScoped<IBookService>(sp =>
                new LoggingBookService(
                    sp.GetRequiredService<BookService>(),
                    sp.GetRequiredService<ILogger<LoggingBookService>>()));

            services.AddScoped<UserService>();
            services.AddScoped<IUserService>(sp =>
                new LoggingUserService(
                    sp.GetRequiredService<UserService>(),
                    sp.GetRequiredService<ILogger<LoggingUserService>>()));

            services.AddScoped<AuthService>();
            services.AddScoped<IAuthService>(sp =>
                new LoggingAuthService(
                    sp.GetRequiredService<AuthService>(),
                    sp.GetRequiredService<ILogger<LoggingAuthService>>()));

            services.AddScoped<IFileService<Book>, BookFileService>();
            services.AddScoped<IFileService<User>, UserFileService>();
            services.AddScoped<IUserLogService, UserLogService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
