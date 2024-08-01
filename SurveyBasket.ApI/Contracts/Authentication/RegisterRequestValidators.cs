namespace SurveyBasket.ApI.Contracts.Authentication
{
    public class RegisterRequestValidators : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidators()
        {
            RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .Length(3, 100);

            RuleFor(x => x.LastName)
                    .NotEmpty()
                    .Length(3, 100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();


            RuleFor(x => x.Password)
                .NotEmpty();
               

        }
    }
}
