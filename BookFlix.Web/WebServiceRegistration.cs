using BookFlix.Web.Mapper_Interfaces;
using BookFlix.Web.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BookFlix.Web
{
    public static class WebServiceRegistration
    {
        private static byte[] GetJwtKey(IConfiguration configuration)
        {
            var jwtKey = configuration["Jwt:Key"];

            if (jwtKey is not null)
            {
                return Encoding.UTF8.GetBytes(jwtKey);
            }

            throw new Exception("JWTNotFound");
        }

        private static void JwtConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            byte[] jwtKeyBytes = GetJwtKey(configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes)
                };
            });
        }

        private static void SwaggerConfiguration(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "BookFlix API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    // Updated description so you don't accidentally type "Bearer" twice in the UI
                    Description = "Enter your JWT token below. Swagger automatically adds the 'Bearer' prefix."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            JwtConfiguration(services, configuration);

            // Add CORS service
            services.AddCors(options =>
            {
                options.AddPolicy("BookFlixApiCorsPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:7217", "https://localhost/BookFlexWebApi", "http://localhost:5215", "http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            services.AddLogging(logging =>
            {
                logging.AddConsole();
            });

            services.AddAuthorization();
            services.AddControllers();
            SwaggerConfiguration(services);

            services.AddScoped<IBookMapper, BookMapper>();
            services.AddScoped<IUserLogMapper, UserLogMapper>();
            services.AddScoped<IUserMapper, UserMapper>();

            return services;
        }
    }
}