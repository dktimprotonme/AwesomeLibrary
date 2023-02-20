using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.DAL.Entities
{
    public class BookTransaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string ISBN { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ExpectedReturnDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? RealReturnDate { get; set; }

        [ForeignKey("ISBN")]
        public Book Book { get; set; }

        [ForeignKey("MemberId")]
        public Member Member { get; set; }
    }
}
