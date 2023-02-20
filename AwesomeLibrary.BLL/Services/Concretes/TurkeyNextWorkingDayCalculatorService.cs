using AwesomeLibrary.BLL.Models.Responses;
using AwesomeLibrary.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AwesomeLibrary.BLL.Services.Concretes
{
    public class TurkeyNextWorkingDayCalculatorService : ITurkeyNextWorkingDayCalculatorService
    {
        public DateTime CalculateNextWorkingDay(DateTime startDate, int dayCount)
        {
            if (dayCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dayCount));
            }
            DateTime result = startDate;
            int remainingDay = dayCount;
            while (true)
            {
                result = result.AddDays(1);
                bool isWorkingDay = IsWorkingDay(result);
                if (isWorkingDay)
                {
                    remainingDay = remainingDay - 1;
                    if (remainingDay == 0)
                    {
                        break;
                    }
                }                              
            }
            return result;
        }
        private static int GetHijriYear(int year)
        {
            var diff = year - 621;
            var hijriYear = Convert.ToInt32(Math.Round(diff + Decimal.Divide(diff, 33)));
            return hijriYear;
        }
        private static DateTime NewYear(int year)
        {
            return new DateTime(year, 1, 1);
        }
        private static DateTime NationalSovereigntyAndChildrensDay(int year)
        {
            return new DateTime(year, 4, 23);
        }
        private static DateTime LabourDay(int year)
        {
            return new DateTime(year, 5, 1);
        }
        private static DateTime RamadanFirstDay(int year)
        {
            var hijriCalendar = new UmAlQuraCalendar();
            return hijriCalendar.ToDateTime(GetHijriYear(year), 10, 1, 0, 0, 0, 0);
        }
        private static DateTime RamadanSecondDay(int year)
        {
            var hijriCalendar = new UmAlQuraCalendar();
            return hijriCalendar.ToDateTime(GetHijriYear(year), 10, 2, 0, 0, 0, 0);
        }
        private static DateTime RamadanThirdDay(int year)
        {
            var hijriCalendar = new UmAlQuraCalendar();
            return hijriCalendar.ToDateTime(GetHijriYear(year), 10, 3, 0, 0, 0, 0);
        }
        private static DateTime YouthAndSportsDay(int year)
        {
            return new DateTime(year, 5, 19);
        }
        private static DateTime FeastOfSacrificesFirstDay(int year)
        {
            var hijriCalendar = new UmAlQuraCalendar();
            return hijriCalendar.ToDateTime(GetHijriYear(year), 12, 10, 0, 0, 0, 0);
        }
        private static DateTime FeastOfSacrificesSecondDay(int year)
        {
            var hijriCalendar = new UmAlQuraCalendar();
            return hijriCalendar.ToDateTime(GetHijriYear(year), 12, 11, 0, 0, 0, 0);
        }
        private static DateTime FeastOfSacrificesThirdDay(int year)
        {
            var hijriCalendar = new UmAlQuraCalendar();
            return hijriCalendar.ToDateTime(GetHijriYear(year), 12, 12, 0, 0, 0, 0);
        }
        private static DateTime FeastOfSacrificesFourthdDay(int year)
        {
            var hijriCalendar = new UmAlQuraCalendar();
            return hijriCalendar.ToDateTime(GetHijriYear(year), 12, 13, 0, 0, 0, 0);
        }
        private static DateTime DemocracyAndNationalUnityDay(int year)
        {
            return new DateTime(year, 7, 15);
        }
        private static DateTime VictoryDay(int year)
        {
            return new DateTime(year, 8, 30);
        }
        private static DateTime RepublicDay(int year)
        {
            return new DateTime(year, 10, 29);
        }
        private static bool IsPublicHoliday(DateTime DateTime)
        {
            return PublicHolidays(DateTime.Year).Contains(DateTime);
        }
        private static bool IsWeekend(DateTime DateTime)
        {
            var weekends = new List<DayOfWeek>() { DayOfWeek.Saturday, DayOfWeek.Sunday };
            if (weekends.Contains(DateTime.DayOfWeek))
            {
                return true;
            }
            return false;
        }
        private static bool IsWorkingDay(DateTime DateTime)
        {
            bool isWeekend = IsWeekend(DateTime);
            bool isPublicHoliday = IsPublicHoliday(DateTime);
            bool isNotWorkingDay = isWeekend || isPublicHoliday;
            bool isWorkingDay = !isNotWorkingDay;
            return isWorkingDay;
        }
        private static List<PublicHolidayResponse> PublicHolidayNames(int year)
        {
            var holidayNames = new List<PublicHolidayResponse>
            {
                new PublicHolidayResponse(){ PublicHolidayDate = NewYear(year), PublicHolidayName = "Yılbaşı Tatili"},
                new PublicHolidayResponse(){ PublicHolidayDate = NationalSovereigntyAndChildrensDay(year), PublicHolidayName = "Ulusal Egemenlik ve Çocuk Bayramı"},
                new PublicHolidayResponse(){ PublicHolidayDate = LabourDay(year), PublicHolidayName = "Emek ve Dayanışma Günü"},
                new PublicHolidayResponse(){ PublicHolidayDate = RamadanFirstDay(year), PublicHolidayName = "Ramazan Bayramı 1. Gün"},
                new PublicHolidayResponse(){ PublicHolidayDate = RamadanSecondDay(year), PublicHolidayName = "Ramazan Bayramı 2. Gün"},
                new PublicHolidayResponse(){ PublicHolidayDate = RamadanThirdDay(year), PublicHolidayName = "Ramazan Bayramı 3. Gün"},
                new PublicHolidayResponse(){ PublicHolidayDate = YouthAndSportsDay(year), PublicHolidayName = "Atatürk’ü Anma, Gençlik ve Spor Bayramı"},
                new PublicHolidayResponse(){ PublicHolidayDate = FeastOfSacrificesFirstDay(year), PublicHolidayName = "Kurban Bayramı 1. Gün"},
                new PublicHolidayResponse(){ PublicHolidayDate = FeastOfSacrificesSecondDay(year), PublicHolidayName = "Kurban Bayramı 2. Gün"},
                new PublicHolidayResponse(){ PublicHolidayDate = FeastOfSacrificesThirdDay(year), PublicHolidayName = "Kurban Bayramı 3. Gün"},
                new PublicHolidayResponse(){ PublicHolidayDate = FeastOfSacrificesFourthdDay(year), PublicHolidayName = "Kurban Bayramı 4. Gün"},
                new PublicHolidayResponse(){ PublicHolidayDate = VictoryDay(year), PublicHolidayName = "Zafer Bayramı"},
                new PublicHolidayResponse(){ PublicHolidayDate = RepublicDay(year), PublicHolidayName = "Cumhuriyet Bayramı"},
            };
            if (year >= 2017)
            {
                holidayNames.Add(new PublicHolidayResponse() { PublicHolidayDate = DemocracyAndNationalUnityDay(year), PublicHolidayName = "Demokrasi ve Milli Birlik Günü" });
            }
            return holidayNames;
        }
        private static List<DateTime> PublicHolidays(int year)
        {
            var publicHolidays = PublicHolidayNames(year)
                .Select(x => x.PublicHolidayDate)
                .OrderBy(x => x)
                .ToList();
            return publicHolidays;
        }
    }
}
