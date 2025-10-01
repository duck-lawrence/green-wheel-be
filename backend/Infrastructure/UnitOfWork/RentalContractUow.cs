using Application.Repositories;
using Application.UnitOfWorks;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class RentalContractUow : IRentalContractUow
    {
        private readonly IGreenWheelDbContext _context;

        public IVehicleRepository Vehicles { get; }
        public IRentalContractRepository RentalContracts { get; }
        public IUserRepository Users { get; }

        public RentalContractUow(
        IGreenWheelDbContext context,
        IVehicleRepository vehicleRepository,
        IRentalContractRepository rentalContractRepository,
        IUserRepository userRepository)
        {
            _context = context;
            Vehicles = vehicleRepository;
            RentalContracts = rentalContractRepository;
            Users = userRepository;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_context is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
