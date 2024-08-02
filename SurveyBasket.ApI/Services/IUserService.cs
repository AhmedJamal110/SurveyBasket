using SurveyBasket.ApI.Contracts.Profile;

namespace SurveyBasket.ApI.Services
{
    public interface IUserService
    {
        Task<TResult<UserResponse>> GetProfileAsync(string userId );

        Task<Result> UpdateProfileAsync(string userId , UpdateProfileRequest request);
        Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    }
}
