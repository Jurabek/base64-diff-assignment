using System;
using System.Linq;
using NUnit.Framework;

namespace Base64Diff.Domain
{
    [TestFixture]
    public class DiffTest
    {
        [Test]
        public void CreateDiffWithNullLeft()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new Diff(null, new byte[0]));
            Assert.AreEqual("left", ex.ParamName);
        }

        [Test]
        public void CreateDiffWithNullRight()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new Diff(new byte[0], null));
            Assert.AreEqual("right", ex.ParamName);
        }
    }
}
