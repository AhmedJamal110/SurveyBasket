using SurveyBasket.ApI.Contracts.Authentication;

namespace SurveyBasket.ApI.Services
{
    public interface IAuthService
    {

        Task<AuthResponse?> GetTokenForUser(string email, string password, CancellationToken cancellationToken = default);

        Task<AuthResponse?> GetRefreshToken(string token, string refreshToken, CancellationToken cancellationToken);
        Task<bool> RevokeRefreshToken(string token, string refreshToken, CancellationToken cancellationToken);


    }
}
