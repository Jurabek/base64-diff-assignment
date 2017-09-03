using System;
using NUnit.Framework;

namespace Base64Diff.Api.Helpers
{
    using Domain;

    [TestFixture]
    public class EnumExtensionsTest
    {
        [Test]
        public void TestEnumWithDescriptions()
        {
            Assert.AreEqual("different-content", DiffStatus.DifferentContent.GetDescription());
            Assert.AreEqual("different-lengths", DiffStatus.DifferentLength.GetDescription());
            Assert.AreEqual("same-content", DiffStatus.SameContent.GetDescription());
        }

        [Test]
        public void TestEnumWithoutDescription()
        {
            Assert.IsNull(StringComparison.CurrentCulture.GetDescription());
        }
    }
}
