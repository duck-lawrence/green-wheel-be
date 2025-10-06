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
    public class ModelImageRepository : GenericRepository<ModelImage>, IModelImageRepository
    {
        public ModelImageRepository(GreenWheelDbContext context) : base(context)
        {
        }
    }
}