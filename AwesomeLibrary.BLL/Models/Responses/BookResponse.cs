using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Models.Responses
{
    public class BookResponse
    {
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
    }
}
