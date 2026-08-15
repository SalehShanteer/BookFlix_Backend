using BookFlix.Core.Service_Interfaces;
using BookFlix.Web.Extensions;
using BookFlix.Web.Mapper_Interfaces;
using BookFlix.Web.Mappers;
using BookFlix.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

namespace BookFlix.Web
{
    public static class WebServiceRegistration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserContext, CurrentUserContext>();

            AddDataProtection(services, configuration);
            AddRateLimiting(services);
            AddAuthentication(services, configuration);
            AddCors(services);
            AddForwardedHeaders(services);

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

        private static byte[] GetJwtKey(IConfiguration configuration)
        {
            var jwtKey = configuration["Jwt:Key"];

            if (jwtKey is not null)
            {
                return Encoding.UTF8.GetBytes(jwtKey);
            }

            throw new Exception("JWTNotFound");
        }

        private static void AddDataProtection(IServiceCollection services, IConfiguration configuration)
        {
            var dataProtectionBuilder = services.AddDataProtection()
              .SetApplicationName("BookFlix");

            var keysPath = configuration["DataProtection:KeysPath"];
            if (!string.IsNullOrWhiteSpace(keysPath))
            {
                dataProtectionBuilder.PersistKeysToFileSystem(new DirectoryInfo(keysPath));
            }
        }
        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
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

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue(HttpResponseExtension.AccessTokenCookieName, out var token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
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

        private static void AddForwardedHeaders(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
                options.ForwardLimit = 1;
            });
        }

        private static void AddRateLimiting(IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        message = "Too many attempts. Please try again later."
                    }, cancellationToken);
                };

                options.AddPolicy("AuthLimiter", httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ip,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });
            });

        }

        private static void AddCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("BookFlixApiCorsPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:7217", "https://localhost/BookFlexWebApi", "http://localhost:5215", "https://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
        }
    }
}