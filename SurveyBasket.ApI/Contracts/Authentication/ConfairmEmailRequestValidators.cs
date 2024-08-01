namespace SurveyBasket.ApI.Contracts.Authentication
{
    public class ConfairmEmailRequestValidators : AbstractValidator<ConfairmEmailRequest>
    {
        public ConfairmEmailRequestValidators()
        {
            RuleFor(x => x.UserId).NotEmpty();
            
            
            RuleFor(x => x.Code).NotEmpty();

        }
    }
}
