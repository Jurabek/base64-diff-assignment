using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Base64Diff.IntegrationTests.Scenarios
{
    [TestFixture]
    public class ApiUtilities : IntegrationTest
    {
        [Test]
        public async Task client_checks_api_status()
        {
            var response = await Client.GetAsync("/status");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("OK", body);
        }
    }
}
