using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.BusinessVariable.Respone
{
    public class BusinessVariableViewRes
    {
        public Guid Id { get; set; }
        public int Key { get; set; }
        public decimal Value { get; set; }
    }
}
