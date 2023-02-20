using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Models.Responses
{
    public class BorrowSearchResponse
    {
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public bool IsAvailable { get; set; }
        public DateOnly? ExpectedReturnDate { get; set; }
    }
}
