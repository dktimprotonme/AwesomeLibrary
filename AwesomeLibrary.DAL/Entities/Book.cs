using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AwesomeLibrary.DAL.Entities
{
    public class Book
    {
        [Key, StringLength(50)]
        public string ISBN { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Author { get; set; }
        public List<BookTransaction> BookTransactions { get; set; } = new();
    }
}
