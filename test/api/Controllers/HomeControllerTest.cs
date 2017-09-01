using System;
using NUnit.Framework;

namespace Base64Diff.Api.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void TestStatus()
        {
            var home = new HomeController();
            Assert.AreEqual("OK", home.Status());
        }
    }
}
