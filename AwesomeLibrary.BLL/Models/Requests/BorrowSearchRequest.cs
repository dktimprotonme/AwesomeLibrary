using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Models.Requests
{
    public class BorrowSearchRequest
    {
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
    }
}
