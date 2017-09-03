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
        readonly List<Difference> _differences = new List<Difference>();

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
        /// A <see cref="IReadOnlyList{Difference}" /> containing the differences (Offset and Length) of
        /// the <see cref="Right"/> part in relation to the <see cref="Left"/> part of the diff, if any.
        /// </summary>
        public IReadOnlyList<Difference> Differences => _differences;

        /// <summary>
        /// Initializes a new instance of the <see cref="Diff" /> class with the specified left and right parts.
        /// </summary>
        /// <param name="left">The <see cref="Left" /> part of the diff</param>
        /// <param name="right">The <see cref="Right" /> part of the diff</param>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null.</exception>
        /// </exception>
        public Diff(byte[] left, byte[] right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
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
        static (DiffStatus status, List<Difference> differences) ExtractDifferences(byte[] left, byte[] right)
        {
            var differences = new List<Difference>();
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
                    differences.Add(new Difference(lastDifference, i - lastDifference));
                    lastDifference = -1;
                }
            }
            if (lastDifference > -1)
                differences.Add(new Difference(lastDifference, left.Length - lastDifference));
            var status = differences.Count == 0 ? DiffStatus.SameContent : DiffStatus.DifferentContent;
            return (status, differences);
        }
    }
}
