using System;

namespace Base64Diff.Domain.Services
{
    /// <summary>
    /// Defines the interface for a store of <see cref="Diff"/> objects.
    /// </summary>
    public interface IDiffStore
    {
        /// <summary>
        /// Retrieves a <see cref="Diff" /> by its ID.
        /// </summary>
        /// <param name="id">The ID of the diff</param>
        /// <returns>
        /// The <see cref="Diff"/> instance for that ID, or <b>null</b> if not found.
        /// </returns>
        Diff Get(int id);

        /// <summary>
        /// Sets the left part of a diff.
        /// </summary>
        /// <param name="id">The ID of the diff</param>
        /// <param name="data">A Base64-encoded string containing the data for the left part of the diff</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="data"/> is not a valid Base64 string.</exception>
        /// <returns>The updated <see cref="Diff"/> instance.</returns>
        Diff SetLeft(int id, string data);

        /// <summary>
        /// Sets the right part of a diff.
        /// </summary>
        /// <param name="id">The ID of the diff</param>
        /// <param name="data">A Base64-encoded string containing the data for the right part of the diff</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="data"/> is not a valid Base64 string.</exception>
        /// <returns>The updated <see cref="Diff"/> instance.</returns>
        Diff SetRight(int id, string data);
    }
}
