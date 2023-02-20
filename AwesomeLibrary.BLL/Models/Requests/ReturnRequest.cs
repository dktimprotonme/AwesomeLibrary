using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Models.Requests
{
    public class ReturnRequest
    {
        public int MemberId { get; set; }
        public List<string> SelectedISBNs { get; set; }
    }
}
