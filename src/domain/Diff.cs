using System;
using System.Collections.Generic;

namespace Base64Diff.Domain
{
    /// <summary>
    /// Represents the difference between two pieces of binary data.
    /// </summary>
    public class Diff
    {
        readonly byte[] _left;
        readonly byte[] _right;
        readonly DiffStatus _status;
        readonly List<(int, int)> _differences = new List<(int, int)>();

        /// <summary>
        /// The binary data on the left part of the diff.
        /// </summary>
        public byte[] Left => _left;

        /// <summary>
        /// The binary data on the right part of the diff.
        /// </summary>
        public byte[] Right => _right;

        /// <summary>
        /// A <see cref="DiffStatus" /> indicating the status of the diff.
        /// </summary>
        public DiffStatus Status => _status;

        /// <summary>
        /// A <see cref="IReadOnlyList{ValueTuple{int, int}}" /> containing the differences (Offset and Length) of
        /// the <see cref="Right"/> part in relation to the <see cref="Left"/> part of the diff, if any.
        /// </summary>
        public IReadOnlyList<(int Offset, int Length)> Differences => _differences;

        /// <summary>
        /// Private constructor, creates an immutable Diff from scratch.
        /// </summary>
        /// <param name="left">The <see cref="Left" /> part of the diff</param>
        /// <param name="right">The <see cref="Right" /> part of the diff</param>
        private Diff(byte[] left, byte[] right)
        {
            _left = (byte[])left.Clone();   // Avoid mutations
            _right = (byte[])right.Clone(); // Avoid mutations
            (_status, _differences) = ExtractDifferences(_left, _right);
        }

        /// <summary>
        /// Extracts the differences between the left and right parts, if any.
        /// </summary>
        /// <param name="left">The <see cref="Left" /> part of the diff</param>
        /// <param name="right">The <see cref="Right" /> part of the diff</param>
        /// <returns>A tuple containing the <see cref="Status"/> and <see cref="Differences" /> between the two parts.</returns>
        static (DiffStatus status, List<(int, int)> differences) ExtractDifferences(byte[] left, byte[] right)
        {
            var differences = new List<(int, int)>();
            if (left.Length != right.Length)
                return (DiffStatus.DifferentLength, differences);
            int lastDifference = -1;
            for (int i = 0; i < left.Length; i++)
            {
                if (lastDifference < 0 && left[i] != right[i])
                {
                    lastDifference = i;
                }
                else if (lastDifference > -1 && left[i] == right[i])
                {
                    differences.Add((lastDifference, i - lastDifference));
                    lastDifference = -1;
                }
            }
            if (lastDifference > -1)
                differences.Add((lastDifference, left.Length - lastDifference));
            var status = differences.Count == 0 ? DiffStatus.SameContent : DiffStatus.DifferentContent;
            return (status, differences);
        }

        /// <summary>
        /// Internal in-memory database.
        /// </summary>
        static readonly Dictionary<int, Diff> Database = new Dictionary<int, Diff>();

        /// <summary>
        /// Finds a <see cref="Diff" /> by its ID.
        /// </summary>
        /// <param name="id">The ID of the Diff</param>
        /// <returns>
        /// The <see cref="Diff"/> instance for that ID, or <b>null</b> if not found.
        /// </returns>
        public static Diff Find(int id)
        {
            return Database.TryGetValue(id, out Diff diff) ? diff : null;
        }

        /// <summary>
        /// Sets the <see cref="Left"/> part of a diff.
        /// </summary>
        /// <param name="id">The ID of the diff</param>
        /// <param name="data">A Base64-encoded string containing the left part of the diff</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="data"/> is not a valid Base64 string.</exception>
        /// <returns>The new <see cref="Diff"/> instance.</returns>
        public static Diff SetLeft(int id, string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            var left = Convert.FromBase64String(data);
            if (!Database.TryGetValue(id, out Diff diff))
            {
                diff = new Diff(left, new byte[0]);
                Database.Add(id, diff);
                return diff;
            }
            return Database[id] = new Diff(left, diff.Right);
        }

        /// <summary>
        /// Sets the <see cref="Right"/> part of a diff.
        /// </summary>
        /// <param name="id">The ID of the diff</param>
        /// <param name="data">A Base64-encoded string containing the left part of the diff</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="data"/> is not a valid Base64 string.</exception>
        /// <returns>The new <see cref="Diff"/> instance.</returns>
        public static Diff SetRight(int id, string data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            var right = Convert.FromBase64String(data);
            if (!Database.TryGetValue(id, out Diff diff))
            {
                diff = new Diff(new byte[0], right);
                Database.Add(id, diff);
                return diff;
            }
            return Database[id] = new Diff(diff.Left, right);
        }
    }
}
