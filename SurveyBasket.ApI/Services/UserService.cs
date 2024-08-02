using SurveyBasket.ApI.Contracts.Profile;
using SurveyBasket.ApI.Errors;

namespace SurveyBasket.ApI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService( UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<TResult<UserResponse>> GetProfileAsync(string userId)
        {
           var user =  await _userManager.Users.Where(x => x.Id == userId)
                                          .ProjectToType<UserResponse>()
                                          .SingleAsync();


            return Result.Success(user);
        }

        public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            var user = await _userManager.Users
                                                        .Where(x => x.Id == userId)
                                                        .ExecuteUpdateAsync(setters =>
                                                            setters
                                                                    .SetProperty(x => x.FirstName, request.FirstName)
                                                                    .SetProperty(x => x.LastName, request.LastName)
                                                           );
          
            return Result.Success();
        }


        public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure( new Error(error.Code, error.Description));

        }





    }
}
