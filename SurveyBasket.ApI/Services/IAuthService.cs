using SurveyBasket.ApI.Contracts.Authentication;

namespace SurveyBasket.ApI.Services
{
    public interface IAuthService
    {

        Task<AuthResponse?> GetTokenForUser(string email, string password, CancellationToken cancellationToken = default);
     
    }
}
