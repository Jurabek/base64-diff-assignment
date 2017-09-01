using System;

namespace Base64Diff.Domain
{
    /// <summary>
    /// Represents the different status of a diff operation.
    /// </summary>
    public enum DiffStatus
    {
        /// <summary>
        /// The two parts have different lengths.
        /// </summary>
        DifferentLength = 0,

        /// <summary>
        /// The two parts are exactly equal.
        /// </summary>
        SameContent = 1,

        /// <summary>
        /// The two parts have the same length, but different content.
        /// </summary>
        DifferentContent = 2
    }
}
