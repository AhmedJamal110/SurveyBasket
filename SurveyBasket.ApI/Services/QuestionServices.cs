using SurveyBasket.ApI.Contracts.Answer;
using SurveyBasket.ApI.Contracts.Polls;
using SurveyBasket.ApI.Contracts.Question;
using SurveyBasket.ApI.Errors;
using System.Collections.Generic;

namespace SurveyBasket.ApI.Services
{
    public class QuestionServices : IQuestionServices
    {
        private readonly ApplicationDbContext _context;

        public QuestionServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TResult<QuestionResponse>> AddQuestionAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
        {

            // check poll id
            var pollExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);
            if (!pollExists)
                return Result.Faliure<QuestionResponse>(PollErrors.PollNotFound);

            var questionExists = await _context.Questions.AnyAsync
                (x => x.Content ==  request .Content && x.PollId == pollId, cancellationToken: cancellationToken);
            if (questionExists)
                return Result.Faliure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);


            var questionInDb = request.Adapt<Question>();
            questionInDb.PollId = pollId;

           // request.Answers.ForEach(answer => questionInDb.Answers.Add(new Answer { Content  = answer}));

            
           await  _context.Questions.AddAsync(questionInDb, cancellationToken);
           await  _context.SaveChangesAsync();

            var response = questionInDb.Adapt<QuestionResponse>();
            return Result.Success(response);
        }

        public async Task<TResult<IEnumerable<QuestionResponse>>> GetAllQuestions(int pollId, CancellationToken cancellationToken =default)
        {
            var pollExists = await _context.Polls.AnyAsync(x => x.Id == pollId);
            if (!pollExists)
                return Result.Faliure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);

                    // without using Projection
            //var questions =await _context.Questions
            //                                        .Where(x => x.PollId == pollId)
            //                                        .Include(x => x.Answers)
            //                                        .Select( q =>  new QuestionResponse(q.Id, q.Content , q.Answers
            //                                        .Select( a => new AnswerResponse(a.Id  , a.Content))))
            //                                        .AsNoTracking()
            //                                            .ToListAsync();

                    // using projection (mapster)
            var questions = await _context.Questions
                                                            .Where(x => x.PollId == pollId)
                                                             .Include(x => x.Answers)
                                                             .ProjectToType<QuestionResponse>()
                                                             .AsNoTracking()
                                                             .ToListAsync(cancellationToken: cancellationToken);

            return Result.Success<IEnumerable<QuestionResponse>>(questions);
        }

        public async Task<TResult<QuestionResponse>> GetQustionById(int pollId, int id, CancellationToken cancellationToken = default)
        {
            //var questionExsits = await _context.Polls.AnyAsync(x => x.Id == pollId);
            //if (!questionExsits)
            //    return Result.Faliure<QuestionResponse>(PollErrors.PollNotFound);

            //var question = await _context.Questions.FindAsync(id);
           

            var question = await _context.Questions
                                                          .Where(x => x.Id == id && x.PollId == pollId)
                                                          .Include(x => x.Answers)
                                                          .ProjectToType<QuestionResponse>()
                                                          .AsNoTracking()
                                                          .SingleOrDefaultAsync(cancellationToken);

            if (question is null)
                return Result.Faliure<QuestionResponse>(QuestionErrors.QuestionNotFound);

            return Result.Success(question);

        }

        public async Task<Result> ToggelStatue(int pollId , int id, CancellationToken cancellationToken = default)
        {
            var question = await _context.Questions
                                                            .SingleOrDefaultAsync(x => x.Id == id && x.PollId == pollId);
            if (question is null)
                return Result.Failure(QuestionErrors.QuestionNotFound);

            question.IsActive = !question.IsActive;

          await  _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
