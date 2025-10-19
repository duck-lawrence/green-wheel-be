using System;
using System.Collections;
using Domain.Entities;

namespace Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        // added: Phương thức get role khi lấy user theo id (Phúc thêm)
        // Mục đích:  response /api/users/me trả về đầy đủ thông tin role,
        // giúp useAuth ở frontend biết chắc user có role “staff”.
        Task<IEnumerable<User>> GetAllAsync(string? phone, string? citizenIdNumber, string? driverLicenseNumber);

        Task<User?> GetByIdWithFullInfoAsync(Guid id);

        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByPhoneAsync(string phone);

        Task<IEnumerable<User>> GetAllStaffAsync(string? name, Guid? stationId);

        
    }
}