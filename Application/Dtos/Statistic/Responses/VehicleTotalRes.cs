using Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Statistic.Responses
{
    public class VehicleTotalRes
    {
        public int Total {  get; set; }
        public List<VehicleStatusCountItem> Items { get; set; } = new();
    }
}
