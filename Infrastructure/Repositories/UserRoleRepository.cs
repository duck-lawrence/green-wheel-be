using Application.Repositories;
using Domain.Entities;
using Infrastructure.ApplicationDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRoleRepository : GenericRepository<Role>, IUserRoleRepository
    {
        public UserRoleRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }
    }
}
