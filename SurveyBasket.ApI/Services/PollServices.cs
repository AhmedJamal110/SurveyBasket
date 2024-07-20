using SurveyBasket.ApI.Contracts.Polls;
using SurveyBasket.ApI.DataDbContext;
using SurveyBasket.ApI.Errors;
using SurveyBasket.ApI.Models;

namespace SurveyBasket.ApI.Services
{
    public class PollServices : IPollServices
    {
        private readonly ApplicationDbContext _context;

        public PollServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TResult<PollResponse>> AddPoll(PollRequest pollRequest, CancellationToken cancellationToken = default)
        {
            var isExsiting = await _context.Polls.AnyAsync(x => x.Title == pollRequest.Title);


                    if (isExsiting)
                return Result.Faliure<PollResponse>(PollErrors.PollDeplucated);

            var polls = pollRequest.Adapt<Poll>();

            await _context.AddAsync(polls, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var pollResponse = polls.Adapt<PollResponse>();
            
            return  Result.Success(pollResponse) ;
        }

        public async Task<bool> DeletePoll(int id , CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id , cancellationToken);
            if (poll is null)
                return false;

            _context.Polls.Remove(poll );
          await   _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IReadOnlyList<Poll>> GetALlPolls(CancellationToken cancellationToken = default)
        =>  await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);


        public async Task<TResult<PollResponse>> GetPollById(int id , CancellationToken cancellationToken = default)
        {
            var poll =  await _context.Polls.FindAsync(id , cancellationToken);

           // var pollResponse = poll.Adapt<PollResponse>();
            return poll is not null 
                ? Result.Success(poll.Adapt<PollResponse>()) 
                : Result.Faliure<PollResponse>(PollErrors.PollNotFound);

        }

        public async Task<bool> ToggelStatue(int id, CancellationToken cancellationToken = default)
        {
           var pollInDb =  await _context.Polls.FindAsync(id, cancellationToken);
            if (pollInDb is null)
                return false;

            pollInDb.IsPublished = !pollInDb.IsPublished;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Result> UpdatePoll(int id , PollRequest pollRequest , CancellationToken cancellationToken = default)
        {

            var isExsiting = await _context.Polls.AnyAsync(x => x.Title == pollRequest.Title && x.Id != id);
            if (isExsiting)
                return Result.Failure(PollErrors.PollDeplucated);


             var pollInDb =  await _context.Polls.FindAsync(id , cancellationToken);

            if (pollInDb is null)
                return Result.Failure(PollErrors.PollNotFound);
               
            //pollInDb.Id = id;
            pollInDb.Title = pollRequest.Title;
            pollInDb.Summary = pollRequest.Summary;
            pollInDb.StartsAt = pollRequest.StartsAt;
            pollInDb.EndsAt = pollRequest.EndsAt;
           // pollInDb.IsPublished = poll.IsPublished;

            
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
