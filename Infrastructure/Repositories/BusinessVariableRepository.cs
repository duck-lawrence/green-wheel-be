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
    public class BusinessVariableRepository : GenericRepository<BusinessVariable>, IBusinessVariableRepository
    {
        public BusinessVariableRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }
    }
}
