using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace Base64Diff.IntegrationTests
{
    /// <summary>
    /// Base class for integration tests.
    /// </summary>
    public abstract class IntegrationTest
    {
        /// <summary>
        /// A host that serves the test requests.
        /// </summary>
        protected TestServer Server { get; private set; }

        /// <summary>
        /// A <see cref="HttpClient" /> that connects to the test server to perform requests.
        /// </summary>
        protected HttpClient Client { get; private set; }

        /// <summary>
        /// Sets up the test server before every test run.
        /// </summary>
        [SetUp]
        public void CreateTestServer()
        {
            this.Server = new TestServer(new WebHostBuilder().UseStartup<Api.Startup>());
            this.Client = this.Server.CreateClient();
        }
    }
}
