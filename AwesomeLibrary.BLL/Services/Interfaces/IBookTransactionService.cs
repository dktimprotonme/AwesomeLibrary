using AwesomeLibrary.BLL.Models.Requests;
using AwesomeLibrary.BLL.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Services.Interfaces
{
    public interface IBookTransactionService
    {
        Task<List<BookTransactionResponse>> GetAllAsync();
        Task<List<UpcomingBookResponse>> GetUpcomingBooksAsync();
        Task<List<LateBookResponse>> GetLateBooksAsync();
        Task<BorrowResponse> BorrowAsync(BorrowRequest borrowRequest);
        Task<ReturnResponse> ReturnAsync(ReturnRequest returnRequest);
        Task CreateTemporaryDataForTestAsync();
    }
}
