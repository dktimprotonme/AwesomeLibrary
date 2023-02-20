using AwesomeLibrary.BLL.Models.Requests;
using AwesomeLibrary.BLL.Models.Responses;
using AwesomeLibrary.BLL.Services.Interfaces;
using AwesomeLibrary.DAL;
using AwesomeLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Services.Concretes
{
    public class BookService : IBookService
    {
        private readonly AwesomeLibraryDbContext _dbContext;
        private readonly IPenaltyCalculatorService _penaltyCalculatorService;

        public BookService(AwesomeLibraryDbContext dbContext, IPenaltyCalculatorService penaltyCalculatorService)
        {
            _dbContext = dbContext;
            _penaltyCalculatorService = penaltyCalculatorService;
        }

        public async Task CreateTemporaryDataForTestAsync()
        {
            var hasAnyBook = await _dbContext.Books.AnyAsync();
            if (!hasAnyBook)
            {
                await _dbContext.Books.AddAsync(new Book() { ISBN = "978-0134757599", Name = "Refactoring: Improving the Design of Existing Code", Author = "Martin Fowler" });
                await _dbContext.Books.AddAsync(new Book() { ISBN = "978-0132350884", Name = "Clean Code: A Handbook of Agile Software Craftsmanship", Author = "Robert C. Martin" });
                await _dbContext.Books.AddAsync(new Book() { ISBN = "978-0984782857", Name = "Cracking the Coding Interview: 189 Programming Questions and Solutions", Author = "Gayle Laakmann McDowell" });
                await _dbContext.Books.AddAsync(new Book() { ISBN = "978-0134494166", Name = "Clean Architecture: A Craftsman's Guide to Software Structure and Design", Author = "Robert C. Martin" });
                await _dbContext.Books.AddAsync(new Book() { ISBN = "978-0137081073", Name = "The Clean Coder: A Code of Conduct for Professional Programmers", Author = "Robert C. Martin" });
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<BookResponse>> GetAllAsync()
        {
            var response = await _dbContext.Books
                .Select(x => new BookResponse()
                {
                    ISBN = x.ISBN,
                    Name = x.Name,
                    Author = x.Author
                }).ToListAsync();

            return response;
        }

        public async Task<List<BorrowSearchResponse>> BorrowSearchAsync(BorrowSearchRequest borrowSearchRequest)
        {
            ArgumentNullException.ThrowIfNull(borrowSearchRequest);

            bool isNullOrWhiteSpaceISBN = string.IsNullOrWhiteSpace(borrowSearchRequest.ISBN);
            bool isNullOrWhiteSpaceBookName = string.IsNullOrWhiteSpace(borrowSearchRequest.BookName);
            bool isNullOrWhiteSpaceBookAuthor = string.IsNullOrWhiteSpace(borrowSearchRequest.BookAuthor);

            bool isNullOrWhiteSpaceAll = isNullOrWhiteSpaceISBN && isNullOrWhiteSpaceBookName && isNullOrWhiteSpaceBookAuthor;

            if (isNullOrWhiteSpaceAll)
            {
                return new List<BorrowSearchResponse>();
            }

            var query = _dbContext.Books
                .Select(x => new BorrowSearchResponse()
                {
                    ISBN = x.ISBN,
                    BookName = x.Name,
                    BookAuthor = x.Author,
                    IsAvailable = x.BookTransactions.Any(y => y.RealReturnDate == null) ? false : true,
                    ExpectedReturnDate = x.BookTransactions.Any(y => y.RealReturnDate == null) ?
                        DateOnly.FromDateTime(x.BookTransactions.First(y => y.RealReturnDate == null).ExpectedReturnDate) : null
                });

            if (!isNullOrWhiteSpaceISBN)
            {
                query = query.Where(x => x.ISBN.Contains(borrowSearchRequest.ISBN.Trim()));
            }

            if (!isNullOrWhiteSpaceBookName)
            {
                query = query.Where(x => x.BookName.Contains(borrowSearchRequest.BookName.Trim()));
            }

            if (!isNullOrWhiteSpaceBookAuthor)
            {
                query = query.Where(x => x.BookAuthor.Contains(borrowSearchRequest.BookAuthor.Trim()));
            }

            var response = await query.ToListAsync();
            return response;
        }

        public async Task<List<ReturnSearchResponse>> ReturnSearchAsync(ReturnSearchRequest returnSearchRequest)
        {
            ArgumentNullException.ThrowIfNull(returnSearchRequest);

            if (returnSearchRequest.MemberId < 1)
            {
                throw new Exception($"{nameof(returnSearchRequest.MemberId)} must be bigger than 0.");
            }

            var member = await _dbContext.Members.FirstOrDefaultAsync(x => x.Id == returnSearchRequest.MemberId);
            if (member == null)
            {
                throw new Exception($"Member not found for {nameof(returnSearchRequest.MemberId)} = {returnSearchRequest.MemberId}");
            }

            var response = _dbContext.BookTransactions
               .Where(x => x.MemberId == member.Id)
               .Where(x => x.RealReturnDate == null)
               .Select(x => new ReturnSearchResponse()
               {
                   ISBN = x.ISBN,
                   BookName = x.Book.Name,
                   MemberFullName = $"{x.Member.Name} {x.Member.Surname}",
                   TransactionDate = DateOnly.FromDateTime(x.TransactionDate),
                   ExpectedReturnDate = DateOnly.FromDateTime(x.ExpectedReturnDate),
                   LateDays = DateTime.Today > x.ExpectedReturnDate ? (DateTime.Today - x.ExpectedReturnDate).Days : null,
                   Penalty = DateTime.Today > x.ExpectedReturnDate ? _penaltyCalculatorService.Calculate((DateTime.Today - x.ExpectedReturnDate).Days) : null,
                   HasPenalty = DateTime.Today > x.ExpectedReturnDate
               })
               .ToList();

            return response;
        }
    }
}
