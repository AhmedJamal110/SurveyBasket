using SurveyBasket.ApI.Contracts.Authentication;

namespace SurveyBasket.ApI.Services
{
    public interface IAuthService
    {

        Task<TResult<AuthResponse>> GetTokenForUser(string email, string password, CancellationToken cancellationToken = default);
        //Task<OneOf<AuthResponse , Error>> GetTokenForUser(string email, string password, CancellationToken cancellationToken = default);

        Task<AuthResponse?> GetRefreshToken(string token, string refreshToken, CancellationToken cancellationToken);
        Task<bool> RevokeRefreshToken(string token, string refreshToken, CancellationToken cancellationToken);


    }
}
