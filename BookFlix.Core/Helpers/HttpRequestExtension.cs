namespace BookFlix.Core.Helpers
{
    public static class HttpRequestExtension
    {
        public const string refreshTokenCookieName = "bf-refresh-token";

        public static string GetRefreshTokenFromCookies(this HttpRequest request)
        {
            if (request.Cookies.TryGetValue(refreshTokenCookieName, out var refreshToken))
            {
                return refreshToken;
            }
            return null;
        }
    }
}
