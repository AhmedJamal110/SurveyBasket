namespace SurveyBasket.ApI.Contracts.Profile
{
    public record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword
        );
}
