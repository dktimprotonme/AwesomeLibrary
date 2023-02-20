using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.BLL.Services.Interfaces
{
    public interface ITurkeyNextWorkingDayCalculatorService
    {
        DateTime CalculateNextWorkingDay(DateTime startDate, int dayCount);     
    }
}
