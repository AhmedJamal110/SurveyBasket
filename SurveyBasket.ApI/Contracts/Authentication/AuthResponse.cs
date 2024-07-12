namespace SurveyBasket.ApI.Contracts.Authentication
{
    public record AuthResponse
        (
             string Id,
            string FirstName,
            string LastName,
            string? Email ,
            string Token ,
            int ExpireIn,
            string RefreshToken,
            DateTime RefreshTokenExpiration

        );
}
