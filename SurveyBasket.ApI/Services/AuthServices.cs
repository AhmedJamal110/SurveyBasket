using Microsoft.AspNetCore.Http.HttpResults;
using SurveyBasket.ApI.Contracts.Authentication;
using SurveyBasket.ApI.JwtService;

namespace SurveyBasket.ApI.Services
{
    public class AuthServices : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtProvider _jwtProvider;

        public AuthServices(UserManager<AppUser> userManager , IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }
        public  async Task<AuthResponse?> GetTokenForUser(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return null;

            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
                return null;

            //genrateToken

            var (token , expireIn ) =   await _jwtProvider.CreateTokenAsync(user);

            var rseponse = new AuthResponse
            (user.Id, user.FirstName, user.LastName, user.Email , token , expireIn);

            return rseponse;
        }
    }
}
