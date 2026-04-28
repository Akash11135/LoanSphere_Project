using LoanSphere_Frontend.Models;

namespace LoanSphere_Frontend.Services
{
    public class CurrentUserService
    {
        private const string UserIdKey = "CurrentUser.UserId";
        private const string FullNameKey = "CurrentUser.FullName";
        private const string EmailKey = "CurrentUser.Email";
        private const string RoleKey = "CurrentUser.Role";
        private const string TokenKey = "CurrentUser.Token";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated => GetCurrentUser() != null;

        public AuthenticatedUser? GetCurrentUser()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                return null;
            }

            var userId = session.GetString(UserIdKey);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            return new AuthenticatedUser
            {
                UserId = userId,
                FullName = session.GetString(FullNameKey) ?? string.Empty,
                Email = session.GetString(EmailKey) ?? string.Empty,
                Role = session.GetString(RoleKey) ?? string.Empty,
                Token = session.GetString(TokenKey) ?? string.Empty
            };
        }

        public void SignIn(AuthResponseViewModel authResponse)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                return;
            }

            session.SetString(UserIdKey, authResponse.UserId);
            session.SetString(FullNameKey, authResponse.FullName ?? string.Empty);
            session.SetString(EmailKey, authResponse.Email ?? string.Empty);
            session.SetString(RoleKey, authResponse.Role ?? string.Empty);
            session.SetString(TokenKey, authResponse.Token ?? string.Empty);
        }

        public void SignOut()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }
    }
}
