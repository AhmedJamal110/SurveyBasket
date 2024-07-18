using Microsoft.AspNetCore.Http.HttpResults;
using SurveyBasket.ApI.Contracts.Authentication;
using SurveyBasket.ApI.Errors;
using SurveyBasket.ApI.JwtService;
using System.Security.Cryptography;

namespace SurveyBasket.ApI.Services
{
    public class AuthServices : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly int _refreshTokenExpirationDays = 14;

        public AuthServices(UserManager<AppUser> userManager, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }
        public async Task<TResult<AuthResponse>>  GetTokenForUser(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Faliure<AuthResponse>(UserError.InvalidCredential);
                //return UserError.InvalidCredential;
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
                return Result.Faliure<AuthResponse>(UserError.InvalidCredential);
               // return UserError.InvalidCredential;

            var (token, expireIn) = await _jwtProvider.CreateTokenAsync(user);

            var refeshToken = GenerateRefreshToken();
            var rereshTokenExpire = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

            var rseponse = new AuthResponse
            (user.Id, user.FirstName, user.LastName, user.Email, token, expireIn, refeshToken, rereshTokenExpire);

            user.RefreshTokens.Add(new RefreshTokens
            {
                Token = refeshToken,
                ExpiresOn = rereshTokenExpire
            });

            await _userManager.UpdateAsync(user);

            return Result.Success(rseponse);

           // return rseponse;
        }

        public async Task<AuthResponse?> GetRefreshToken(string token, string refreshToken, CancellationToken cancellationToken)
        {
            var userId = _jwtProvider.ValidatToken(token);
            if (userId is null)
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return null;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null)
                return null;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            var (newToken, expireIn) = await _jwtProvider.CreateTokenAsync(user);
            var newrefreshToken = GenerateRefreshToken();
            var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

            user.RefreshTokens.Add(new RefreshTokens
            {
                Token = newrefreshToken,
                ExpiresOn = newRefreshTokenExpiration,
            });

            await _userManager.UpdateAsync(user);

            return new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email, newToken, expireIn, newrefreshToken, newRefreshTokenExpiration);

        }


        public async Task<bool> RevokeRefreshToken(string token, string refreshToken, CancellationToken cancellationToken)
        {
           var userId =  _jwtProvider.ValidatToken(token);
            if (userId is null)
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return false;

            var userRefreshToken = user.RefreshTokens.FirstOrDefault(X => X.Token == refreshToken && X.IsActive);
            if (userRefreshToken is null)
                return false;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            return true;

        }




        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    
    
    
    }

}
