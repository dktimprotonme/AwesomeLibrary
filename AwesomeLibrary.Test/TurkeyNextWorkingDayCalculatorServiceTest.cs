using AwesomeLibrary.BLL.Services.Concretes;
using AwesomeLibrary.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace AwesomeLibrary.Test
{
    [TestClass]
    public class TurkeyNextWorkingDayCalculatorServiceTest
    {
        private readonly ITurkeyNextWorkingDayCalculatorService _turkeyNextWorkingDayCalculatorService;
        public TurkeyNextWorkingDayCalculatorServiceTest()
        {
            var services = new ServiceCollection();
            services.AddScoped<ITurkeyNextWorkingDayCalculatorService, TurkeyNextWorkingDayCalculatorService>();
            var serviceProvider = services.BuildServiceProvider();
            _turkeyNextWorkingDayCalculatorService = serviceProvider.GetRequiredService<ITurkeyNextWorkingDayCalculatorService>();
        }
        [TestMethod]
        public void CalculateLastWorkingDayOn2022_Test()
        {
            var firstWorkingDayOn2022 = new DateTime(2022, 1, 3);
            var workingDayCountOn2022 = 251; //Only whole day public holidays are counted
            var nextWorkingDay = _turkeyNextWorkingDayCalculatorService.CalculateNextWorkingDay(firstWorkingDayOn2022, workingDayCountOn2022);
            var lastWorkingDayOn2022 = new DateTime(2022, 12, 30); 
            Assert.IsTrue(nextWorkingDay == lastWorkingDayOn2022);
        }
        [TestMethod]
        public void CalculateLastWorkingDayOn2023_Test()
        {
            var firstWorkingDayOn2023 = new DateTime(2023, 1, 2);
            var workingDayCountOn2023 = 252; //Only whole day public holidays are counted
            var nextWorkingDay = _turkeyNextWorkingDayCalculatorService.CalculateNextWorkingDay(firstWorkingDayOn2023, workingDayCountOn2023);
            var lastWorkingDayOn2023 = new DateTime(2023, 12, 29);
            Assert.IsTrue(nextWorkingDay == lastWorkingDayOn2023);
        }
    }
}