using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySteam.Data;
using System;
using System.Threading.Tasks;

namespace MySteam.Tests
{
    [TestClass]
    public class ApiHelperTests
    {
        [TestMethod]
        public async Task TestShouldFail()
        {
            await ApiHelper.SuperSimpleAsync();
        }

        [TestMethod]
        public void UrlShouldBeSteam()
        {
            var result = ApiHelper.Instance.Details();
            Console.WriteLine(result);
            Assert.AreEqual(result, "http://api.steampowered.com/");
        }

        [TestMethod]
        public async Task InstanceShouldFail()
        {
            await ApiHelper.Instance.SimpleAsync();
        }
    }
}
