using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Base64Diff.Api.Controllers
{
    using Domain.Services;
    using Helpers;

    /// <summary>
    /// Handles the API diffing requests.
    /// </summary>
    [Route("v1/diff")]
    [Produces("application/json")]
    public class DiffController : Controller
    {
        readonly IDiffStore DiffStore;

        /// <summary>
        /// Creates a new instance of the <see cref="DiffController"/> class that uses the specified diff store to perform diff operations.
        /// </summary>
        /// <param name="diffStore">A service implementing <see cref="IDiffStore"/></param>
        public DiffController(IDiffStore diffStore)
        {
            DiffStore = diffStore ?? throw new ArgumentNullException(nameof(diffStore));
        }

        /// <summary>
        /// Handles requests for diff data at the endpoint `GET /v1/diff/:id`.
        /// </summary>
        /// <param name="id">The ID of the diff being requested</param>
        /// <returns>
        /// A <see cref="JsonResult"/> with the diff data if a diff exists with the speciefied ID;
        /// Otherwise, a <see cref="NotFoundResult"/>.
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var diff = DiffStore.Get(id);
            if (diff == null) return NotFound();
            return Json(new
            {
                status = diff.Status.GetDescription(),
                differences = diff.Differences
            });
        }

        /// <summary>
        /// Handles requests for setting the left part of a diff at the endpoint `PUT /v1/diff/:id/left`.
        /// </summary>
        /// <param name="id">The ID of the diff being requested</param>
        /// <param name="diffData">The data being posted</param>
        /// <returns>A <see cref="JsonResult"/> indicating the result of the operation.</returns>
        [HttpPut("{id}/left")]
        public IActionResult SetLeft(int id, [FromBody]DiffData diffData)
        {
            try
            {
                DiffStore.SetLeft(id, diffData.Data);
            }
            catch (FormatException)
            {
                return MalformedBase64();
            }
            return Success();
        }

        /// <summary>
        /// Handles requests for setting the right part of a diff at the endpoint `PUT /v1/diff/:id/left`.
        /// </summary>
        /// <param name="id">The ID of the diff being requested</param>
        /// <param name="diffData">The data being posted</param>
        /// <returns>A <see cref="JsonResult"/> indicating the result of the operation.</returns>
        [HttpPut("{id}/right")]
        public IActionResult SetRight(int id, [FromBody]DiffData diffData)
        {
            try
            {
                DiffStore.SetRight(id, diffData.Data);
            }
            catch (FormatException)
            {
                return MalformedBase64();
            }
            return Success();
        }

        /// <summary>
        /// Represents data being posted to set either the left or right part of a diff.
        /// </summary>
        public class DiffData
        {
            /// <summary>
            /// The string containing Base-64 data.
            /// </summary>
            public string Data;
        }

        /// <summary>
        /// Returns a success JSON message.
        /// </summary>
        IActionResult Success()
        {
            return Json(new { success = true });
        }

        /// <summary>
        /// Returns an JSON error message indicating that the base64 string was malformed.
        /// </summary>
        IActionResult MalformedBase64()
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new
            {
                error = "Malformed Base64 string data"
            });
        }
    }
}
