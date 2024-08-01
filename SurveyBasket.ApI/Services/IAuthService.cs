using SurveyBasket.ApI.Contracts.Authentication;

namespace SurveyBasket.ApI.Services
{
    public interface IAuthService
    {

        Task<TResult<AuthResponse>> GetTokenForUser(string email, string password, CancellationToken cancellationToken = default);
        //Task<OneOf<AuthResponse , Error>> GetTokenForUser(string email, string password, CancellationToken cancellationToken = default);

        Task<AuthResponse?> GetRefreshToken(string token, string refreshToken, CancellationToken cancellationToken);
        Task<bool> RevokeRefreshToken(string token, string refreshToken, CancellationToken cancellationToken);

        Task<Result> ResgisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

        Task<Result> ConfairmEmailAsync(ConfairmEmailRequest request , CancellationToken cancellationToken = default);


        Task<Result> ResendConfairmEmailAsync(ResendConfirmEmail request, CancellationToken cancellationToken = default);

    }
}
