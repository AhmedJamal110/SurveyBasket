namespace SurveyBasket.ApI.Contracts.Authentication
{
    public class ResendConfirmEmailValidators : AbstractValidator<ResendConfirmEmail>
    {
        public ResendConfirmEmailValidators()
        {
            RuleFor(x => x.Email).NotEmpty()
                                            .EmailAddress();
        }

    }
}
