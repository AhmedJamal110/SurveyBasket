namespace SurveyBasket.ApI.Contracts.Profile
{
    public class ChangePasswordRequestValidators: AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidators()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();


            RuleFor(x => x.NewPassword)
                    .NotEmpty()
                    .NotEqual(x => x.CurrentPassword)
                    .WithMessage("New Password Cant Equal Current Password");
        }

    }
}
