using AwesomeLibrary.BLL.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<List<MemberResponse>> GetAllAsync();
        Task CreateTemporaryDataForTestAsync();
    }
}
