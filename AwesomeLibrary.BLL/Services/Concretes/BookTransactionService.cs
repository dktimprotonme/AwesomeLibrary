using AwesomeLibrary.BLL.Models.Requests;
using AwesomeLibrary.BLL.Models.Responses;
using AwesomeLibrary.BLL.Services.Interfaces;
using AwesomeLibrary.DAL;
using AwesomeLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Services.Concretes
{
    public class BookTransactionService : IBookTransactionService
    {
        private readonly AwesomeLibraryDbContext _dbContext;
        private readonly IPenaltyCalculatorService _penaltyCalculatorService;
        private readonly ITurkeyNextWorkingDayCalculatorService _turkeyNextWorkingDayCalculatorService;

        public BookTransactionService(AwesomeLibraryDbContext dbContext, IPenaltyCalculatorService penaltyCalculatorService, ITurkeyNextWorkingDayCalculatorService turkeyNextWorkingDayCalculatorService)
        {
            _dbContext = dbContext;
            _penaltyCalculatorService = penaltyCalculatorService;
            _turkeyNextWorkingDayCalculatorService = turkeyNextWorkingDayCalculatorService;
        }

        public async Task<List<BookTransactionResponse>> GetAllAsync()
        {
            var response = await _dbContext.BookTransactions
                .Select(x => new BookTransactionResponse()
                {
                    Id = x.Id,
                    ISBN = x.ISBN,
                    MemberId = x.MemberId,
                    TransactionDate = DateOnly.FromDateTime(x.TransactionDate),
                    ExpectedReturnDate = DateOnly.FromDateTime(x.ExpectedReturnDate),
                    RealReturnDate = x.RealReturnDate.HasValue ? DateOnly.FromDateTime(x.RealReturnDate.Value) : null
                }).ToListAsync();

            return response;
        }

        public async Task<List<UpcomingBookResponse>> GetUpcomingBooksAsync()
        {
            var response = await _dbContext.BookTransactions
                .Where(x => x.RealReturnDate == null)
                .Where(x => EF.Functions.DateDiffDay(DateTime.Today, x.ExpectedReturnDate) <= 2)
                .Where(x => EF.Functions.DateDiffDay(DateTime.Today, x.ExpectedReturnDate) > 0)
                .Select(x => new UpcomingBookResponse()
                {
                    ISBN = x.ISBN,
                    BookName = x.Book.Name,
                    MemberId = x.MemberId,
                    MemberFullName = $"{x.Member.Name} {x.Member.Surname}",
                    TransactionDate = DateOnly.FromDateTime(x.TransactionDate),
                    ExpectedReturnDate = DateOnly.FromDateTime(x.ExpectedReturnDate),
                    UpcomingDays = (x.ExpectedReturnDate - DateTime.Today).Days
                })
                .ToListAsync();

            return response;
        }

        public async Task<List<LateBookResponse>> GetLateBooksAsync()
        {
            var response = await _dbContext.BookTransactions
                .Where(x => x.RealReturnDate == null)
                .Where(x => x.ExpectedReturnDate < DateTime.Today)
                .Select(x => new LateBookResponse()
                {
                    ISBN = x.ISBN,
                    BookName = x.Book.Name,
                    MemberId = x.MemberId,
                    MemberFullName = $"{x.Member.Name} {x.Member.Surname}",
                    TransactionDate = DateOnly.FromDateTime(x.TransactionDate),
                    ExpectedReturnDate = DateOnly.FromDateTime(x.ExpectedReturnDate),
                    LateDays = (DateTime.Today - x.ExpectedReturnDate).Days,
                    Penalty = _penaltyCalculatorService.Calculate((DateTime.Today - x.ExpectedReturnDate).Days)
                })
                .ToListAsync();

            return response;
        }

        public async Task<BorrowResponse> BorrowAsync(BorrowRequest borrowRequest)
        {
            ArgumentNullException.ThrowIfNull(borrowRequest);
            ArgumentNullException.ThrowIfNull(borrowRequest.SelectedISBNs);

            if (!borrowRequest.SelectedISBNs.Any())
            {
                throw new Exception($"{nameof(borrowRequest.SelectedISBNs)} is empty.");
            }

            if (borrowRequest.MemberId < 1)
            {
                throw new Exception($"{nameof(borrowRequest.MemberId)} must be bigger than 0.");
            }

            var member = await _dbContext.Members.FirstOrDefaultAsync(x => x.Id == borrowRequest.MemberId);
            if (member == null)
            {
                throw new Exception($"Member not found for {nameof(borrowRequest.MemberId)} = {borrowRequest.MemberId}");
            }

            var selectedBooks = await _dbContext.Books.Where(x => borrowRequest.SelectedISBNs.Contains(x.ISBN)).ToListAsync();
            if (selectedBooks.Count != borrowRequest.SelectedISBNs.Count)
            {
                var foundedISBNs = selectedBooks.Select(x => x.ISBN).ToList();
                var notFoundISBNs = borrowRequest.SelectedISBNs.Where(x => !foundedISBNs.Contains(x)).ToList();
                if (notFoundISBNs.Any())
                {
                    var stringBuilder = new StringBuilder();
                    notFoundISBNs.ForEach(x => stringBuilder.AppendLine($"Selected ISBN not found. ISBN = {x}"));
                    var message = stringBuilder.ToString();
                    throw new Exception(message);
                }
            }

            var notAvailableBooks = await _dbContext.BookTransactions
                .Where(x => borrowRequest.SelectedISBNs.Contains(x.ISBN))
                .Where(x => x.RealReturnDate == null)
                .ToListAsync();

            if (notAvailableBooks.Any())
            {
                var stringBuilder = new StringBuilder();
                foreach (var item in notAvailableBooks)
                {
                    stringBuilder.AppendLine($"Selected Book was already borrowed.");
                    stringBuilder.AppendLine($"ISBN = {item.ISBN}");
                    stringBuilder.AppendLine($"Book Name = {item.Book.Name}");
                }
                var message = stringBuilder.ToString();
                throw new Exception(message);
            }

            var workingDay30th = _turkeyNextWorkingDayCalculatorService.CalculateNextWorkingDay(DateTime.Today, 30);

            foreach (var item in selectedBooks)
            {
                var bookTransaction = new BookTransaction();
                bookTransaction.ISBN = item.ISBN;
                bookTransaction.MemberId = member.Id;
                bookTransaction.TransactionDate = DateTime.Today;
                bookTransaction.ExpectedReturnDate = workingDay30th;
                await _dbContext.BookTransactions.AddAsync(bookTransaction);
            }

            await _dbContext.SaveChangesAsync();

            var borrowResponse = new BorrowResponse() { ExpectedReturnDate = DateOnly.FromDateTime(workingDay30th) };
            return borrowResponse;
        }

        public async Task<ReturnResponse> ReturnAsync(ReturnRequest returnRequest)
        {
            ArgumentNullException.ThrowIfNull(returnRequest);

            var member = await _dbContext.Members.FirstOrDefaultAsync(x => x.Id == returnRequest.MemberId);
            if (member == null)
            {
                throw new Exception($"Member not found for {nameof(returnRequest.MemberId)} = {returnRequest.MemberId}");
            }

            var selectedBooks = await _dbContext.Books.Where(x => returnRequest.SelectedISBNs.Contains(x.ISBN)).ToListAsync();
            if (selectedBooks.Count != returnRequest.SelectedISBNs.Count)
            {
                var foundedISBNs = selectedBooks.Select(x => x.ISBN).ToList();
                var notFoundISBNs = returnRequest.SelectedISBNs.Where(x => !foundedISBNs.Contains(x)).ToList();
                if (notFoundISBNs.Any())
                {
                    var stringBuilder = new StringBuilder();
                    notFoundISBNs.ForEach(x => stringBuilder.AppendLine($"Selected ISBN not found. ISBN = {x}"));
                    var message = stringBuilder.ToString();
                    throw new Exception(message);
                }
            }

            var alreadyReturnedBooks = await _dbContext.BookTransactions
                .Where(x => x.MemberId == member.Id)
                .Where(x => returnRequest.SelectedISBNs.Contains(x.ISBN))
                .Where(x => x.RealReturnDate != null)
                .ToListAsync();

            if (alreadyReturnedBooks.Any())
            {
                var stringBuilder = new StringBuilder();
                foreach (var item in alreadyReturnedBooks)
                {
                    stringBuilder.AppendLine($"Selected Book was already returned.");
                    stringBuilder.AppendLine($"ISBN = {item.ISBN}");
                    stringBuilder.AppendLine($"Book Name = {item.Book.Name}");
                }
                var message = stringBuilder.ToString();
                throw new Exception(message);
            }

            var willReturnBooks = await _dbContext.BookTransactions
               .Where(x => x.MemberId == member.Id)
               .Where(x => returnRequest.SelectedISBNs.Contains(x.ISBN))
               .Where(x => x.RealReturnDate == null)
               .ToListAsync();

            willReturnBooks.ForEach(x => x.RealReturnDate = DateTime.Today);
            _dbContext.BookTransactions.UpdateRange(willReturnBooks);

            await _dbContext.SaveChangesAsync();

            decimal totalPenalty = 0;
            foreach (var item in willReturnBooks)
            {
                var lateDays = (DateTime.Today - item.ExpectedReturnDate).Days;
                if (lateDays > 0)
                {
                    var penalty = _penaltyCalculatorService.Calculate(lateDays);
                    totalPenalty = totalPenalty + penalty;
                }
            }
            bool hasPenalty = totalPenalty > 0;
            var borrowResponse = new ReturnResponse() { HasPenalty = hasPenalty, TotalPenalty = totalPenalty };
            return borrowResponse;
        }

        public async Task CreateTemporaryDataForTestAsync()
        {
            var hasAnyBookTransaction = await _dbContext.BookTransactions.AnyAsync();
            if (!hasAnyBookTransaction)
            {
                var member = await _dbContext.Members.FirstOrDefaultAsync();
                if (member != null)
                {
                    var dates = new List<DateTime>() { new DateTime(2023, 1, 6), new DateTime(2023, 1, 16), new DateTime(2023, 2, 1) };
                    var books = await _dbContext.Books.Take(3).ToListAsync();
                    foreach (var item in books.Select((value, index) => new { index, value }))
                    {
                        var selectedDate = dates.ElementAt(item.index);
                        _dbContext.BookTransactions.Add(new BookTransaction()
                        {
                            Book = item.value,
                            Member = member,
                            TransactionDate = selectedDate,
                            ExpectedReturnDate = _turkeyNextWorkingDayCalculatorService.CalculateNextWorkingDay(selectedDate, 30),
                            RealReturnDate = item.index == 2 ? DateTime.Today : null
                        });
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
