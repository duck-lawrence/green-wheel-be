using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User?> GetByPhoneAsync(string phone)
        {
            var user = await _dbContext.Users
                .Include(x => x.CitizenIdentity)
                .Include(x => x.DriverLicense).FirstOrDefaultAsync(x => x.Phone == phone);
            return user;
        }
        //Hàm GetByIdWithRoleAsync chỉ mở rộng cách lấy dữ liệu user: nó vẫn trả về User?, 
        // nhưng thêm Include(user => user.Role) và Include(user => user.Staff) 
        // để load thêm thông tin liên quan (role, staff). Backend trước đây khi gọi GetByIdAsync sẽ không có các navigation này, nên /users/me không trả được trường role. 
        // Bây giờ UserService.GetMe gọi hàm mới, nhờ đó JSON phản hồi có role, roleId, roleDetail, stationId. (Phúc thêm)
        // Mục đích:  response /api/users/me trả về đầy đủ thông tin role, 
        // giúp useAuth ở frontend biết chắc user có role “staff”.
        public async Task<User?> GetByIdWithFullInfoAsync(Guid id)
        {
            // added: eager load role to expose its metadata for clients
            return await _dbContext.Users
                .Include(user => user.Role)
                .Include(user => user.DriverLicense)
                .Include(user => user.CitizenIdentity)
                .Include(user => user.Staff)
                    .ThenInclude(staff => staff.Station)
                .FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}

