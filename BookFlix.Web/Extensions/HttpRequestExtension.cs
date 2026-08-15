using Microsoft.AspNetCore.Http;

namespace BookFlix.Web.Extensions
{
    public static class HttpRequestExtension
    {
        public const string RefreshTokenCookieName = "bf-refresh-token";

        public static string GetRefreshTokenFromCookies(this HttpRequest request)
        {
            if (request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
            {
                return refreshToken;
            }
            return null;
        }
    }
}
