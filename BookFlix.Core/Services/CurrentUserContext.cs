using BookFlix.Core.Service_Interfaces;
using System.ComponentModel;
using System.Security.Claims;

namespace BookFlix.Core.Services
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public string GetClaim(string claimType)
        {
            
            return User.FindFirst(claimType)?.Value;
        }

        public T? GetClaim<T>(string claimType) where T : struct
        {
            var value = GetClaim(claimType);
            if (string.IsNullOrEmpty(value)) return null;

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T?)converter.ConvertFromString(value);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<T> GetClaims<T>(string claimType)
        {
            var claims = User?.FindAll(claimType);
            if (claims == null) return [];

            var converter = TypeDescriptor.GetConverter(typeof(T));
            var result = new List<T>();

            foreach (var claim in claims)
            {
                try
                {
                    var converted = (T)converter.ConvertFromString(claim.Value);
                    if (converted != null)
                    {
                        result.Add(converted);
                    }
                }
                catch
                {
                }
            }

            return result;
        }

        public bool IsAdmin => HasRole("Admin");

        public Guid UserID
        {
            get
            {
                var userIdClaim = GetClaim(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(userIdClaim, out var userId))
                {
                    return userId;
                }
                throw new InvalidOperationException("User ID claim is missing or invalid.");
            }
        }

        public bool HasRole(string role) =>
            User?.IsInRole(role) ?? false;

        public bool HasClaim(string claimType, string value) =>
            User?.HasClaim(claimType, value) ?? false;
    }
}
