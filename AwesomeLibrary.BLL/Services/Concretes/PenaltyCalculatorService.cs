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
    public class PenaltyCalculatorService : IPenaltyCalculatorService
    {
        public decimal Calculate(int lateDays)
        {
            if (lateDays < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(lateDays));
            }
            if (lateDays == 1)
            {
                return 0;
            }
            decimal coefficient = 0.2m;
            if (lateDays == 2)
            {
                return coefficient;
            }
            decimal total = 0;
            total = total + coefficient;
            int previousFibonacci = 1;
            int currentFibonacci = 1;
            int remainingDays = lateDays - 2;
            for (int i = 1; i <= remainingDays; i++)
            {
                total = (currentFibonacci * coefficient) + total;
                int nextFibonacci = currentFibonacci + previousFibonacci;
                previousFibonacci = currentFibonacci;
                currentFibonacci = nextFibonacci;
            }
            return total;
        }
    }
}
