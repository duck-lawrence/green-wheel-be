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
    public class StationFeedbackRepository : GenericRepository<StationFeedback>, IStationFeedbackRepository
    {
        public StationFeedbackRepository(IGreenWheelDbContext dbContext) : base(dbContext)
        {
        }
    }
}