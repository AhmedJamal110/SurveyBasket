namespace SurveyBasket.ApI.Contracts.Polls
{
    public class PollRequestValidators : AbstractValidator<PollRequest>
    {
        public PollRequestValidators()
        {
            RuleFor(P => P.Title)
                    .NotEmpty()
                    .Length(3, 100);

            RuleFor(P => P.Summary)
                .NotEmpty()
                .Length(3, 1500);

            RuleFor(P => P.StartsAt)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(P => P.EndsAt)
                .NotEmpty();

            RuleFor(P => P)
                .Must(HasValidDate)
                .WithName(nameof(PollRequest.EndsAt))
                .WithMessage("{PropertyName} Must be geater than or equal StartsAt ");

        }

        private bool HasValidDate(PollRequest request)
        {
            return request.EndsAt >= request.StartsAt;
        }

    }
}
