namespace SurveyBasket.ApI.Contracts.Question
{
    public class QuestionRequestValidators : AbstractValidator<QuestionRequest>
    {
        public QuestionRequestValidators()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .Length(3, 1000);

            RuleFor(x => x.Answers)
                .Must( A => A.Count() > 1)
                .WithMessage("Answers must be more than 1");

            RuleFor(x => x.Answers)
                .Must(A => A.Distinct().Count() == A.Count())
                .WithMessage("you cant add duplicated answers for the same question");
        }
    }
}
