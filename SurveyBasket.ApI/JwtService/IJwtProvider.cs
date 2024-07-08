namespace SurveyBasket.ApI.JwtService
{
    public interface IJwtProvider
    {

        Task<(string token , int expireIn )> CreateTokenAsync(AppUser user);
    }
}
