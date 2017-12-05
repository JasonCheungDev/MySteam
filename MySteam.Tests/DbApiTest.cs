using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySteam.Data;
using MySteam.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MySteam.Tests
{
    [TestClass]
    public class DbApiTest
    {
        const string API_KEY = "758AEEE709F200A44D5A076B68F7636F";  // Xaieon API Key
        const string TEST_ACC_URL = "76561198020784166";            // Xaieon
        const string TEST_OLD_ACC_URL = "76561198051910246";        // Spartoi 

        private MySteamContext mContext; 

        public DbApiTest()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            InitContext();
        }

        public void InitContext()
        {
            var builder = new DbContextOptionsBuilder<MySteamContext>()
                .UseInMemoryDatabase("TestContext");
            mContext = new MySteamContext(builder.Options);
        }

        [TestMethod]
        public async Task TestIfDuplicateRequest_UsesDatabase()
        {
            ApiHelper api = ApiHelper.Instance;

            var id = new List<int>();
            id.Add(400);

            var request1 = await mContext.GetDetailedGameData(400);
            var request2 = await mContext.GetDetailedGameData(400);

            // Console should print first from API, second from DB
        }

    }
}
