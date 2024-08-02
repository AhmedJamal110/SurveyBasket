namespace SurveyBasket.ApI.Contracts.Profile
{
    public class UpdateProfileRequestValidators : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidators()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(1, 100);


            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(1, 100);
        }
    }
}
