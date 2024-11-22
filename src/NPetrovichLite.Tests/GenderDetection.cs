using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NPetrovichLite.Tests
{
    [TestFixture]
    public sealed class GenderDetection
    {
        private Petrovich petrovich;

        [OneTimeSetUp]
        public void Init()
        {
            petrovich = new Petrovich();
        }

        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.OpencorporaGenderDetectionData), new object[] { "firstnames.popular.gender.tsv", NamePart.FirstName})]
        public void TestGenderDetection(string value, NamePart part, Gender expected)
        {
            Gender gender = petrovich.GetGender(value, part);
            Assert.That(expected.Equals(gender), $"Value: {value} Part: {part}");
        }

        [Test]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.ReadPeopleCombinedGenderData))]
        public void TestGenderDetection(string lastName, string firstName, string midName, Gender expected)
        {
            Gender? gender = petrovich.GetGender(lastName, firstName, midName);
            Assert.That(expected.Equals(gender), String.Format("last: {0}, first: {1}, mid: {2}", lastName, firstName, midName));
        }
    }
}
