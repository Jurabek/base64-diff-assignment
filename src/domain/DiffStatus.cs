using System;
using System.ComponentModel;

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
        [Description("different-lengths")]
        DifferentLength = 0,

        /// <summary>
        /// The two parts are exactly equal.
        /// </summary>
        [Description("same-content")]
        SameContent = 1,

        /// <summary>
        /// The two parts have the same length, but different content.
        /// </summary>
        [Description("different-content")]
        DifferentContent = 2
    }
}
