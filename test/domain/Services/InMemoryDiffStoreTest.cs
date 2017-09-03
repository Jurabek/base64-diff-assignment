using System;
using System.Linq;
using NUnit.Framework;

namespace Base64Diff.Domain.Services
{
    [TestFixture]
    public class InMemoryDiffStoreTest
    {
        private InMemoryDiffStore Store;

        [SetUp]
        public void CreateStore()
        {
            Store = new InMemoryDiffStore();
        }

        /// <summary>
        /// Helper to assert the total state of a Diff
        /// </summary>
        static void AssertDiffData(Diff diff, int leftLength, int rightLength, DiffStatus status, params (int, int)[] differences)
        {
            Assert.AreEqual(leftLength, diff.Left.Length);
            Assert.AreEqual(rightLength, diff.Right.Length);
            Assert.AreEqual(status, diff.Status);
            Assert.AreEqual(differences.Length, diff.Differences.Count);
            var differencesTuples = diff.Differences.Select(d => (d.Offset, d.Length));
            CollectionAssert.AreEqual(differences, differencesTuples);
        }

        [Test]
        public void DiffNotFound()
        {
            Assert.IsNull(Store.Get(int.MinValue));
        }

        [Test]
        public void SetLeft()
        {
            var diff = Store.SetLeft(1, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");     // The quick brown fox jumps over the lazy dog
            AssertDiffData(diff, 43, 0, DiffStatus.DifferentLength);
            AssertDiffData(Store.Get(1), 43, 0, DiffStatus.DifferentLength);
        }

        [Test]
        public void SetRight()
        {
            var diff = Store.SetRight(2, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");    // The quick brown fox jumps over the lazy dog
            AssertDiffData(diff, 0, 43, DiffStatus.DifferentLength);
            AssertDiffData(Store.Get(2), 0, 43, DiffStatus.DifferentLength);
        }

        [Test]
        public void SetBothDifferentLength()
        {
            Store.SetLeft(3, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");                // The quick brown fox jumps over the lazy dog
            var diff = Store.SetRight(3, "Rm9vIGJhcg==");                                                    // Foo bar
            AssertDiffData(diff, 43, 7, DiffStatus.DifferentLength);
            AssertDiffData(Store.Get(3), 43, 7, DiffStatus.DifferentLength);
        }

        [Test]
        public void SetBothSameContent()
        {
            Store.SetLeft(4, "Rm9vIGJhcg==");                // Foo bar
            var diff = Store.SetRight(4, "Rm9vIGJhcg==");    // Foo bar
            AssertDiffData(diff, 7, 7, DiffStatus.SameContent);
            AssertDiffData(Store.Get(4), 7, 7, DiffStatus.SameContent);
        }

        [Test]
        public void SetBothDifferentContent()
        {
            Store.SetLeft(5, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");                // The quick brown fox jumps over the lazy dog
            var diff = Store.SetRight(5, "VGhlIGxhenkgYnJvd24gZm94IGp1bXBzIG92ZXIgdGhlIHF1aWNrIGRvZw==");    // The lazy brown fox jumps over the quick dog
            AssertDiffData(diff, 43, 43, DiffStatus.DifferentContent, (4, 35));
            AssertDiffData(Store.Get(5), 43, 43, DiffStatus.DifferentContent, (4, 35));
        }

        [Test]
        public void SetBothDifferentContentAtTheEnd()
        {
            Store.SetLeft(6, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");                // The quick brown fox jumps over the lazy dog
            var diff = Store.SetRight(6, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGNhdA==");    // The quick brown fox jumps over the lazy cat
            AssertDiffData(diff, 43, 43, DiffStatus.DifferentContent, (40, 3));
        }

        [Test]
        public void SetBothDifferentContentSeveralDifferences()
        {
            Store.SetLeft(6, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");                // The quick brown fox jumps over the lazy dog
            var diff = Store.SetRight(6, "VGhlIHdpdHR5IGJyb3duIGZveCBsZWFwcyBvbnRvIHRoZSBuaWNlIGRvZw==");    // The witty brown fox leaps onto the nice dog
            AssertDiffData(diff, 43, 43, DiffStatus.DifferentContent, (4, 5), (20, 3), (27, 3), (35, 4));
            AssertDiffData(Store.Get(6), 43, 43, DiffStatus.DifferentContent, (4, 5), (20, 3), (27, 3), (35, 4));
        }

        [Test]
        public void SetLeftToNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Store.SetLeft(42, null));
            Assert.AreEqual("data", ex.ParamName);
        }

        [Test]
        public void SetRightToNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Store.SetRight(42, null));
            Assert.AreEqual("data", ex.ParamName);
        }

        [Test]
        public void SetLeftToInvalidBase64String()
        {
            Assert.Throws<FormatException>(() => Store.SetLeft(42, "not_a_valid_base64_string"));
        }

        [Test]
        public void SetRightToInvalidBase64String()
        {
            Assert.Throws<FormatException>(() => Store.SetRight(42, "not_a_valid_base64_string"));
        }
    }
}
