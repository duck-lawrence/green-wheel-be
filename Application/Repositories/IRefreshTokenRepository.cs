using Domain.Entities;

namespace Application.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByRefreshToken(string refreshToken, bool getRevoked);
         Task<int> RevokeRefreshToken(string token);
        Task<int> RevokeRefreshTokenByUserID(string userID);
    }
}
