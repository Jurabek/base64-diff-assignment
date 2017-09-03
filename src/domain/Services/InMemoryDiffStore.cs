using System;
using System.Collections.Generic;

namespace Base64Diff.Domain.Services
{
    /// <summary>
    /// Stores diffs objects in memory.
    /// </summary>
    public class InMemoryDiffStore : IDiffStore
    {
        /// <summary>
        /// Internal in-memory database.
        /// </summary>
        readonly Dictionary<int, Diff> Database = new Dictionary<int, Diff>();

        /// <summary>
        /// Retrieves a <see cref="Diff" /> by its ID.
        /// </summary>
        /// <param name="id">The ID of the diff</param>
        /// <returns>
        /// The <see cref="Diff"/> instance for that ID, or <b>null</b> if not found.
        /// </returns>
        public Diff Get(int id)
        {
            return Database.TryGetValue(id, out Diff diff) ? diff : null;
        }

        /// <summary>
        /// Sets the left part of a diff.
        /// </summary>
        /// <param name="id">The ID of the diff</param>
        /// <param name="data">A Base64-encoded string containing the data for the left part of the diff</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="data"/> is not a valid Base64 string.</exception>
        /// <returns>The updated <see cref="Diff"/> instance.</returns>
        public Diff SetLeft(int id, string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            var left = Convert.FromBase64String(data);
            Database.TryGetValue(id, out Diff diff);
            return Database[id] = new Diff(left, diff?.Right ?? new byte[0]);
        }

        /// <summary>
        /// Sets the right part of a diff.
        /// </summary>
        /// <param name="id">The ID of the diff</param>
        /// <param name="data">A Base64-encoded string containing the data for the right part of the diff</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="data"/> is not a valid Base64 string.</exception>
        /// <returns>The updated <see cref="Diff"/> instance.</returns>
        public Diff SetRight(int id, string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            var right = Convert.FromBase64String(data);
            Database.TryGetValue(id, out Diff diff);
            return Database[id] = new Diff(diff?.Left ?? new byte[0], right);
        }
    }
}
