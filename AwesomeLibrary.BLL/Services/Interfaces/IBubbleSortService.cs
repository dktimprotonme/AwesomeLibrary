using AwesomeLibrary.BLL.Models.Requests;
using AwesomeLibrary.BLL.Models.Responses;
using AwesomeLibrary.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Services.Interfaces
{
    public interface IBubbleSortService
    {
        int[] Sort(int[] arr);
    }
}
