using System;
using NUnit.Framework;

namespace Base64Diff.Domain
{
    [TestFixture]
    public class DiffTest
    {
        /// <summary>
        /// Helper to assert the total state of a Diff
        /// </summary>
        static void AssertDiffData(Diff diff, int leftLength, int rightLength, DiffStatus status, params (int, int)[] differences)
        {
            Assert.AreEqual(leftLength, diff.Left.Length);
            Assert.AreEqual(rightLength, diff.Right.Length);
            Assert.AreEqual(status, diff.Status);
            CollectionAssert.AreEqual(differences, diff.Differences);
        }

        [Test]
        public void DiffNotFound()
        {
            Assert.IsNull(Diff.Find(int.MinValue));
        }

        [Test]
        public void SetLeft()
        {
            var diff = Diff.SetLeft(1, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");     // The quick brown fox jumps over the lazy dog
            AssertDiffData(diff, 43, 0, DiffStatus.DifferentLength);
            AssertDiffData(Diff.Find(1), 43, 0, DiffStatus.DifferentLength);
        }

        [Test]
        public void SetRight()
        {
            var diff = Diff.SetRight(2, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");    // The quick brown fox jumps over the lazy dog
            AssertDiffData(diff, 0, 43, DiffStatus.DifferentLength);
            AssertDiffData(Diff.Find(2), 0, 43, DiffStatus.DifferentLength);
        }

        [Test]
        public void SetBothDifferentLength()
        {
            Diff.SetLeft(3, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");                // The quick brown fox jumps over the lazy dog
            var diff = Diff.SetRight(3, "Rm9vIGJhcg==");                                                    // Foo bar
            AssertDiffData(diff, 43, 7, DiffStatus.DifferentLength);
            AssertDiffData(Diff.Find(3), 43, 7, DiffStatus.DifferentLength);
        }

        [Test]
        public void SetBothSameContent()
        {
            Diff.SetLeft(4, "Rm9vIGJhcg==");                // Foo bar
            var diff = Diff.SetRight(4, "Rm9vIGJhcg==");    // Foo bar
            AssertDiffData(diff, 7, 7, DiffStatus.SameContent);
            AssertDiffData(Diff.Find(4), 7, 7, DiffStatus.SameContent);
        }

        [Test]
        public void SetBothDifferentContent()
        {
            Diff.SetLeft(5, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");                // The quick brown fox jumps over the lazy dog
            var diff = Diff.SetRight(5, "VGhlIGxhenkgYnJvd24gZm94IGp1bXBzIG92ZXIgdGhlIHF1aWNrIGRvZw==");    // The lazy brown fox jumps over the quick dog
            AssertDiffData(diff, 43, 43, DiffStatus.DifferentContent, (4, 35));
            AssertDiffData(Diff.Find(5), 43, 43, DiffStatus.DifferentContent, (4, 35));
        }

        [Test]
        public void SetBothDifferentContentAtTheEnd()
        {
            Diff.SetLeft(6, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");                // The quick brown fox jumps over the lazy dog
            var diff = Diff.SetRight(6, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGNhdA==");    // The quick brown fox jumps over the lazy cat
            AssertDiffData(diff, 43, 43, DiffStatus.DifferentContent, (40, 3));
        }

        [Test]
        public void SetBothDifferentContentSeveralDifferences()
        {
            Diff.SetLeft(6, "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw==");                // The quick brown fox jumps over the lazy dog
            var diff = Diff.SetRight(6, "VGhlIHdpdHR5IGJyb3duIGZveCBsZWFwcyBvbnRvIHRoZSBuaWNlIGRvZw==");    // The witty brown fox leaps onto the nice dog
            AssertDiffData(diff, 43, 43, DiffStatus.DifferentContent, (4, 5), (20, 3), (27, 3), (35, 4));
            AssertDiffData(Diff.Find(6), 43, 43, DiffStatus.DifferentContent, (4, 5), (20, 3), (27, 3), (35, 4));
        }

        [Test]
        public void SetLeftToNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Diff.SetLeft(42, null));
            Assert.AreEqual("data", ex.ParamName);
        }

        [Test]
        public void SetRightToNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Diff.SetRight(42, null));
            Assert.AreEqual("data", ex.ParamName);
        }

        [Test]
        public void SetLeftToInvalidBase64String()
        {
            Assert.Throws<FormatException>(() => Diff.SetLeft(42, "not_a_valid_base64_string"));
        }

        [Test]
        public void SetRightToInvalidBase64String()
        {
            Assert.Throws<FormatException>(() => Diff.SetRight(42, "not_a_valid_base64_string"));
        }
    }
}
