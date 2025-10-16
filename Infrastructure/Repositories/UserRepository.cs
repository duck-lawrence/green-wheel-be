using Application.Constants;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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

        public async Task<IEnumerable<User>> GetAllAsync(string? phone, string? citizenIdNumber, string? driverLicenseNumber)
        {
            var query = _dbContext.Users
                .Include(user => user.Role)
                .Include(user => user.DriverLicense)
                .Include(user => user.CitizenIdentity)
                .Include(user => user.Staff)
                    .ThenInclude(staff => staff.Station)
                .AsQueryable()
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(u => u.Phone == phone);

            if (!string.IsNullOrWhiteSpace(citizenIdNumber))
                query = query.Where(u => u.CitizenIdentity != null && u.CitizenIdentity.Number == citizenIdNumber);

            if (!string.IsNullOrWhiteSpace(driverLicenseNumber))
                query = query.Where(u => u.DriverLicense != null && u.DriverLicense.Number == driverLicenseNumber);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllStaffAsync(string? name, Guid? stationId)
        {
            var query = _dbContext.Users
               .Include(user => user.Role)
               .Include(user => user.DriverLicense)
               .Include(user => user.CitizenIdentity)
               .Include(user => user.Staff)
                   .ThenInclude(staff => staff.Station)
                .Where(u => u.Staff != null && u.Role.Name == RoleName.Staff)
               .AsQueryable()
               .AsNoTracking();
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(u => u.FirstName.ToLower().Contains(name.ToLower()) ||
                                    u.LastName.ToLower().Contains(name.ToLower()));
            if (stationId != null)
                query = query.Where(u => u.Staff.StationId == stationId);
            return await query.ToListAsync();
        } 

        public async Task<User?> GetByPhoneAsync(string phone)
        {
            var user = await _dbContext.Users
                .Include(user => user.Role)
                .Include(user => user.DriverLicense)
                .Include(user => user.CitizenIdentity)
                .Include(user => user.Staff)
                    .ThenInclude(staff => staff.Station)
                .FirstOrDefaultAsync(x => x.Phone == phone);
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