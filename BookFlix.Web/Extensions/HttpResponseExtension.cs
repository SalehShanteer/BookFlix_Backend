namespace BookFlix.Web.Extensions
{
    public static class HttpResponseExtension
    {
        public const string AccessTokenCookieName = "bf-access-token";
        public const string RefreshTokenCookieName = "bf-refresh-token";

        public static void SetTokenCookies(this HttpResponse response, string accessToken, string refreshToken, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");

            double accessExpiryMinutes = double.TryParse(jwtSettings["ExpireMinutes"], out var accMin) ? accMin : 30;
            double refreshExpiryDays = double.TryParse(jwtSettings["RefreshTokenExpireDays"], out var refDays) ? refDays : 7;
            var isHttps = response.HttpContext.Request.IsHttps;

            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = isHttps,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes(accessExpiryMinutes)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = isHttps,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(refreshExpiryDays)
            };

            response.Cookies.Append(AccessTokenCookieName, accessToken, accessTokenOptions);
            response.Cookies.Append(RefreshTokenCookieName, refreshToken, refreshTokenOptions);
        }

        public static void DeleteTokenCookies(this HttpResponse response)
        {
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            };
            response.Cookies.Delete(AccessTokenCookieName, accessTokenOptions);
            response.Cookies.Delete(RefreshTokenCookieName, refreshTokenOptions);
        }
    }
}
