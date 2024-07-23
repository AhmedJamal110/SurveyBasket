using SurveyBasket.ApI.Contracts.Question;

namespace SurveyBasket.ApI.Services
{
    public interface IQuestionServices
    {
        Task<TResult<QuestionResponse>> AddQuestionAsync(int pollId , QuestionRequest request , CancellationToken cancellationToken = default);
        Task<TResult<IEnumerable<QuestionResponse>>> GetAvailableQuestionAsync(int pollId, string userId, CancellationToken cancellationToken);
        Task<TResult<IEnumerable<QuestionResponse>>> GetAllQuestions( int pollId , CancellationToken cancellationToken = default);
        Task<TResult<QuestionResponse>> GetQustionById(int pollId, int id, CancellationToken cancellationToken = default);

        Task<Result> ToggelStatue( int pollId ,int id, CancellationToken cancellationToken = default);

    }
}
