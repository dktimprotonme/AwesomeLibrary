using AwesomeLibrary.BLL.Models.Requests;
using AwesomeLibrary.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeLibrary.Web.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        { 
            var response = await _bookService.GetAllAsync();
            return Ok(response);
        }

        [HttpPost("borrow-search")]
        public async Task<IActionResult> BorrowSearchAsync(BorrowSearchRequest borrowSearchRequest)
        {
            var response = await _bookService.BorrowSearchAsync(borrowSearchRequest);
            return Ok(response);
        }

        [HttpPost("return-search")]
        public async Task<IActionResult> ReturnSearchAsync(ReturnSearchRequest returnSearchRequest)
        {
            var response = await _bookService.ReturnSearchAsync(returnSearchRequest);
            return Ok(response);
        }
    }
}
