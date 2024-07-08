namespace SurveyBasket.ApI.Contracts.Authentication
{
    public class AuthRequestValidators : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidators()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
                

        }
    }
}
