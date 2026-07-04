using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BookFlix.Core.Helpers
{
    public static class HttpResponseExtension
    {
        public const string accessTokenCookieName = "bf-access-token";
        public const string refreshTokenCookieName = "bf-refresh-token";

        public static void SetTokenCookies(this HttpResponse response, string accessToken, string refreshToken, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");

            double accessExpiryMinutes = double.TryParse(jwtSettings["ExpireMinutes"], out var accMin) ? accMin : 30;
            double refreshExpiryDays = double.TryParse(jwtSettings["RefreshTokenExpireDays"], out var refDays) ? refDays : 7;

            var pathBase = response.HttpContext.Request.PathBase.Value;
            var appPath = string.IsNullOrEmpty(pathBase) ? "/" : pathBase;
            var authPath = (string.IsNullOrEmpty(pathBase) ? "" : pathBase) + "/api/auth";

            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = appPath,
                Expires = DateTimeOffset.UtcNow.AddMinutes(accessExpiryMinutes)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = authPath,
                Expires = DateTimeOffset.UtcNow.AddDays(refreshExpiryDays)
            };

            response.Cookies.Append(accessTokenCookieName, accessToken, accessTokenOptions);
            response.Cookies.Append(refreshTokenCookieName, refreshToken, refreshTokenOptions);
        }

        public static void DeleteTokenCookies(this HttpResponse response)
        {
            var pathBase = response.HttpContext.Request.PathBase.Value;
            var appPath = string.IsNullOrEmpty(pathBase) ? "/" : pathBase;
            var authPath = (string.IsNullOrEmpty(pathBase) ? "" : pathBase) + "/api/auth";

            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = appPath
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = authPath
            };
            response.Cookies.Delete(accessTokenCookieName, accessTokenOptions);
            response.Cookies.Delete(refreshTokenCookieName, refreshTokenOptions);
        }
    }
}