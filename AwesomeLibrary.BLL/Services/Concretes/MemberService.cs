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
    public class MemberService : IMemberService
    {
        private readonly AwesomeLibraryDbContext _dbContext;

        public MemberService(AwesomeLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MemberResponse>> GetAllAsync()
        {
            var response = await _dbContext.Members
                .Select(x => new MemberResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname
                }).ToListAsync();

            return response;
        }

        public async Task CreateTemporaryDataForTestAsync()
        {
            var hasAnyMember = await _dbContext.Members.AnyAsync();
            if (!hasAnyMember)
            {
                await _dbContext.Members.AddAsync(new Member() { Name = "Deniz", Surname = "Kısır" });
                await _dbContext.Members.AddAsync(new Member() { Name = "John", Surname = "Doe" });
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
