namespace SurveyBasket.ApI.Contracts.Authentication
{
    public class RefreshTokenRequestValidators : AbstractValidator<RefrshTokenRequest>
    {
        public RefreshTokenRequestValidators()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
