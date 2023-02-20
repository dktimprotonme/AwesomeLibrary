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
    public class BookTransactionResponse
    {
        public int Id { get; set; }  
        public string ISBN { get; set; }    
        public int MemberId { get; set; }     
        public DateOnly TransactionDate { get; set; }
        public DateOnly ExpectedReturnDate { get; set; } 
        public DateOnly? RealReturnDate { get; set; }
    }
}
