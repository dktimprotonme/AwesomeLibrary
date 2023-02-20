using AwesomeLibrary.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Models.Responses
{
    public class LateBookResponse
    {
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public int MemberId { get; set; }
        public string MemberFullName { get; set; }
        public DateOnly TransactionDate { get; set; }
        public DateOnly ExpectedReturnDate { get; set; }
        public int LateDays { get; set; }
        public decimal? Penalty { get; set; }
    }
}
