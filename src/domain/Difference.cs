using System;
using System.Collections.Generic;

namespace Base64Diff.Domain
{
    /// <summary>
    /// Represents a single difference (Offset + length) found somewhere between two pieces of binary data.
    /// </summary>
    public class Difference
    {
        readonly int _offset;
        readonly int _length;

        /// <summary>
        /// The zero-based index position in the data where the difference is found
        /// </summary>
        public int Offset => _offset;

        /// <summary>
        /// The size of the difference
        /// </summary>
        public int Length => _length;


        /// <summary>
        /// Initializes a new instance of the <see cref="Difference" /> class with the given offset and length.
        /// </summary>
        /// <param name="offset">The zero-based index position in the data where the difference is found</param>
        /// <param name="length">The size of the difference</param>
        public Difference(int offset, int length)
        {
            _offset = offset;
            _length = length;
        }
    }
}
