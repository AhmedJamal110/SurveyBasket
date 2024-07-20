using SurveyBasket.ApI.Contracts.Answer;

namespace SurveyBasket.ApI.Contracts.Question
{
    public record QuestionResponse
            (
            int Id,
            string Content,
            IEnumerable<AnswerResponse> Answers
        );
}
