using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
      

        public RefreshTokenRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<RefreshToken?> GetByRefreshToken(string refreshToken, bool getRevoked)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(_refreshToken => _refreshToken.Token == refreshToken 
                                            && _refreshToken.IsRevoked == getRevoked);   
        }

        //hàm này nhận vào string token, tìm nó trong db và revoked nó
        public async Task<int> RevokeRefreshToken(string token)
        {
            
            RefreshToken tokenFromDB = await GetByRefreshToken(token, false);
            if(tokenFromDB != null)
            {
                tokenFromDB.IsRevoked = true;
                int n = await UpdateAsync(tokenFromDB);
                return n;
            }
            throw new UnauthorizedAccessException();
        }

        //hàm này sẽ giúp ta revoke tất cả token có trong DB mà userID bằng vs userID ta truyền vào và chưa bị revoke và còn hạn
        public async Task<int> RevokeRefreshTokenByUserID(string userID)
        {
            
            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //EF theo dõi token thông qua _dbContext nên thay đổi xog saveChange thì trong DB thay đổi theo luôn
            var tokens = _dbContext.RefreshTokens.Where(rt => rt.UserId.ToString() == userID && rt.IsRevoked == false && rt.ExpiresAt > DateTime.Now);
            foreach( var token in tokens)
            {
                token.IsRevoked = true;
            }
            return await _dbContext.SaveChangesAsync();
        }
    }
}
