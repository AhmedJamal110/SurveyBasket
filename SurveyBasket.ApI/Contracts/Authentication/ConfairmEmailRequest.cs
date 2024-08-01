namespace SurveyBasket.ApI.Contracts.Authentication
{
    public record ConfairmEmailRequest
    (
        string UserId,
        string Code
        
     );
}
