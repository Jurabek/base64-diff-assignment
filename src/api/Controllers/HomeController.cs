using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Base64Diff.Api.Controllers
{
    /// <summary>
    /// Handles the API utilities requests.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Handles status requests at the endpoint `GET /status`.
        /// </summary>
        /// <remarks>
        /// This endpoint is meant to be used by monitoring services to measure service uptime.
        /// </remarks>
        /// <returns>
        /// The plain-text string `OK`.
        /// </returns>
        [HttpGet("status")]
        public string Status() => "OK";
    }
}
