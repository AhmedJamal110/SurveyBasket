namespace SurveyBasket.ApI.Contracts.Authentication
{
    public record RefrshTokenRequest
        (
            string Token ,
            string RefreshToken
        
        );
}
