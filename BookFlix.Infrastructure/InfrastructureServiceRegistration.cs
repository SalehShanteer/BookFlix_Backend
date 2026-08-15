using Amazon.Runtime;
using Amazon.S3;
using BookFlix.Core.Abstractions;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using BookFlix.Infrastructure.Repositories;
using BookFlix.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookFlix.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>((options) =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                AppDbContextConfiguration.Configure((DbContextOptionsBuilder<AppDbContext>)options, connectionString);
            });

            AddS3Service(services, configuration);

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserLogRepository, UserLogRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUploadedFileRepository, UploadedFileRepository>();
            services.AddScoped<IFileStorageService, S3FileStorageService>();

            return services;
        }

        private static void AddS3Service(IServiceCollection services, IConfiguration configuration)
        {
            var s3Section = configuration.GetSection("S3");
            var s3Config = new AmazonS3Config
            {
                ServiceURL = s3Section["ServiceUrl"],
                ForcePathStyle = s3Section.GetValue("ForcePathStyle", true)
            };
            var credentials = new BasicAWSCredentials(s3Section["AccessKey"], s3Section["SecretKey"]);

            services.AddSingleton<IAmazonS3>(new AmazonS3Client(credentials, s3Config));
        }
    }
}
