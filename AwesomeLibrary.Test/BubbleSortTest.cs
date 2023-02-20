using AwesomeLibrary.BLL.Services.Concretes;
using AwesomeLibrary.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeLibrary.Test
{
    [TestClass]
    public class BubbleSortTest
    {
        private readonly IBubbleSortService _bubbleSortService;
        public BubbleSortTest()
        {
            var services = new ServiceCollection();
            services.AddScoped<IBubbleSortService, BubbleSortService>();
            var serviceProvider = services.BuildServiceProvider();
            _bubbleSortService = serviceProvider.GetRequiredService<IBubbleSortService>();
        }
        [TestMethod]
        public void BubbleSort_11_93_45_98_13_55_Test()
        {
            int[] arr = { 11, 93, 45, 98, 13, 55 };
            var sorted = _bubbleSortService.Sort(arr);
            int[] expected = { 11, 13, 45, 55, 93, 98 };
            Assert.IsTrue(new HashSet<int>(sorted).SetEquals(expected));  
        }
    }
}
