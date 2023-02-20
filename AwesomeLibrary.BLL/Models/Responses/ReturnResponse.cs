using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Models.Responses
{
    public class ReturnResponse
    {
        public decimal? TotalPenalty { get; set; }
        public bool HasPenalty { get; set; }
    }
}
