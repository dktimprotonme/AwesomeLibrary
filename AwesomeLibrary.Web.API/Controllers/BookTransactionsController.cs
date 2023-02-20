using AwesomeLibrary.BLL.Models.Requests;
using AwesomeLibrary.BLL.Services.Concretes;
using AwesomeLibrary.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeLibrary.Web.API.Controllers
{
    [Route("api/book-transactions")]
    [ApiController]
    public class BookTransactionsController : ControllerBase
    {
        private readonly IBookTransactionService _bookTransactionService;

        public BookTransactionsController(IBookTransactionService bookTransactionService)
        {
            _bookTransactionService = bookTransactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _bookTransactionService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("upcoming-books")]
        public async Task<IActionResult> GetUpcomingBooksAsync()
        {
            var response = await _bookTransactionService.GetUpcomingBooksAsync();
            return Ok(response);
        }

        [HttpGet("late-books")]
        public async Task<IActionResult> GetLateBooksAsync()
        {
            var response = await _bookTransactionService.GetLateBooksAsync();
            return Ok(response);
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowAsync(BorrowRequest borrowRequest)
        {
            var response = await _bookTransactionService.BorrowAsync(borrowRequest);
            return Ok(response);
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnAsync(ReturnRequest returnRequest)
        {
            var response = await _bookTransactionService.ReturnAsync(returnRequest);
            return Ok(response);
        }
    }
}
