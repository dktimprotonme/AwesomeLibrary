using AwesomeLibrary.BLL.Services.Concretes;
using AwesomeLibrary.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace AwesomeLibrary.Test
{
    [TestClass]
    public class PenaltyCalculatorServiceTest
    {
        private readonly IPenaltyCalculatorService _penaltyCalculatorService;
        public PenaltyCalculatorServiceTest()
        {
            var services = new ServiceCollection();
            services.AddScoped<IPenaltyCalculatorService, PenaltyCalculatorService>();
            var serviceProvider = services.BuildServiceProvider();
            _penaltyCalculatorService = serviceProvider.GetRequiredService<IPenaltyCalculatorService>();
        }
        [TestMethod]
        public void CalculatePenaltyFor_7_LateDays_Test()
        {
            var lateDays = 7;
            var calculatedPenalty = _penaltyCalculatorService.Calculate(lateDays);
            decimal expectedPenalty = 4;
            Assert.IsTrue(calculatedPenalty == expectedPenalty);
        }
    }
}